using HittingSet.Data.Contract;

namespace BioAlgorithm.Data.Contract.HittingSet.Data.Contract.Interfaces
{
    public interface IHittingSetRepository
    {
        Task<string> DeleteHittingSetAlgorithmGroupAsync(HittingSetAlgorithmGroupDimension selectedAlgorithmGroup);

        Task<string> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? step = null);


        Task<string> DeleteHittingSetAlgorithmAsync(HittingSetAlgorithmGroup selectedAlgorithm);
        Task<List<HittingSetAlgorithmGroupDimension>> GetHittingSetAlgorithmGroupDimensionsAsync(string algorithmGroupListSort);


        Task<List<HittingSetAlgorithmGroup>> GetAlgorithmsAsync();

        Task<List<HittingSetAlgorithWithDimension>> GetHittingSetAlgorithmWithDimensionsAsync();

        Task<List<HittingSetPerfomance>> GetHittingSetPerformanceListAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, string order);


        Task<List<HittingSetPerfomanceCompare>> GetHittingSetPerformanceCompareListAsync(HittingSetPerfomanceCompareFilter hittingSetPerfomanceCompareFilter);
        Task<string> UpdateIsomorphicAsync(long hittingSetInputId, string inputData, string result, bool isBipart = false);
        Task<string> UpdateTypeTaskAsync(long hittingSetInputId, int typeTask);

        Task<string> CompleteUpdateIsomorphicAsync(bool isBipart = false);
        Task<string> CompleteUpdateTypeTaskAsync();

        Task ClearIsomorphicAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, bool isBipart = false);
        Task<List<HittingSetInputDao>> GetHittingSetInputsAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, string order);

        Task ClearTaskTypeAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter);
        Task<List<(string Algorithname, int Count)>> GetGeedyAlgorithmGroupDimensionsAsync(int dimension, int numberOfSet, long maxCount);
        Task SetGeedyComparisonAsync(int dimension, int numberOfSet, long maxCount);
        Task<List<InputDataGreedyComparison>> GetInputDataGreedyComparisonsAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, string order);
        Task<List<InputDataSlim>> GetInputDataAsync(int dimension, int numberOfSet, long maxCount);
   }
}
