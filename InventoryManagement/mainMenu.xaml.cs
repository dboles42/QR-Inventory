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
using Windows.UI.Popups;
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
            if (mainMenu.CurrUser.ReadPermission)
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

        private async void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (mainMenu.CurrUser.RemovePermission)
            {
                if (InventoryList.SelectedItem == null)
                {
                    MessageDialog msgbox = new MessageDialog("You have to select an item before executing this command.");
                    await msgbox.ShowAsync();
                }
                else
                {
                    i1.RemoveAsset((Asset)InventoryList.SelectedItem);
                    InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
                    MessageDialog msgbox = new MessageDialog("The asset has been successfully removed.");
                    await msgbox.ShowAsync();
                }
            }
        }

        private async void RemoveAllButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrUser.RemovePermission)
            {
                MessageDialog msgbox = new MessageDialog("This will delete all assets in the inventory. Are you sure bro?");
                msgbox.Commands.Add(new UICommand { Label = "Yes bro", Id = 0 });
                msgbox.Commands.Add(new UICommand { Label = "No bro", Id = 1 });
                msgbox.Commands.Add(new UICommand { Label = "Not sure bro", Id = 2 });
                var answer = await msgbox.ShowAsync();
                if ((int)answer.Id == 0)
                {
                    i1.ClearInventory();
                    InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
                    MessageDialog msgbox2 = new MessageDialog("Okay bro");
                    await msgbox2.ShowAsync();
                }
                else if ((int)answer.Id == 1)
                {
                    MessageDialog msgbox2 = new MessageDialog("That's fine man");
                    await msgbox2.ShowAsync();
                }
            }
        }

        /// <summary>
        /// Saves the database with the inventory shown on the mainMenu list and Exits the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrUser.WritePermission || CurrUser.RemovePermission)
            {
                AssetDataAccessKey.RemoveAllRows();
                AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            }
            Application.Current.Exit();
        }

        /// <summary>
        /// Updates the database with the inventory that is seen on the mainMenu scroll list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrUser.WritePermission || CurrUser.RemovePermission)
            {
                AssetDataAccessKey.RemoveAllRows();
                AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
                MessageDialog msgbox = new MessageDialog("The database has been successfully updated.");
                await msgbox.ShowAsync();
            }
        }

        private void ScanButtonClick(object sender, RoutedEventArgs e)
        {
            AssetDataAccessKey.RemoveAllRows();
            AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            CurrentAsset = (Asset)InventoryList.SelectedItem;
            this.Frame.Navigate(typeof(BarCodeScanner));
        }

        private async void PrintButtonClick(object sender, RoutedEventArgs e)
        {
            if (InventoryList.SelectedItem == null)
            {
                MessageDialog msgbox = new MessageDialog("You have to select an item before executing this command.");
                await msgbox.ShowAsync();
            }
            else
            {
                AssetDataAccessKey.RemoveAllRows();
                AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
                CurrentAsset = (Asset)InventoryList.SelectedItem;
                this.Frame.Navigate(typeof(BarcodeGenerator));
            }
        }

        private void AddItemButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!mainMenu.CurrUser.WritePermission)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }

        private void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!mainMenu.CurrUser.RemovePermission)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }

        private void UpdateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(mainMenu.CurrUser.RemovePermission || mainMenu.CurrUser.WritePermission))
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
