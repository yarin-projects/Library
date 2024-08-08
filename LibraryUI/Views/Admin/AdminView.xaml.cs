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

namespace LibraryUI.Views.Admin
{
    /// <summary>
    /// Represents the main window of the admin interface for managing the library collection.
    /// </summary>
    public partial class AdminView : Window
    {
        /// <summary>
        /// Initializes a new instance of the AdminView class.
        /// </summary>
        public AdminView()
        {
            InitializeComponent();
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Event handler for the click event of the "Add Book" button.
        /// Opens the AddBookView to allow the user to add a new book.
        /// </summary>
        private void AddBookClick(object sender, RoutedEventArgs e)
        {
            var addBookView = new AddBookView();
            addBookView.Owner = Application.Current.MainWindow;
            addBookView.ShowDialog();
        }

        /// <summary>
        /// Event handler for the click event of the "Add Journal" button.
        /// Opens the AddJournalView to allow the user to add a new journal.
        /// </summary>
        private void AddJournalClick(object sender, RoutedEventArgs e)
        {
            var addJournalView = new AddJournalView();
            addJournalView.Owner = Application.Current.MainWindow;
            addJournalView.ShowDialog();
        }

        /// <summary>
        /// Event handler for the click event of the "Search Collection" button.
        /// Opens the SearchCollectionView to allow the user to search the collection.
        /// </summary>
        private void SearchCollectionClick(object sender, RoutedEventArgs e)
        {
            var manageCollectionView = new ManageCollectionView();
            manageCollectionView.Owner = Application.Current.MainWindow;
            manageCollectionView.ShowDialog();
        }

        /// <summary>
        /// Event handler for closing the application.
        /// </summary>
        private void CloseApplicationClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
