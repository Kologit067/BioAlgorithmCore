
namespace BaseContract.Interfaces
{
    public interface IHittingSetAlgorithm
    {
        string AlgorithmName { get; }
        IHittingSetStatisticAccumulator StatisticAccumulator { get; set; }
        void Execute(int[][] pListOfSet);
        int CurrentMinimum { get; }
        List<string> OptimalSets { get; }
        string SolutionAsString { get; }

    }
}
