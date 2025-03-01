﻿using BookLib;
using BookLib.Exceptions;
using BookLib.Models;
using BookLib.UserManagement;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryUI.Views.Admin
{
    /// <summary>
    /// Represents a window for managing the library collection in the admin interface.
    /// </summary>
    public partial class ManageCollectionView : Window
    {
        // Instance of LibCollection class for managing library collection data
        private LibCollection _libCollection = LibCollection.Init;

        /// <summary>
        /// Default constructor for creating a new instance of ManageCollectionView.
        /// Initializes the view and performs an initial search of the library collection.
        /// </summary>
        public ManageCollectionView()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            cmbBoxSortMethod.Items.Add("Type");
            cmbBoxSortMethod.Items.Add("Title");
            cmbBoxSortMethod.Items.Add("Price");
            cmbBoxSortMethod.Items.Add("Category");
            cmbBoxSortMethod.SelectedIndex = 0;
            btnSortDirection.Content = SortDirection.Ascending;
            SearchCollection();
        }

        /// <summary>
        /// Event handler for the Search button click.
        /// Initiates a search of the library collection based on user input.
        /// </summary>
        private void SearchClick(object sender, RoutedEventArgs e)
        {
            SearchCollection();
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
        /// Searches the library collection based on the specified criteria and populates the search results.
        /// </summary>
        public void SearchCollection()
        {
            // Clear previous search results
            lvSearch.Items.Clear();

            // Parse price input
            double.TryParse(txtPrice.Text, out double minPrice);
            double.TryParse(txtMaxPrice.Text, out double maxPrice);

            if (!ValidatePriceInputs())
                return;

            // List to store search results
            List<AbstractItem> list = new List<AbstractItem>();
            bool sortDirection;
            if ((string)btnSortDirection.Content == SortDirection.Ascending)
                sortDirection = true;
            else
                sortDirection = false;
                switch (cmbBoxSortMethod.SelectedIndex)
                {
                    case 0:
                        _libCollection.SortByType(sortDirection);
                        break;
                    case 1:
                        _libCollection.SortByTitle(sortDirection);
                        break;
                    case 2:
                        _libCollection.SortByPrice(sortDirection);
                        break;
                    case 3:
                        _libCollection.SortByCategory(sortDirection);
                        break;
                }

            try
            {
                list = _libCollection.GetItemsByItemToMaxPrice(new Book { Title = txtTitle.Text, Price = minPrice }, maxPrice);
            }
            catch (ItemNotFoundException ex)
            {
                // Handle item not found exception
                ShowErrorTxt(txtSearchError, ex.Message);
            }

            // Try searching by item
            try
            {
                if (list.Count < 1 && string.IsNullOrWhiteSpace(txtMaxPrice.Text))
                    list = _libCollection.GetItemsByItem(new Book { Title = txtTitle.Text, Price = minPrice });
            }
            catch (ItemNotFoundException ex)
            {
                // Handle item not found exception
                ShowErrorTxt(txtSearchError, ex.Message);
            }

            // If no results found and title is specified, try searching by title
            try
            {
                if (list.Count < 1 && string.IsNullOrWhiteSpace(txtPrice.Text) && string.IsNullOrWhiteSpace(txtMaxPrice.Text))
                    list = _libCollection.GetItemsByTitle(txtTitle.Text);
            }
            catch (ItemNotFoundException ex)
            {
                // Handle item not found exception
                ShowErrorTxt(txtSearchError, ex.Message);
            }

            // Populate search results if found
            if (list.Count > 0)
            {
                txtSearchError.Visibility = Visibility.Collapsed;
                foreach (AbstractItem item in list)
                {
                    lvSearch.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Validates the input for minimum and maximum price fields.
        /// </summary>
        /// <returns>True if the input is valid; otherwise, false.</returns>
        public bool ValidatePriceInputs()
        {
            // Check if the minimum price input field is not empty.
            if (!string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                // Try to parse the input text as a double.
                if (!double.TryParse(txtPrice.Text, out double minPrice))
                {
                    // Show an error message if the input is not a valid number.
                    ShowErrorTxt(txtMinPriceError, $"Price has to be a number");
                    return false;
                }

                // Hide any previous error message for the minimum price field.
                txtMinPriceError.Visibility = Visibility.Collapsed;

                // Check if the entered minimum price is less than $3.00.
                if (minPrice < 3)
                {
                    // Show an error message if the minimum price is less than $3.00.
                    ShowErrorTxt(txtMinPriceError, $"Price has to be bigger than {2.99:c2}");
                    return false;
                }

                // Hide any previous error message for the minimum price field.
                txtMinPriceError.Visibility = Visibility.Collapsed;
            }

            // Check if the maximum price input field is not empty.
            if (!string.IsNullOrWhiteSpace(txtMaxPrice.Text))
            {
                // Try to parse the input text as a double.
                if (!double.TryParse(txtMaxPrice.Text, out double maxPrice))
                {
                    // Show an error message if the input is not a valid number.
                    ShowErrorTxt(txtMaxPriceError, $"Price has to be a number");
                    return false;
                }

                // Hide any previous error message for the maximum price field.
                txtMaxPriceError.Visibility = Visibility.Collapsed;

                // Check if the entered maximum price is less than $3.00.
                if (maxPrice < 3)
                {
                    // Show an error message if the maximum price is less than $3.00.
                    ShowErrorTxt(txtMaxPriceError, $"Price has to be bigger than {2.99:c2}");
                    return false;
                }

                // Hide any previous error message for the maximum price field.
                txtMaxPriceError.Visibility = Visibility.Collapsed;
            }

            // If both input fields are valid, return true.
            return true;
        }


        /// <summary>
        /// Event handler for the selection change in the search results list view.
        /// Displays detailed information about the selected item in a new window.
        /// </summary>
        private void lvSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((AbstractItem)lvSearch.SelectedItem == null)
                return;

            //Get the selected item
            var item = (AbstractItem)lvSearch.SelectedItem;

            // Display detailed information based on the item type
            if (item.GetType() == typeof(Book))
            {
                var selectedBookView = new SelectedBookView(item);
                selectedBookView.ShowDialog();
            }
            else
            {
                var selectedJournalView = new SelectedJournalView(item);
                selectedJournalView.ShowDialog();
            }

            // Perform a new search to update the search results
            SearchCollection();
        }

        /// <summary>
        /// Event handler for closing the window.
        /// </summary>
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event handler for when the sort method selection is changed.
        /// </summary>
        private void SortMethodSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if the ListView control is null, then return without performing any action.
            if (lvSearch == null)
                return;

            // Call the SearchCollection method to perform the search.
            SearchCollection();
        }

        /// <summary>
        /// Handles the click event for changing the sort direction and triggers the search collection.
        /// </summary>
        private void SortDirectionClick(object sender, RoutedEventArgs e)
        {
            btnSortDirection.Content = (string)btnSortDirection.Content == SortDirection.Ascending ? SortDirection.Descending : 
                                                                                                     SortDirection.Ascending;
            SearchCollection();
        }

        /// <summary>
        /// Event handler for clearing the filter criteria.
        /// </summary>
        private void ClearFilterClick(object sender, RoutedEventArgs e)
        {
            txtMaxPrice.Text = string.Empty; 
            txtPrice.Text = string.Empty;
            txtTitle.Text = string.Empty;
        }
    }
}
