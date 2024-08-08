using BookLib;
using BookLib.Exceptions;
using BookLib.Models;
using BookLib.UserManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LibraryUI.Views.User
{
    /// <summary>
    /// Represents a window for displaying details of a selected journal and performing actions like borrowing or buying it.
    /// </summary>
    public partial class UserSelectedJournalView : Window
    {
        // Instance of the selected journal
        private Journal _selectedJournal;

        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;

        // Instance of UserManager class for managing user data
        private UserManager _userManager = UserManager.Init;

        /// <summary>
        /// Initializes a new instance of the UserSelectedJournalView class.
        /// </summary>
        /// <param name="item">The selected journal.</param>
        public UserSelectedJournalView(Journal item)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            _selectedJournal = item;
            DisplayJournalDetails();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            txtBorrowError.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Displays the details of the selected journal.
        /// </summary>
        private void DisplayJournalDetails()
        {
            Title = "Journal: " + _selectedJournal.Title;
            txtDate.Text = _selectedJournal.PublishDate.ToString("d");
            txtCopyNum.Text = _selectedJournal.CopyNum.ToString("n0");
            txtISSN.Text = _selectedJournal.ISSN;
            txtCategory.Text = _selectedJournal.Category.ToString();
            txtMonths.Text = _selectedJournal.Months.ToString();
            txtPrice.Text = _selectedJournal.Price.ToString("c2");
            txtDiscount.Text = _selectedJournal.DiscountActive ? "10%" : "";
            txtDiscountActive.Text = _selectedJournal.DiscountActive ? "Discount Active" : "Discount Inactive";
            txtDiscountActive.Foreground = _selectedJournal.DiscountActive ? Brushes.PaleGreen :
                                                                          new SolidColorBrush(Color.FromRgb(250, 95, 95));
        }

        /// <summary>
        /// Handles the click event of the Borrow Journal button.
        /// </summary>
        private void BorrowJournalClick(object sender, RoutedEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnBuy.IsEnabled = false;
            txtBorrowError.Visibility = Visibility.Collapsed;
            Journal j;
            List<AbstractItem> list;
            try
            {
                _userManager.GetItemByIsbnOrIssn(_selectedJournal, false);
                txtBorrowError.Text = "Journal already borrowed!";
                txtBorrowError.Visibility = Visibility.Visible;
                btnBorrow.IsEnabled = true;
                btnBuy.IsEnabled = true;
            }
            catch (ItemNotFoundException)
            {
                try
                {
                    list = _userManager.GetItemByIsbnOrIssn(_selectedJournal, true);
                    list[0].CopiesOwned++;
                    list[0].ReturnDate = DateTime.Now.AddDays(16);
                    _userManager.Add(list[0], false);
                }
                catch (ItemNotFoundException)
                {
                    j = _selectedJournal.CopyJournalDetails();
                    j.CopiesOwned++;
                    j.ReturnDate = DateTime.Now.AddDays(16);
                    _userManager.Add(j, false);
                }
                _libCollection.Remove(_selectedJournal);
                _selectedJournal.CopyNum--;
                if (_selectedJournal.CopyNum > 0)
                    _libCollection.Add(_selectedJournal);
                txtSuccess.Text = "Journal borrowed successfully!";
                txtSuccess.Visibility = Visibility.Visible;
                WaitTwoSecondsAndCloseWindow();
            }
        }

        /// <summary>
        /// Handles the click event of the Buy Journal button.
        /// </summary>
        private void BuyJournalClick(object sender, RoutedEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnBuy.IsEnabled = false;
            txtBorrowError.Visibility = Visibility.Collapsed;
            Journal j;
            List<AbstractItem> list;
            try
            {
                list = _userManager.GetItemByIsbnOrIssn(_selectedJournal, false);
                _userManager.Remove(list[0], false);
                j = (Journal)list[0];
                j.CopiesOwned++;
                _userManager.Add(j, false);
                try
                {
                    list = _userManager.GetItemByIsbnOrIssn(_selectedJournal, true);
                    _userManager.Remove(list[0], true);
                }
                catch (ItemNotFoundException) { }
                _userManager.Add(j, true);
            }
            catch (ItemNotFoundException)
            {
                try
                {
                    list = _userManager.GetItemByIsbnOrIssn(_selectedJournal, true);
                    list[0].CopiesOwned++;
                }
                catch (ItemNotFoundException)
                {
                    j = _selectedJournal.CopyJournalDetails();
                    j.CopiesOwned++;
                    _userManager.Add(j, true);
                }
            }
            _libCollection.Remove(_selectedJournal);
            _selectedJournal.CopyNum--;
            if (_selectedJournal.CopyNum > 0)
                _libCollection.Add(_selectedJournal);
            txtSuccess.Text = "Journal bought successfully!";
            txtSuccess.Visibility = Visibility.Visible;
            WaitTwoSecondsAndCloseWindow();
        }

        /// <summary>
        /// Waits for two seconds and closes the window.
        /// </summary>
        private async void WaitTwoSecondsAndCloseWindow()
        {
            await Task.Delay(2000);
            Close();
        }

        /// <summary>
        /// Handles the click event of the Close Window button.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
