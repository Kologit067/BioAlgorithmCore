using Microsoft.Extensions.Configuration;
using System.Data;
using BioAlgorithm.Data.Contract.HittingSet.Data.Contract;
using BioAlgorithm.Data.Contract.HittingSet.Data.Contract.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using HittingSet.Data.Contract;

namespace BioAlgorithm.Data.HittingSet.Data
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetRepository
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetRepository : IHittingSetRepository
    {
        private readonly string _connectionString;
        public HittingSetRepository(string? connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                //_connectionString = ConfigurationManager.ConnectionStrings["BioAlgorithm"].ConnectionString;
                //ConfigurationManager configurationManager = new ConfigurationManager();
                var builder = new ConfigurationBuilder().AddXmlFile("app.config");

                var appConfiguration = builder.Build();
                IConfigurationSection connStrings = appConfiguration.GetSection("ConnectionStrings");
                _connectionString = connStrings.GetSection("BioAlgorithm").Value;
            }
            else
            {
                _connectionString = connectionString;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteHittingSetAlgorithmGroupAsync(HittingSetAlgorithmGroupDimension selectedAlgorithmGroup)
        {
            return await DeleteAsync(selectedAlgorithmGroup.Algorithm, selectedAlgorithmGroup.NumberOfSet, selectedAlgorithmGroup.Dimension, selectedAlgorithmGroup.MaxCount);
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteHittingSetInputGroupAsync(HittingSetInputGroup selectedAlgorithmGroup)
        {
            return await DeleteInputAsync(selectedAlgorithmGroup.NumberOfSet, selectedAlgorithmGroup.Dimension, selectedAlgorithmGroup.MaxCount);
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? maxCount = null)
        {
            string? error = null;


            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            try
            {
                SqlCommand addCommand = new SqlCommand("dbo.deleteHittingSetPerfomance", connection);
                addCommand.CommandType = CommandType.StoredProcedure;
                addCommand.CommandTimeout = 300;
                SqlParameter tvpParam2 = addCommand.Parameters.AddWithValue("@Dimension", dimension);
                tvpParam2.SqlDbType = SqlDbType.Int;
                SqlParameter tvpParam3 = addCommand.Parameters.AddWithValue("@Algorithm", algorithm);
                tvpParam3.SqlDbType = SqlDbType.VarChar;
                SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@NumberOfSet", numberOfSet);
                tvpParam.SqlDbType = SqlDbType.Int;
                SqlParameter tvpParam4 = addCommand.Parameters.AddWithValue("@MaxCount", maxCount);
                tvpParam4.SqlDbType = SqlDbType.BigInt;

                await addCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            finally
            {
                connection.Close();
            }
            return error;

        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteInputAsync(int? numberOfSet = null, int? dimension = null, decimal? maxCount = null)
        {
            string? error = null;


            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            try
            {
                SqlCommand addCommand = new SqlCommand("dbo.deleteHittingSetInput", connection);
                addCommand.CommandType = CommandType.StoredProcedure;
                addCommand.CommandTimeout = 300;
                SqlParameter tvpParam2 = addCommand.Parameters.AddWithValue("@Dimension", dimension);
                tvpParam2.SqlDbType = SqlDbType.Int;
                SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@NumberOfSet", numberOfSet);
                tvpParam.SqlDbType = SqlDbType.Int;
                SqlParameter tvpParam4 = addCommand.Parameters.AddWithValue("@MaxCount", maxCount);
                tvpParam4.SqlDbType = SqlDbType.BigInt;

                await addCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            finally
            {
                connection.Close();
            }
            return error;

        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteHittingSetAlgorithmAsync(HittingSetAlgorithmGroup selectedAlgorithm)
        {
            return await DeleteAsync(selectedAlgorithm.Algorithm);
        }
        public async Task<List<HittingSetAlgorithmGroupDimension>> GetHittingSetAlgorithmGroupDimensionsAsync(string algorithmGroupListSort)
        {
            try
            {
                List<HittingSetAlgorithmGroupDimension> algorithmGroups = new List<HittingSetAlgorithmGroupDimension>();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = $@"SELECT [Algorithm], [NumberOfSet], [Dimension], [MaxCount], COUNT(*) as TotalCount, 
       SUM([NumberOfIteration]) as NumberOfIteration, SUM([Duration]) as TotalDuration,
	   SUM([Duration])/COUNT(*) as AverageDuration
FROM [dbo].[HittingSetPerfomance] AS rp
INNER JOIN [dbo].[HittingSetInput] AS ri
ON (rp.HittingSetInputId = ri.HittingSetInputId)
GROUP BY [Algorithm], [NumberOfSet], [Dimension], [MaxCount]
";
                    if (!string.IsNullOrEmpty(algorithmGroupListSort))
                        sql += $@"ORDER BY {algorithmGroupListSort}
";
                    algorithmGroups = (await db.QueryAsync<HittingSetAlgorithmGroupDimension>(sql, commandTimeout: 300)).ToList();
                }
                return algorithmGroups;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
                //return new List<HittingSetAlgorithmGroupDimension>();
            }
        }

        public async Task<List<(string Algorithname,int Count)>> GetGeedyAlgorithmGroupDimensionsAsync(int dimension, int numberOfSet, long maxCount)
        {
            try
            {
                List<(string Algorithname, int Count)> algorithmGroups = new List<(string Algorithname, int Count)>();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = $@"SELECT [Algorithm],  COUNT(*) as TotalCount 
FROM [dbo].[HittingSetPerfomance] AS rp
INNER JOIN [dbo].[HittingSetInput] AS ri
ON (rp.HittingSetInputId = ri.HittingSetInputId)
WHERE [NumberOfSet] = {numberOfSet} AND [Dimension] = {dimension} AND [MaxCount] = {maxCount} AND [Algorithm] IN ('HittingSetGreedyImprove','HittingSetGreedyImproveRD','HittingSetGreedyRelation','HittingSetGreedySimple','HittingSetBranchAndBoundByValue')
GROUP BY [Algorithm] 
";
                    algorithmGroups = (await db.QueryAsync<(string Algorithname, int Count)>(sql, commandTimeout: 180)).ToList();
                }
                return algorithmGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
                //return new List<HittingSetAlgorithmGroupDimension>();
            }
        }


        public async Task SetGeedyComparisonAsync(int dimension, int numberOfSet, long maxCount)
        {
            try
            {
                
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = $@"
UPDATE ri 
  SET ri.GreedyComparison = 
	 CASE
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND  r.BestValue = rgd.BestValue
				THEN 0
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND  r.BestValue <> rgd.BestValue
				THEN 1
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 2
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 3
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 4
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 5
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 6
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 7
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 8
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 9
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 10
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 11
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 12
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 13
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 14
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 15
	END 
  FROM [dbo].[HittingSetInput] AS ri
  INNER JOIN [dbo].[HittingSetPerfomance] as r
  ON r.HittingSetInputId = ri.HittingSetInputId AND r.Algorithm = 'HittingSetBranchAndBoundByValue' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet} AND ri.MaxCount = {maxCount} 
  INNER JOIN [dbo].[HittingSetPerfomance] as rgs
  ON rgs.HittingSetInputId = ri.HittingSetInputId AND rgs.Algorithm = 'HittingSetGreedySimple' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet}  AND ri.MaxCount = {maxCount}
  INNER JOIN [dbo].[HittingSetPerfomance] as rgr
  ON rgr.HittingSetInputId = ri.HittingSetInputId AND rgr.Algorithm = 'HittingSetGreedyRelation' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet}  AND ri.MaxCount = {maxCount}
  INNER JOIN [dbo].[HittingSetPerfomance] as rgi
  ON rgi.HittingSetInputId = ri.HittingSetInputId AND rgi.Algorithm = 'HittingSetGreedyImprove' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet}  AND ri.MaxCount = {maxCount}
  INNER JOIN [dbo].[HittingSetPerfomance] as rgd
  ON rgd.HittingSetInputId = ri.HittingSetInputId AND rgd.Algorithm = 'HittingSetGreedyImproveRD' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet}  AND ri.MaxCount = {maxCount}
";
                    await db.ExecuteAsync(sql, commandTimeout: 300);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
                //return new List<HittingSetAlgorithmGroupDimension>();
            }
        }

        public async Task<List<HittingSetInputGroup>> GetHittingSetInputGroupAsync(string algorithmGroupListSort)
        {
            try
            {
                List<HittingSetInputGroup> groups = new List<HittingSetInputGroup>();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = $@"WITH CTE 
AS
(
SELECT [NumberOfSet], [Dimension], [MaxCount], COUNT(*) as TotalCount, SUM (IIF([TypeTask] > 0, 1, 0)) AS SumTypaTask,
SUM (IIF([Isomorphic] IS NOT NULL, 1, 0)) AS SumIsomorphic, SUM (IIF(IsomorphicBipart IS NOT NULL, 1, 0)) AS SumIsomorphicBipart
FROM [dbo].[HittingSetInput] AS ri
GROUP BY [NumberOfSet], [Dimension], [MaxCount]
)
SELECT [NumberOfSet], [Dimension], [MaxCount], TotalCount, SumTypaTask, SUBSTRING(CAST(CAST(SumTypaTask AS NUMERIC(19,4))/TotalCount AS VARCHAR),1,5) AS TypeTaskRelation, SumIsomorphic, SumIsomorphicBipart
FROM CTE ";
                    if (!string.IsNullOrEmpty(algorithmGroupListSort))
                        sql += $@"ORDER BY {algorithmGroupListSort}
";
                    groups = (await db.QueryAsync<HittingSetInputGroup>(sql, commandTimeout: 600)).ToList();
                }
                return groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<HittingSetAlgorithWithDimension>> GetHittingSetAlgorithmWithDimensionsAsync()
        {
            List<HittingSetAlgorithWithDimension> algorithmWithDimensions = new List<HittingSetAlgorithWithDimension>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"SELECT [Algorithm], [NumberOfSet], [Dimension], [MaxCount]
FROM [dbo].[HittingSetPerfomance] AS rp
INNER JOIN [dbo].[HittingSetInput] AS ri
ON (rp.HittingSetInputId = ri.HittingSetInputId)
GROUP BY [Algorithm], [NumberOfSet], [Dimension], [MaxCount]
";
                algorithmWithDimensions = ( await db.QueryAsync<HittingSetAlgorithWithDimension>(query, commandTimeout: 300)).ToList();
            }
            return algorithmWithDimensions;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<HittingSetAlgorithmGroup>> GetAlgorithmsAsync()
        {
            List<HittingSetAlgorithmGroup> algorithmGroups = new List<HittingSetAlgorithmGroup>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = @"WITH CTE AS
(
SELECT [Algorithm], [NumberOfSet], [Dimension], COUNT(*) as cnt, SUM([NumberOfIteration]) as SumNumberOfIteration, SUM([Duration]) as SumDuration
FROM [dbo].[HittingSetPerfomance] AS rp
INNER JOIN [dbo].[HittingSetInput] AS ri
ON (rp.HittingSetInputId = ri.HittingSetInputId)
GROUP BY [Algorithm], [NumberOfSet], [Dimension]
)
SELECT [Algorithm], 
	SUM(cnt) AS TotalCount, 
	COUNT(*) as CountByDimension, 
	SUM(SumNumberOfIteration) as NumberOfIteration, 
	SUM(SumDuration) as TotalDuration,
	SUM(SumDuration)/SUM(cnt) AS AverageDuration
FROM CTE
GROUP BY [Algorithm]";
                algorithmGroups = ( await db.QueryAsync<HittingSetAlgorithmGroup>(sql,commandTimeout: 300)).ToList();
            }
            return algorithmGroups;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<HittingSetPerfomance>> GetHittingSetPerformanceListAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, string order)
        {
            string top = "";
            if (hittingSetPerfomanceFilter.Top.HasValue)
            {
                top = $"TOP ({hittingSetPerfomanceFilter.Top})";
            }
            string where = "";
            List<string> whereList = new List<string>();
            if (!string.IsNullOrWhiteSpace(hittingSetPerfomanceFilter.Algorithm))
            {
                whereList.Add($"[Algorithm] = '{hittingSetPerfomanceFilter.Algorithm}'");
            }
            if (hittingSetPerfomanceFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"[NumberOfSet] = {hittingSetPerfomanceFilter.NumberOfSet}");
            }
            if (hittingSetPerfomanceFilter.Dimension.HasValue)
            {
                whereList.Add($"[Dimension] = {hittingSetPerfomanceFilter.Dimension}");
            }
            if (hittingSetPerfomanceFilter.Step.HasValue && hittingSetPerfomanceFilter.Step.Value > 0)
            {
                whereList.Add($"[Step] = {hittingSetPerfomanceFilter.Step}");
            }
            if (hittingSetPerfomanceFilter.MaxCount.HasValue)
            {
                whereList.Add($"[MaxCount] = {hittingSetPerfomanceFilter.MaxCount}");
            }
            if (!string.IsNullOrWhiteSpace(hittingSetPerfomanceFilter.InputLen))
            {
                whereList.Add($"[InputLen] = '{hittingSetPerfomanceFilter.InputLen}'");
            }
            if (!string.IsNullOrWhiteSpace(hittingSetPerfomanceFilter.InputLenSort))
            {
                whereList.Add($"[InputLenSort] = '{hittingSetPerfomanceFilter.InputLenSort}'");
            }
            if (hittingSetPerfomanceFilter.NumberOfIterationFrom.HasValue)
            {
                whereList.Add($"[NumberOfIteration] >= {hittingSetPerfomanceFilter.NumberOfIterationFrom}");
            }
            if (hittingSetPerfomanceFilter.NumberOfIterationTo.HasValue)
            {
                whereList.Add($"[NumberOfIteration] <= {hittingSetPerfomanceFilter.NumberOfIterationTo}");
            }
            if (hittingSetPerfomanceFilter.DurationFrom.HasValue)
            {
                whereList.Add($"[Duration] >= {hittingSetPerfomanceFilter.DurationFrom}");
            }
            if (hittingSetPerfomanceFilter.DurationTo.HasValue)
            {
                whereList.Add($"[Duration] <= {hittingSetPerfomanceFilter.DurationTo}");
            }
            if (hittingSetPerfomanceFilter.CountTerminalFrom.HasValue)
            {
                whereList.Add($"[CountTerminal] >= {hittingSetPerfomanceFilter.CountTerminalFrom}");
            }
            if (hittingSetPerfomanceFilter.CountTerminalTo.HasValue)
            {
                whereList.Add($"[CountTerminal] <= {hittingSetPerfomanceFilter.CountTerminalTo}");
            }
            if (hittingSetPerfomanceFilter.BestValue.HasValue)
            {
                whereList.Add($"[BestValue] = {hittingSetPerfomanceFilter.BestValue}");
            }
            if (hittingSetPerfomanceFilter.IsComplete.HasValue)
            {
                whereList.Add($"[IsComplete] = {hittingSetPerfomanceFilter.IsComplete}");
            }
            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            List<HittingSetPerfomance> hittingSetPerfomances = new List<HittingSetPerfomance>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"SELECT {top} [HittingSetPerfomanceId]
      ,[NumberOfSet],[Dimension],[Step],MaxCount,[InputLen],[InputLenSort]
      ,[InputLenAvg],[InputData],[InputDataShort],[Algorithm],[NumberOfIteration]
	  ,[Duration],[DurationMilliSeconds],[DateComplete],[IsComplete]
      ,[LastRoute],[OptimalRoute],[CountTerminal],[BestValue]
      ,[UpdateOptcount],[ElemenationCount]
FROM [dbo].[HittingSetPerfomance] AS rp
INNER JOIN [dbo].[HittingSetInput] AS ri
ON (rp.HittingSetInputId = ri.HittingSetInputId)
{where}
ORDER BY {order}";
                hittingSetPerfomances = ( await db.QueryAsync<HittingSetPerfomance>(query, commandTimeout: 300)).ToList();
            }
            return hittingSetPerfomances;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<HittingSetInputDao>> GetHittingSetInputsAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, string order)
        {
            string top = "";
            if (hittingSetPerfomanceFilter.Top.HasValue)
            {
                top = $"TOP ({hittingSetPerfomanceFilter.Top})";
            }
            string where = "";
            List<string> whereList = new List<string>();
            if (hittingSetPerfomanceFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"[NumberOfSet] = {hittingSetPerfomanceFilter.NumberOfSet}");
            }
            if (hittingSetPerfomanceFilter.Dimension.HasValue)
            {
                whereList.Add($"[Dimension] = {hittingSetPerfomanceFilter.Dimension}");
            }
            if (hittingSetPerfomanceFilter.Step.HasValue && hittingSetPerfomanceFilter.Step.Value > 0)
            {
                whereList.Add($"[Step] = {hittingSetPerfomanceFilter.Step}");
            }
            if (hittingSetPerfomanceFilter.MaxCount.HasValue)
            {
                whereList.Add($"[MaxCount] = {hittingSetPerfomanceFilter.MaxCount}");
            }
            if (!string.IsNullOrWhiteSpace(hittingSetPerfomanceFilter.InputLen))
            {
                whereList.Add($"[InputLen] = '{hittingSetPerfomanceFilter.InputLen}'");
            }
            if (!string.IsNullOrWhiteSpace(hittingSetPerfomanceFilter.InputLenSort))
            {
                whereList.Add($"[InputLenSort] = '{hittingSetPerfomanceFilter.InputLenSort}'");
            }

            if (hittingSetPerfomanceFilter.TaskTypeFilter != 0 )
            {
                string[] taskTypeFilterTypeArr = hittingSetPerfomanceFilter.TaskTypeFilterType.Split('/');
                string whereTaskType = null;
                if (taskTypeFilterTypeArr[0] == "Or")
                {
                    whereTaskType = $"(([TypeTask] & {hittingSetPerfomanceFilter.TaskTypeFilter}) != 0)";
                }
                if (taskTypeFilterTypeArr[0] == "And")
                {
                    whereTaskType = $"(([TypeTask] & {hittingSetPerfomanceFilter.TaskTypeFilter}) = {hittingSetPerfomanceFilter.TaskTypeFilter})";
                }
                if (taskTypeFilterTypeArr[0] == "Exact =")
                {
                    whereTaskType = $"([TypeTask] = {hittingSetPerfomanceFilter.TaskTypeFilter})";
                }
                if (!string.IsNullOrEmpty(whereTaskType))
                {
                    if (taskTypeFilterTypeArr.Length > 1)
                        whereTaskType = $"NOT {whereTaskType}";
                    whereList.Add(whereTaskType);
                }
            }

            if (hittingSetPerfomanceFilter.GreedyComparisonFilter != 0)
            {
                string[] greedyComparisonFilterTypeArr = hittingSetPerfomanceFilter.GreedyComparisonFilterType.Split('/');
                string whereGreedyComparison = null;
                if (greedyComparisonFilterTypeArr[0] == "Or")
                {
                    whereGreedyComparison = $"([GreedyComparison] & {hittingSetPerfomanceFilter.GreedyComparisonFilter}) != 0";
                }
                if (greedyComparisonFilterTypeArr[0] == "And")
                {
                    whereGreedyComparison = $"([GreedyComparison] & {hittingSetPerfomanceFilter.GreedyComparisonFilter}) = {hittingSetPerfomanceFilter.GreedyComparisonFilter}";
                }
                if (greedyComparisonFilterTypeArr[0] == "Exact =")
                {
                    whereGreedyComparison = $"([GreedyComparison] = {hittingSetPerfomanceFilter.GreedyComparisonFilter})";
                }
                if (!string.IsNullOrEmpty(whereGreedyComparison))
                {
                    if (greedyComparisonFilterTypeArr.Length > 1)
                        whereGreedyComparison = $"NOT {whereGreedyComparison}";
                    whereList.Add(whereGreedyComparison);
                }
            }

            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            List<HittingSetInputDao> HittingSetInputs = new List<HittingSetInputDao>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"SELECT {top} HittingSetInputId, [NumberOfSet],[Dimension],[Step],MaxCount,[InputLen],[InputLenSort]
      ,[InputLenAvg],[InputData],[InputDataShort],Isomorphic,IsomorphicBipart, IsomorphismResult, IsomorphismBipartResult, TypeTask, GreedyComparison
FROM [dbo].[HittingSetInput] AS ri
{where}
ORDER BY {order}";
                HittingSetInputs = (await db.QueryAsync<HittingSetInputDao>(query, commandTimeout: 300)).ToList();
            }
            updateIsomorphicDict.Clear();
            return HittingSetInputs;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<InputDataSlim>> GetInputDataAsync(int dimension, int numberOfSet, long maxCount)
        {
            string where = $"WHERE [NumberOfSet] = {numberOfSet} AND [Dimension] = {dimension} AND [MaxCount] = {maxCount}";


            List<InputDataSlim> HittingSetInputs = new List<InputDataSlim>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"SELECT HittingSetInputId, [NumberOfSet],[Dimension],[Step],MaxCount,[InputLen],[InputLenSort]
      ,[InputLenAvg],[InputData],[InputDataShort],Isomorphic,IsomorphicBipart, IsomorphismResult, IsomorphismBipartResult, TypeTask, GreedyComparison
FROM [dbo].[HittingSetInput] AS ri
{where}";
                HittingSetInputs = (await db.QueryAsync<InputDataSlim>(query, commandTimeout: 300)).ToList();
            }
            return HittingSetInputs;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<InputDataGreedyComparison>> GetInputDataGreedyComparisonsAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, string order)
        {
            string top = "";
            if (hittingSetPerfomanceFilter.Top.HasValue)
            {
                top = $"TOP ({hittingSetPerfomanceFilter.Top})";
            }
            string where = "";
            List<string> whereList = new List<string>();
            whereList.Add($"[NumberOfSet] = {hittingSetPerfomanceFilter.NumberOfSet}");
            whereList.Add($"[Dimension] = {hittingSetPerfomanceFilter.Dimension}");
            whereList.Add($"[MaxCount] = {hittingSetPerfomanceFilter.MaxCount}");
            if (!string.IsNullOrWhiteSpace(hittingSetPerfomanceFilter.InputLen))
            {
                whereList.Add($"[InputLen] = '{hittingSetPerfomanceFilter.InputLen}'");
            }
            if (!string.IsNullOrWhiteSpace(hittingSetPerfomanceFilter.InputLenSort))
            {
                whereList.Add($"[InputLenSort] = '{hittingSetPerfomanceFilter.InputLenSort}'");
            }

            if (hittingSetPerfomanceFilter.TaskTypeFilter != 0)
            {
                string[] taskTypeFilterTypeArr = hittingSetPerfomanceFilter.TaskTypeFilterType.Split('/');
                string whereTaskType = null;
                if (taskTypeFilterTypeArr[0] == "Or")
                {
                    whereTaskType = $"(([TypeTask] & {hittingSetPerfomanceFilter.TaskTypeFilter}) != 0)";
                }
                if (taskTypeFilterTypeArr[0] == "And")
                {
                    whereTaskType = $"(([TypeTask] & {hittingSetPerfomanceFilter.TaskTypeFilter}) = {hittingSetPerfomanceFilter.TaskTypeFilter})";
                }
                if (taskTypeFilterTypeArr[0] == "Exact =")
                {
                    whereTaskType = $"([TypeTask] = {hittingSetPerfomanceFilter.TaskTypeFilter})";
                }
                if (!string.IsNullOrEmpty(whereTaskType))
                {
                    if (taskTypeFilterTypeArr.Length > 1)
                        whereTaskType = $"NOT {whereTaskType}";
                    whereList.Add(whereTaskType);
                }
            }

            if (hittingSetPerfomanceFilter.GreedyComparisonFilter != 0 )
            {
                string[] greedyComparisonFilterTypeArr = hittingSetPerfomanceFilter.GreedyComparisonFilterType.Split('/');
                string whereGreedyComparison = null;
                if (greedyComparisonFilterTypeArr[0] == "Or")
                {
                    whereGreedyComparison = $"([GreedyComparison] & {hittingSetPerfomanceFilter.GreedyComparisonFilter}) != 0";
                }
                if (greedyComparisonFilterTypeArr[0] == "And")
                {
                    whereGreedyComparison = $"([GreedyComparison] & {hittingSetPerfomanceFilter.GreedyComparisonFilter}) = {hittingSetPerfomanceFilter.GreedyComparisonFilter}";
                }
                if (greedyComparisonFilterTypeArr[0] == "Exact =")
                {
                    whereGreedyComparison = $"([GreedyComparison] = {hittingSetPerfomanceFilter.GreedyComparisonFilter})";
                }
                if (!string.IsNullOrEmpty(whereGreedyComparison))
                {
                    if (greedyComparisonFilterTypeArr.Length > 1)
                        whereGreedyComparison = $"NOT {whereGreedyComparison}";
                    whereList.Add(whereGreedyComparison);
                }
            }

            if (!string.IsNullOrEmpty(hittingSetPerfomanceFilter.PairComparison) && hittingSetPerfomanceFilter.PairComparison != "N/A")
            {
                List<int> pairComparisonArr = hittingSetPerfomanceFilter.PairComparison.Split("<").Select(p => int.Parse(p)).ToList();
                whereList.Add($" {algorithmGreedyBestValueNames[pairComparisonArr[0] - 1]} < {algorithmGreedyBestValueNames[pairComparisonArr[1] - 1]}");
            }

            if (!string.IsNullOrEmpty(hittingSetPerfomanceFilter.SelectedGreedy) && hittingSetPerfomanceFilter.SelectedGreedy != "N/A")
            {
                int greedy = int.Parse(hittingSetPerfomanceFilter.SelectedGreedy) - 1;
                for (int i = 0; i < algorithmGreedyBestValueNames.Length; i++)
                {
                    if (i != greedy)
                        whereList.Add($" {algorithmGreedyBestValueNames[greedy]} > {algorithmGreedyBestValueNames[i]}");
                }
            }

            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            List<InputDataGreedyComparison> HittingSetInputs = new List<InputDataGreedyComparison>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"
  SELECT ri.HittingSetInputId, 
     r.[Algorithm], r.BestValue, 
	 CASE
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND  r.BestValue = rgd.BestValue
				THEN 0
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND  r.BestValue <> rgd.BestValue
				THEN 1
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 2
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 3
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 4
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 5
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 6
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 7
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 8
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 9
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 10
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 11
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 12
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 13
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 14
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 15
	END AS GreedyComparisonCalc, GreedyComparison,
	rgs.[Algorithm] as AlgorithmSimple, rgs.BestValue as BestValueSimple, 
	rgr.[Algorithm] as AlgorithmRelation, rgr.BestValue as BestValueRelation, 
	rgi.[Algorithm] as AlgorithmImprove, rgi.BestValue as BestValueImprove, 
	rgd.[Algorithm] as AlgorithmImproveRD, rgd.BestValue as BestValueImproveRD, 
	TypeTask,InputData, [NumberOfSet], [Dimension], MAXCOUNT
  FROM [dbo].[HittingSetInput] AS ri
  INNER JOIN [dbo].[HittingSetPerfomance] as r 
  ON r.HittingSetInputId = ri.HittingSetInputId AND r.Algorithm = 'HittingSetBranchAndBoundByValue' AND ri.Dimension = {hittingSetPerfomanceFilter.Dimension} AND ri.NumberOfSet = {hittingSetPerfomanceFilter.NumberOfSet} AND ri.MaxCount = {hittingSetPerfomanceFilter.MaxCount} 
  INNER JOIN [dbo].[HittingSetPerfomance] as rgs
  ON rgs.HittingSetInputId = ri.HittingSetInputId AND rgs.Algorithm = 'HittingSetGreedySimple' AND ri.Dimension = {hittingSetPerfomanceFilter.Dimension} AND ri.NumberOfSet = {hittingSetPerfomanceFilter.NumberOfSet}  AND ri.MaxCount = {hittingSetPerfomanceFilter.MaxCount}
  INNER JOIN [dbo].[HittingSetPerfomance] as rgr
  ON rgr.HittingSetInputId = ri.HittingSetInputId AND rgr.Algorithm = 'HittingSetGreedyRelation' AND ri.Dimension = {hittingSetPerfomanceFilter.Dimension} AND ri.NumberOfSet = {hittingSetPerfomanceFilter.NumberOfSet}  AND ri.MaxCount = {hittingSetPerfomanceFilter.MaxCount}
  INNER JOIN [dbo].[HittingSetPerfomance] as rgi
  ON rgi.HittingSetInputId = ri.HittingSetInputId AND rgi.Algorithm = 'HittingSetGreedyImprove' AND ri.Dimension = {hittingSetPerfomanceFilter.Dimension} AND ri.NumberOfSet = {hittingSetPerfomanceFilter.NumberOfSet}  AND ri.MaxCount = {hittingSetPerfomanceFilter.MaxCount}
  INNER JOIN [dbo].[HittingSetPerfomance] as rgd
  ON rgd.HittingSetInputId = ri.HittingSetInputId AND rgd.Algorithm = 'HittingSetGreedyImproveRD' AND ri.Dimension = {hittingSetPerfomanceFilter.Dimension} AND ri.NumberOfSet = {hittingSetPerfomanceFilter.NumberOfSet} AND ri.MaxCount = {hittingSetPerfomanceFilter.MaxCount}
{where}
ORDER BY {order}";
                HittingSetInputs = (await db.QueryAsync<InputDataGreedyComparison>(query, commandTimeout: 300)).ToList();
            }
            return HittingSetInputs;
        }
        private readonly string[] algorithmGreedyBestValueNames = new string[] { "rgs.BestValue", "rgr.BestValue", "rgi.BestValue", "rgd.BestValue" };
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<HittingSetPerfomanceCompare>> GetHittingSetPerformanceCompareListAsync(HittingSetPerfomanceCompareFilter HittingSetPerfomanceCompareFilter)
        {
            string top = "";
            if (HittingSetPerfomanceCompareFilter.Top.HasValue)
            {
                top = $"TOP ({HittingSetPerfomanceCompareFilter.Top})";
            }
            string where = "";
            List<string> whereList = new List<string>();

            if (HittingSetPerfomanceCompareFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"ra.[NumberOfSet] = {HittingSetPerfomanceCompareFilter.NumberOfSet}");
            }
            if (HittingSetPerfomanceCompareFilter.Dimension.HasValue)
            {
                whereList.Add($"ra.[Dimension] = {HittingSetPerfomanceCompareFilter.Dimension}");
            }
            if (HittingSetPerfomanceCompareFilter.Step.HasValue)
            {
                whereList.Add($"ra.[Step] = {HittingSetPerfomanceCompareFilter.Step}");
            }
            if (HittingSetPerfomanceCompareFilter.MaxCount.HasValue)
            {
                whereList.Add($"ra.[MaxCount] = {HittingSetPerfomanceCompareFilter.MaxCount}");
            }
            if (!string.IsNullOrWhiteSpace(HittingSetPerfomanceCompareFilter.BestValueCompare) && HittingSetPerfomanceCompareFilter.BestValueCompare != "N/A")
            {
                whereList.Add($"(ra.BestValue {HittingSetPerfomanceCompareFilter.BestValueCompare.Replace("1", "").Replace("2", "")} rb.BestValue OR ra.BestValue IS NULL OR rb.BestValue IS NULL )");
            }
            if (!string.IsNullOrWhiteSpace(HittingSetPerfomanceCompareFilter.NumberIterationCompare) && HittingSetPerfomanceCompareFilter.NumberIterationCompare != "N/A")
            {
                whereList.Add($"(ra.[NumberOfIteration] {HittingSetPerfomanceCompareFilter.NumberIterationCompare.Replace("1", "").Replace("2", "")} rb.[NumberOfIteration] OR ra.[NumberOfIteration] IS NULL OR rb.[NumberOfIteration] IS NULL )");
            }
            if (!string.IsNullOrWhiteSpace(HittingSetPerfomanceCompareFilter.DurationCompare) && HittingSetPerfomanceCompareFilter.DurationCompare != "N/A")
            {
                whereList.Add($"(ra.Duration {HittingSetPerfomanceCompareFilter.DurationCompare.Replace("1", "").Replace("2", "")} rb.Duration OR ra.Duration IS NULL OR rb.Duration IS NULL )");
            }
            if (!string.IsNullOrWhiteSpace(HittingSetPerfomanceCompareFilter.ElemenationCountCompare) && HittingSetPerfomanceCompareFilter.ElemenationCountCompare != "N/A")
            {
                whereList.Add($"(ra.ElemenationCount {HittingSetPerfomanceCompareFilter.ElemenationCountCompare.Replace("1", "").Replace("2", "")} rb.ElemenationCount OR ra.ElemenationCount IS NULL OR rb.ElemenationCount IS NULL )");
            }
            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            List<HittingSetPerfomanceCompare> hittingSetPerfomancesCompare = new List<HittingSetPerfomanceCompare>();
            string query = $@"WITH ra AS
(
SELECT ria.[NumberOfSet], ria.[Dimension], ria.[InputData], ria.[InputDataShort], ria.Step, ria.MaxCount,
       rpa.Algorithm, rpa.BestValue, rpa.OptimalRoute, rpa.[NumberOfIteration],rpa.[Duration], rpa.ElemenationCount
FROM [dbo].[HittingSetPerfomance] AS rpa
INNER JOIN [dbo].[HittingSetInput] AS ria
ON (rpa.HittingSetInputId = ria.HittingSetInputId)
),
rb AS
(
SELECT rib.[NumberOfSet], rib.[Dimension], rib.[InputData], rib.[InputDataShort], rib.Step, rib.MaxCount,
       rpb.Algorithm, rpb.BestValue, rpb.OptimalRoute, rpb.[NumberOfIteration], rpb.[Duration], rpb.ElemenationCount
FROM [dbo].[HittingSetPerfomance] AS rpb
INNER JOIN [dbo].[HittingSetInput] AS rib
ON (rpb.HittingSetInputId = rib.HittingSetInputId)
)
SELECT ra.[NumberOfSet], ra.[Dimension], ra.[InputData], ra.[InputDataShort], ra.Step, ra.MaxCount,
       ra.Algorithm as Algorithm1, rb.Algorithm as Algorithm2, 
       ra.BestValue as BestValue1, rb.BestValue as BestValue2, 
	   ra.OptimalRoute as OptimalRoute1, rb.OptimalRoute as OptimalRoute2,
	   ra.[NumberOfIteration] as NumberOfIteration1, rb.[NumberOfIteration] as NumberOfIteration2,
       ra.[Duration] as Duration1, rb.[Duration] as Duration2,
       ra.ElemenationCount as ElemenationCount1, rb.ElemenationCount as ElemenationCount2
FROM ra INNER JOIN rb
ON (ra.InputData = rb.InputData AND ra.[NumberOfSet] = rb.[NumberOfSet] AND ra.[Dimension] = rb.[Dimension] AND
ra.[Algorithm] = '{HittingSetPerfomanceCompareFilter.Algorithm1}' AND rb.[Algorithm] = '{HittingSetPerfomanceCompareFilter.Algorithm2}') 
{where}
";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                hittingSetPerfomancesCompare = (await db.QueryAsync<HittingSetPerfomanceCompare>(query, commandTimeout: 300)).ToList();
            }
            return hittingSetPerfomancesCompare;
        }
        private int updateIsomorphicBufferSize = 1000;
        private Dictionary<long, (string,string)> updateIsomorphicDict = new Dictionary<long, (string,string)>();
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> UpdateIsomorphicAsync(long HittingSetInputId, string inputData, string result, bool isBipart = false)
        {
            string error = string.Empty;
            updateIsomorphicDict.Add(HittingSetInputId, (inputData, result));

            if (updateIsomorphicDict.Count >= updateIsomorphicBufferSize)
            {
                error = await SaveUpdateIsomorphicAsync(updateIsomorphicDict, isBipart);
                updateIsomorphicDict.Clear();
            }
            return error;
        }

        private int updateTaskTypeBufferSize = 1000;
        private Dictionary<long, int> updateTaskTypeDict = new Dictionary<long, int>();
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> UpdateTypeTaskAsync(long HittingSetInputId, int typeTask)
        {
            string error = string.Empty;
            updateTaskTypeDict.Add(HittingSetInputId, typeTask);

            if (updateTaskTypeDict.Count >= updateTaskTypeBufferSize)
            {
                error = await SaveUpdateTypeTaskAsync(updateTaskTypeDict);
                updateTaskTypeDict.Clear();
            }
            return error;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> CompleteUpdateIsomorphicAsync(bool isBipart = false)
        {
            string error = await SaveUpdateIsomorphicAsync(updateIsomorphicDict, isBipart);
            updateIsomorphicDict.Clear();
            return error;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> CompleteUpdateTypeTaskAsync()
        {
            string error = await SaveUpdateTypeTaskAsync(updateTaskTypeDict);
            updateIsomorphicDict.Clear();
            return error;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async Task<string> SaveUpdateIsomorphicAsync(Dictionary<long, (string, string)> updateIsomorphicDict, bool isBipart = false)
        {
            string error = string.Empty;
            try
            {

                DataTable isonorphicTable = new DataTable();
                isonorphicTable.Columns.Add("Id", System.Type.GetType("System.Int64"));
                isonorphicTable.Columns.Add("InputData", System.Type.GetType("System.String"));
                isonorphicTable.Columns.Add("Result", System.Type.GetType("System.String"));


                foreach (var ui in updateIsomorphicDict)
                {
                    isonorphicTable.Rows.Add(ui.Key, ui.Value.Item1, ui.Value.Item2);
                }

                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                try
                {
                    SqlCommand addCommand = new SqlCommand("dbo.spUpdateIsomorphic", connection);
                    addCommand.CommandType = CommandType.StoredProcedure;
                    addCommand.CommandTimeout = 300;
                    SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@UpdateIsomorphics", isonorphicTable);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.UpdateIsomorphicType";
                    SqlParameter tvpParam2 = addCommand.Parameters.AddWithValue("@IsBipart", isBipart);
                    tvpParam2.SqlDbType = SqlDbType.Bit;
                    await addCommand.ExecuteNonQueryAsync();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return error;
        }


        //----------------------------------------------------------------------------------------------------------------------
        private async Task<string> SaveUpdateTypeTaskAsync(Dictionary<long, int> updateTypeTaskDict)
        {
            string error = string.Empty;
            try
            {

                DataTable typeTaskTable = new DataTable();
                typeTaskTable.Columns.Add("Id", System.Type.GetType("System.Int64"));
                typeTaskTable.Columns.Add("TypeTask", System.Type.GetType("System.Int32"));


                foreach (var ui in updateTypeTaskDict)
                {
                    typeTaskTable.Rows.Add(ui.Key, ui.Value);
                }

                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                try
                {
                    SqlCommand addCommand = new SqlCommand("dbo.spUpdateTypeTask", connection);
                    addCommand.CommandType = CommandType.StoredProcedure;
                    addCommand.CommandTimeout = 300;
                    SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@UpdateTypeTask", typeTaskTable);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.UpdateTypeTaskType";
                    await addCommand.ExecuteNonQueryAsync();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return error;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task ClearIsomorphicAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, bool isBipart = false)
        {
            string where = "";
            List<string> whereList = new List<string>();
            if (hittingSetPerfomanceFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"[NumberOfSet] = {hittingSetPerfomanceFilter.NumberOfSet}");
            }
            if (hittingSetPerfomanceFilter.Dimension.HasValue)
            {
                whereList.Add($"[Dimension] = {hittingSetPerfomanceFilter.Dimension}");
            }
            if (hittingSetPerfomanceFilter.Step.HasValue && hittingSetPerfomanceFilter.Step.Value > 0)
            {
                whereList.Add($"[Step] = {hittingSetPerfomanceFilter.Step}");
            }
            if (hittingSetPerfomanceFilter.MaxCount.HasValue)
            {
                whereList.Add($"[MaxCount] = {hittingSetPerfomanceFilter.MaxCount}");
            }
            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = string.Empty;
                if (!isBipart)
                {
                    query = $@"UPDATE [dbo].[HittingSetInput]
SET [Isomorphic] = NULL, [IsomorphismResult] = NULL
{where}
";
                }
                else
                {
                    query = $@"UPDATE [dbo].[HittingSetInput]
SET [IsomorphicBipart] = NULL, [IsomorphismBipartResult] = NULL
{where}
";
                }
                await db.ExecuteAsync(query);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task ClearTaskTypeAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter)
        {
            string where = "";
            List<string> whereList = new List<string>();
            if (hittingSetPerfomanceFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"[NumberOfSet] = {hittingSetPerfomanceFilter.NumberOfSet}");
            }
            if (hittingSetPerfomanceFilter.Dimension.HasValue)
            {
                whereList.Add($"[Dimension] = {hittingSetPerfomanceFilter.Dimension}");
            }
            if (hittingSetPerfomanceFilter.Step.HasValue && hittingSetPerfomanceFilter.Step.Value > 0)
            {
                whereList.Add($"[Step] = {hittingSetPerfomanceFilter.Step}");
            }
            if (hittingSetPerfomanceFilter.MaxCount.HasValue)
            {
                whereList.Add($"[MaxCount] = {hittingSetPerfomanceFilter.MaxCount}");
            }
            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = string.Empty;
                query = $@"UPDATE [dbo].[HittingSetInput]
SET [TypeTask] = NULL
{where}
";
                await db.ExecuteAsync(query);
            }
            //----------------------------------------------------------------------------------------------------------------------
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
