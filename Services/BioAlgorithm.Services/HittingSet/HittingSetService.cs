using AlgorithmHittingSet;
using AlgorithmHittingSet.BinaryTreeEnumeration;
using AlgorithmHittingSet.BranchAndBound;
using AlgorithmHittingSet.Greedy;
using AlgorithmHittingSet.TriangleEnumeration;
using AlgorithmHittingSet.TriangleEnumeration.SelectElement;
using BaseContract.Interfaces;
using BioAlgorithm.Data.Contract.HittingSet.Data.Contract;
using BioAlgorithm.Data.Contract.HittingSet.Data.Contract.Interfaces;
using BioAlgorithm.Services.Contract;
using BioAlgorithmModel.BipartiteGraphModel;
using GraphLib;
using HittingSet.Data.Contract;
using IsomorphismGraph;

namespace BioAlgorithm.Services.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetService
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetService : IHittingSetService
    {
        private readonly IHittingSetRepository hittingSetRepository;
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetService(IHittingSetRepository hittingSetRepository)
        {
            this.hittingSetRepository = hittingSetRepository;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> TestIsomorphismAsync(string algorithmName, int dimension, int numberOfSet, long maxCount, bool isBipart = false)
        {
            string error = string.Empty;
            try
            {
                HittingSetPerfomanceFilter hittingSetPerfomanceFilterDto = new HittingSetPerfomanceFilter
                {
                    Algorithm = algorithmName,
                    Dimension = dimension,
                    NumberOfSet = numberOfSet,
                    MaxCount = maxCount
                };
                await hittingSetRepository.ClearIsomorphicAsync(hittingSetPerfomanceFilterDto, isBipart);
                List<HittingSetInputDao> items = await hittingSetRepository.GetHittingSetInputsAsync(hittingSetPerfomanceFilterDto, "InputDataShort");
                if (!isBipart)
                {
                    items.ForEach(item =>
                    {
                        item.Isomorphic = null;
                        item.IsomorphismResult = null;
                    });
                }
                else
                {
                    items.ForEach(item =>
                    {
                        item.Isomorphic = null;
                        item.IsomorphismBipartResult = null;
                    });
                }
                for (int i = 0; i < items.Count; i++)
                {
                    HittingSetInputDao itemOut = items[i];
                    MultiGraph graph1 = new MultiGraph(itemOut.InputData);
                    BipartiteGraph bipartiteGraph1 = new BipartiteGraph(itemOut.InputData);
                    if (itemOut.Isomorphic == null)
                    {
                        for (int j = i; j < items.Count; j++)
                        {
                            HittingSetInputDao itemIn = items[j];
                            if (itemIn.Isomorphic == null)
                            {
                                bool result = true;
                                if (i != j)
                                {
                                    MultiGraph graph2 = new MultiGraph(itemIn.InputData);
                                    BipartiteGraph bipartiteGraph2 = new BipartiteGraph(itemIn.InputData);
                                    if (!isBipart)
                                    {
                                        IsomorphismMultiGraph algorithm = new IsomorphismMultiGraph(graph1, graph2);
                                        result = algorithm.IsIsomorphic();
                                        if (result)
                                        {
                                            itemIn.Isomorphic = itemOut.InputData;
                                            error = await hittingSetRepository.UpdateIsomorphicAsync(itemIn.HittingSetInputId, itemOut.InputData, algorithm.ShowFullString, false);
                                        }
                                    }
                                    else
                                    {
                                        IsomorphismBipartGraph algorithm = new IsomorphismBipartGraph(bipartiteGraph1, bipartiteGraph2);
                                        result = algorithm.IsIsomorphic();
                                        if (result)
                                        {
                                            itemIn.Isomorphic = itemOut.InputData;
                                            error = await hittingSetRepository.UpdateIsomorphicAsync(itemIn.HittingSetInputId, itemOut.InputData, algorithm.ShowFullString, true);
                                        }
                                    }
                                }
                                else
                                    error = await hittingSetRepository.UpdateIsomorphicAsync(itemIn.HittingSetInputId, itemIn.InputData, "", false);
                            }
                            if (!string.IsNullOrEmpty(error))
                            {
                                return error;
                            }
                        }
                    }
                }
                error = await hittingSetRepository.CompleteUpdateIsomorphicAsync(isBipart);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return error;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> DefineTaskTypeAsync(string algorithmName, int dimension, int numberOfSet, long maxCount)
        {
            string error = string.Empty;
            try
            {
                HittingSetPerfomanceFilter hittingSetPerfomanceFilterDto = new HittingSetPerfomanceFilter
                {
                    Algorithm = algorithmName,
                    Dimension = dimension,
                    NumberOfSet = numberOfSet,
                    MaxCount = maxCount
                };
                await hittingSetRepository.ClearTaskTypeAsync(hittingSetPerfomanceFilterDto);
                List<HittingSetInputDao> items = await hittingSetRepository.GetHittingSetInputsAsync(hittingSetPerfomanceFilterDto, "InputDataShort");
                items.ForEach(item =>
                {
                    item.TypeTask = 0;
                });
                for (int i = 0; i < items.Count; i++)
                {
                    HittingSetInputDao itemOut = items[i];
                    MultiGraph graph = new MultiGraph(itemOut.InputData);
//                    BipartiteGraph bipartiteGraph1 = new BipartiteGraph(itemOut.InputData);
                    int taskType = graph.DefineType();
                    items[i].TypeTask = taskType;
                    error = await hittingSetRepository.UpdateTypeTaskAsync(items[i].HittingSetInputId, items[i].TypeTask);
                     if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }
                }
                error = await hittingSetRepository.CompleteUpdateTypeTaskAsync();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return error;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> DefineGreedyComparisonAsync(string algorithmName, int dimension, int numberOfSet, long maxCount)
        {
            string error = string.Empty;
            try
            {
                List<(string Algorithname, int Count)> nameList = await hittingSetRepository.GetGeedyAlgorithmGroupDimensionsAsync(dimension, numberOfSet, maxCount);
                if (nameList.Count < 5)
                {
                    if (maxCount == 0)
                    {
                        List<IHittingSetAlgorithm> hittingSetAlgorithms = new List<IHittingSetAlgorithm>()
                        {
                            new HittingSetBranchAndBoundByValue(dimension),
                            new HittingSetGreedySimple(),
                            new HittingSetGreedyImprove(),
                            new HittingSetGreedyRelation(),
                            new HittingSetGreedyImproveRD()

                        };

                        HittingSetAlgorithmRunner enumeration = new HittingSetAlgorithmRunner(hittingSetAlgorithms, dimension, numberOfSet, 1000, 1, 1);
                        await enumeration.ExecuteAsync();

                    }
                    else
                    {
                        List<IHittingSetAlgorithm> hittingSetAlgorithms = new List<IHittingSetAlgorithm>()
                        {
                            new HittingSetBranchAndBoundByValue(dimension),
                            new HittingSetGreedySimple(),
                            new HittingSetGreedyImprove(),
                            new HittingSetGreedyRelation(),
                            new HittingSetGreedyImproveRD()

                        };
                        HittingSetAlgorithmStepRunner enumerationStep = new HittingSetAlgorithmStepRunner(hittingSetAlgorithms,
    "SkipEnumerationBigInteger", "Without Matrix", dimension, numberOfSet, maxCount, 1000, 1, 1);
                        await enumerationStep.ExecuteAsync();

                    }
                }
                await hittingSetRepository.SetGeedyComparisonAsync(dimension, numberOfSet, maxCount);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return error;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<HittingSetAlgorithmGroup>> GetAlgorithmsAsync()
        {
            return await hittingSetRepository.GetAlgorithmsAsync();
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<HittingSetAlgorithmGroupDimension>> GetHittingSetAlgorithmGroupDimensionsAsync(string order)
        {
            return await hittingSetRepository.GetHittingSetAlgorithmGroupDimensionsAsync(order);
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<HittingSetInputDao>> GetHittingSetInputsAsync(HittingSetPerfomanceFilter hittingSetPerfomanceFilter, string SelectedInputDataSort)
        {
            List<HittingSetInputDao> items = await hittingSetRepository.GetHittingSetInputsAsync(hittingSetPerfomanceFilter, SelectedInputDataSort);
            return items;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task ExecuteAlgorithmAsync(List<(string algorithm,string algorithmDetail)> algorithms, int dimension, int numberOfSet, bool isInputForce)
        {
            int сardinality = dimension;
            int length = numberOfSet;
            List<IHittingSetAlgorithm> hittingSetAlgorithms = GetAlgorithm(algorithms, dimension);

            if (isInputForce)
            {
                HittingSetAlgorithmRunner enumeration = new HittingSetAlgorithmRunner(hittingSetAlgorithms, сardinality, length, 1000, 1, 1);
                await enumeration.ExecuteAsync();
            }
            else
            {
                List<InputDataSlim> inputDataList = await hittingSetRepository.GetInputDataAsync(dimension, numberOfSet, 0);
                if (inputDataList.Count > 0)
                {
                    HittingSetAlgorithmDbRunner enumeration = new HittingSetAlgorithmDbRunner(hittingSetAlgorithms, inputDataList, сardinality, length, 0, 1000, 1, 1);
                    await enumeration.ExecuteAsync();

                }
                else
                {
                    HittingSetAlgorithmRunner enumeration = new HittingSetAlgorithmRunner(hittingSetAlgorithms, сardinality, length, 1000, 1, 1);
                    await enumeration.ExecuteAsync();
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task ExecuteAlgorithmStepAsync(List<(string algorithm, string algorithmDetail)> algorithms, string CalculationStep, string CombinationType,
            int dimension, int numberOfSet, long maxCount, bool isInputForce)
        {
            int сardinality = dimension;
            int length = numberOfSet;
            List<IHittingSetAlgorithm> hittingSetAlgorithms = GetAlgorithm(algorithms, dimension);

            if (isInputForce)
            {
                HittingSetAlgorithmStepRunner enumerationStep = new HittingSetAlgorithmStepRunner(hittingSetAlgorithms,
                    CalculationStep, CombinationType, сardinality, length, maxCount, 1000, 1, 1);
                await enumerationStep.ExecuteAsync();
            }
            else
            {
                List<InputDataSlim> inputDataList = await hittingSetRepository.GetInputDataAsync(dimension, numberOfSet, maxCount);
                if (inputDataList.Count > 0)
                {
                    HittingSetAlgorithmDbRunner enumeration = new HittingSetAlgorithmDbRunner(hittingSetAlgorithms, inputDataList, сardinality, length, maxCount, 1000, 1, 1);
                    await enumeration.ExecuteAsync();
                }
                else
                {
                    HittingSetAlgorithmStepRunner enumerationStep = new HittingSetAlgorithmStepRunner(hittingSetAlgorithms,
                        CalculationStep, CombinationType, сardinality, length, maxCount, 1000, 1, 1);
                    await enumerationStep.ExecuteAsync();
                }
            }

            
        }
        //----------------------------------------------------------------------------------------------------------------------
        private List<IHittingSetAlgorithm> GetAlgorithm(List<(string algorithm, string algorithmDetail)> algorithms, int dimension)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            List<IHittingSetAlgorithm> hittingSetAlgorithm = algorithms.Where(a => a.algorithm != "AllGreedy").Select( a =>
            {
                IHittingSetAlgorithm? ac = a switch
                {
                    { algorithm: "BruteForceHittingSetBinaryNumbers", algorithmDetail: _ } => new BruteForceHittingSetBinaryNumbers(),
                    { algorithm: "BruteForceHittingSetBinaryNumbersVer2", algorithmDetail: _ } => new BruteForceHittingSetBinaryNumbersVer2() ,
                    { algorithm: "BruteForceHittingSetAsTree", algorithmDetail: _ } => new BruteForceHittingSetAsTree(dimension),
                    { algorithm: "BruteForceHittingSetAsTreeDirect", algorithmDetail: _ } => new BruteForceHittingSetAsTreeDirect(dimension),
                    { algorithm: "HittingSetBranchAndBound", algorithmDetail: _ } => new HittingSetBranchAndBound(dimension),
                    { algorithm: "HittingSetBranchAndBoundByValue", algorithmDetail: _ } => new HittingSetBranchAndBoundByValue(dimension),
                    { algorithm: "HittingSetBranchAndBoundFirst", algorithmDetail: _ } => new HittingSetBranchAndBoundFirst(dimension),
                    { algorithm: "HittingSetGreedySimple", algorithmDetail: _ } => new HittingSetGreedySimple(),
                    { algorithm: "HittingSetGreedyImprove", algorithmDetail: _ } => new HittingSetGreedyImprove(),
                    { algorithm: "HittingSetGreedyRelation", algorithmDetail: _ } => new HittingSetGreedyRelation(),
                    { algorithm: "HittingSetGreedyImproveRD", algorithmDetail: _ } => new HittingSetGreedyImproveRD(),
                    { algorithm: "HittingSetTriangleBranchAndBound", algorithmDetail: _ } => new HittingSetTriangleBranchAndBound(dimension),
                    { algorithm: "HittingSetTriangle", algorithmDetail: _ } => new HittingSetTriangle(dimension),
                    { algorithm: "Empty", algorithmDetail: _ } => new EmptyHSAlgorithm(dimension),
                    { algorithm: "HittingSetTriangleStrategy", algorithmDetail: "SelectElementSimpleStrategy" } => new HittingSetTriangleStrategy(dimension, new SelectElementSimpleStrategy()),
                    { algorithm: "HittingSetTriangleStrategy", algorithmDetail: "SelectElementRelationStrategy" } => new HittingSetTriangleStrategy(dimension, new SelectElementRelationStrategy()),
                    { algorithm: "HittingSetTriangleStrategy", algorithmDetail: "SelectElementImproveStrategy" } => new HittingSetTriangleStrategy(dimension, new SelectElementImproveStrategy()),
                    { algorithm: "HittingSetTriangleStrategy", algorithmDetail: "SelectElementImproveRDStrategy" } => new HittingSetTriangleStrategy(dimension, new SelectElementImproveRDStrategy()),
                    _ => null
                };
                return ac;
            }).ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            if (algorithms.Any(a => a.algorithm == "AllGreedy"))
            {
                hittingSetAlgorithm.Add(new HittingSetGreedySimple());
                hittingSetAlgorithm.Add(new HittingSetGreedyImprove());
                hittingSetAlgorithm.Add(new HittingSetGreedyRelation());
                hittingSetAlgorithm.Add(new HittingSetGreedyImproveRD());
            }
            return hittingSetAlgorithm;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
