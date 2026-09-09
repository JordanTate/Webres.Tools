using Deployment.Core.Connection;
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
    /// Interaction logic for ConnectionView.xaml
    /// </summary>
    public partial class ConnectionView : UserControl
    {
        private readonly ConnectionViewModel _viewModel;
        public ConnectionView(ConnectionViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        #region Events
        private async void Validate_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.GetValidationResult();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Continue();
        }
        #endregion
    }
}
