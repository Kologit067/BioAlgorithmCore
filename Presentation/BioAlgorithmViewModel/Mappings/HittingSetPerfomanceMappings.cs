using BioAlgorithmViewModel.HittingSet.Dto;
using HittingSet.Data.Contract;

namespace BioAlgorithmViewModel.Mappings
{
   
    public static class HittingSetPerfomanceMappings
    {
        public static HittingSetPerfomance? ToEntity(this HittingSetPerfomanceDto input)
        {
            return input == null
            ? null
                : new HittingSetPerfomance()
                {
                    Algorithm = input.Algorithm,
                    NumberOfSet = input.NumberOfSet,
                    Dimension = input.Dimension,
                    Step = input.Step,
                    MaxCount = input.MaxCount,
                    InputLen = input.InputLen,
                    InputLenSort = input.InputLenSort,
                    InputLenAvg = int.Parse(input.InputLenAvg),
                    InputData = input.InputData,
                    InputDataShort = input.InputDataShort,
                    NumberOfIteration = input.NumberOfIteration,
                    Duration = input.Duration,
                    DurationMilliSeconds = input.DurationMilliSeconds,
                    DateComplete = input.DateComplete,
                    IsComplete = input.IsComplete,
                    LastRoute = input.LastRoute,
                    OptimalRoute = input.OptimalRoute,
                    CountTerminal = int.Parse(input.CountTerminal),
                    BestValue = int.Parse(input.BestValue),
                    UpdateOptcount = int.Parse(input.UpdateOptcount),
                    ElemenationCount = int.Parse(input.BestValue),
                    Isomorphic = input.Isomorphic
                };
        }

        public static IEnumerable<HittingSetPerfomance> ToEntity(this IEnumerable<HittingSetPerfomanceDto> input) => input?.Select(i => i.ToEntity()).ToList();

        public static HittingSetPerfomanceDto? Map(this HittingSetPerfomance input)
        {
            return input == null
                ? null
                : new HittingSetPerfomanceDto()
                {
                    Algorithm = input.Algorithm,
                    NumberOfSet = input.NumberOfSet,
                    Dimension = input.Dimension,
                    Step = input.Step,
                    MaxCount = input.MaxCount,
                    InputLen = input.InputLen,
                    InputLenSort = input.InputLenSort,
                    InputLenAvg = input.InputLenAvg.ToString(),
                    InputData = input.InputData,
                    InputDataShort = input.InputDataShort,
                    NumberOfIteration = input.NumberOfIteration,
                    Duration = input.Duration,
                    DurationMilliSeconds = input.DurationMilliSeconds,
                    DateComplete = input.DateComplete,
                    IsComplete = input.IsComplete,
                    LastRoute = input.LastRoute,
                    OptimalRoute = input.OptimalRoute,
                    CountTerminal = input.CountTerminal.ToString(),
                    BestValue = input.BestValue.ToString(),
                    UpdateOptcount = input.UpdateOptcount.ToString(),
                    ElemenationCount = input.BestValue.ToString(),
                    Isomorphic = input.Isomorphic
                };
        }
        public static IEnumerable<HittingSetPerfomanceDto> Map(this IEnumerable<HittingSetPerfomance> input) => input?.Select(i => i.Map()).ToList();

    }

}
