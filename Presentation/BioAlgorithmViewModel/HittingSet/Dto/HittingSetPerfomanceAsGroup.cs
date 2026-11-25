using BioAlgorithmViewModel.HittingSet.Dto;

namespace BioAlgorithm.HittingSetModel
{
    public class HittingSetPerfomanceAsGroup
    {
        public string ColumnGroupName { get; set; }
        public string ColumnGroupValue { get; set; }
        public List<HittingSetPerfomanceDto> HittingSetPerfomanceList { get; set; }
    }
}
