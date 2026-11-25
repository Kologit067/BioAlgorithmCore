using System.Windows.Input;
using BioAlgorithm.Data.HittingSet.Data;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithmViewModel.HittingSet.Utility;

namespace BioAlgorithmViewModel.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetBaseViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public abstract class HittingSetBaseViewModel : ViewModelBase
    {
        protected readonly HittingSetRepository hittingSetRepository;
        //----------------------------------------------------------------------------------------------------------------------
        private int totalNumberRows;
        public int TotalNumberRows
        {
            get
            {
                return totalNumberRows;
            }
            set
            {
                totalNumberRows = value;
                OnPropertyChanged(nameof(TotalNumberRows));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetFilterViewModel hittingSetDataFilter;
        public HittingSetFilterViewModel HittingSetFilter
        {
            get
            {
                return hittingSetDataFilter;
            }
            set
            {
                hittingSetDataFilter = value;
                OnPropertyChanged(nameof(HittingSetFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetBaseViewModel(HittingSetRepository hittingSetRepository)
        {
            this.hittingSetRepository = hittingSetRepository;
            HittingSetFilter = new HittingSetFilterViewModel();
            HittingSetFilter.Top = 1000;
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
            AlgorithmGroupToFilterMessage message = new AlgorithmGroupToFilterMessage();
            FillAlgorithmGroupToFilterMessage(message);
            Messenger.Default.Send<AlgorithmGroupToFilterMessage>(message, typeof(AlgorithmGroupToFilterMessage));
        }
        protected virtual void FillAlgorithmGroupToFilterMessage(AlgorithmGroupToFilterMessage message)
        {

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand makeGraphCommand;
        public ICommand MakeGraphCommand
        {
            get
            {
                if (makeGraphCommand == null)
                {
                    makeGraphCommand = new DelegateCommand(MakeGraphAction, CanMakeGraphAction);
                }
                return makeGraphCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void MakeGraphAction()
        {
            Messenger.Default.Send<HittingSetTabChangeMessage>(new HittingSetTabChangeMessage()
            {
                HittingSetTabName = "BipartiteGraph"
            }, typeof(HittingSetTabChangeMessage));
            Messenger.Default.Send<InputDataToGraphMessage>(new InputDataToGraphMessage()
            {
                InputData = GetGraphData()
            }, typeof(InputDataToGraphMessage));
        }
        protected virtual string GetGraphData()
        {
            return string.Empty;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanMakeGraphAction()
        {
            return true;
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
}
