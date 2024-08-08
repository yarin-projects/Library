using BookLib;
using BookLib.Exceptions;
using BookLib.Models;
using BookLib.UserManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LibraryUI.Views.User.BorrowedViews
{
    /// <summary>
    /// Interaction logic for displaying details of a borrowed book by the user.
    /// </summary>
    public partial class UserSelectedBorrowedBookView : Window
    {
        // Instance of the selected book
        private Book _selectedBook;

        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;

        // Instance of UserManager class for managing user data
        private UserManager _userManager = UserManager.Init;

        /// <summary>
        /// Constructor for the <see cref="UserSelectedBorrowedBookView"/> class.
        /// </summary>
        /// <param name="item">The selected book to display details for.</param>
        public UserSelectedBorrowedBookView(Book item)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            _selectedBook = item;
            DisplayBookDetails();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
        }

        /// <summary>
        /// Displays the details of the selected book.
        /// </summary>
        private void DisplayBookDetails()
        {
            Title = "Book: " + _selectedBook.Title;
            txtDate.Text = _selectedBook.PublishDate.ToString("d");
            txtCopyNum.Text = _selectedBook.CopiesOwned.ToString("n0");
            txtISBN.Text = _selectedBook.ISBN;
            txtCategory.Text = _selectedBook.Category.ToString();
            txtPrice.Text = _selectedBook.Price.ToString("c2");
            txtReturnDate.Text = _selectedBook.ReturnDate.ToString("d");
            string discount = GetDiscountText(_selectedBook.Discount);
            txtDiscount.Text = _selectedBook.DiscountActive ? discount : "";
            txtDiscountActive.Text = _selectedBook.DiscountActive ? "Discount Active" : "Discount Inactive";
            txtDiscountActive.Foreground = _selectedBook.DiscountActive ? Brushes.PaleGreen :
                                                                          new SolidColorBrush(Color.FromRgb(250, 95, 95));
        }

        /// <summary>
        /// Converts the discount value to its corresponding text representation.
        /// </summary>
        /// <param name="discount">The discount value to convert.</param>
        /// <returns>The text representation of the discount.</returns>
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
        /// Event handler for the Close button click event.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Delays for two seconds and then closes the window asynchronously.
        /// </summary>
        private async void WaitTwoSecondsAndCloseWindow()
        {
            await Task.Delay(2000);
            Close();
        }

        /// <summary>
        /// Event handler for the Buy button click event.
        /// </summary>
        private void BuyBookClick(object sender, RoutedEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnBuy.IsEnabled = false;
            _userManager.Remove(_selectedBook, false);
            try
            {
                List<AbstractItem> list = _userManager.GetItemByIsbnOrIssn(_selectedBook, true);
            }
            catch (ItemNotFoundException)
            {
                _userManager.Add(_selectedBook, true);
            }
            txtSuccess.Text = "Book bought successfully!";
            txtSuccess.Visibility = Visibility.Visible;
            WaitTwoSecondsAndCloseWindow();
        }

        /// <summary>
        /// Event handler for the Return button click event.
        /// </summary>
        private void ReturnBookClick(object sender, RoutedEventArgs e)
        {
            btnBorrow.IsEnabled = false;
            btnBuy.IsEnabled = false;
            _userManager.Remove(_selectedBook, false);
            Book b;
            List<AbstractItem> list;
            try
            {
                list = _userManager.GetItemByIsbnOrIssn(_selectedBook, true);
                list[0].CopiesOwned--;
            }
            catch (ItemNotFoundException) { }
            try
            {
                list = _libCollection.GetItemByIsbnOrIssn(_selectedBook);
                list[0].CopyNum++;
            }
            catch (ItemNotFoundException)
            {
                b = _selectedBook.CopyBookDetails();
                b.CopyNum++;
                _libCollection.Add(b);
            }
            txtSuccess.Text = "Book returned successfully!";
            txtSuccess.Visibility = Visibility.Visible;
            WaitTwoSecondsAndCloseWindow();
        }
    }
}
