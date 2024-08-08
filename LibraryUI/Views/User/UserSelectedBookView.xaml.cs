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
    /// Interaction logic for displaying details of a selected book and performing actions like borrowing or buying it.
    /// </summary>
    public partial class UserSelectedBookView : Window
    {
        // Instance of the selected book
        private Book _selectedBook;

        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;
        
        // Instance of UserManager class for managing user data
        private UserManager _userManager = UserManager.Init;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSelectedBookView"/> class.
        /// </summary>
        /// <param name="item">The selected book.</param>
        public UserSelectedBookView(Book item)
        {
            InitializeComponent();
            _selectedBook = item;
            DisplayBookDetails();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            txtBorrowError.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Displays details of the selected book on the UI.
        /// </summary>
        private void DisplayBookDetails()
        {
            Title = "Book: " + _selectedBook.Title;
            txtDate.Text = _selectedBook.PublishDate.ToString("d");
            txtCopyNum.Text = _selectedBook.CopyNum.ToString("n0");
            txtISBN.Text = _selectedBook.ISBN;
            txtCategory.Text = _selectedBook.Category.ToString();
            txtPrice.Text = _selectedBook.Price.ToString("c2");
            string discount = GetDiscountText(_selectedBook.Discount);
            txtDiscount.Text = _selectedBook.DiscountActive ? discount : "";
            txtDiscountActive.Text = _selectedBook.DiscountActive ? "Discount Active" : "Discount Inactive";
            txtDiscountActive.Foreground = _selectedBook.DiscountActive ? Brushes.PaleGreen : 
                                                                          new SolidColorBrush(Color.FromRgb(250, 95, 95));
        }

        /// <summary>
        /// Gets the text representation of a discount percentage based on the given discount value.
        /// </summary>
        /// <param name="discount">The discount value.</param>
        /// <returns>The text representation of the discount percentage.</returns>
        private string GetDiscountText(double discount)
        {
            switch (discount)
            {
                case 0.1:
                    return "10%";
                case 0.2:
                    return "20%";
                case 0.3:
                    return "30%";
                case 0.4:
                    return "40%";
                case 0.5:
                    return "50%";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Handles the event when the user clicks the "Borrow" button.
        /// </summary>
        private void BorrowBookClick(object sender, RoutedEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnBuy.IsEnabled = false;
            txtBorrowError.Visibility = Visibility.Collapsed;
            Book b;
            List<AbstractItem> list;
            try
            {
                _userManager.GetItemByIsbnOrIssn(_selectedBook, false);
                txtBorrowError.Text = "Book already borrowed!";
                txtBorrowError.Visibility = Visibility.Visible;
                btnBorrow.IsEnabled = true;
                btnBuy.IsEnabled = true;
            }
            catch (ItemNotFoundException)
            {
                try
                {
                    list = _userManager.GetItemByIsbnOrIssn(_selectedBook, true);
                    list[0].CopiesOwned++;
                    list[0].ReturnDate = DateTime.Now.AddDays(16);
                    _userManager.Add(list[0], false);
                }
                catch (ItemNotFoundException)
                {
                    b = _selectedBook.CopyBookDetails();
                    b.CopiesOwned++;
                    b.ReturnDate = DateTime.Now.AddDays(16);
                    _userManager.Add(b, false);
                }
                _libCollection.Remove(_selectedBook);
                _selectedBook.CopyNum--;
                if (_selectedBook.CopyNum > 0)
                    _libCollection.Add(_selectedBook);
                txtSuccess.Text = "Book borrowed successfully!";
                txtSuccess.Visibility = Visibility.Visible;
                WaitTwoSecondsAndCloseWindow();
            }
        }

        /// <summary>
        /// Handles the event when the user clicks the "Buy" button.
        /// </summary>
        private void BuyBookClick(object sender, RoutedEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnBuy.IsEnabled = false;
            txtBorrowError.Visibility = Visibility.Collapsed;
            Book b;
            List<AbstractItem> list;
            try
            {
                list = _userManager.GetItemByIsbnOrIssn(_selectedBook, false);
                _userManager.Remove(list[0], false);
                b = (Book)list[0];
                b.CopiesOwned++;
                _userManager.Add(b, false);
                try
                {
                    list = _userManager.GetItemByIsbnOrIssn(_selectedBook, true);
                    _userManager.Remove(list[0], true);
                }
                catch (ItemNotFoundException) { }
                _userManager.Add(b, true);
            }
            catch (ItemNotFoundException)
            {
                try
                {
                    list = _userManager.GetItemByIsbnOrIssn(_selectedBook, true);
                    list[0].CopiesOwned++;
                }
                catch (ItemNotFoundException)
                {
                    b = _selectedBook.CopyBookDetails();
                    b.CopiesOwned++;
                    _userManager.Add(b, true);
                }
            }
            _libCollection.Remove(_selectedBook);
            _selectedBook.CopyNum--;
            if (_selectedBook.CopyNum > 0)
                _libCollection.Add(_selectedBook);
            txtSuccess.Text = "Book bought successfully!";
            txtSuccess.Visibility = Visibility.Visible;
            WaitTwoSecondsAndCloseWindow();
        }

        /// <summary>
        /// Delays closing the window by two seconds before closing it.
        /// </summary>
        private async void WaitTwoSecondsAndCloseWindow()
        {
            await Task.Delay(2000);
            Close();
        }

        /// <summary>
        /// Handles the event when the user clicks the "Close" button to close the window.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
