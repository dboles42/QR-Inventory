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
    /// A page used to search for assets in the list. Searching can be specified using the name,
    /// serial number, or model number of an asset.
    /// </summary>
    public sealed partial class Search : Page
    {
        /// <summary>
        /// Page constructor
        /// </summary>
    
        Inventory i1 = new Inventory();
        public Search()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// When this button is clicked, we get the three parameters from the text boxes and use them to find assets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            //Check if all the boxes are empty
            if(string.IsNullOrWhiteSpace(NameTextBox.Text) && string.IsNullOrWhiteSpace(SerialNumberTextBox.Text)
                && string.IsNullOrWhiteSpace(ModelNumberTextBox.Text))
            {
                flyoutText.Text = "You have to enter at least one parameter to search.";
                Flyout.ShowAttachedFlyout((FrameworkElement)sender);
                InventoryList.ItemsSource = null;       //Don't display anything in the list view

            }
            else
            {
                i1.listOfAssets = i1.FilterInventory(NameTextBox.Text, SerialNumberTextBox.Text, ModelNumberTextBox.Text);
                //Check if the list is empty
                if (i1.listOfAssets.Count == 0)
                {
                    flyoutText.Text = "We could not find any assets with the specified parameters.";
                    Flyout.ShowAttachedFlyout((FrameworkElement)sender);
                    InventoryList.ItemsSource = null;       //Don't display anything in the list view
                }
                else
                {
                    //Sort the list then show it
                    i1.SortInventory();
                    InventoryList.ItemsSource = i1.RetrieveAllAssets();
                }           
            }
        }

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

        /// <summary>
        /// Back button that sends the user to the mainMenu if clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(mainMenu));
        }
    }
}
