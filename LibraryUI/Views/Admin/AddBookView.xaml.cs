using BookLib;
using BookLib.Models;
using BookLib.UserManagement;
using System;
using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryUI.Views.Admin
{
    /// <summary>
    /// Represents a window for adding or updating book entries in the library collection.
    /// </summary>
    public partial class AddBookView : Window
    {
        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;
        bool isUpdate = false;
        Book b;

        /// <summary>
        /// Initializes a new instance of the AddBookView class for adding a new book.
        /// </summary>
        public AddBookView()
        {
            InitializeComponent();
            InitializeData();
            txtSuccess.Text = "Book added succesfully to library!";
        }

        /// <summary>
        /// Initializes a new instance of the AddBookView class for updating existing book data.
        /// </summary>
        /// <param name="b1">The book to be updated.</param>
        public AddBookView(Book b1)
        {
            b = b1;
            InitializeComponent();
            InitializeData();
            Title = "Update Book Data";
            btnAdd.Content = "Update Book";
            txtTitle.Text = b1.Title;
            txtPrice.Text = b1.Price.ToString();
            datePickerInput.SelectedDate = b1.PublishDate;
            txtCopyNum.Text = b1.CopyNum.ToString();
            cmbBoxCategory.SelectedValue = b1.Category;
            switch (b1.Discount)
            {
                case 0:
                    cmbBoxDiscount.SelectedIndex = 0;
                    break;
                case 0.1:
                    cmbBoxDiscount.SelectedIndex = 1;
                    break;
                case 0.2:
                    cmbBoxDiscount.SelectedIndex = 2;
                    break;
                case 0.3:
                    cmbBoxDiscount.SelectedIndex = 3;
                    break;
                case 0.4:
                    cmbBoxDiscount.SelectedIndex = 4;
                    break;
                case 0.5:
                    cmbBoxDiscount.SelectedIndex = 5;
                    break;
            }
            isUpdate = true;
            if (b1.DiscountActive)
            {
                txtPrice.Text = b1.GetOriginalPrice().ToString();
                chkBoxDiscountActive.IsChecked = b1.DiscountActive;
            }
            txtSuccess.Text = "Book Data Updated Successfully!";
        }

        /// <summary>
        /// Initializes data and sets default values for view components.
        /// </summary>
        private void InitializeData()
        {
            datePickerInput.SelectedDate = DateTime.Now.AddDays(-1);
            datePickerInput.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddYears(-120)));
            datePickerInput.BlackoutDates.Add(new CalendarDateRange(DateTime.Now, DateTime.MaxValue));
            cmbBoxCategory.ItemsSource = Enum.GetValues(typeof(BookCategories));
            cmbBoxDiscount.Items.Add("None");
            cmbBoxDiscount.Items.Add("10%");
            cmbBoxDiscount.Items.Add("20%");
            cmbBoxDiscount.Items.Add("30%");
            cmbBoxDiscount.Items.Add("40%");
            cmbBoxDiscount.Items.Add("50%");
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            btnAdd.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            chkBoxDiscountActive.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows an error message in the specified TextBlock.
        /// </summary>
        /// <param name="textBlock">The TextBlock to display the error message.</param>
        /// <param name="msg">The error message to display.</param>
        private void ShowErrorTxt(TextBlock textBlock, string msg)
        {
            textBlock.Text = msg;
            textBlock.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Event handler for the click event of the "Add Book" button.
        /// Adds a new book or updates existing book data based on the input provided.
        /// </summary>
        private void AddBookClick(object sender, RoutedEventArgs e)
        {
            bool validated = true;
            bool isNum, isCopyNum;
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text.Length < 3)
            {
                ShowErrorTxt(txtTitleError, "'Title' has to be 3 characters long and can't be empty");
                validated = false;
            }
            else
                txtTitleError.Visibility = Visibility.Hidden;
            isNum = double.TryParse(txtPrice.Text, out double num);
            if (!isNum)
            {
                ShowErrorTxt(txtPriceError, "'Price' has to be a number");
                validated = false;
            }
            else
                txtPriceError.Visibility = Visibility.Hidden;
            if (num < 3 && isNum)
            {
                ShowErrorTxt(txtPriceError, $"'Price' has to be  bigger than {2.99:c2}");
                validated = false;
            }
            else if (isNum)
                txtPriceError.Visibility = Visibility.Hidden;
            isCopyNum = int.TryParse(txtCopyNum.Text, out int copyNum);
            if (!isCopyNum)
            {
                ShowErrorTxt(txtCopyNumError, "'Copy Number' has to be a number");
                validated = false;
            }
            else
                txtCopyNumError.Visibility = Visibility.Hidden;
            if (copyNum < 1 && isCopyNum)
            {
                ShowErrorTxt(txtCopyNumError, $"'Copy Number' has to be  bigger than 0");
                validated = false;
            }
            else if (isCopyNum)
                txtCopyNumError.Visibility = Visibility.Hidden;
            if (validated)
            {
                AddBook(num, copyNum);
            }
            else
                txtSuccess.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Adds a book to the library collection based on the provided data.
        /// If it's an update operation, remove the existing book and close the window after a delay
        /// </summary>
        /// <param name="num">The price of the book.</param>
        /// <param name="copyNum">The number of copies of the book.</param>
        private async void AddBook(double num, int copyNum)
        {
            string isbn;
            if (isUpdate)
                isbn = b.ISBN;
            else
                isbn = Guid.NewGuid().ToString().Remove(11);
            Enum.TryParse(cmbBoxCategory.SelectedValue.ToString(), out BookCategories result);
            Book b1 = new Book(txtTitle.Text.Trim(), datePickerInput.SelectedDate.Value,
                               copyNum, isbn, result, num, GetDiscount())
            { DiscountActive = (bool)chkBoxDiscountActive.IsChecked };
            _libCollection.Add(b1);
            txtSuccess.Visibility = Visibility.Visible;
            if (isUpdate)
            {
                _libCollection.Remove(b);
            }
            await Task.Delay(2000);
            Close();
        }

        /// <summary>
        /// Retrieves the discount value based on the selected discount option.
        /// </summary>
        /// <returns>The discount value as a double.</returns>
        private double GetDiscount()
        {
            switch (cmbBoxDiscount.SelectedIndex)
            {
                case 0:
                    chkBoxDiscountActive.Visibility = Visibility.Collapsed;
                    return 0;
                case 1:
                    chkBoxDiscountActive.Visibility = Visibility.Visible;
                    return 0.1;
                case 2:
                    chkBoxDiscountActive.Visibility = Visibility.Visible;
                    return 0.2;
                case 3:
                    chkBoxDiscountActive.Visibility = Visibility.Visible;
                    return 0.3;
                case 4:
                    chkBoxDiscountActive.Visibility = Visibility.Visible;
                    return 0.4;
                case 5:
                    chkBoxDiscountActive.Visibility = Visibility.Visible;
                    return 0.5;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Event handler for closing the window.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event handler for the selection change event of the discount ComboBox.
        /// Retrieves the discount value based on the selected discount option.
        /// </summary>
        private void cmbBoxDiscountSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetDiscount();
        }
    }
}
