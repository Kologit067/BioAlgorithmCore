using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithmViewModel.HittingSet.Utility;
using BioAlgorithm.Data.HittingSet.Data;
using HittingSet.Data.Contract;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BioAlgorithmViewModel.Interfaces;

namespace BioAlgorithmViewModel.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetPerformanceAlgorithmViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetPerformanceAlgorithmViewModel : ViewModelBase, IInputAlgorithmViewModel
    {
        private HittingSetRepository hittingSetRepository;
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetAlgorithmGroup> hittingSetAlgorithmGroups;
        public ObservableCollection<HittingSetAlgorithmGroup> HittingSetAlgorithmGroups
        {
            get
            {
                return hittingSetAlgorithmGroups;
            }
            set
            {
                hittingSetAlgorithmGroups = value;
                OnPropertyChanged(nameof(HittingSetAlgorithmGroups));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetAlgorithmGroup selectedAlgorithm;
        public HittingSetAlgorithmGroup SelectedAlgorithm
        {
            get
            {
                return selectedAlgorithm;
            }
            set
            {
                selectedAlgorithm = value;
                OnPropertyChanged(nameof(SelectedAlgorithm));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetPerformanceAlgorithmViewModel(HittingSetRepository hittingSetRepository)
        {
            this.hittingSetRepository = hittingSetRepository;
            HittingSetAlgorithmGroups = new ObservableCollection<HittingSetAlgorithmGroup>();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshHittingSetAlgorithmListCommand;
        public ICommand RefreshHittingSetAlgorithmListCommand
        {
            get
            {
                if (refreshHittingSetAlgorithmListCommand == null)
                {
                    refreshHittingSetAlgorithmListCommand = new DelegateCommand(RefreshHittingSetAlgorithmListAction, CanRefreshHittingSetAlgorithmListAction);
                }
                return refreshHittingSetAlgorithmListCommand;
            }
        }
        private bool refreshHittingSetAlgorithmListEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshHittingSetAlgorithmListAction()
        {
            ExecutionState = "Query running...";
            refreshHittingSetAlgorithmListEnable = false;

            HittingSetAlgorithmGroups.Clear();
            List<HittingSetAlgorithmGroup> algorithms = await hittingSetRepository.GetAlgorithmsAsync();
            foreach (HittingSetAlgorithmGroup a in algorithms)
                HittingSetAlgorithmGroups.Add(a);
            ExecutionState = "Query completed.";
            refreshHittingSetAlgorithmListEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshHittingSetAlgorithmListAction()
        {
            return refreshHittingSetAlgorithmListEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toFilterCommand;
        public ICommand ToFilterCommand
        {
            get
            {
                if (toFilterCommand == null)
                {
                    toFilterCommand = new DelegateCommand(ToFilterAction, CanToFilterAction);
                }
                return toFilterCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToFilterAction()
        {
            Messenger.Default.Send<HittingSetTabChangeMessage>(new HittingSetTabChangeMessage()
            {
                HittingSetTabName = "HittingSetPerformance"
            }, typeof(HittingSetTabChangeMessage));
            Messenger.Default.Send<AlgorithmToFilterMessage>(new AlgorithmToFilterMessage() { Algorithm = SelectedAlgorithm.Algorithm}, typeof(AlgorithmToFilterMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toAsGroupFilterCommand;
        public ICommand ToAsGroupFilterCommand
        {
            get
            {
                if (toAsGroupFilterCommand == null)
                {
                    toAsGroupFilterCommand = new DelegateCommand(ToFilterAsGroupAction, CanToFilterAsGroupAction);
                }
                return toAsGroupFilterCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToFilterAsGroupAction()
        {
            Messenger.Default.Send<HittingSetTabChangeMessage>(new HittingSetTabChangeMessage()
            {
                HittingSetTabName = "HittingSetPerformanceAsGroup"
            }, typeof(HittingSetTabChangeMessage));
            Messenger.Default.Send<AlgorithmToFilterByGroupMessage>(new AlgorithmToFilterByGroupMessage() { Algorithm = SelectedAlgorithm.Algorithm }, typeof(AlgorithmToFilterByGroupMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterAsGroupAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand deleteAlgorithmCommand;
        public ICommand DeleteAlgorithmCommand
        {
            get
            {
                if (deleteAlgorithmCommand == null)
                {
                    deleteAlgorithmCommand = new DelegateCommand(DeleteAlgorithmAction, CanDeleteAlgorithmAction);
                }
                return deleteAlgorithmCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void DeleteAlgorithmAction()
        {
            Messenger.Default.Send<DeleteAlgorithmInputMessage>(new DeleteAlgorithmInputMessage()
            {
                DeleteAlgorithmInputType = DeleteAlgorithmInputTypeEnum.Algorithm,
                InputAlgorithmViewModel = this,
                Algorithm = SelectedAlgorithm.Algorithm
            }, typeof(DeleteAlgorithmInputMessage));


            //ExecutionState = "Operation running...";
            //refreshHittingSetAlgorithmListEnable = false;
            //string? result = await hittingSetRepository.DeleteHittingSetAlgorithmAsync(SelectedAlgorithm);
            //if (string.IsNullOrEmpty(result))
            //{
            //    HittingSetAlgorithmGroups.Remove(SelectedAlgorithm);
            //    ExecutionState = "Operation completed";
            //}
            //{
            //    ExecutionState = $"Operation failed: {result}";
            //}
            //refreshHittingSetAlgorithmListEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteAlgorithmAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void DeleteSelectedItem()
        {
            HittingSetAlgorithmGroups.Remove(SelectedAlgorithm);
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
