
using BioAlgorithmViewModel.Common;

namespace BioAlgorithm.Core.HittingSet
{
    public class AlgorithmDetailViewModel : ViewModelBase
    {
        
        private string algorithm;
        public string Algorithm
        {
            get
            {
                return algorithm;
            }
            set
            {
                algorithm = value;
                OnPropertyChanged(nameof(Algorithm));
            }
        }
        private string algorithmDetail;
        public string AlgorithmDetail
        {
            get
            {
                return algorithmDetail;
            }
            set
            {
                algorithmDetail = value;
                OnPropertyChanged(nameof(AlgorithmDetail));
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public bool AlgorithmDetailReadOnly
        {
            get
            {
                if (Algorithm == "HittingSetTriangleStrategy")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
                        
        public override string ToString()
        {
            return $"{Algorithm} / {AlgorithmDetail}";
        }
    }
}
