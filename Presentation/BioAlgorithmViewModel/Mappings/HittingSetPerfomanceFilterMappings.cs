using BioAlgorithmViewModel.HittingSet;
using HittingSet.Data.Contract;

namespace BioAlgorithmViewModel.Mappings
{
    public static class HittingSetPerfomanceFilterMappings
    {
        public static HittingSetPerfomanceFilter? Map(this HittingSetFilterViewModel input)
        {
            return input == null
            ? null
                : new HittingSetPerfomanceFilter()
                {
                    Top = input.Top,
                    Algorithm = input.Algorithm,
                    NumberOfSet = input.NumberOfSet,
                    Dimension = input.Dimension,
                    Step = input.Step,
                    MaxCount = input.MaxCount,
                    InputLen = input.InputLen,
                    InputLenSort = input.InputLenSort,
                    NumberOfIterationFrom = input.NumberOfIterationFrom,
                    NumberOfIterationTo = input.NumberOfIterationTo,
                    DurationFrom = input.DurationFrom,
                    DurationTo = input.DurationTo,
                    IsComplete = input.IsComplete,
                    CountTerminalFrom = input.CountTerminalFrom,
                    CountTerminalTo = input.CountTerminalTo,
                    BestValue = input.BestValue,
                    TaskTypeFilterType = input.TaskTypeFilterType,
                    GreedyComparisonFilterType = input.GreedyComparisonFilterType,
                    PairComparison = input.PairComparison,
                    SelectedGreedy = input.SelectedGreedy,
                };
        }

        public static IEnumerable<HittingSetPerfomanceFilter> Map(this IEnumerable<HittingSetFilterViewModel> input) => input?.Select(i => i.Map()).ToList();

        public static HittingSetFilterViewModel? ToEntity(this HittingSetPerfomanceFilter input)
        {
            return input == null
                ? null
                : new HittingSetFilterViewModel()
                {
                    Top = input.Top,
                    Algorithm = input.Algorithm,
                    NumberOfSet = input.NumberOfSet,
                    Dimension = input.Dimension,
                    Step = input.Step,
                    MaxCount = input.MaxCount,
                    InputLen = input.InputLen,
                    InputLenSort = input.InputLenSort,
                    NumberOfIterationFrom = input.NumberOfIterationFrom,
                    NumberOfIterationTo = input.NumberOfIterationTo,
                    DurationFrom = input.DurationFrom,
                    DurationTo = input.DurationTo,
                    IsComplete = input.IsComplete,
                    CountTerminalFrom = input.CountTerminalFrom,
                    CountTerminalTo = input.CountTerminalTo,
                    BestValue = input.BestValue,
                    TaskTypeFilterType = input.TaskTypeFilterType,
                    GreedyComparisonFilterType = input.GreedyComparisonFilterType,
                    PairComparison = input.PairComparison,
                    SelectedGreedy = input.SelectedGreedy
                };
        }
        public static IEnumerable<HittingSetFilterViewModel> ToEntity(this IEnumerable<HittingSetPerfomanceFilter> input) => input?.Select(i => i.ToEntity()).ToList();

    }

}
