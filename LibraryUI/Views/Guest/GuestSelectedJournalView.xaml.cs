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

namespace LibraryUI.Views.Guest
{
    /// <summary>
    /// Interaction logic for displaying details of a selected journal by a guest user.
    /// </summary>
    public partial class GuestSelectedJournalView : Window
    {
        // Instance of the selected journal
        private Journal _selectedJournal;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuestSelectedJournalView"/> class.
        /// </summary>
        /// <param name="item">The selected journal item.</param>
        public GuestSelectedJournalView(Journal item)
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
            txtDiscount.Text = _selectedJournal.DiscountActive ? "10%" : "";
            txtDiscountActive.Text = _selectedJournal.DiscountActive ? "Discount Active" : "Discount Inactive";
            txtDiscountActive.Foreground = _selectedJournal.DiscountActive ? Brushes.PaleGreen :
                                                                          new SolidColorBrush(Color.FromRgb(250, 95, 95));
        }

        /// <summary>
        /// Handles the click event when the close window button is clicked.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
