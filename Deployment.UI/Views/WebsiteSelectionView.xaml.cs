using Deployment.Core.Website;
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
    /// Interaction logic for WebsiteSelectionView.xaml
    /// </summary>
    public partial class WebsiteSelectionView : UserControl
    {
        private readonly WebsiteSelectionViewModel _viewModel;
        public WebsiteSelectionView(WebsiteSelectionViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }

        #region Events
        private void Website_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            WebsiteInfo? website = e.AddedItems[0] as WebsiteInfo;
            _viewModel.SelectWebsite(website!);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Continue();
        }
        #endregion
    }
}
