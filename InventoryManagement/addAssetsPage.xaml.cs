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
using DataAccessLibrary;
using Windows.UI.Popups;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class addAssetsPage : Page
    {
        Inventory i1 = new Inventory();
        DataAccess DataAccessKey = new DataAccess("Asset");

        public addAssetsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// routes user back to the mainMenu page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DataAccessKey.RemoveAllRows();
            DataAccessKey.InsertListToTable(i1.listOfAssets);
            this.Frame.Navigate(typeof(mainMenu));
        }

        /// <summary>
        /// When add button is clicked, contents in textboxes are passed to Inventory asset list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                flyoutText.Text = "Please enter a name for the asset.";
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else if (string.IsNullOrWhiteSpace(descriptionText.Text))
            {
                flyoutText.Text = "Please enter a description for the asset.";
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else if( (string.IsNullOrWhiteSpace(priceText.Text)) || !(Double.TryParse(priceText.Text, out double price)))
            {
                flyoutText.Text = "Please enter a number for the price.";
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else if (string.IsNullOrWhiteSpace(serialnumText.Text))
            {
                flyoutText.Text = "Please enter a serial number for the asset.";
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else if ((string.IsNullOrWhiteSpace(priceText.Text)) || !(Int32.TryParse(modelnumText.Text, out int modelNum)))
            {
                flyoutText.Text = "Please enter the model number.";
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else
            {
                i1.AddAsset(nameTextBox.Text, descriptionText.Text,priceText.Text, modelNum, serialnumText.Text);
                MessageDialog msgbox = new MessageDialog("The asset has been successfully added.\nWould you like to add more assets?");
                msgbox.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
                msgbox.Commands.Add(new UICommand { Label = "No", Id = 1 });
                var answer = await msgbox.ShowAsync();
                if ((int)answer.Id == 1)
                {
                    DataAccessKey.RemoveAllRows();
                    DataAccessKey.InsertListToTable(i1.listOfAssets);
                    Frame.Navigate(typeof(mainMenu));
                }
                else
                {
                    //Reset the value of the text boxes
                    nameTextBox.Text = descriptionText.Text = priceText.Text = modelnumText.Text = serialnumText.Text = "";
                    flyoutText.Text = "Please enter the values for the new asset.";
                    FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                }
            }
        }
    }
}
