using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Helpers;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.HittingSet.Dto;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithmViewModel.HittingSet.Utility;
using BioAlgorithm.Data.HittingSet.Data;
using HittingSet.Data.Contract;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using BioAlgorithm.HittingSetModel;

namespace BioAlgorithmViewModel.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetPerformanceAlgorithmWithGroupViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetPerformanceAlgorithmWithGroupViewModel : ViewModelBase
    {
        private readonly HittingSetRepository hittingSetRepository;
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetPerfomanceAsGroup> hittingSetPerformanceAsGroupList;
        public ObservableCollection<HittingSetPerfomanceAsGroup> HittingSetPerformanceAsGroupList
        {
            get
            {
                return hittingSetPerformanceAsGroupList;
            }
            set
            {
                hittingSetPerformanceAsGroupList = value;
                OnPropertyChanged(nameof(HittingSetPerformanceAsGroupList));
            }
        }
        private HittingSetPerfomanceAsGroup selectedHittingSetPerformanceAsGroup;
        public HittingSetPerfomanceAsGroup SelectedHittingSetPerformanceAsGroup
        {
            get
            {
                return selectedHittingSetPerformanceAsGroup;
            }
            set
            {
                selectedHittingSetPerformanceAsGroup = value;
                OnPropertyChanged(nameof(SelectedHittingSetPerformanceAsGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<string> groupColumnItems;
        public ObservableCollection<string> GroupColumnItems
        {
            get
            {
                return groupColumnItems;
            }
            set
            {
                groupColumnItems = value;
                OnPropertyChanged(nameof(GroupColumnItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedGroupColumn;
        public string SelectedGroupColumn
        {
            get
            {
                return selectedGroupColumn;
            }
            set
            {
                selectedGroupColumn = value;
                OnPropertyChanged(nameof(SelectedGroupColumn));
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
        public HittingSetPerformanceAlgorithmWithGroupViewModel(HittingSetRepository hittingSetRepository)
        {
            GroupColumnItems =
            [
                "InputLenSort",
                "InputDataShort",
                "OptimalRoute",
                "BestValue"
            ];
            HittingSetPerformanceSortItems = new List<string>()
            {
                "InputLenSort, OptimalRoute",
                "InputLenSort, BestValue",
                "OptimalRoute, InputLenSort",
                "OptimalRoute, BestValue",
                "BestValue, OptimalRoute",
                "BestValue, InputLenSort",
            };
            PropertyChanged += HittingSetPerformanceAlgorithmWithGroupViewModel_PropertyChanged;
            SelectedGroupColumn = GroupColumnItems.FirstOrDefault();
            this.hittingSetRepository = hittingSetRepository;
            SelectedHittingSetPerformanceSort = HittingSetPerformanceSortItems[0];
            HittingSetPerfomanceFilter = new HittingSetFilterViewModel();

            Messenger.Default.Register<AlgorithmToFilterByGroupMessage>(this, OnAlgorithmToFilterMessageReceived, typeof(AlgorithmToFilterByGroupMessage));
            Messenger.Default.Register<AlgorithmGroupToFilterByGroupMessage>(this, OnAlgorithmGroupToFilterMessageReceived, typeof(AlgorithmGroupToFilterByGroupMessage));

            hittingSetPerfomanceFilter.Top = 1000;
            HittingSetPerformanceAsGroupList = new ObservableCollection<HittingSetPerfomanceAsGroup>();
        }

        private void OnAlgorithmGroupToFilterMessageReceived(AlgorithmGroupToFilterByGroupMessage algorithmGroupToFilterMessage)
        {
            HittingSetPerfomanceFilter.Algorithm = algorithmGroupToFilterMessage.Algorithm;
            HittingSetPerfomanceFilter.Dimension = algorithmGroupToFilterMessage.Dimension;
            HittingSetPerfomanceFilter.NumberOfSet = algorithmGroupToFilterMessage.NumberOfSet;
            HittingSetPerfomanceFilter.Step = algorithmGroupToFilterMessage.Step;
            HittingSetPerfomanceFilter.MaxCount = algorithmGroupToFilterMessage.MaxCount;
            //RefreshHittingSetPerformanceListAction();
        }

        private void OnAlgorithmToFilterMessageReceived(AlgorithmToFilterByGroupMessage algorithmToFilterMessage)
        {
            HittingSetPerfomanceFilter.Algorithm = algorithmToFilterMessage.Algorithm;
            HittingSetPerfomanceFilter.Dimension = null;
            HittingSetPerfomanceFilter.NumberOfSet = null;
            HittingSetPerfomanceFilter.Step = null;
            HittingSetPerfomanceFilter.MaxCount = null;
            //RefreshHittingSetPerformanceListAction();
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

            HittingSetPerformanceAsGroupList.Clear();
            HittingSetPerfomanceFilter hittingSetPerfomanceFilterDto = HittingSetPerfomanceFilter.Map();
            string order = SelectedHittingSetPerformanceSort.Replace(SelectedGroupColumn, string.Empty);
            order = $"{SelectedGroupColumn},{order}".Replace(",,", ",").TrimEnd(new char[] { ',', ' ' });
            List<HittingSetPerfomance> rp = await hittingSetRepository.GetHittingSetPerformanceListAsync(
                hittingSetPerfomanceFilterDto, order);
            List<HittingSetPerfomanceDto> items = rp.Map().ToList();
            HittingSetPerformanceAsGroupList = items.GroupBy(GetGroupKey(SelectedGroupColumn).Compile()).Select(g => 
            new HittingSetPerfomanceAsGroup() { 
                ColumnGroupName = SelectedGroupColumn, 
                ColumnGroupValue = g.Key, 
                HittingSetPerfomanceList = g.ToList() }).ToObservable();

            ExecutionState = "Query completed.";
            refreshHittingSetAlgorithmGroupEnable = true;

        }
        //----------------------------------------------------------------------------------------------------------------------
        private static Expression<Func<HittingSetPerfomanceDto, string>> GetGroupKey(string property)
        {
            var parameter = Expression.Parameter(typeof(HittingSetPerfomanceDto));
            var body = Expression.Property(parameter, property);
            return Expression.Lambda<Func<HittingSetPerfomanceDto, string>>(body, parameter);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshHittingSetPerformanceListAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void HittingSetPerformanceAlgorithmWithGroupViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
