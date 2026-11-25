using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithmViewModel.HittingSet.Utility;
using BioAlgorithm.Data.HittingSet.Data;
using HittingSet.Data.Contract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BioAlgorithmViewModel.HittingSet
{
    public class HittingSetPerformanceViewModel : HittingSetBaseViewModel
    {
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetPerfomance> hittingSetPerformanceList;
        public ObservableCollection<HittingSetPerfomance> HittingSetPerformanceList
        {
            get
            {
                return hittingSetPerformanceList;
            }
            set
            {
                hittingSetPerformanceList = value;
                OnPropertyChanged(nameof(HittingSetPerformanceList));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetPerfomance selectedHittingSetItem;
        public HittingSetPerfomance SelectedHittingSetItem
        {
            get
            {
                return selectedHittingSetItem;
            }
            set
            {
                selectedHittingSetItem = value;
                OnPropertyChanged(nameof(SelectedHittingSetItem));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedHittingSetPerformanceSort;
        public string SelectedHittingSetPerformanceSort
        {
            get
            {
                return selectedHittingSetPerformanceSort;
            }
            set
            {
                selectedHittingSetPerformanceSort = value;
                OnPropertyChanged(nameof(SelectedHittingSetPerformanceSort));
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        private List<string> hittingSetPerformanceSortItems;
        public List<string> HittingSetPerformanceSortItems
        {
            get
            {
                return hittingSetPerformanceSortItems;
            }
            set
            {
                hittingSetPerformanceSortItems = value;
                OnPropertyChanged(nameof(HittingSetPerformanceSortItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetFilterViewModel hittingSetPerfomanceFilter;
        public HittingSetFilterViewModel HittingSetPerfomanceFilter
        {
            get
            {
                return hittingSetPerfomanceFilter;
            }
            set
            {
                hittingSetPerfomanceFilter = value;
                OnPropertyChanged(nameof(HittingSetPerfomanceFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetPerformanceViewModel(HittingSetRepository hittingSetRepository) : base(hittingSetRepository)
        {
            HittingSetPerformanceList = new ObservableCollection<HittingSetPerfomance>();
            HittingSetPerformanceSortItems = new List<string>()
            {
                "Algorithm, Dimension, NumberOfSet, Step, InputDataShort",
                "Algorithm, Dimension, NumberOfSet, Step, HittingSetPerfomanceId",
                "Algorithm, NumberOfSet, Dimension, Step, InputDataShort",
                "Algorithm, NumberOfSet, Dimension, Step, HittingSetPerfomanceId",
                "Dimension, NumberOfSet, Algorithm, Step, InputDataShort",
                "Dimension, NumberOfSet, Algorithm, Step, HittingSetPerfomanceId",
                "NumberOfSet, Dimension, Algorithm, Step, InputDataShort",
                "NumberOfSet, Dimension, Algorithm, Step, HittingSetPerfomanceId"
            };
            SelectedHittingSetPerformanceSort = HittingSetPerformanceSortItems[0];
//            this.hittingSetRepository = hittingSetRepository;
            HittingSetPerfomanceFilter = new HittingSetFilterViewModel();
            Messenger.Default.Register<AlgorithmToFilterMessage>(this, OnAlgorithmToFilterMessageReceived, typeof(AlgorithmToFilterMessage));
            Messenger.Default.Register<AlgorithmGroupToFilterMessage>(this, OnAlgorithmGroupToFilterMessageReceived, typeof(AlgorithmGroupToFilterMessage));
            HittingSetPerfomanceFilter.Top = 1000;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetPerformanceViewModel(HittingSetRepository hittingSetRepository, AlgorithmGroupOpenWindowMessage message)
            : this(hittingSetRepository)
        {

            hittingSetPerfomanceFilter = new HittingSetFilterViewModel();
            hittingSetPerfomanceFilter.Algorithm = message.Algorithm;
            hittingSetPerfomanceFilter.Dimension = message.Dimension;
            hittingSetPerfomanceFilter.NumberOfSet = message.NumberOfSet;
            hittingSetPerfomanceFilter.Step = message.Step;
            hittingSetPerfomanceFilter.MaxCount = message.MaxCount;
            hittingSetPerfomanceFilter.Top = 1000;
        }
        private bool refreshHittingSetAlgorithmGroupEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshHittingSetPerformanceListCommand;
        public ICommand RefreshHittingSetPerformanceListCommand
        {
            get
            {
                if (refreshHittingSetPerformanceListCommand == null)
                {
                    refreshHittingSetPerformanceListCommand = new DelegateCommand(RefreshHittingSetPerformanceListAction, CanRefreshHittingSetPerformanceListAction);
                }
                return refreshHittingSetPerformanceListCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshHittingSetPerformanceListAction()
        {
            ExecutionState = "Query running...";
            refreshHittingSetAlgorithmGroupEnable = false;

            HittingSetPerformanceList.Clear();
            HittingSetPerfomanceFilter? hittingSetPerfomanceFilterDto = HittingSetPerfomanceFilter.Map();
            List<HittingSetPerfomance> items = await hittingSetRepository.GetHittingSetPerformanceListAsync(hittingSetPerfomanceFilterDto, SelectedHittingSetPerformanceSort);
            foreach (HittingSetPerfomance item in items)
                HittingSetPerformanceList.Add(item);
            TotalNumberRows = items.Count;

            ExecutionState = "Query completed.";
            refreshHittingSetAlgorithmGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshHittingSetPerformanceListAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        ////----------------------------------------------------------------------------------------------------------------------
        //private ICommand makeGraphCommand;
        //public ICommand MakeGraphCommand
        //{
        //    get
        //    {
        //        if (makeGraphCommand == null)
        //        {
        //            makeGraphCommand = new DelegateCommand(MakeGraphAction, CanMakeGraphAction);
        //        }
        //        return makeGraphCommand;
        //    }
        //}
        ////----------------------------------------------------------------------------------------------------------------------
        //private void MakeGraphAction()
        //{
        //    Messenger.Default.Send<HittingSetTabChangeMessage>(new HittingSetTabChangeMessage()
        //    {
        //        HittingSetTabName = "BipartiteGraph"
        //    }, typeof(HittingSetTabChangeMessage));
        //    Messenger.Default.Send<InputDataToGraphMessage>(new InputDataToGraphMessage()
        //    {
        //        InputData = SelectedHittingSetItem.InputData
        //    }, typeof(InputDataToGraphMessage));
        //}
        ////----------------------------------------------------------------------------------------------------------------------
        //private bool CanMakeGraphAction()
        //{
        //    return true;
        //}
        protected override string GetGraphData()
        {
            return SelectedHittingSetItem.InputData;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OnAlgorithmToFilterMessageReceived(AlgorithmToFilterMessage algorithmToFilterMessage)
        {
            HittingSetPerfomanceFilter.Algorithm = algorithmToFilterMessage.Algorithm;
            HittingSetPerfomanceFilter.Dimension = null;
            HittingSetPerfomanceFilter.NumberOfSet = null;
            HittingSetPerfomanceFilter.Step = null;
            HittingSetPerfomanceFilter.MaxCount = null;
            RefreshHittingSetPerformanceListAction();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OnAlgorithmGroupToFilterMessageReceived(AlgorithmGroupToFilterMessage algorithmGroupToFilterMessage)
        {
            HittingSetPerfomanceFilter.Algorithm = algorithmGroupToFilterMessage.Algorithm;
            HittingSetPerfomanceFilter.Dimension = algorithmGroupToFilterMessage.Dimension;
            HittingSetPerfomanceFilter.NumberOfSet = algorithmGroupToFilterMessage.NumberOfSet;
            HittingSetPerfomanceFilter.Step = algorithmGroupToFilterMessage.Step;
            HittingSetPerfomanceFilter.MaxCount = algorithmGroupToFilterMessage.MaxCount;
            RefreshHittingSetPerformanceListAction();
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
