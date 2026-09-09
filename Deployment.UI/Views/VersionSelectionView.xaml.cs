using Deployment.Core.Version;
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
    /// Interaction logic for VersionSelectionView.xaml
    /// </summary>
    public partial class VersionSelectionView : UserControl
    {
        private readonly VersionSelectionViewModel _viewModel;
        public VersionSelectionView(VersionSelectionViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        #region Events
        private void Version_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            VersionInfo? version = e.AddedItems[0] as VersionInfo;
            _viewModel.SelectVersion(version!);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Continue();
        }
        #endregion
    }
}
