

namespace BioAlgorithmViewModel.HittingSet.Messages
{
    public class AlgorithmGroupOpenWindowMessage
    {
        public string Algorithm { get; set; }
        public int NumberOfSet { get; set; }
        public int Dimension { get; set; }
        public long Step { get; set; }
        public long MaxCount { get; set; }
    }
}
