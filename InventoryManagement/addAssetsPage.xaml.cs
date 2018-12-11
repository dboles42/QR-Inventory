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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class addAssetsPage : Page
    {
        private string nameInput { get; set; }
        private string descripInput { get; set; }
        private double priceInput { get; set; }
        private int modelNumInput { get; set; }
        private string serialNumInput { get; set; }

        Inventory i1 = new Inventory();
        DataAccess DataAccessKey = new DataAccess("Asset");

        public addAssetsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// routes user back to inventory page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(mainMenu));
        }

        /// <summary>
        /// When add button is clicked, contents in textboxes are passed to Inventory asset list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            i1.AddAsset((string)nameTextBox.Text,(string)descriptionText.Text, priceText.Text.ToString(), int.Parse(modelnumText.Text), serialnumText.Text.ToString());
            DataAccessKey.RemoveAllRows();
            DataAccessKey.InsertListToTable(i1.listOfAssets);
            Frame.Navigate(typeof(mainMenu));
        }
    }
}
