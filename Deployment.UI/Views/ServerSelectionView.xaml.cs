using Deployment.Core.Server;
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
    /// Interaction logic for ServerSelectionView.xaml
    /// </summary>
    public partial class ServerSelectionView : UserControl
    {
        private readonly ServerSelectionViewModel _viewModel;
        public ServerSelectionView(ServerSelectionViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        #region Events
        private void Server_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            ServerInfo? server = e.AddedItems[0] as ServerInfo;
            _viewModel.SelectServer(server!);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Continue();
        }
        #endregion
    }
}
