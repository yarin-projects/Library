using BookLib;
using BookLib.Exceptions;
using BookLib.Models;
using BookLib.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryUI.Views.User.BorrowedViews
{
    /// <summary>
    /// Interaction logic for displaying details of a borrowed journal by the user.
    /// </summary>
    public partial class UserSelectedBorrowedJournalView : Window
    {
        // Instance of the selected journal
        private Journal _selectedJournal;

        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;

        // Instance of UserManager class for managing user data
        private UserManager _userManager = UserManager.Init;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSelectedBorrowedJournalView"/> class.
        /// </summary>
        /// <param name="item">The selected journal item.</param>
        public UserSelectedBorrowedJournalView(Journal item)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            _selectedJournal = item;
            DisplayJournalDetails();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
        }

        /// <summary>
        /// Displays the details of the selected journal.
        /// </summary>
        private void DisplayJournalDetails()
        {
            Title = "Journal: " + _selectedJournal.Title;
            txtDate.Text = _selectedJournal.PublishDate.ToString("d");
            txtCopyNum.Text = _selectedJournal.CopiesOwned.ToString("n0");
            txtISSN.Text = _selectedJournal.ISSN;
            txtCategory.Text = _selectedJournal.Category.ToString();
            txtMonths.Text = _selectedJournal.Months.ToString();
            txtPrice.Text = _selectedJournal.Price.ToString("c2");
            txtReturnDate.Text = _selectedJournal.ReturnDate.ToString("d");
            txtDiscount.Text = _selectedJournal.DiscountActive ? "10%" : "";
            txtDiscountActive.Text = _selectedJournal.DiscountActive ? "Discount Active" : "Discount Inactive";
            txtDiscountActive.Foreground = _selectedJournal.DiscountActive ? Brushes.PaleGreen :
                                                                          new SolidColorBrush(Color.FromRgb(250, 95, 95));
        }

        /// <summary>
        /// Closes the window when the close button is clicked.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Asynchronously waits for two seconds and then closes the window.
        /// </summary>
        private async void WaitTwoSecondsAndCloseWindow()
        {
            await Task.Delay(2000);
            Close();
        }

        /// <summary>
        /// Handles the event when the user clicks the "Buy" button for the journal.
        /// </summary>
        private void BuyJournalClick(object sender, RoutedEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnBuy.IsEnabled = false;
            _userManager.Remove(_selectedJournal, false);
            try
            {
                List<AbstractItem> list = _userManager.GetItemByIsbnOrIssn(_selectedJournal, true);
            }
            catch (ItemNotFoundException)
            {
                _userManager.Add(_selectedJournal, true);
            }
            txtSuccess.Text = "Journal bought successfully!";
            txtSuccess.Visibility = Visibility.Visible;
            WaitTwoSecondsAndCloseWindow();
        }

        /// <summary>
        /// Handles the event when the user clicks the "Return" button for the journal.
        /// </summary>
        private void ReturnJournalClick(object sender, RoutedEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnBuy.IsEnabled = false;
            _userManager.Remove(_selectedJournal, false);
            Journal j;
            List<AbstractItem> list;
            try
            {
                list = _userManager.GetItemByIsbnOrIssn(_selectedJournal, true);
                list[0].CopiesOwned--;
            }
            catch (ItemNotFoundException) { }
            try
            {
                list = _libCollection.GetItemByIsbnOrIssn(_selectedJournal);
                list[0].CopyNum++;
            }
            catch (ItemNotFoundException)
            {
                j = _selectedJournal.CopyJournalDetails();
                j.CopyNum++;
                _libCollection.Add(j);
            }
            txtSuccess.Text = "Journal returned successfully!";
            txtSuccess.Visibility = Visibility.Visible;
            WaitTwoSecondsAndCloseWindow();
        }
    }
}
