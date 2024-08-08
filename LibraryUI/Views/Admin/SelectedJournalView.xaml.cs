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
    /// Represents a window for displaying details of a selected journal item in the admin interface.
    /// </summary>
    public partial class SelectedJournalView : Window
    {
        // Instance of the selected journal
        private Journal _selectedJournal;

        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;

        /// <summary>
        /// Initializes a new instance of the SelectedJournalView class with the specified journal item.
        /// </summary>
        /// <param name="item">The journal item to display.</param>
        public SelectedJournalView(AbstractItem item)
        {
            InitializeComponent();
            _selectedJournal = (Journal)item;
            DisplayJournalDetails();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        /// <summary>
        ///  Displays information about the selected journal item.
        /// </summary>
        private void DisplayJournalDetails()
        {
            //Set window title to include journal title
            Title = "Journal: " + _selectedJournal.Title;

            // Display journal details
            txtDate.Text = _selectedJournal.PublishDate.ToString("d");
            txtCopyNum.Text = _selectedJournal.CopyNum.ToString("n0");
            txtMonths.Text = _selectedJournal.Months.ToString();
            txtCategory.Text = _selectedJournal.Category.ToString();
            txtISSN.Text = _selectedJournal.ISSN;
            txtPrice.Text = _selectedJournal.Price.ToString("c2");

            // Set toggle discount button content based on discount status
            btnToggleDiscount.Content = _selectedJournal.DiscountActive ? "Deactivate Discount" : "Activate Discount";
            btnToggleDiscount.Background = _selectedJournal.DiscountActive ? new SolidColorBrush(Color.FromRgb(250, 95, 95)) :
                                                               Brushes.PaleGreen;
        }

        /// <summary>
        /// Event handler for the Remove Journal button click.
        /// Removes the selected journal from the library collection.
        /// </summary>
        private void RemoveJournalClick(object sender, RoutedEventArgs e)
        {
            _libCollection.Remove(_selectedJournal);
            Close();
        }

        /// <summary>
        /// Event handler for the Update Journal button click.
        /// Opens the AddJournalView window for updating the details of the selected journal.
        /// </summary>
        private void UpdateJournalClick(object sender, RoutedEventArgs e)
        {
            var addJournalView = new AddJournalView(_selectedJournal);
            addJournalView.ShowDialog();
            Close();
        }

        /// <summary>
        /// Event handler for the Toggle Discount button click.
        /// Toggles the discount status of the selected journal and updates the display accordingly.
        /// </summary>
        private void ToggleDiscountClick(object sender, RoutedEventArgs e)
        {
            _selectedJournal.DiscountActive = !_selectedJournal.DiscountActive;
            DisplayJournalDetails();
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
