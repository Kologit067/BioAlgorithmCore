using BioAlgorithmViewModel.BipartiteGraphModel;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithmViewModel.HittingSet.Utility;
using BioAlgorithm.Data.HittingSet.Data;

namespace BioAlgorithmViewModel.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetViewModel : ViewModelBase
    {
        private HittingSetRepository hittingSetRepository;
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetPerformanceViewModel hittingSetPerformance;
        public HittingSetPerformanceViewModel HittingSetPerformance
        {
            get
            {
                return hittingSetPerformance;
            }
            set
            {
                hittingSetPerformance = value;
                OnPropertyChanged(nameof(HittingSetPerformance));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetPerformanceAlgorithmViewModel hittingSetPerformanceAlgorithm;
        public HittingSetPerformanceAlgorithmViewModel HittingSetPerformanceAlgorithm
        {
            get
            {
                return hittingSetPerformanceAlgorithm;
            }
            set
            {
                hittingSetPerformanceAlgorithm = value;
                OnPropertyChanged(nameof(HittingSetPerformanceAlgorithm));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetPerformanceGroupViewModel hittingSetPerformanceGroup;
        public HittingSetPerformanceGroupViewModel HittingSetPerformanceGroup
        {
            get
            {
                return hittingSetPerformanceGroup;
            }
            set
            {
                hittingSetPerformanceGroup = value;
                OnPropertyChanged(nameof(HittingSetPerformanceGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetInputGroupViewModel hittingSetInputGroup;
        public HittingSetInputGroupViewModel HittingSetInputGroup
        {
            get
            {
                return hittingSetInputGroup;
            }
            set
            {
                hittingSetInputGroup = value;
                OnPropertyChanged(nameof(HittingSetInputGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetPerformanceAlgorithmCompareViewModel hittingSetPerformanceAlgorithmCompare;
        public HittingSetPerformanceAlgorithmCompareViewModel HittingSetPerformanceAlgorithmCompare
        {
            get
            {
                return hittingSetPerformanceAlgorithmCompare;
            }
            set
            {
                hittingSetPerformanceAlgorithmCompare = value;
                OnPropertyChanged(nameof(HittingSetPerformanceAlgorithmCompare));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetPerformanceAlgorithmWithGroupViewModel hittingSetPerformanceAlgorithmWithGroup;
        public HittingSetPerformanceAlgorithmWithGroupViewModel HittingSetPerformanceAlgorithmWithGroup
        {
            get
            {
                return hittingSetPerformanceAlgorithmWithGroup;
            }
            set
            {
                hittingSetPerformanceAlgorithmWithGroup = value;
                OnPropertyChanged(nameof(HittingSetPerformanceAlgorithmWithGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetInputDataViewModel hittingSetInput;
        public HittingSetInputDataViewModel HittingSetInput
        {
            get
            {
                return hittingSetInput;
            }
            set
            {
                hittingSetInput = value;
                OnPropertyChanged(nameof(HittingSetInput));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetInputGreedyComparisonViewModel hittingSetInputGreedyComparison;
        public HittingSetInputGreedyComparisonViewModel HittingSetInputGreedyComparison
        {
            get
            {
                return hittingSetInputGreedyComparison;
            }
            set
            {
                hittingSetInputGreedyComparison = value;
                OnPropertyChanged(nameof(HittingSetInputGreedyComparison));
            }
        }
        
        //----------------------------------------------------------------------------------------------------------------------
        private BipartiteGraphViewModel bipartiteGraph;
        public BipartiteGraphViewModel BipartiteGraph
        {
            get
            {
                return bipartiteGraph;
            }
            set
            {
                bipartiteGraph = value;
                OnPropertyChanged(nameof(BipartiteGraph));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private int selectedTab;
        public int SelectedTab
        {
            get
            {
                return selectedTab;
            }
            set
            {
                selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetViewModel()
        {
            hittingSetRepository = new HittingSetRepository();
            HittingSetPerformance = new HittingSetPerformanceViewModel(hittingSetRepository);
            HittingSetPerformanceAlgorithm = new HittingSetPerformanceAlgorithmViewModel(hittingSetRepository);
            HittingSetPerformanceGroup = new HittingSetPerformanceGroupViewModel(hittingSetRepository);
            HittingSetInputGroup = new HittingSetInputGroupViewModel(hittingSetRepository);
            HittingSetPerformanceAlgorithmCompare = new HittingSetPerformanceAlgorithmCompareViewModel(hittingSetRepository);
            HittingSetPerformanceAlgorithmWithGroup = new HittingSetPerformanceAlgorithmWithGroupViewModel(hittingSetRepository);
            HittingSetInput = new HittingSetInputDataViewModel(hittingSetRepository);
            HittingSetInputGreedyComparison = new HittingSetInputGreedyComparisonViewModel(hittingSetRepository);
            BipartiteGraph = new BipartiteGraphViewModel();
            Messenger.Default.Register<HittingSetTabChangeMessage>(this, OnAlgorithmGroupToFilterMessageReceived, typeof(HittingSetTabChangeMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OnAlgorithmGroupToFilterMessageReceived(HittingSetTabChangeMessage message)
        {
            SelectedTab = message.HittingSetTabName switch
            {
                "HittingSetPerformance" => 5,
                "BipartiteGraph" => 8,
                "HittingSetPerformanceAsGroup" => 7,
                "InputData" => 3,
                "InputDataGreedyComparison" => 4,
                _ => 0
            };
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
