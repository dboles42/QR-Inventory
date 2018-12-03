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
            i1.AddAsset("Omar's phone", "iPhone 7s", 4, 700, 22, true);
            i1.AddAsset("Emilio's phone", "Samsung", 4, 500, 33, true);
            i1.AddAsset("Amack's phone", "iPhone 2", 4, 809, 66, true);
            //Asset asset1 = new Asset("Omar's phone", "iPhone 7s", a, 700, 22, true);
            //InsertIntoTable(asset1);
            //Asset asset2 = new Asset("Emilio's phone", "Samsung", a, 500, 33, true);
            //InsertIntoTable(asset2);
            //Asset asset3 = new Asset("David's phone", "Nokia", a, 250, 44, true);
            //InsertIntoTable(asset3);
            //Asset asset4 = new Asset("Chris's phone", "Pixel 2", a, 707, 55, true);
            //InsertIntoTable(asset4);
            //Asset asset5 = new Asset("Amack's phone", "iPhone 2", a, 809, 66, true);
            //InsertIntoTable(asset5)
            InventoryList.ItemsSource = i1.RetriveAllAssets();
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

        private void AddItemButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(addAssetsPage));
        }
        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            Asset item = (Asset) InventoryList.SelectedItem;
            i1.RemoveAsset(item);
            InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
        }
        private void ClearAllButtonClick(object sender, RoutedEventArgs e)
        {
            i1.ClearInventory();
            InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
        }
    }
}
