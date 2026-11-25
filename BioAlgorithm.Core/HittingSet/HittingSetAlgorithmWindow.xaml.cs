using BioAlgorithmViewModel.HittingSet;
using BioAlgorithmViewModel.HittingSet.Messages;
using BioAlgorithm.Data.HittingSet.Data;
using System.Windows;

namespace BioAlgorithm.Core.HittingSet
{
    /// <summary>
    /// Interaction logic for HittingSetAlgorithmWindow.xaml
    /// </summary>
    public partial class HittingSetAlgorithmWindow : Window
    {
        private HittingSetPerformanceViewModel? viewModel;
        private AlgorithmGroupOpenWindowMessage message;
        public HittingSetAlgorithmWindow(AlgorithmGroupOpenWindowMessage message)
        {
            InitializeComponent();
            this.message  = message;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HittingSetRepository hittingSetRepository = new HittingSetRepository();
            viewModel = new HittingSetPerformanceViewModel(hittingSetRepository, message);
            DataContext = viewModel;
        }
    }
}
