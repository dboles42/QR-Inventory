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
    public sealed partial class LoginPage : Page
    {
        DataAccess LoginDataAccessKey = new DataAccess("Login");
        bool HasAccess { get; set; }
        /// <summary>
        /// Constructor for the Login Page
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegistrationPage));
        }
        /// <summary>
        /// Checks the database to see if the user has valid information to login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.CurrUser = LoginDataAccessKey.getUser(UsernameTextBox.Text,PasswordText.Password);
            if(mainMenu.CurrUser is null)
            {
                HasAccess = false;
            }
            else
            {
                HasAccess = true;
                this.Frame.Navigate(typeof(mainMenu));
            }
        }
        /// <summary>
        /// Shows Access Denied if the user doesnt enter valid information to login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (HasAccess == false)
            {
                Flyout.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }


    }
}
