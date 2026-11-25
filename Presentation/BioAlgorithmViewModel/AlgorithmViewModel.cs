using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.HittingSet;

namespace BioAlgorithmViewModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class AlgorithmViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class AlgorithmViewModel : ViewModelBase
    {
        private HittingSetViewModel hittingSets;
        public HittingSetViewModel HittingSets
        {
            get
            {
                return hittingSets;
            }
            set
            {
                hittingSets = value;
                OnPropertyChanged(nameof(HittingSets));
            }
        }
        public AlgorithmViewModel()
        {
            HittingSets = new HittingSetViewModel();
        }

    }
}
