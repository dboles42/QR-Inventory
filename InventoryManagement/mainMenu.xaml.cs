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
using UserObj;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagement
{
    public partial class mainMenu : Page
    {
        Inventory i1 = new Inventory();
        public static Asset CurrentAsset { get; set; }
        public static User CurrUser { get; set; }
        DataAccess AssetDataAccessKey = new DataAccess("Asset");
        public mainMenu()
        {
            this.InitializeComponent();
            if ((bool)mainMenu.CurrUser.ReadPermission)
            {
                InventoryList.ItemsSource = i1.RetriveAllAssets();
            }
            
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
            if (mainMenu.CurrUser.WritePermission)
            {
                AssetDataAccessKey.RemoveAllRows();
                AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
                this.Frame.Navigate(typeof(addAssetsPage));
            }
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (mainMenu.CurrUser.RemovePermission)
            {
                Asset item = (Asset)InventoryList.SelectedItem;
                i1.RemoveAsset(item);
                InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
            }
        }
        private void RemoveAllButtonClick(object sender, RoutedEventArgs e)
        {
            if (mainMenu.CurrUser.RemovePermission)
            {
                i1.ClearInventory();
                InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
            }
        }

        /// <summary>
        /// Saves the database with the inventory shown on the mainMenu list and Exits the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            AssetDataAccessKey.RemoveAllRows();
            AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            Application.Current.Exit();
        }

        /// <summary>
        /// Updates the database with the inventory that is seen on the mainMenu scroll list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            if (mainMenu.CurrUser.WritePermission || mainMenu.CurrUser.RemovePermission)
            {
                AssetDataAccessKey.RemoveAllRows();
                AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            }
        }

        private void ScanButtonClick(object sender, RoutedEventArgs e)
        {
            AssetDataAccessKey.RemoveAllRows();
            AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            CurrentAsset = (Asset)InventoryList.SelectedItem;
            this.Frame.Navigate(typeof(BarCodeScanner));
        }

        private void PrintButtonClick(object sender, RoutedEventArgs e)
        {
            AssetDataAccessKey.RemoveAllRows();
            AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            CurrentAsset = (Asset)InventoryList.SelectedItem;
            this.Frame.Navigate(typeof(BarcodeGenerator));
        }

        private void UpdateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(mainMenu.CurrUser.WritePermission || mainMenu.CurrUser.RemovePermission))
            {
                Flyout.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }

        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!mainMenu.CurrUser.RemovePermission)
            {
                Flyout.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }

        private void AddItemButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(!mainMenu.CurrUser.WritePermission)
            {
                Flyout.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
