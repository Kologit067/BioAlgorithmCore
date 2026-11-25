using System.Collections.ObjectModel;
using System.Windows.Input;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.HittingSet.Utility;
using BioAlgorithm.Data.Contract.HittingSet.Data.Contract;
using BioAlgorithm.Data.HittingSet.Data;
using HittingSet.Data.Contract;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithm.Services.HittingSet;

namespace BioAlgorithmViewModel.HittingSet
{
    public class HittingSetInputDataViewModel : HittingSetBaseViewModel
    {
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetInputDao> inputDataList;
        public ObservableCollection<HittingSetInputDao> InputDataList
        {
            get
            {
                return inputDataList;
            }
            set
            {
                inputDataList = value;
                OnPropertyChanged(nameof(InputDataList));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetInputDao selectedInputtem;
        public HittingSetInputDao SelectedInputItem
        {
            get
            {
                return selectedInputtem;
            }
            set
            {
                selectedInputtem = value;
                OnPropertyChanged(nameof(SelectedInputItem));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedInputDataSort;
        public string SelectedInputDataSort
        {
            get
            {
                return selectedInputDataSort;
            }
            set
            {
                selectedInputDataSort = value;
                OnPropertyChanged(nameof(SelectedInputDataSort));
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        private List<string> inputDataSortItems;
        public List<string> InputDataSortItems
        {
            get
            {
                return inputDataSortItems;
            }
            set
            {
                inputDataSortItems = value;
                OnPropertyChanged(nameof(InputDataSortItems));
            }
        }
        //---------------------------------------------------------------------------------------------------------------------- 
        public List<string> TaskTypeFilterTypeSource
        {
            get => new List<string>() { "Or", "And", "Exact =", "Or/Not", "And/Not", "Exact =/Not" };
        }
        //---------------------------------------------------------------------------------------------------------------------- 
        public List<string> GreedyComparisonFilterTypeSource
        {
            get => new List<string>() { "Or", "And", "Exact =", "Or/Not", "And/Not", "Exact =/Not" };
        }
        //private string taskTypeFilterType;
        //public string TaskTypeFilterType
        //{
        //    get
        //    {
        //        return taskTypeFilterType;
        //    }
        //    set
        //    {
        //        taskTypeFilterType = value;
        //        OnPropertyChanged(nameof(TaskTypeFilterType));
        //    }
        //}
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter1;
        public bool TaskTypeFilter1
        {
            get
            {
                return taskTypeFilter1;
            }
            set
            {
                taskTypeFilter1 = value;
                OnPropertyChanged(nameof(TaskTypeFilter1));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter2;
        public bool TaskTypeFilter2
        {
            get
            {
                return taskTypeFilter2;
            }
            set
            {
                taskTypeFilter2 = value;
                OnPropertyChanged(nameof(TaskTypeFilter2));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter4;
        public bool TaskTypeFilter4
        {
            get
            {
                return taskTypeFilter4;
            }
            set
            {
                taskTypeFilter4 = value;
                OnPropertyChanged(nameof(TaskTypeFilter4));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter8;
        public bool TaskTypeFilter8
        {
            get
            {
                return taskTypeFilter8;
            }
            set
            {
                taskTypeFilter8 = value;
                OnPropertyChanged(nameof(TaskTypeFilter8));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter16;
        public bool TaskTypeFilter16
        {
            get
            {
                return taskTypeFilter16;
            }
            set
            {
                taskTypeFilter16 = value;
                OnPropertyChanged(nameof(TaskTypeFilter16));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter32;
        public bool TaskTypeFilter32
        {
            get
            {
                return taskTypeFilter32;
            }
            set
            {
                taskTypeFilter32 = value;
                OnPropertyChanged(nameof(TaskTypeFilter32));
            }
        }


        //----------------------------------------------------------------------------------------------------------------------
        private bool greedySimpleFilter;
        public bool GreedySimpleFilter
        {
            get
            {
                return greedySimpleFilter;
            }
            set
            {
                greedySimpleFilter = value;
                OnPropertyChanged(nameof(GreedySimpleFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool greedyRelationFilter;
        public bool GreedyRelationFilter
        {
            get
            {
                return greedyRelationFilter;
            }
            set
            {
                greedyRelationFilter = value;
                OnPropertyChanged(nameof(GreedyRelationFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool greedyImproveFilter;
        public bool GreedyImproveFilter
        {
            get
            {
                return greedyImproveFilter;
            }
            set
            {
                greedyImproveFilter = value;
                OnPropertyChanged(nameof(GreedyImproveFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool greedyImproveRDFilter;
        public bool GreedyImproveRDFilter
        {
            get
            {
                return greedyImproveRDFilter;
            }
            set
            {
                greedyImproveRDFilter = value;
                OnPropertyChanged(nameof(GreedyImproveRDFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetInputDataViewModel(HittingSetRepository hittingSetRepository) : base(hittingSetRepository)
        {
            InputDataList = new ObservableCollection<HittingSetInputDao>();
            InputDataSortItems = new List<string>()
            {
                "Dimension, NumberOfSet, Step, InputDataShort",
                "Dimension, NumberOfSet, Step, HittingSetInputId",
                "NumberOfSet, Dimension, Step, InputDataShort",
                "NumberOfSet, Dimension, Step, HittingSetInputId"
            };
            SelectedInputDataSort = InputDataSortItems[0];
            HittingSetFilter.TaskTypeFilterType = TaskTypeFilterTypeSource[0];
            HittingSetFilter.GreedyComparisonFilterType = GreedyComparisonFilterTypeSource[0];
            Messenger.Default.Register<AlgorithmGroupToFilterInputDataMessage>(this, OnAlgorithmGroupToFilterInputDataMessageReceived, typeof(AlgorithmGroupToFilterInputDataMessage));
        }
        private bool refreshHittingSetAlgorithmGroupEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshHittingSetInputListCommand;
        public ICommand RefreshHittingSetInputListCommand
        {
            get
            {
                if (refreshHittingSetInputListCommand == null)
                {
                    refreshHittingSetInputListCommand = new DelegateCommand(RefreshHittingSetInputListAction, CanRefreshHittingSetInputListAction);
                }
                return refreshHittingSetInputListCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshHittingSetInputListAction()
        {
            ExecutionState = "Query running...";
            refreshHittingSetAlgorithmGroupEnable = false;

            InputDataList.Clear();
            int taskTypeFilter = 0;
            if (TaskTypeFilter1)
                taskTypeFilter = taskTypeFilter | 1;
            if (TaskTypeFilter2)
                taskTypeFilter = taskTypeFilter | 2;
            if (TaskTypeFilter4)
                taskTypeFilter = taskTypeFilter | 4;
            if (TaskTypeFilter8)
                taskTypeFilter = taskTypeFilter | 8;
            if (TaskTypeFilter16)
                taskTypeFilter = taskTypeFilter | 16;
            if (TaskTypeFilter32)
                taskTypeFilter = taskTypeFilter | 32;

            int greedyComparisonFilter = 0;
            if (greedyImproveRDFilter)
                greedyComparisonFilter = greedyComparisonFilter | 1;
            if (greedyImproveFilter)
                greedyComparisonFilter = greedyComparisonFilter | 2;
            if (GreedyRelationFilter)
                greedyComparisonFilter = greedyComparisonFilter | 4;
            if (GreedySimpleFilter)
                greedyComparisonFilter = greedyComparisonFilter | 8;

            HittingSetPerfomanceFilter? hittingSetPerfomanceFilterDto = HittingSetFilter.Map();
            if (hittingSetPerfomanceFilterDto != null)
            {
                hittingSetPerfomanceFilterDto.TaskTypeFilter = taskTypeFilter;
                hittingSetPerfomanceFilterDto.GreedyComparisonFilter = greedyComparisonFilter;
            }
            List<HittingSetInputDao> items = await hittingSetRepository.GetHittingSetInputsAsync(hittingSetPerfomanceFilterDto, SelectedInputDataSort);
            foreach (HittingSetInputDao item in items)
                InputDataList.Add(item);
            TotalNumberRows = items.Count;

            ExecutionState = "Query completed.";
            refreshHittingSetAlgorithmGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshHittingSetInputListAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool testIsomorphismEnable = true;
        private ICommand testIsomorphismCommand;
        public ICommand TestIsomorphismCommand
        {
            get
            {
                if (testIsomorphismCommand == null)
                {
                    testIsomorphismCommand = new DelegateCommand(TestIsomorphismAction, CanTestIsomorphismAction);
                }
                return testIsomorphismCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void TestIsomorphismAction()
        {
            ExecutionState = "Test calculation running...";
            testIsomorphismEnable = false;

            HittingSetService hittingSetService = new HittingSetService(hittingSetRepository);
            string error = await hittingSetService.TestIsomorphismAsync(string.Empty,
                SelectedInputItem.Dimension,
                SelectedInputItem.NumberOfSet,
                SelectedInputItem.MaxCount, false);
            ExecutionState = !string.IsNullOrEmpty(error) ? $"Test calculation failed: {error}" : "Test calculation completed";
            testIsomorphismEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanTestIsomorphismAction()
        {
            return testIsomorphismEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand testIsomorphismBipartCommand;
        public ICommand TestIsomorphismBipartCommand
        {
            get
            {
                if (testIsomorphismBipartCommand == null)
                {
                    testIsomorphismBipartCommand = new DelegateCommand(TestIsomorphismBipartAction, CanTestIsomorphismBipartAction);
                }
                return testIsomorphismBipartCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void TestIsomorphismBipartAction()
        {
            ExecutionState = "Test calculation running...";
            testIsomorphismEnable = false;

            HittingSetService hittingSetService = new HittingSetService(hittingSetRepository);
            string error = await hittingSetService.TestIsomorphismAsync(string.Empty,
                SelectedInputItem.Dimension,
                SelectedInputItem.NumberOfSet,
                SelectedInputItem.MaxCount, true);
            ExecutionState = !string.IsNullOrEmpty(error) ? $"Test calculation failed: {error}" : "Test calculation completed";
            testIsomorphismEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanTestIsomorphismBipartAction()
        {
            return testIsomorphismEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OnAlgorithmGroupToFilterInputDataMessageReceived(AlgorithmGroupToFilterInputDataMessage algorithmGroupToFilterMessage)
        {
            HittingSetFilter.Dimension = algorithmGroupToFilterMessage.Dimension;
            HittingSetFilter.NumberOfSet = algorithmGroupToFilterMessage.NumberOfSet;
            HittingSetFilter.MaxCount = algorithmGroupToFilterMessage.MaxCount;
            RefreshHittingSetInputListAction();
        }
        protected override void FillAlgorithmGroupToFilterMessage(AlgorithmGroupToFilterMessage message)
        {
            message.Dimension = SelectedInputItem.Dimension;
            message.NumberOfSet = SelectedInputItem.NumberOfSet;
            message.Step = SelectedInputItem.Step;
            message.MaxCount = SelectedInputItem.MaxCount;
        }
        protected override string GetGraphData()
        {
            return SelectedInputItem.InputData;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runTaskCommand;
        public ICommand RunTaskCommand
        {
            get
            {
                if (runTaskCommand == null)
                {
                    runTaskCommand = new DelegateCommand(RunTaskAction, CanRunTaskAction);
                }
                return runTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunTaskAction()
        {
            Messenger.Default.Send<StartTaskMessage>(new StartTaskMessage()
            {
                Algorithm = null,
                Dimension = null,
                NumberOfSet = null,
                MaxCount = null
            }, typeof(StartTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunTaskAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runTaskStepCommand;
        public ICommand RunTaskStepCommand
        {
            get
            {
                if (runTaskStepCommand == null)
                {
                    runTaskStepCommand = new DelegateCommand(RunTaskStepAction, CanRunTaskStepAction);
                }
                return runTaskStepCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunTaskStepAction()
        {
            Messenger.Default.Send<StartTaskStepMessage>(new StartTaskStepMessage()
            {
                Algorithm = null,
                Dimension = null,
                NumberOfSet = null,
                MaxCount = null
            }, typeof(StartTaskStepMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunTaskStepAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runIsomorphismTaskCommand;
        public ICommand RunIsomorphismTaskCommand
        {
            get
            {
                if (runIsomorphismTaskCommand == null)
                {
                    runIsomorphismTaskCommand = new DelegateCommand(RunIsomorphismTaskAction, CanRunIsomorphismTaskAction);
                }
                return runIsomorphismTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunIsomorphismTaskAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.Isomorphism,
                Dimension = null,
                NumberOfSet = null,
                Step = null,
                MaxCount = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunIsomorphismTaskAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runIsomorphismBipartTaskCommand;
        public ICommand RunIsomorphismBipartTaskCommand
        {
            get
            {
                if (runIsomorphismBipartTaskCommand == null)
                {
                    runIsomorphismBipartTaskCommand = new DelegateCommand(RunIsomorphismBipartTaskAction, CanRunIsomorphismBipartTaskAction);
                }
                return runIsomorphismBipartTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunIsomorphismBipartTaskAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.IsomorphismByPart,
                Dimension = null,
                NumberOfSet = null,
                Step = null,
                MaxCount = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunIsomorphismBipartTaskAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runDefineTypeTaskCommand;
        public ICommand RunDefineTypeTaskCommand
        {
            get
            {
                if (runDefineTypeTaskCommand == null)
                {
                    runDefineTypeTaskCommand = new DelegateCommand(RunDefineTypeTaskAction, CanRunDefineTypeTaskAction);
                }
                return runDefineTypeTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunDefineTypeTaskAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.DefineTypeTask,
                Dimension = null,
                NumberOfSet = null,
                Step = null, 
                MaxCount = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunDefineTypeTaskAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runDefineGreedyComparisonCommand;
        public ICommand RunDefineGreedyComparisonCommand
        {
            get
            {
                if (runDefineGreedyComparisonCommand == null)
                {
                    runDefineGreedyComparisonCommand = new DelegateCommand(RunDefineGreedyComparisonAction, CanRunGreedyComparisonTaskAction);
                }
                return runDefineGreedyComparisonCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunDefineGreedyComparisonAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.DefineGreedyComparison,
                Dimension = null,
                NumberOfSet = null,
                Step = null,
                MaxCount = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunGreedyComparisonTaskAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
