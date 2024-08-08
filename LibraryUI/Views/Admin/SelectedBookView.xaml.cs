using BookLib.Models;
using BookLib;
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
using BookLib.UserManagement;

namespace LibraryUI.Views.Admin
{
    /// <summary>
    /// Represents a window for displaying details of a selected book item in the admin interface.
    /// </summary>
    public partial class SelectedBookView : Window
    {
        // Instance of the selected book
        private Book _selectedBook;

        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;

        /// <summary>
        /// Initializes a new instance of the SelectedBookView class with the specified book item.
        /// </summary>
        /// <param name="item">The book item to display.</param>
        public SelectedBookView(AbstractItem item)
        {
            InitializeComponent();
            _selectedBook = (Book)item;
            DisplayBookDetails();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        /// <summary>
        /// Displays information about the selected book item.
        /// </summary>
        private void DisplayBookDetails()
        {
            // Enable discount toggle button by default
            btnToggleDiscount.IsEnabled = true;

            // Set window title to include book title
            Title = "Book: " + _selectedBook.Title;

            // Display book details
            txtDate.Text = _selectedBook.PublishDate.ToString("d");
            txtCopyNum.Text = _selectedBook.CopyNum.ToString("n0");
            txtISBN.Text = _selectedBook.ISBN;
            txtCategory.Text = _selectedBook.Category.ToString();
            txtPrice.Text = _selectedBook.Price.ToString("c2");

            //Determine and display discount information
            string discount = "";
            switch (_selectedBook.Discount)
            {
                case 0:
                    discount = "None";
                    break;
                case 0.1:
                    discount = "10%";
                    break;
                case 0.2:
                    discount = "20%";
                    break;
                case 0.3:
                    discount = "30%";
                    break;
                case 0.4:
                    discount = "40%";
                    break;
                case 0.5:
                    discount = "50%";
                    break;
            }
            txtDiscount.Text = discount;

            //Set toggle discount button content and availability based on discount status
            if (_selectedBook.Discount == 0)
            {
                btnToggleDiscount.Content = "Discount Unavailable";
                btnToggleDiscount.IsEnabled = false;
            }
            else
            {
                btnToggleDiscount.Content = _selectedBook.DiscountActive ? "Deactivate Discount" : "Activate Discount";
                btnToggleDiscount.Background = _selectedBook.DiscountActive ? new SolidColorBrush(Color.FromRgb(250, 95, 95)) :
                                                                   Brushes.PaleGreen;
            }
        }

        /// <summary>
        /// Event handler for the Remove Book button click.
        /// Removes the selected book from the library collection.
        /// </summary>
        private void RemoveBookClick(object sender, RoutedEventArgs e)
        {
            _libCollection.Remove(_selectedBook);
            Close();
        }

        /// <summary>
        /// Event handler for the Update Book button click.
        /// Opens the AddBookView window for updating the details of the selected book.
        /// </summary>
        private void UpdateBookClick(object sender, RoutedEventArgs e)
        {
            var addBookView = new AddBookView(_selectedBook);
            addBookView.ShowDialog();
            Close();
        }

        /// <summary>
        /// Event handler for the Toggle Discount button click.
        /// Toggles the discount status of the selected book and updates the display accordingly.
        /// </summary>
        private void ToggleDiscountClick(object sender, RoutedEventArgs e)
        {
            _libCollection.Remove(_selectedBook);
            _selectedBook.DiscountActive = !_selectedBook.DiscountActive;
            _libCollection.Add(_selectedBook);
            DisplayBookDetails();
        }

        /// <summary>
        /// Event handler for closing the window.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
