using Deployment.Core.Deployment;
using Deployment.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Deployment.UI.Views
{
    /// <summary>
    /// Interaction logic for DeploymentView.xaml
    /// </summary>
    public partial class DeploymentView : UserControl
    {
        private readonly DeploymentViewModel _viewModel;

        public DeploymentView(DeploymentViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            Loaded += DeploymentView_Loaded;
        }

        #region Events
        private async void DeploymentView_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            Loaded -= DeploymentView_Loaded;
            await System.Windows.Threading.Dispatcher.Yield();

            await _viewModel.StartAsync();
        }
        private void NewDeploymentButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.StartNewDeployment();
        }
        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Finish();
        }
        #endregion
    }
}
