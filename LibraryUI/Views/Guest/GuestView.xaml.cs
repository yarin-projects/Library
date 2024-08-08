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
    /// Represents the main view for guests accessing the library system.
    /// </summary>
    public partial class GuestView : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuestView"/> class.
        /// </summary>
        public GuestView()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            btnClose.Background = new SolidColorBrush(Color.FromRgb(180, 180, 180));
        }

        /// <summary>
        /// Handles the click event when the Browse Collection button is clicked.
        /// </summary>
        private void BrowseCollectionClick(object sender, RoutedEventArgs e)
        {
            var guestBrowseCollection = new GuestBrowseCollectionView();
            guestBrowseCollection.Owner = Application.Current.MainWindow;
            guestBrowseCollection.ShowDialog();
        }

        /// <summary>
        /// Handles the click event when the Close Application button is clicked.
        /// </summary>
        private void CloseApplicationClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
