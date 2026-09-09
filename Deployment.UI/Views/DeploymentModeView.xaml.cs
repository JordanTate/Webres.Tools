using Deployment.Core.Configuration;
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
    /// Interaction logic for DeploymentModeView.xaml
    /// </summary>
    public partial class DeploymentModeView : UserControl
    {
        private readonly DeploymentModeViewModel _viewModel;
        public DeploymentModeView(DeploymentModeViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
        }

        #region Events
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectMode(DeploymentMode.Preview);
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectMode(DeploymentMode.Execute);
        }
        #endregion
    }
}
