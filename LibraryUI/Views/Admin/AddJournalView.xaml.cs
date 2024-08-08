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
    /// Represents a window for adding or updating journal entries in the library collection.
    /// </summary>
    public partial class AddJournalView : Window
    {
        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;
        private Months months;
        private string[] monthsArr = Enum.GetNames(typeof(Months));
        private bool[] monthBoolFlags = { false, false, false, false, false, false, false, false, false, false, false, false };
        bool isUpdate = false;
        Journal j;

        /// <summary>
        /// Default constructor for creating a new instance of AddJournalView.
        /// Initializes the view and sets default values.
        /// </summary>
        public AddJournalView()
        {
            InitializeComponent();
            InitializeData();
            txtSuccess.Text = "Journal added succesfully to library!";
        }

        /// <summary>
        /// Constructor for creating a new instance of AddJournalView for updating existing journal data.
        /// Initializes the view with existing data.
        /// </summary>
        /// <param name="j1">The journal object containing existing data to be updated.</param>
        public AddJournalView(Journal j1)
        {
            j = j1;
            InitializeComponent();
            InitializeData();
            Title = "Update Journal Data";
            btnAdd.Content = "Update Journal";
            txtTitle.Text = j1.Title;
            txtPrice.Text = j1.Price.ToString();
            datePickerInput.SelectedDate = j1.PublishDate;
            txtCopyNum.Text = j1.CopyNum.ToString();
            cmbBoxCategory.SelectedValue = j1.Category;
            for (int i = 0; i < monthBoolFlags.Length; i++)
            {
                Enum.TryParse(monthsArr[i], out Months result);
                if (j1.Months.HasFlag(result))
                    monthBoolFlags[i] = true;
            }
            UpdateMonths();
            txtSuccess.Text = "Journal Data Updated Successfully!";
            isUpdate = true;
            if (j1.DiscountActive)
            {
                txtPrice.Text = j1.GetOriginalPrice().ToString();
                chkBoxDiscountActive.IsChecked = j1.DiscountActive;
            }
        }

        /// <summary>
        /// Initializes data and sets default values for view components.
        /// </summary>
        private void InitializeData()
        {
            datePickerInput.SelectedDate = DateTime.Now.AddDays(-1);
            datePickerInput.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddYears(-120)));
            datePickerInput.BlackoutDates.Add(new CalendarDateRange(DateTime.Now, DateTime.MaxValue));
            cmbBoxCategory.ItemsSource = Enum.GetValues(typeof(JournalCategories));
            cmbBoxMonths.ItemsSource = Enum.GetValues(typeof(Months));
            var greyColor = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            btnAdd.Background = greyColor;
            btnAddMonth.Background = greyColor;
            btnRemoveMonth.Background = greyColor;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
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
        /// Event handler for adding a journal entry.
        /// Validates input fields and adds a new journal to the library collection.
        /// </summary>
        private void AddJournalClick(object sender, RoutedEventArgs e)
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
                ShowErrorTxt(txtPriceError, $"'Price' has to be  bigger than {2.99:c}");
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
            if (months <= 0)
            {
                ShowErrorTxt(txtMonthError, "Must select atleast 1 month!");
                validated = false;
            }
            else
                txtMonthError.Visibility = Visibility.Hidden;
            if (validated)
            {
                AddJournal(num, copyNum);
            }
            else
                txtSuccess.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Adds a journal to the library collection based on the provided data.
        /// If it's an update operation, remove the existing journal and close the window after a delay
        /// </summary>
        /// <param name="num">The price of the journal.</param>
        /// <param name="copyNum">The number of copies of the journal.</param>
        private async void AddJournal(double num, int copyNum)
        {
            string issn;
            if (isUpdate)
                issn = j.ISSN;
            else
                issn = Guid.NewGuid().ToString().Remove(11);
            Enum.TryParse(cmbBoxCategory.SelectedValue.ToString(), out JournalCategories result);
            Journal j1 = new Journal(txtTitle.Text.Trim(), datePickerInput.SelectedDate.Value,
                               copyNum, result, months, num, issn)
            { DiscountActive = (bool)chkBoxDiscountActive.IsChecked };
            _libCollection.Add(j1);
            txtSuccess.Visibility = Visibility.Visible;
            if (isUpdate)
                _libCollection.Remove(j);
            await Task.Delay(2000);
            Close();
        }

        /// <summary>
        /// Event handler for adding a month to the journal entry.
        /// </summary>
        private void AddMonthClick(object sender, RoutedEventArgs e)
        {
            monthBoolFlags[cmbBoxMonths.SelectedIndex] = true;
            UpdateMonths();
        }

        /// <summary>
        /// Event handler for removing a month from the journal entry.
        /// </summary>
        private void RemoveMonthClick(object sender, RoutedEventArgs e)
        {
            monthBoolFlags[cmbBoxMonths.SelectedIndex] = false;
            UpdateMonths();
        }

        /// <summary>
        /// Updates the list of months based on user selection.
        /// </summary>
        private void UpdateMonths()
        {
            txtMonths.Text = "";
            int counter = 1;
            months = 0;
            for (int i = 0; i < monthBoolFlags.Length; i++)
            {
                if (monthBoolFlags[i])
                {
                    Enum.TryParse(monthsArr[i], out Months result);
                    months |= result;
                    if (txtMonths.Text.Length > 27 * counter)
                    {
                        counter++;
                        txtMonths.Text += "\n" + monthsArr[i] + ", ";
                    }
                    else
                        txtMonths.Text += monthsArr[i] + ", ";
                }
            }
            if (txtMonths.Text.Length > 2)
                txtMonths.Text = txtMonths.Text.Remove(txtMonths.Text.Length - 2, 2);
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
