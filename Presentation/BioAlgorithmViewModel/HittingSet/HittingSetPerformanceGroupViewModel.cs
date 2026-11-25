using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithmViewModel.HittingSet.Utility;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BioAlgorithm.Data.HittingSet.Data;
using HittingSet.Data.Contract;
using BioAlgorithmViewModel.Interfaces;

namespace BioAlgorithmViewModel.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetPerformanceGroupViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetPerformanceGroupViewModel : HittingSetBaseViewModel, IInputAlgorithmViewModel
    {
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetAlgorithmGroupDimension> hittingSetAlgorithmGroupByDimensions;
        public ObservableCollection<HittingSetAlgorithmGroupDimension> HittingSetAlgorithmGroupByDimensions
        {
            get
            {
                return hittingSetAlgorithmGroupByDimensions;
            }
            set
            {
                hittingSetAlgorithmGroupByDimensions = value;
                OnPropertyChanged(nameof(HittingSetAlgorithmGroupByDimensions));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetAlgorithmGroupDimension selectedAlgorithmGroup;
        public HittingSetAlgorithmGroupDimension SelectedAlgorithmGroup
        {
            get
            {
                return selectedAlgorithmGroup;
            }
            set
            {
                selectedAlgorithmGroup = value;
                OnPropertyChanged(nameof(SelectedAlgorithmGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string algorithmGroupListSort;
        public string AlgorithmGroupListSort
        {
            get
            {
                return algorithmGroupListSort;
            }
            set
            {
                algorithmGroupListSort = value;
                OnPropertyChanged(nameof(AlgorithmGroupListSort));
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        private List<string> algorithmGroupSortItems;
        public List<string> AlgorithmGroupSortItems
        {
            get
            {
                return algorithmGroupSortItems;
            }
            set
            {
                algorithmGroupSortItems = value;
                OnPropertyChanged(nameof(AlgorithmGroupSortItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetPerformanceGroupViewModel(HittingSetRepository hittingSetRepository) : base(hittingSetRepository)
        {
            HittingSetAlgorithmGroupByDimensions = new ObservableCollection<HittingSetAlgorithmGroupDimension>();
            AlgorithmGroupSortItems = new List<string>()
            {
                "Algorithm, Dimension, NumberOfSet",
                "Algorithm, NumberOfSet, Dimension",
                "Dimension, NumberOfSet, Algorithm",
                "NumberOfSet, Dimension, Algorithm"
            };
            AlgorithmGroupListSort = AlgorithmGroupSortItems[0];
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshHittingSetAlgorithmGroupListCommand;
        public ICommand RefreshHittingSetAlgorithmGroupListCommand
        {
            get
            {
                if (refreshHittingSetAlgorithmGroupListCommand == null)
                {
                    refreshHittingSetAlgorithmGroupListCommand = new DelegateCommand(RefreshHittingSetAlgorithmGroupListAction, CanRefreshHittingSetAlgorithmGroupListAction);
                }
                return refreshHittingSetAlgorithmGroupListCommand;
            }
        }
        private bool refreshHittingSetAlgorithmGroupEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshHittingSetAlgorithmGroupListAction()
        {
            ExecutionState = "Query running...";
            refreshHittingSetAlgorithmGroupEnable = false;

            HittingSetAlgorithmGroupByDimensions.Clear();
            List<HittingSetAlgorithmGroupDimension> algorithms = await hittingSetRepository.GetHittingSetAlgorithmGroupDimensionsAsync(AlgorithmGroupListSort);
            foreach (HittingSetAlgorithmGroupDimension a in algorithms)
                HittingSetAlgorithmGroupByDimensions.Add(a);
            ExecutionState = "Query completed.";
            refreshHittingSetAlgorithmGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshHittingSetAlgorithmGroupListAction()
        {
            return refreshHittingSetAlgorithmGroupEnable;
        }
        ////----------------------------------------------------------------------------------------------------------------------
        //private ICommand toFilterCommand;
        //public ICommand ToFilterCommand
        //{
        //    get
        //    {
        //        if (toFilterCommand == null)
        //        {
        //            toFilterCommand = new DelegateCommand(ToFilterAction, CanToFilterAction);
        //        }
        //        return toFilterCommand;
        //    }
        //}
        ////----------------------------------------------------------------------------------------------------------------------
        //private void ToFilterAction()
        //{
        //    Messenger.Default.Send<HittingSetTabChangeMessage>(new HittingSetTabChangeMessage()
        //    {
        //        HittingSetTabName = "HittingSetPerformance"
        //    }, typeof(HittingSetTabChangeMessage));
        //    Messenger.Default.Send<AlgorithmGroupToFilterMessage>(new AlgorithmGroupToFilterMessage() {
        //        Algorithm = SelectedAlgorithmGroup.Algorithm,
        //        Dimension = SelectedAlgorithmGroup.Dimension,
        //        NumberOfSet = SelectedAlgorithmGroup.NumberOfSet,
        //        Step = SelectedAlgorithmGroup.Step
        //    }, typeof( AlgorithmGroupToFilterMessage ));
        //}
        ////----------------------------------------------------------------------------------------------------------------------
        //private bool CanToFilterAction()
        //{
        //    return true;
        //}
        protected override void FillAlgorithmGroupToFilterMessage(AlgorithmGroupToFilterMessage message)
        {
            message.Algorithm = SelectedAlgorithmGroup.Algorithm;
            message.Dimension = SelectedAlgorithmGroup.Dimension;
            message.NumberOfSet = SelectedAlgorithmGroup.NumberOfSet;
            message.Step = SelectedAlgorithmGroup.Step;
            message.MaxCount = SelectedAlgorithmGroup.MaxCount;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toFilterInGroupCommand;
        public ICommand ToFilterInGroupCommand
        {
            get
            {
                if (toFilterInGroupCommand == null)
                {
                    toFilterInGroupCommand = new DelegateCommand(ToFilterInGroupAction, CanToFilterInGroupAction);
                }
                return toFilterInGroupCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToFilterInGroupAction()
        {
            Messenger.Default.Send<HittingSetTabChangeMessage>(new HittingSetTabChangeMessage()
            {
                HittingSetTabName = "HittingSetPerformanceAsGroup"
            }, typeof(HittingSetTabChangeMessage));
            Messenger.Default.Send<AlgorithmGroupToFilterByGroupMessage>(new AlgorithmGroupToFilterByGroupMessage()
            {
                Algorithm = SelectedAlgorithmGroup.Algorithm,
                Dimension = SelectedAlgorithmGroup.Dimension,
                NumberOfSet = SelectedAlgorithmGroup.NumberOfSet,
                Step = SelectedAlgorithmGroup.Step,
                MaxCount = SelectedAlgorithmGroup.MaxCount
            }, typeof(AlgorithmGroupToFilterByGroupMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterInGroupAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toFilterInInputCommand;
        public ICommand ToFilterInInputCommand
        {
            get
            {
                if (toFilterInInputCommand == null)
                {
                    toFilterInInputCommand = new DelegateCommand(ToFilterInInputAction, CanToFilterInInputAction);
                }
                return toFilterInInputCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToFilterInInputAction()
        {
            Messenger.Default.Send<HittingSetTabChangeMessage>(new HittingSetTabChangeMessage()
            {
                HittingSetTabName = "InputData"
            }, typeof(HittingSetTabChangeMessage));
            Messenger.Default.Send<AlgorithmGroupToFilterInputDataMessage>(new AlgorithmGroupToFilterInputDataMessage()
            {
                Dimension = SelectedAlgorithmGroup.Dimension,
                NumberOfSet = SelectedAlgorithmGroup.NumberOfSet,
                Step = SelectedAlgorithmGroup.Step,
                MaxCount = SelectedAlgorithmGroup.MaxCount
            }, typeof(AlgorithmGroupToFilterInputDataMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterInInputAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toInputGeedyComparisonCommand;
        public ICommand ToInputGeedyComparisonCommand
        {
            get
            {
                if (toInputGeedyComparisonCommand == null)
                {
                    toInputGeedyComparisonCommand = new DelegateCommand(ToInputGeedyComparisonAction, CanToInputGeedyComparisonAction);
                }
                return toInputGeedyComparisonCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToInputGeedyComparisonAction()
        {
            Messenger.Default.Send<HittingSetTabChangeMessage>(new HittingSetTabChangeMessage()
            {
                HittingSetTabName = "InputDataGreedyComparison"
            }, typeof(HittingSetTabChangeMessage));
            Messenger.Default.Send<InputDataGreedyComparisonToFilterMessage>(new InputDataGreedyComparisonToFilterMessage()
            {
                Dimension = SelectedAlgorithmGroup.Dimension,
                NumberOfSet = SelectedAlgorithmGroup.NumberOfSet,
                Step = SelectedAlgorithmGroup.Step,
                MaxCount = SelectedAlgorithmGroup.MaxCount
            }, typeof(InputDataGreedyComparisonToFilterMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToInputGeedyComparisonAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand openInWindowCommand;
        public ICommand OpenInWindowCommand
        {
            get
            {
                if (openInWindowCommand == null)
                {
                    openInWindowCommand = new DelegateCommand(OpenInWindowAction, CanOpenInWindowAction);
                }
                return openInWindowCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OpenInWindowAction()
        {
            Messenger.Default.Send<AlgorithmGroupOpenWindowMessage>(new AlgorithmGroupOpenWindowMessage()
            {
                Algorithm = SelectedAlgorithmGroup.Algorithm,
                Dimension = SelectedAlgorithmGroup.Dimension,
                NumberOfSet = SelectedAlgorithmGroup.NumberOfSet,
                Step = SelectedAlgorithmGroup.Step,
                MaxCount = SelectedAlgorithmGroup.MaxCount
            }, typeof(AlgorithmGroupOpenWindowMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanOpenInWindowAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand deleteGroupCommand;
        public ICommand DeleteGroupCommand
        {
            get
            {
                if (deleteGroupCommand == null)
                {
                    deleteGroupCommand = new DelegateCommand(DeleteGroupAction, CanDeleteGroupAction);
                }
                return deleteGroupCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void DeleteGroupAction()
        {
            Messenger.Default.Send<DeleteAlgorithmInputMessage>(new DeleteAlgorithmInputMessage()
            {
                DeleteAlgorithmInputType = DeleteAlgorithmInputTypeEnum.AlgorithmInput,
                InputAlgorithmViewModel = this,
                Algorithm = SelectedAlgorithmGroup.Algorithm,
                MaxCount = SelectedAlgorithmGroup.MaxCount,
                NumberOfSet = SelectedAlgorithmGroup.NumberOfSet,
                Dimension = SelectedAlgorithmGroup.Dimension,
            }, typeof(DeleteAlgorithmInputMessage));

            //ExecutionState = "Operation running...";
            //refreshHittingSetAlgorithmGroupEnable = false;
            //string? result = await hittingSetRepository.DeleteHittingSetAlgorithmGroupAsync(SelectedAlgorithmGroup);
            //if (string.IsNullOrEmpty(result))
            //{
            //    HittingSetAlgorithmGroupByDimensions.Remove(SelectedAlgorithmGroup);
            //    ExecutionState = "Operation completed";
            //}
            //{
            //    ExecutionState = $"Operation failed: {result}";
            //}
            //refreshHittingSetAlgorithmGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteGroupAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void DeleteSelectedItem()
        {
            HittingSetAlgorithmGroupByDimensions.Remove(SelectedAlgorithmGroup);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runGreedyComparisonCommand;
        public ICommand RunGreedyComparisonCommand
        {
            get
            {
                if (runGreedyComparisonCommand == null)
                {
                    runGreedyComparisonCommand = new DelegateCommand(RunGreedyComparisonAction, CanRunGreedyComparisonAction);
                }
                return runGreedyComparisonCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunGreedyComparisonAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.DefineGreedyComparison,
                Dimension = SelectedAlgorithmGroup.Dimension,
                NumberOfSet = SelectedAlgorithmGroup.NumberOfSet,
                Step = SelectedAlgorithmGroup.Step,
                MaxCount = SelectedAlgorithmGroup.MaxCount
            }, typeof(StartInputTaskMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunGreedyComparisonAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
