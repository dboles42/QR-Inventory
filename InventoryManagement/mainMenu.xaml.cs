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
using Windows.Storage.Streams;
using Windows.UI.Popups;
using AssetObj;
using DataAccessLibrary;
using UserObj;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Windows.Storage;

namespace InventoryManagement
{
    /// <summary>
    /// This page is the mainMenu. The user can view the inventory here and request operations
    /// </summary>
    public partial class mainMenu : Page
    {
        Inventory i1 = new Inventory();
        public static Asset CurrentAsset { get; set; }      //The currently selected asset
        public static User CurrUser { get; set; }
        DataAccess AssetDataAccessKey = new DataAccess("Asset");

        /// <summary>
        /// Page Constructor
        /// </summary>
        public mainMenu()
        {
            this.InitializeComponent();
            //Show the list view if the user has read permission
            if (mainMenu.CurrUser.ReadPermission)
            {
                InventoryList.ItemsSource = i1.RetriveAllAssets();
            }
        }

        /// <summary>
        /// Adds an asset when the button is clicked if the user has permission
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItemButtonClick(object sender, RoutedEventArgs e)
        {
            //If the user doesn't have write permission, show them that they don't have access
            if (!mainMenu.CurrUser.WritePermission)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            //Update the database then go to the addAssets page
            else
            {
                AssetDataAccessKey.RemoveAllRows();
                AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
                this.Frame.Navigate(typeof(addAssetsPage));
            }
        }

        /// <summary>
        /// Removes an asset when one is selected and the user has remove permissions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            //If the user doesn't have remove permission, show them that access is denied
            if (!mainMenu.CurrUser.RemovePermission)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else
            {
                //Check if the user hasn't selected an asset
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

        /// <summary>
        /// Removes all assets when the user has remove permissions and requests verification. Refreshes the page to show changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RemoveAllButtonClick(object sender, RoutedEventArgs e)
        {
            if (!mainMenu.CurrUser.RemovePermission)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else
            {
                MessageDialog msgbox = new MessageDialog("This will delete all assets in the inventory. Are you sure?");
                msgbox.Commands.Add(new UICommand { Label = "Yes, I am sure", Id = 0 });
                msgbox.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
                var answer = await msgbox.ShowAsync();
                if ((int)answer.Id == 0)
                {
                    i1.ClearInventory();
                    InventoryList.ItemsSource = i1.RetriveAllAssets();  //Refresh the List View
                    MessageDialog msgbox2 = new MessageDialog("All assets successfully deleted from the inventory");
                    await msgbox2.ShowAsync();
                }
                else if ((int)answer.Id == 1)
                {
                    MessageDialog msgbox2 = new MessageDialog("Operation Cancelled");
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
            //If the user doesnt have permissions, show them that access is denied
            if (!(mainMenu.CurrUser.RemovePermission || mainMenu.CurrUser.WritePermission))
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else
            {
                AssetDataAccessKey.RemoveAllRows();
                AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
                MessageDialog msgbox = new MessageDialog("The database has been successfully updated.");
                await msgbox.ShowAsync();
            }
        }

        /// <summary>
        /// If the respective print button is clicked the program navigates to BarCodeScanner page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanButtonClick(object sender, RoutedEventArgs e)
        {
            AssetDataAccessKey.RemoveAllRows();
            AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            CurrentAsset = (Asset)InventoryList.SelectedItem;
            this.Frame.Navigate(typeof(BarCodeScanner));
        }

        /// <summary>
        /// If the respective print button is clicked the program navigates to BarcodeGenerator page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Goes back to login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOffClick(object sender, RoutedEventArgs e)
        {
            //Update the database if the user has permissions
            if (CurrUser.WritePermission || CurrUser.RemovePermission)
            {
                AssetDataAccessKey.RemoveAllRows();
                AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            }
            this.Frame.Navigate(typeof(LoginPage));
        }

        /// <summary>
        /// Sends the user to the search page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            //Refresh the database then go to the search page
            AssetDataAccessKey.RemoveAllRows();
            AssetDataAccessKey.InsertListToTable(i1.listOfAssets);
            this.Frame.Navigate(typeof(Search));
        }

        /// <summary>
        /// Export Existing list to Excel using Epplus library.  Epplus requires filestream NOT StorageFIle. 
        /// Since path definitions for Filestreams are locked by
        /// security controls, I convert the StorageFile (which gives permissions in win10 to file) to system.IO.Filestream before
        /// saving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            int RowIndex = 1;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("AssetInventory");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            //Setup Header
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[RowIndex, 1].Value = "Name";
            workSheet.Cells[RowIndex, 2].Value = "Serial Number";
            workSheet.Cells[RowIndex, 3].Value = "Model Number";
            workSheet.Cells[RowIndex, 4].Value = "Description";
            workSheet.Cells[RowIndex, 5].Value = "Price";
            workSheet.Cells[RowIndex, 6].Value = "Check In";
            ++RowIndex; //bump row index before iterating through list

            foreach (Asset A in i1.listOfAssets)
            {
                workSheet.Cells[RowIndex, 1].Value = A.Name.ToString();
                workSheet.Cells[RowIndex, 2].Value = A.SerialNumber.ToString();
                workSheet.Cells[RowIndex, 3].Value = A.ModelNumber.ToString();
                workSheet.Cells[RowIndex, 4].Value = A.Description.ToString();
                workSheet.Cells[RowIndex, 5].Value = A.Price.ToString();
                workSheet.Cells[RowIndex, 6].Value = A.CheckIn.ToString();
                ++RowIndex;  //increment next row
            }

            string filename = "test.xlsx";
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile file = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                var stream = System.IO.File.Open(file.Path, FileMode.Open);
                bool check = stream is FileStream;
                excel.SaveAs(stream);
                stream.Close();
                var success = await Windows.System.Launcher.LaunchFileAsync(file);

            }
            catch (FileLoadException ex)
            {
                MessageDialog msgbox = new MessageDialog(ex.Message.ToString());
                await msgbox.ShowAsync();
            }
        }
    }
}
