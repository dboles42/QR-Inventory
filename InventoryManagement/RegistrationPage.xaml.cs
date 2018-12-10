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
        bool IsTextBoxEmpty { get; set; }
        DataAccess LoginDataAccessKey = new DataAccess("Login");
        public RegistrationPage()
        {
            this.InitializeComponent();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            User Registered = new User();
            if (UsernameTextBox.Text.CompareTo("") == 0 ||
                PasswordText.Password.CompareTo("") == 0)
            {
                IsTextBoxEmpty = true;
            }
            else
            {
                IsTextBoxEmpty = false;
                Registered.Username = UsernameTextBox.Text;
                Registered.Password = PasswordText.Password;
                Registered.ReadPermission = (bool)ReadCheck.IsChecked;
                Registered.WritePermission = (bool)WriteCheck.IsChecked;
                Registered.RemovePermission = (bool)RemoveCheck.IsChecked;
                LoginDataAccessKey.InsertUserToTable(Registered);
                this.Frame.Navigate(typeof(LoginPage));
            }
        }

        private void RegistrationButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsTextBoxEmpty == true)
            {
                Flyout.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }
    }
}
