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
    public partial class mainMenu : Page
    {
        Inventory i1 = new Inventory();
        public static Asset CurrentAsset { get; set; }

        public mainMenu()
        {
            this.InitializeComponent();
            i1.AddAsset("Omar's phone", "iPhone 7s", 4, 700, 22, true); //test code
            i1.AddAsset("Emilio's phone", "Samsung", 4, 500, 33, true); //test code
            i1.AddAsset("Amack's phone", "iPhone 8", 4, 809, 66, true); //test code
            DataAccess.InsertIntoTable(i1.listOfAssets);                //test code
            InventoryList.ItemsSource = i1.RetriveAllAssets();
        }

        
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //What's this?
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
        private void RemoveAllButtonClick(object sender, RoutedEventArgs e)
        {
            i1.ClearInventory();
            InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
        }

        /// <summary>
        /// Saves the database with the inventory shown on the mainMenu list and Exits the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            DataAccess.RemoveAllRows();
            DataAccess.InsertIntoTable(i1.listOfAssets);
            Application.Current.Exit();
        }

        /// <summary>
        /// Updates the database with the inventory that is seen on the mainMenu scroll list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            DataAccess.RemoveAllRows();
            DataAccess.InsertIntoTable(i1.listOfAssets);
        }

        private void ScanButtonClick(object sender, RoutedEventArgs e)
        {
            DataAccess.RemoveAllRows();
            DataAccess.InsertIntoTable(i1.listOfAssets);
            CurrentAsset = (Asset)InventoryList.SelectedItem;
        }

        private void PrintButtonClick(object sender, RoutedEventArgs e)
        {
            DataAccess.RemoveAllRows();
            DataAccess.InsertIntoTable(i1.listOfAssets);
            CurrentAsset = (Asset)InventoryList.SelectedItem;
            this.Frame.Navigate(typeof(BarcodeGenerator));
        }
    }
}
