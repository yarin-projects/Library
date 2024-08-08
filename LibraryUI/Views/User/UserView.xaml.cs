using BookLib;
using BookLib.UserManagement;
using LibraryUI.Views.User.BorrowedViews;
using LibraryUI.Views.User.BoughtViews;
using System.Windows;
using System.Windows.Media;

namespace LibraryUI.Views.User
{
    /// <summary>
    /// Represents the main window for a user's view in the application.
    /// </summary>
    public partial class UserView : Window
    {
        // Instance of UserManager class for managing user data
        private UserManager _userManager = UserManager.Init;

        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;

        /// <summary>
        /// Initializes a new instance of the UserView class.
        /// </summary>
        public UserView()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            txtDeleteAccountError.Visibility = Visibility.Collapsed;

            // Set window title to include the username of the current user
            Title += _userManager.User.Username;
        }

        /// <summary>
        /// Handles the click event of the View Bought Collection button to open the bought collection view.
        /// </summary>
        private void ViewBoughtCollectionClick(object sender, RoutedEventArgs e)
        {
            var boughtCollectionView = new BoughtCollectionView();
            boughtCollectionView.Owner = Application.Current.MainWindow;
            boughtCollectionView.ShowDialog();
        }

        /// <summary>
        /// Handles the click event of the View Borrowed Collection button to open the borrowed collection view.
        /// </summary>
        private void ViewBorrowedCollectionClick(object sender, RoutedEventArgs e)
        {
            var borrowedCollectionView = new BorrowedCollectionView();
            borrowedCollectionView.Owner = Application.Current.MainWindow;
            borrowedCollectionView.ShowDialog();
        }

        /// <summary>
        /// Handles the click event of the Browse Collection button to open the browse collection view.
        /// </summary>
        private void BrowseCollectionClick(object sender, RoutedEventArgs e)
        {
            var browseCollectionView = new BrowseCollectionView();
            browseCollectionView.Owner = Application.Current.MainWindow;
            browseCollectionView.ShowDialog();
        }

        /// <summary>
        /// Handles the click event of the Close button to clear user data and close the window.
        /// </summary>
        private void CloseApplicationClick(object sender, RoutedEventArgs e)
        {
            _userManager.ClearUserData();
            _libCollection.ReloadLibDataFromFile();
            Close();
        }

        /// <summary>
        /// Handles the click event of the Delete Account Data button to delete user account data.
        /// </summary>
        private void DeleteAccountDataClick(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", 
                                                                MessageBoxButton.YesNo);
            
            //Check if user has borrowed items before deleting account
            if (_userManager.CountBorrowed > 0)
            {
                txtDeleteAccountError.Visibility = Visibility.Visible;
                if (_userManager.CountBorrowed == 1)
                    txtDeleteAccountError.Text = "Can't delete an account with a borrowed book!\nReturn the book or buy it.";
                else
                    txtDeleteAccountError.Text = $"Can't delete an account with borrowed books!\nReturn the books or buy them." +
                                                 $"\n{_userManager.CountBorrowed:n0} currently are borrowed in the system.";
                return;
            }
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // If no borrowed items, delete user account and reload library collection data from file
                _userManager.DeleteUserAccount();
                _libCollection.ReloadLibDataFromFile();
                Close();
            }
        }
    }
}
