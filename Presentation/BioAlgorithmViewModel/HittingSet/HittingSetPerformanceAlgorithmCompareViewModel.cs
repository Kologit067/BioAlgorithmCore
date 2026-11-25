using BioAlgorithmViewModel.Common;
using System.Collections.ObjectModel;
using BioAlgorithmViewModel.Helpers;
using System.Windows.Input;
using BioAlgorithm.Data.HittingSet.Data;
using HittingSet.Data.Contract;
using BioAlgorithm.Data.Contract.HittingSet.Data.Contract.Interfaces;

namespace BioAlgorithmViewModel.HittingSet
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetPerformanceAlgorithmCompareViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetPerformanceAlgorithmCompareViewModel : ViewModelBase
    {
        private IHittingSetRepository hittingSetRepository;
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetAlgorithWithDimension> algorithmWithDimensions;
        public ObservableCollection<HittingSetAlgorithWithDimension> AlgorithmWithDimensions
        {
            get
            {
                return algorithmWithDimensions;
            }
            set
            {
                algorithmWithDimensions = value;
                OnPropertyChanged(nameof(AlgorithmWithDimensions));
            }
        }
        ////----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetAlgorithmCompare> hittingSetAlgorithmCompareList;
        public ObservableCollection<HittingSetAlgorithmCompare> HittingSetAlgorithmCompareList
        {
            get
            {
                return hittingSetAlgorithmCompareList;
            }
            set
            {
                hittingSetAlgorithmCompareList = value;
                OnPropertyChanged(nameof(HittingSetAlgorithmCompareList));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<int> numberOfSetItems;
        public ObservableCollection<int> NumberOfSetItems
        {
            get
            {
                return numberOfSetItems;
            }
            set
            {
                numberOfSetItems = value;
                OnPropertyChanged(nameof(NumberOfSetItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private int selectedNumberOfSet;
        public int SelectedNumberOfSet
        {
            get
            {
                return selectedNumberOfSet;
            }
            set
            {
                selectedNumberOfSet = value;
                OnPropertyChanged(nameof(SelectedNumberOfSet));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<int> dimensionItems;
        public ObservableCollection<int> DimensionItems
        {
            get
            {
                return dimensionItems;
            }
            set
            {
                dimensionItems = value;
                OnPropertyChanged(nameof(DimensionItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private int selectedDimension;
        public int SelectedDimension
        {
            get
            {
                return selectedDimension;
            }
            set
            {
                selectedDimension = value;
                OnPropertyChanged(nameof(SelectedDimension));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<long> stepItems;
        public ObservableCollection<long> StepItems
        {
            get
            {
                return stepItems;
            }
            set
            {
                stepItems = value;
                OnPropertyChanged(nameof(StepItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<long> maxCountItems;
        public ObservableCollection<long> MaxCountItems
        {
            get
            {
                return maxCountItems;
            }
            set
            {
                maxCountItems = value;
                OnPropertyChanged(nameof(MaxCountItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private long selectedStep;
        public long SelectedStep
        {
            get
            {
                return selectedStep;
            }
            set
            {
                selectedStep = value;
                OnPropertyChanged(nameof(SelectedStep));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private long selectedMaxCount;
        public long SelectedMaxCount
        {
            get
            {
                return selectedMaxCount;
            }
            set
            {
                selectedMaxCount = value;
                OnPropertyChanged(nameof(SelectedMaxCount));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<string> algorithmItems;
        public ObservableCollection<string> AlgorithmItems
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
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedAlgorithm1;
        public string SelectedAlgorithm1
        {
            get
            {
                return selectedAlgorithm1;
            }
            set
            {
                selectedAlgorithm1 = value;
                OnPropertyChanged(nameof(SelectedAlgorithm1));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedAlgorithm2;
        public string SelectedAlgorithm2
        {
            get
            {
                return selectedAlgorithm2;
            }
            set
            {
                selectedAlgorithm2 = value;
                OnPropertyChanged(nameof(SelectedAlgorithm2));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<string> compareOptionItems;
        public ObservableCollection<string> CompareOptionItems
        {
            get
            {
                return compareOptionItems;
            }
            set
            {
                compareOptionItems = value;
                OnPropertyChanged(nameof(CompareOptionItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedBestValueCompare;
        public string SelectedBestValueCompare
        {
            get
            {
                return selectedBestValueCompare;
            }
            set
            {
                selectedBestValueCompare = value;
                OnPropertyChanged(nameof(SelectedBestValueCompare));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedDurationCompare;
        public string SelectedDurationCompare
        {
            get
            {
                return selectedDurationCompare;
            }
            set
            {
                selectedDurationCompare = value;
                OnPropertyChanged(nameof(SelectedDurationCompare));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedcbNumberIterationCompare;
        public string SelectedcbNumberIterationCompare
        {
            get
            {
                return selectedcbNumberIterationCompare;
            }
            set
            {
                selectedcbNumberIterationCompare = value;
                OnPropertyChanged(nameof(SelectedcbNumberIterationCompare));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedElemenationCountCompare;
        public string SelectedElemenationCountCompare
        {
            get
            {
                return selectedElemenationCountCompare;
            }
            set
            {
                selectedElemenationCountCompare = value;
                OnPropertyChanged(nameof(SelectedElemenationCountCompare));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetPerfomanceCompare> hittingSetPerfomanceCompareList;
        public ObservableCollection<HittingSetPerfomanceCompare> HittingSetPerfomanceCompareList
        {
            get
            {
                return hittingSetPerfomanceCompareList;
            }
            set
            {
                hittingSetPerfomanceCompareList = value;
                OnPropertyChanged(nameof(HittingSetPerfomanceCompareList));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetPerfomanceCompare selectedHittingSetPerfomanceCompare;
        public HittingSetPerfomanceCompare SelectedHittingSetPerfomanceCompare
        {
            get
            {
                return selectedHittingSetPerfomanceCompare;
            }
            set
            {
                selectedHittingSetPerfomanceCompare = value;
                OnPropertyChanged(nameof(SelectedHittingSetPerfomanceCompare));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetPerformanceAlgorithmCompareViewModel(HittingSetRepository hittingSetRepository)
        {
            CompareOptionItems = new ObservableCollection<string>()
            {
                "N/A",
                "1 > 2",
                "1 < 2",
                "1 <> 2",
                "1 = 2"
            };
            PropertyChanged += HittingSetPerformanceAlgorithmCompareViewModel_PropertyChanged;
            this.hittingSetRepository = hittingSetRepository;
            HittingSetAlgorithmCompareList = new ObservableCollection<HittingSetAlgorithmCompare>();
            SelectedBestValueCompare = "1 > 2";
            SelectedcbNumberIterationCompare = "N/A";
            SelectedDurationCompare = "N/A";
            SelectedElemenationCountCompare = "N/A";
            refreshHittingSetCompareListEnable = false;
            //Task<List<HittingSetAlgorithWithDimension>> task = hittingSetRepository.GetHittingSetAlgorithmWithDimensionsAsync();
            //task.ContinueWith(async antedecent =>
            //{
            //    List<HittingSetAlgorithWithDimension> listAlgorithms = await task;
            //    AlgorithmWithDimensions = listAlgorithms.ToObservable();
            //    IEnumerable<int> list = AlgorithmWithDimensions.GroupBy(a => a.NumberOfSet).OrderBy(n => n.Key).Select(n => n.Key);
            //    NumberOfSetItems = list.ToObservable<int>();
            //    if (NumberOfSetItems.Count > 0)
            //        SelectedNumberOfSet = NumberOfSetItems[0];
            //    refreshHittingSetCompareListEnable = true;
            //});
            //List<HittingSetAlgorithWithDimension> list = hittingSetRepository.GetHittingSetAlgorithmWithDimensionsAsync();

        }
        //----------------------------------------------------------------------------------------------------------------------
        private void HittingSetPerformanceAlgorithmCompareViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedNumberOfSet":
                    UpdateDimensionItems();
                    UpdateMaxCountItems();
                    UpdateStepItems();
                    UpdateAlgorithmItems();
                    break;
                case "SelectedDimension":
                    UpdateMaxCountItems();
                    UpdateStepItems();
                    UpdateAlgorithmItems();
                    break;
                case "SelectedStep":
                    UpdateAlgorithmItems();
                    break;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void UpdateDimensionItems()
        {
            var list = AlgorithmWithDimensions.Where(a => a.NumberOfSet == selectedNumberOfSet).GroupBy(a => a.Dimension).OrderBy(n => n.Key).Select(n => n.Key);
            DimensionItems = list.ToObservable<int>();
            if (!DimensionItems.Contains(SelectedDimension))
                if (DimensionItems.Count > 0)
                    SelectedDimension = DimensionItems[0];
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void UpdateMaxCountItems()
        {
            var listMaxCount = AlgorithmWithDimensions.Where(a => a.NumberOfSet == selectedNumberOfSet
&& a.Dimension == selectedDimension).GroupBy(a => a.MaxCount).OrderBy(n => n.Key).Select(n => n.Key);
            MaxCountItems = listMaxCount.ToObservable<long>();
            if (!MaxCountItems.Contains(SelectedMaxCount))
                if (MaxCountItems.Count > 0)
                    SelectedMaxCount = MaxCountItems[0];

        }
        //----------------------------------------------------------------------------------------------------------------------
        public void UpdateStepItems()
        {
            var listStep = AlgorithmWithDimensions.Where(a => a.NumberOfSet == selectedNumberOfSet
&& a.Dimension == selectedDimension).GroupBy(a => a.Step).OrderBy(n => n.Key).Select(n => n.Key);
            StepItems = listStep.ToObservable<long>();
            if (!StepItems.Contains(SelectedStep))
                if (StepItems.Count > 0)
                    SelectedStep = StepItems[0];

        }
        //----------------------------------------------------------------------------------------------------------------------
        public void UpdateAlgorithmItems()
        {
            var listAlgorithm = AlgorithmWithDimensions.Where(a => a.NumberOfSet == SelectedNumberOfSet
         && a.Dimension == SelectedDimension && a.MaxCount == selectedMaxCount).GroupBy(a => a.Algorithm).OrderBy(n => n.Key).Select(n => n.Key);
            AlgorithmItems = listAlgorithm.ToObservable();
            if (!AlgorithmItems.Contains(SelectedAlgorithm1))
                if (AlgorithmItems.Count > 0)
                    SelectedAlgorithm1 = AlgorithmItems[0];
            if (!AlgorithmItems.Contains(SelectedAlgorithm2))
                if (AlgorithmItems.Count > 0)
                    SelectedAlgorithm2 = AlgorithmItems[0];
        }
        private bool refreshHittingSetCompareListEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshHittingSetCompareListCommand;
        public ICommand RefreshHittingSetCompareListCommand
        {
            get
            {
                if (refreshHittingSetCompareListCommand == null)
                {
                    refreshHittingSetCompareListCommand = new DelegateCommand(RefreshHittingSetCompareListAction, CanRefreshHittingSetCompareListAction);
                }
                return refreshHittingSetCompareListCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshHittingSetCompareListAction()
        {
            ExecutionState = "Query running...";
            refreshHittingSetCompareListEnable = false;
            HittingSetPerfomanceCompareList?.Clear();
            HittingSetPerfomanceCompareFilter HittingSetPerfomanceCompareFilter = new HittingSetPerfomanceCompareFilter()
            {
                Algorithm1 = SelectedAlgorithm1,
                Algorithm2 = SelectedAlgorithm2,
                NumberOfSet = SelectedNumberOfSet,
                Dimension = SelectedDimension,
                Step = SelectedStep,
                MaxCount = SelectedMaxCount,
                BestValueCompare = SelectedBestValueCompare,
                DurationCompare = SelectedDurationCompare,
                NumberIterationCompare = SelectedcbNumberIterationCompare,
                ElemenationCountCompare = SelectedElemenationCountCompare
            };
            List<HittingSetPerfomanceCompare> items = await hittingSetRepository.GetHittingSetPerformanceCompareListAsync(HittingSetPerfomanceCompareFilter);
            HittingSetPerfomanceCompareList = items.ToObservable();
            ExecutionState = "Query completed.";
            refreshHittingSetCompareListEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshHittingSetCompareListAction()
        {
            return refreshHittingSetCompareListEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
