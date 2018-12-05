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

using InventoryManagement;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagement
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class addAssetsPage : Page
    {

        /// <summary>
        /// variables used to create new Asset based on userInput on AddAsset page
        /// </summary>
        private string nameInput { get; set; }
        private string descriptionInput { get; set; }
        private string idNumInput { get; set; }
        private double priceInput { get; set; }
        private int modelNumInput { get; set; }
        private int serialNumInput { get; set; }
        
        public addAssetsPage()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Will add inputed information in textboxes to inventory list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
           //i1.AddAsset(nameInput, descriptionInput, modelNumInput, serialNumInput);
        }

        /// <summary>
        /// Back button reverts user to mainpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Clicl(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(mainMenu));
        }

        /// <summary>
        /// saves text inputted to NameTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Name_SelectionChanged(object sender, RoutedEventArgs e)
        {
            nameInput = nameTextBox.Text;
        }

        /// <summary>
        /// Saves text from Description text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DescriptionText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            descriptionInput = descriptionText.Text;
        }

        /// <summary>
        /// saves text from price textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriceText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            priceInput = double.Parse(priceText.Text);
        }

        /// <summary>
        /// saves text from model number text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelNumberText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            modelNumInput = int.Parse(modelnumText.Text);
        }

        /// <summary>
        /// saves text in serial number textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialNumberText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            serialNumInput = int.Parse(serialnumText.Text);
        }

    }
}
