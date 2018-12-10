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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagement
{
    public partial class mainMenu : Page
    {
        Inventory i1 = new Inventory();
        public static Asset CurrentAsset { get; set; }
        DataAccess DataAccessKey = new DataAccess();
        public mainMenu()
        {
            this.InitializeComponent();
            i1.AddAsset("Omar's phone", "iPhone 7s", 4, 700, "22", false); //test code
            i1.AddAsset("Emilio's phone", "Samsung", 4, 500, "33", true); //test code
            i1.AddAsset("Amack's phone", "iPhone 8", 4, 809, "66", true); //test code
            DataAccessKey.InsertIntoTable(i1.listOfAssets);                //test code
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
            DataAccessKey.RemoveAllRows();
            DataAccessKey.InsertIntoTable(i1.listOfAssets);
            this.Frame.Navigate(typeof(addAssetsPage));
        }

        private async void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if(InventoryList.SelectedItem == null)
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

        private async void RemoveAllButtonClick(object sender, RoutedEventArgs e)
        {
            MessageDialog msgbox = new MessageDialog("This will delete all assets in the inventory. Are you sure bro?");
            msgbox.Commands.Add(new UICommand { Label = "Yes bro", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "No bro", Id = 1 });
            msgbox.Commands.Add(new UICommand { Label = "Not sure bro", Id = 2 });
            var answer = await msgbox.ShowAsync();
            if((int) answer.Id == 0)
            {
                i1.ClearInventory();
                InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
                MessageDialog msgbox2 = new MessageDialog("Okay bro");
                await msgbox2.ShowAsync();
            }
            else if((int) answer.Id == 1)
            {
                MessageDialog msgbox2 = new MessageDialog("That's fine man");
                await msgbox2.ShowAsync();
            }
        }

        /// <summary>
        /// Saves the database with the inventory shown on the mainMenu list and Exits the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            DataAccessKey.RemoveAllRows();
            DataAccessKey.InsertIntoTable(i1.listOfAssets);
            Application.Current.Exit();
        }

        /// <summary>
        /// Updates the database with the inventory that is seen on the mainMenu scroll list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            DataAccessKey.RemoveAllRows();
            DataAccessKey.InsertIntoTable(i1.listOfAssets);
            MessageDialog msgbox = new MessageDialog("The database has been successfully updated.");
            await msgbox.ShowAsync();
        }

        private void ScanButtonClick(object sender, RoutedEventArgs e)
        {
            DataAccessKey.RemoveAllRows();
            DataAccessKey.InsertIntoTable(i1.listOfAssets);
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
                DataAccessKey.RemoveAllRows();
                DataAccessKey.InsertIntoTable(i1.listOfAssets);
                CurrentAsset = (Asset)InventoryList.SelectedItem;
                this.Frame.Navigate(typeof(BarcodeGenerator));
            }  
        }

        private void InventoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
