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
using UserObj;
using DataAccessLibrary;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrationPage : Page
    {
        DataAccess LoginDataAccessKey = new DataAccess("Login");
        /// <summary>
        /// Registrations page constructor
        /// </summary>
        public RegistrationPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// When the username and password is filled it allows you to register an account with the database with respective permissions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            User Registered = new User();
            if (UsernameTextBox.Text.CompareTo("") == 0 ||
                PasswordText.Password.CompareTo("") == 0)
            {
                Flyout.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else
            {
                Registered.Username = UsernameTextBox.Text;
                Registered.Password = PasswordText.Password;
                Registered.ReadPermission = (bool)ReadCheck.IsChecked;
                Registered.WritePermission = (bool)WriteCheck.IsChecked;
                Registered.RemovePermission = (bool)RemoveCheck.IsChecked;
                LoginDataAccessKey.InsertUserToTable(Registered);
                this.Frame.Navigate(typeof(LoginPage));
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
