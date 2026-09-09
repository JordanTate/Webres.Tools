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
    /// Interaction logic for PreDeploymentView.xaml
    /// </summary>
    public partial class PreDeploymentView : UserControl
    {
        private readonly PreDeploymentViewModel _viewModel;
        public PreDeploymentView(PreDeploymentViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        #region Events
        private void Deploy_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Deploy();
        }
        #endregion
    }
}
