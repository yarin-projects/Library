using BookLib;
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

namespace LibraryUI.Views.User.BoughtViews
{
    /// <summary>
    /// Represents a window displaying the details of a book that the user has bought.
    /// </summary>
    public partial class UserSelectedBoughtBookView : Window
    {
        // Instance of the selected book
        private Book _selectedBook;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSelectedBoughtBookView"/> class.
        /// </summary>
        /// <param name="item">The selected book.</param>
        public UserSelectedBoughtBookView(Book item)
        {
            InitializeComponent();
            _selectedBook = item;
            DisplayBookDetails();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
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
            string discount = GetDiscountText(_selectedBook.Discount);
            txtDiscount.Text = _selectedBook.DiscountActive ? discount : "";
            txtDiscountActive.Text = _selectedBook.DiscountActive ? "Discount Active" : "Discount Inactive";
            txtDiscountActive.Foreground = _selectedBook.DiscountActive ? Brushes.PaleGreen :
                                                                          new SolidColorBrush(Color.FromRgb(250, 95, 95));
        }

        /// <summary>
        /// Gets the discount text based on the discount percentage.
        /// </summary>
        /// <param name="discount">The discount percentage.</param>
        /// <returns>The discount text.</returns>
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
        /// Handles the click event of the Close button.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
