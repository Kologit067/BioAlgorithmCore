using BaseContract.Interfaces;
using StatisticsStorage.Accumulators.Objects;
using System.Diagnostics;

namespace StatisticsStorage.Accumulators
{
    //--------------------------------------------------------------------------------------------------------------------
    // class HittingSetStatisticAccumulator
    //--------------------------------------------------------------------------------------------------------------------
    public class HittingSetStatisticAccumulator : IHittingSetStatisticAccumulator 
    {
        protected List<HittingSetPerfomance> _hittingSetPerfomances;
        protected HittingSetPerfomance _currentHittingSetPerfomance;
        protected IHittingSetSaver _hittingSetSaver;
        protected int _bufferSize;
        protected int _numberOfSet;
        protected int _dimension;
        protected decimal _step;
        protected decimal _maxCount;
        Stopwatch stopWatchSave;
        Stopwatch stopWatchCalc;
        //--------------------------------------------------------------------------------------------------------------------
        public HittingSetStatisticAccumulator(IHittingSetSaver hittingSetSaver, int numberOfSet, int dimension, decimal maxCount = 0, decimal step = 1, int bufferSize = 100)
        {
            _numberOfSet = numberOfSet;
            _dimension = dimension;
            _step = step;
            _maxCount = maxCount;
            _hittingSetSaver = hittingSetSaver;
            _bufferSize = bufferSize;
            _hittingSetPerfomances = new List<HittingSetPerfomance>();
            stopWatchSave = new Stopwatch();
            stopWatchCalc = new Stopwatch();
            stopWatchCalc.Start();
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void CreateStatistics(int[][] listOfSet, string inputDataShort, string algorithm)
        {
            _currentHittingSetPerfomance = new HittingSetPerfomance(_numberOfSet, _dimension,_maxCount, _step, listOfSet, inputDataShort, algorithm);
            _hittingSetPerfomances.Add(_currentHittingSetPerfomance);
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void SaveStatisticData(long duration, long durationMilliSeconds, DateTime dateComplete,
            bool isComplete, string lastRoute, List<string> optimalSets, int bestValue)
        {
            _currentHittingSetPerfomance.SaveStatisticData(duration, durationMilliSeconds, dateComplete,
            isComplete, lastRoute, optimalSets, bestValue);
            if (_hittingSetPerfomances.Count > _bufferSize)
            {
                stopWatchCalc.Stop();
                long timeCalc = stopWatchCalc.ElapsedMilliseconds;
                stopWatchSave = new Stopwatch();
                stopWatchSave.Start();
                _hittingSetSaver.Save(_hittingSetPerfomances);
                stopWatchSave.Stop();
                long timeSave = stopWatchSave.ElapsedMilliseconds;
                double rel = timeSave * 1.0 / timeCalc;
                string stat = $"{timeSave} / {timeCalc} = {rel}. timeSave/buffer = {1.0*timeSave/_bufferSize}";
                stopWatchCalc = new Stopwatch();
                stopWatchCalc.Start();

                _hittingSetPerfomances.Clear();
            }
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void IterationCountInc()
        {
            _currentHittingSetPerfomance.IterationCountInc();
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void TerminalCountInc()
        {
            _currentHittingSetPerfomance.TerminalCountInc();
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void UpdateOptcountInc()
        {
            _currentHittingSetPerfomance.UpdateOptcountInc();
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void ElemenationCountInc()
        {
            _currentHittingSetPerfomance.ElemenationCountInc();
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void SaveRemain()
        {
            if (_hittingSetPerfomances.Count > 0)
                _hittingSetSaver.Save(_hittingSetPerfomances);
            _hittingSetPerfomances.Clear();
        }
        //--------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? maxCount = 0)
        {
            return await _hittingSetSaver.DeleteAsync(algorithm, numberOfSet, dimension, maxCount);
        }

        public void RemoveLastStatistic()
        {
            if (_hittingSetPerfomances.Count > 0)
                _hittingSetPerfomances.RemoveAt(_hittingSetPerfomances.Count - 1);
        }
        //--------------------------------------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------------------------------------
    // class FakeHittingSetStatisticAccumulator
    //--------------------------------------------------------------------------------------------------------------------
    public class FakeHittingSetStatisticAccumulator : IHittingSetStatisticAccumulator
    { 
        //--------------------------------------------------------------------------------------------------------------------
        public FakeHittingSetStatisticAccumulator()
        {
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void CreateStatistics(int[][] listOfSet, string inputDataShort, string algorithm)
        {
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void SaveStatisticData(long duration, long durationMilliSeconds, DateTime dateComplete,
            bool isComplete, string lastRoute, List<string> optimalSets, int bestValue)
        {
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void IterationCountInc()
        {
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void TerminalCountInc()
        {
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void UpdateOptcountInc()
        {
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void ElemenationCountInc()
        {
        }
        //--------------------------------------------------------------------------------------------------------------------
        public void SaveRemain()
        {
        }
        //--------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? step = null)
        {
            return await Task<string?>.FromResult("");
        }
        //--------------------------------------------------------------------------------------------------------------------
    }
}
