using BioAlgorithm.Data.HittingSet.Data;
using System.Windows.Input;
using BioAlgorithmViewModel.Common;
using System.Collections.ObjectModel;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithm.Services.HittingSet;
using BioAlgorithm.Core.HittingSet;

namespace BioAlgorithmViewModel.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class PrepareAndExecuteViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class PrepareAndExecuteViewModel : CaseDefinitionViewModel
    {
        protected HittingSetRepository hittingSetRepository;
        //----------------------------------------------------------------------------------------------------------------------
        protected ObservableCollection<AlgorithmDetailViewModel> algorithmItems;
        public ObservableCollection<AlgorithmDetailViewModel> AlgorithmItems
        {
            get
            {
                return algorithmItems;
            }
            set
            {
                algorithmItems = value;
                OnPropertyChanged(nameof(AlgorithmItems));
            }
        }
        protected bool isInputForce;
        public bool IsInputForce
        {
            get
            {
                return isInputForce;
            }
            set
            {
                isInputForce = value;
                OnPropertyChanged(nameof(IsInputForce));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public PrepareAndExecuteViewModel()
        {

        }
        public PrepareAndExecuteViewModel(StartTaskMessage message)
        {
            if (message.Algorithm != null && algorithmItems != null)
            {
                AlgorithmDetailViewModel? messageAlgorithm = algorithmItems.FirstOrDefault( a => a.Algorithm == message.Algorithm);
                if (messageAlgorithm != null)
                {
                    messageAlgorithm.IsSelected = true;
                }
            }
            if (message.NumberOfSet != null)
            {
                NumberOfSet = message.NumberOfSet;
            }
            if (message.Dimension != null)
            {
                Dimension = message.Dimension;
            }
            hittingSetRepository = new HittingSetRepository();
            FillAlgorithm();
            PropertyChanged += PrepareAndExecuteViewModel_PropertyChanged;
        }

        protected void FillAlgorithm()
        {

            AlgorithmItems = new ObservableCollection<AlgorithmDetailViewModel>()
            {
                new AlgorithmDetailViewModel() {Algorithm = "BruteForceHittingSetBinaryNumbers" },
                new AlgorithmDetailViewModel() {Algorithm = "BruteForceHittingSetBinaryNumbersVer2" },
                new AlgorithmDetailViewModel() {Algorithm = "BruteForceHittingSetAsTree" },
                new AlgorithmDetailViewModel() {Algorithm = "BruteForceHittingSetAsTreeDirect" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetBranchAndBound" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetBranchAndBoundByValue" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetBranchAndBoundFirst" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetGreedySimple" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetGreedyImprove" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetGreedyRelation" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetGreedyImproveRD" },
                new AlgorithmDetailViewModel() {Algorithm = "AllGreedy" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetTriangleBranchAndBound" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetTriangle" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetTriangleStrategy", AlgorithmDetail = "SelectElementSimpleStrategy" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetTriangleStrategy", AlgorithmDetail = "SelectElementRelationStrategy" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetTriangleStrategy", AlgorithmDetail = "SelectElementImproveStrategy" },
                new AlgorithmDetailViewModel() {Algorithm = "HittingSetTriangleStrategy", AlgorithmDetail = "SelectElementImproveRDStrategy" },
                new AlgorithmDetailViewModel() {Algorithm = "Empty" }
            };
            
        }
        protected void PrepareAndExecuteViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Algorithm))
            {
                
            }
        }

        protected bool ExecuteAlgorithmEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand executeAlgorithmCommand;
        public ICommand ExecuteAlgorithmCommand
        {
            get
            {
                if (executeAlgorithmCommand == null)
                {
                    executeAlgorithmCommand = new DelegateCommand(ExecuteAlgorithmAction, CanExecuteAlgorithmAction);
                }
                return executeAlgorithmCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected async void ExecuteAlgorithmAction()
        {
            ExecutionState = "Task running...";
            ExecuteAlgorithmEnable = false;

            HittingSetService hittingSetService = new HittingSetService(hittingSetRepository);
            if (Dimension.HasValue && NumberOfSet.HasValue)
            {
                var selectedAlgoriths = AlgorithmItems.Where(a => a.IsSelected).Select(a => (a.Algorithm, a.AlgorithmDetail)).ToList();
                if (selectedAlgoriths.Count > 0)
                    await hittingSetService.ExecuteAlgorithmAsync(selectedAlgoriths, Dimension.Value, NumberOfSet.Value, IsInputForce);
                else
                {
                    ExecutionState = "Algoriths are not selected.";
                    ExecuteAlgorithmEnable = true;
                    return;
                }
            }

            ExecutionState = "Task completed.";
            ExecuteAlgorithmEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected bool CanExecuteAlgorithmAction()
        {
            return ExecuteAlgorithmEnable && Dimension.HasValue && NumberOfSet.HasValue;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
