using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using AssetObj;
using DataAccessLibrary;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class mainMenu : Page
    {
        Inventory i1 = new Inventory();
        public ObservableCollection<Asset> listItems { get; } = new ObservableCollection<Asset>();
        //public ListView itemListView = new ListView();

        public mainMenu()
        {
            this.InitializeComponent();
            InventoryList.ItemsSource = DataAccess.RetriveAllAssets();
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// simple button that takes user to first page if needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BarCodeScanner));
        }

        private void w1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView l1 = sender as ListView;
            string selected = l1.SelectedItem.ToString();
            //MessageDialog dlg = new MessageDialog("selected color: " + selected);

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(addAssetsPage));
        }

        private void InventoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
