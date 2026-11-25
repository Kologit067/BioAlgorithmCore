using BaseContract.Interfaces;
using HittingSetTest.Base;
using StatisticsStorage.Accumulators;
using StatisticsStorage.Savers;

namespace BioAlgorithm.Services.HittingSet
{
    //--------------------------------------------------------------------------------------
    // class HittingSetAlgorithmRunner
    //--------------------------------------------------------------------------------------
    public class HittingSetAlgorithmRunner : EnumerateHittingSetTestBase
    {
        protected List<HittingSetStatisticAccumulator> _statisticAccumulators;

        protected readonly List<IHittingSetAlgorithm> hittingSetAlgorithms;
        //--------------------------------------------------------------------------------------
        public HittingSetAlgorithmRunner(List<IHittingSetAlgorithm> hittingSetAlgorithms, int pCardinality, int pLength, int bufferSize, int pMinimumValue = 1, int pForwardAdditive = 1, bool isSave = true)
            : base(pCardinality, pLength, pMinimumValue, pForwardAdditive)
        {
            _isSave = isSave;
            this.hittingSetAlgorithms = hittingSetAlgorithms;
            _statisticAccumulators = new List<HittingSetStatisticAccumulator>();
            if (_isSave)
            {
                foreach (IHittingSetAlgorithm algorithm in hittingSetAlgorithms)
                {
                    IHittingSetSaver saver = null;
                    if (algorithm.AlgorithmName == "Empty")
                    {
                        saver = new HittingSetInputSaver();
                    }
                    else
                    {
                        saver = new HittingSetSaver();
                    }

                    var statisticAccumulator = new HittingSetStatisticAccumulator(saver, pLength, pCardinality, 0, 1, bufferSize);

                    algorithm.StatisticAccumulator = statisticAccumulator;
                    _statisticAccumulators.Add(statisticAccumulator);
                }
            }
        }
        //--------------------------------------------------------------------------------------
        public async Task ExecuteAsync()
        {
            foreach (IHittingSetAlgorithm algorithm in hittingSetAlgorithms)
                await algorithm.StatisticAccumulator.DeleteAsync(algorithm.AlgorithmName, _fSize , _fCardinality, 0);
            Execute();
        }
        //--------------------------------------------------------------------------------------
        protected override void ActAction(int[][] listOfSet)
        {
            foreach (IHittingSetAlgorithm algorithm in hittingSetAlgorithms)
                algorithm.Execute(listOfSet);

        }
        //--------------------------------------------------------------------------------------
        protected override void AssertAction()
        {
//            _statisticAccumulator.RemoveLastStatistic();
        }
        //--------------------------------------------------------------------------------------
        protected override void PostAction()
        {
            foreach (IHittingSetAlgorithm algorithm in hittingSetAlgorithms)
                algorithm.StatisticAccumulator.SaveRemain();
        }
        //--------------------------------------------------------------------------------------
    }

}
