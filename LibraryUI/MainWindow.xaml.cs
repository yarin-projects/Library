using BookLib;
using LibraryUI.Views;
using System.Windows;
using MahApps.Metro.Controls;
using System;
using System.Windows.Media;
using LibraryUI.Views.Admin;
using BookLib.UserManagement;
using System.IO;
using BookLib.Models;
using LibraryUI.Views.User;
using System.Windows.Controls;
using LibraryUI.Views.Guest;
using System.Threading;

namespace LibraryUI
{
    /// <summary>
    /// Represents the main window of the application.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Instance of UserManager class for managing user data
        private UserManager _userManager = UserManager.Init;

        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtUserError.Visibility = Visibility.Hidden;

            // Set culture to en-US for consistent formatting
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        /// <summary>
        /// Handles the click event of the Close button to close the application.
        /// </summary>
        private void CloseApplicationClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the click event of the Guest Login button to open the guest view.
        /// </summary>
        private void GuestLoginClick(object sender, RoutedEventArgs e)
        {
            txtUserError.Visibility = Visibility.Hidden;
            var guestView = new GuestView();
            guestView.Owner = Application.Current.MainWindow;
            guestView.ShowDialog();
        }

        /// <summary>
        /// Handles the click event of the Account Login button to open the appropriate view based on user credentials.
        /// </summary>
        private void AccountLoginClick(object sender, RoutedEventArgs e)
        {
            if (_userManager.User != null)
            {
                _userManager.ClearUserData();
                _libCollection.ReloadLibDataFromFile();
            }
            txtUserError.Visibility = Visibility.Hidden;
            if (txtUsername.Text == "admin" && txtPassword.Password == "admin")
            {
                var adminView = new AdminView();
                adminView.Owner = Application.Current.MainWindow;
                adminView.ShowDialog();
                return;
            }
            if (Directory.Exists("Data/Users/" + txtUsername.Text + "_" + txtPassword.Password))
            {
                _userManager.AddUser(new User(txtUsername.Text, txtPassword.Password));
                var userView = new UserView();
                userView.Owner = Application.Current.MainWindow;
                userView.ShowDialog();
                return;
            }
            ShowErrorTxt(txtUserError, "Account doesn't exist");
        }

        /// <summary>
        /// Displays an error message in the specified TextBlock element.
        /// </summary>
        /// <param name="textBlock">The TextBlock element to display the error message.</param>
        /// <param name="msg">The error message to display.</param>
        private void ShowErrorTxt(TextBlock textBlock, string msg)
        {
            textBlock.Text = msg;
            textBlock.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the click event of the Register button to create a new user account.
        /// </summary>
        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            if (_userManager.User != null)
            {
                _userManager.ClearUserData();
                _libCollection.ReloadLibDataFromFile();
            }
            txtUserError.Visibility = Visibility.Hidden;
            if (txtUsername.Text.Contains("_"))
            {
                ShowErrorTxt(txtUserError, "Username can't contain the character '_'.");
                return;
            }
            string[] users = Directory.GetDirectories("Data/Users");
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].Remove(0, 11).Split('_')[0] == txtUsername.Text)
                {
                    ShowErrorTxt(txtUserError, "Account already exists");
                    return;
                }
            }
            if ((txtUsername.Text == "admin" && txtPassword.Password == "admin" ) || 
                Directory.Exists("Data/Users/" + txtUsername.Text + "_" + txtPassword.Password))
            {
                ShowErrorTxt(txtUserError, "Account already exists");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowErrorTxt(txtUserError, "Username and Password have to be at least 5 characters long and can't be empty");
                return;
            }
            if (txtUsername.Text.Length < 5 || txtPassword.Password.Length < 5)
            {
                ShowErrorTxt(txtUserError, "Username and Password have to be at least 5 characters long and can't be empty");
                return;
            }
            _userManager.AddUser(new User(txtUsername.Text, txtPassword.Password));
            var userView = new UserView();
            userView.Owner = Application.Current.MainWindow;
            userView.ShowDialog();
        }
    }
}
