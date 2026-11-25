
using BioAlgorithm.Data.Contract.HittingSet.Data.Contract;
using BioAlgorithm.Data.HittingSet.Data;
using BioAlgorithm.Services.HittingSet;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.HittingSet.Messages;
using HittingSet.Data.Contract;
using System.Windows.Input;

namespace BioAlgorithmViewModel.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class DeleteAlgorithmInputViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class DeleteAlgorithmInputViewModel : CaseDefinitionViewModel
    {
        protected readonly HittingSetRepository hittingSetRepository;
        private readonly DeleteAlgorithmInputTypeEnum  deleteAlgorithmInputType;
        private readonly DeleteAlgorithmInputMessage _message;
        private string title;
        public event Action CloseAction;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public DeleteAlgorithmInputViewModel(DeleteAlgorithmInputMessage message)
        {
            _message = message;
            deleteAlgorithmInputType = message.DeleteAlgorithmInputType;
            if (message.NumberOfSet != null)
            {
                NumberOfSet = message.NumberOfSet;
            }
            if (message.Algorithm != null)
            {
                Algorithm = message.Algorithm;
            }
            if (message.Dimension != null)
            {
                Dimension = message.Dimension;
            }
            if (message.MaxCount != null)
            {
                MaxCount = message.MaxCount;
            }
            else
            {
                MaxCount = 0;
            }
            hittingSetRepository = new HittingSetRepository();

            Title = message.DeleteAlgorithmInputType switch
            {
                DeleteAlgorithmInputTypeEnum.Algorithm => "Delete from HittingSetPerfomance table",
                DeleteAlgorithmInputTypeEnum.AlgorithmInput => "Delete from HittingSetPerfomance table",
                DeleteAlgorithmInputTypeEnum.Input => "Delete from HittingSetInput table",
            };

            Content = message.DeleteAlgorithmInputType switch
            {
                DeleteAlgorithmInputTypeEnum.Algorithm => $"All rows with column Algorith = {Algorithm} will be deleted",
                DeleteAlgorithmInputTypeEnum.AlgorithmInput => $"All rows with column Algorith = {Algorithm}, column Dimension = {Dimension}, column NumberOfSet = {NumberOfSet}, column MaxCount = {MaxCount} will be deleted",
                DeleteAlgorithmInputTypeEnum.Input => $"All rows with column column Dimension = {Dimension}, column NumberOfSet = {NumberOfSet}, column MaxCount = {MaxCount} will be deleted",
            };
        }


        private bool ExecuteTaskEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new DelegateCommand(DeleteAction, CanDeleteAction);
                }
                return deleteCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void DeleteAction()
        {
            ExecutionState = "Task running...";
            ExecuteTaskEnable = false;
            string error;

            string? result = null;
            HittingSetService hittingSetService = new HittingSetService(hittingSetRepository);
            switch (deleteAlgorithmInputType)
            {
                case DeleteAlgorithmInputTypeEnum.Algorithm:

                    result = await hittingSetRepository.DeleteHittingSetAlgorithmAsync(new HittingSetAlgorithmGroup() { Algorithm = Algorithm });
                    if (string.IsNullOrEmpty(result))
                    {
                        _message.InputAlgorithmViewModel.DeleteSelectedItem();
                        ExecutionState = "Operation completed";
                    }
                    {
                        ExecutionState = $"Operation failed: {result}";
                    }

                    break;
                case DeleteAlgorithmInputTypeEnum.AlgorithmInput:
                    result = await hittingSetRepository.DeleteHittingSetAlgorithmGroupAsync(new HittingSetAlgorithmGroupDimension()
                    {
                        Algorithm = Algorithm,
                        Dimension = Dimension.Value,
                        NumberOfSet = NumberOfSet.Value,
                        MaxCount = MaxCount.Value
                    });
                    if (string.IsNullOrEmpty(result))
                    {
                        _message.InputAlgorithmViewModel.DeleteSelectedItem();
                        ExecutionState = "Operation completed";
                    }
                    else
                    {
                        ExecutionState = $"Operation failed: {result}";
                    }
                    break;
                case DeleteAlgorithmInputTypeEnum.Input:
                    result = await hittingSetRepository.DeleteHittingSetInputGroupAsync(new HittingSetInputGroup() 
                    { 
                        Dimension = Dimension.Value,
                        NumberOfSet = NumberOfSet.Value,
                        MaxCount = MaxCount.Value
                    });
                    if (string.IsNullOrEmpty(result))
                    {
                        _message.InputAlgorithmViewModel.DeleteSelectedItem();
                        ExecutionState = "Operation completed";
                    }
                    else
                    {
                        ExecutionState = $"Operation failed: {result}";
                    }
                    break;
            }

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteAction()
        {
            return ExecuteTaskEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new DelegateCommand(CancelAction, CanCancelAction);
                }
                return cancelCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void CancelAction()
        {
            CloseAction?.Invoke();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanCancelAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
