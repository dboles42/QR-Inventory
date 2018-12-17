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
                Inventory i1 = new Inventory();
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
