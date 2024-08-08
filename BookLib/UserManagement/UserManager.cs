using BookLib.Exceptions;
using BookLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookLib.UserManagement
{
    /// <summary>
    /// Manages user-related operations such as handling borrowed and bought items.
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// File path for storing borrowed books.
        /// </summary>
        private string fileBorrowedBooks;

        /// <summary>
        /// File path for storing borrowed journals.
        /// </summary>
        private string fileBorrowedJournals;

        /// <summary>
        /// File path for storing bought books.
        /// </summary>
        private string fileBoughtBooks;

        /// <summary>
        /// File path for storing bought journals.
        /// </summary>
        private string fileBoughtJournals;

        /// <summary>
        /// The current user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Path for user-specific data.
        /// </summary>
        public string UserPath { get => User.Username + "_" + User.Password; }

        /// <summary>
        /// Count of borrowed items.
        /// </summary>
        public int CountBorrowed { get => _borrowedItemsList.Count; }

        /// <summary>
        /// Count of bought items.
        /// </summary>
        public int CountBought { get => _boughtItemsList.Count; }

        /// <summary>
        ///  List to store borrowed items.
        /// </summary>
        public List<AbstractItem> BorrowedItemsList { get => _borrowedItemsList;  }
        private List<AbstractItem> _borrowedItemsList;

        /// <summary>
        /// List to store bought items.
        /// </summary>
        public List<AbstractItem> BoughtItemsList { get => _boughtItemsList;  }
        private List<AbstractItem> _boughtItemsList;

        /// <summary>
        /// Repository instance for data access.
        /// </summary>
        readonly Repository dal = new Repository();

        /// <summary>
        /// Singleton instance of UserManager.
        /// </summary>
        public static UserManager Init { get; } = new UserManager();

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private UserManager()
        {
            _borrowedItemsList = new List<AbstractItem>();
            _boughtItemsList = new List<AbstractItem>();
        }

        /// <summary>
        /// Adds a new user to the UserManager and initializes user-specific directories and files.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if the user parameter is null.</exception>
        /// <exception cref="UserAlreadyInManagerException">Thrown if a user already exists in the manager.</exception>
        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (User != null)
                throw new UserAlreadyInManagerException("Manager already has an existing user: " + user.Username);
            // Set the current user to the provided user.
            User = user;

            // Create directories for storing borrowed and bought items specific to the user.
            Directory.CreateDirectory("Data/Users/" + UserPath + "/Borrowed");
            Directory.CreateDirectory("Data/Users/" + UserPath + "/Bought");

            // Set file paths for borrowed and bought books and journals.
            fileBorrowedBooks = "Data/Users/" + UserPath + "/Borrowed/books.xml";
            fileBoughtBooks = "Data/Users/" + UserPath + "/Bought/books.xml";
            fileBorrowedJournals = "Data/Users/" + UserPath + "/Borrowed/journals.xml";
            fileBoughtJournals = "Data/Users/" + UserPath + "/Bought/journals.xml";

            //Attempts to see if user already has borrowed/bought journals/books
            try
            {

                var borrowedBooks = dal.LoadData<List<Book>>(fileBorrowedBooks);
                for (int i = 0; i < borrowedBooks.Count; i++)
                {
                    if (borrowedBooks[i].DiscountActive)
                        borrowedBooks[i].RecoverOriginalPrice();
                }
                _borrowedItemsList.AddRange(borrowedBooks);
            }
            catch { }
            try
            {
                var boughtBooks = dal.LoadData<List<Book>>(fileBoughtBooks);
                for (int i = 0; i < boughtBooks.Count; i++)
                {
                    if (boughtBooks[i].DiscountActive)
                        boughtBooks[i].RecoverOriginalPrice();
                }
                _boughtItemsList.AddRange(boughtBooks);
            }
            catch { }
            try
            {
                var borrowedJournals = dal.LoadData<List<Journal>>(fileBorrowedJournals);
                for (int i = 0; i < borrowedJournals.Count; i++)
                {
                    if (borrowedJournals[i].DiscountActive)
                        borrowedJournals[i].RecoverOriginalPrice();
                }
                _borrowedItemsList.AddRange(borrowedJournals);
            }
            catch { }
            try
            {
                var boughtJournals = dal.LoadData<List<Journal>>(fileBoughtJournals);
                for (int i = 0; i < boughtJournals.Count; i++)
                {
                    if (boughtJournals[i].DiscountActive)
                        boughtJournals[i].RecoverOriginalPrice();
                }
                _boughtItemsList.AddRange(boughtJournals);
            }
            catch { }
        }

        /// <summary>
        /// Clears all user-related data, including user information, bought items list, and borrowed items list.
        /// </summary>
        public void ClearUserData()
        {
            User = null;
            _boughtItemsList.Clear();
            _borrowedItemsList.Clear();
        }

        /// <summary>
        /// Deletes the user's account, including all associated data files.
        /// </summary>
        public void DeleteUserAccount()
        {
            Directory.Delete("Data/Users/" + UserPath, true);
            ClearUserData();
        }

        /// <summary>
        /// Retrieves a list of items by searching for a specified title.
        /// </summary>
        /// <param name="title">The title to search for.</param>
        /// <param name="boughtItems">True if searching in bought items; false if searching in borrowed items.</param>
        /// <returns>A list of items matching the specified title.</returns>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        /// <exception cref="ItemNotFoundException">Thrown if no items match the search criteria.</exception>
        public List<AbstractItem> GetItemsByTitle(string title, bool boughtItems)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            // Determine the count and list of items based on whether bought items are being searched.
            int count;
            List<AbstractItem> tmp;
            if (boughtItems)
            {
                count = CountBought;
                tmp = _boughtItemsList;
            }
            else
            {
                count = CountBorrowed;
                tmp = _borrowedItemsList;
            }

            // Iterate through the items and add those with matching titles to the list.
            List<AbstractItem> list = new List<AbstractItem>();
            for (int i = 0; i < count; i++)
            {
                if (tmp[i].Title.ToLower().Contains(title.ToLower()))
                    list.Add(tmp[i]);
            }
            if (list.Count < 1)
                throw new ItemNotFoundException($"Item couldn't be found!");
            return list;
        }

        /// <summary>
        /// Retrieves a list of items by searching for a specified item and price.
        /// </summary>
        /// <param name="searchItem">The item to search for.</param>
        /// <param name="boughtItems">True if searching in bought items; false if searching in borrowed items.</param>
        /// <returns>A list of items matching the specified item and price.</returns>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        /// <exception cref="ItemNotFoundException">Thrown if no items match the search criteria.</exception>
        public List<AbstractItem> GetItemsByItem(AbstractItem searchItem, bool boughtItems)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            // Determine the count and list of items based on whether bought items are being searched.
            int count;
            List<AbstractItem> tmp;
            if (boughtItems)
            {
                count = CountBought;
                tmp = _boughtItemsList;
            }
            else
            {
                count = CountBorrowed;
                tmp = _borrowedItemsList;
            }

            // Iterate through the items and add those with matching title and price to the list.
            List<AbstractItem> list = new List<AbstractItem>();
            for (int i = 0; i < count; i++)
            {
                if (tmp[i].Title.ToLower().Contains(searchItem.Title.ToLower()) &&
                    tmp[i].Price >= searchItem.Price)
                    list.Add(tmp[i]);
            }
            if (list.Count < 1)
                throw new ItemNotFoundException($"Item couldn't be found!");
            return list;
        }

        /// <summary>
        /// Retrieves a list of items by searching for a specified item and a maximum price.
        /// </summary>
        /// <param name="searchItem">The item to search for.</param>
        /// <param name="maxPrice">The maximum price for filtering the items.</param>
        /// <param name="boughtItems">True if searching in bought items; false if searching in borrowed items.</param>
        /// <returns>A list of items matching the specified item and price range.</returns>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        /// <exception cref="ItemNotFoundException">Thrown if no items match the search criteria.</exception>
        public List<AbstractItem> GetItemsByItemToMaxPrice(AbstractItem searchItem, double maxPrice, bool boughtItems)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            // Determine the count and list of items based on whether bought items are being searched.
            int count;
            List<AbstractItem> tmp;
            if (boughtItems)
            {
                count = CountBought;
                tmp = _boughtItemsList;
            }
            else
            {
                count = CountBorrowed;
                tmp = _borrowedItemsList;
            }

            // Iterate through the items and add those with matching title, price, and within the specified price range to the list.
            List<AbstractItem> list = new List<AbstractItem>();
            for (int i = 0; i < count; i++)
            {
                if (tmp[i].Title.ToLower().Contains(searchItem.Title.ToLower()) &&
                    tmp[i].Price >= searchItem.Price && tmp[i].Price <= maxPrice)
                {
                    list.Add(tmp[i]);
                }
            }

            if (list.Count < 1)
                throw new ItemNotFoundException($"Item couldn't be found!");
            return list;
        }

        /// <summary>
        /// Retrieves items from the bought or borrowed items list based on the ISBN or ISSN of the search item.
        /// </summary>
        /// <param name="searchItem">The search item containing the ISBN or ISSN to search for.</param>
        /// <param name="boughtItems">Specifies whether to search in bought items list (true) or borrowed items list (false).</param>
        /// <returns>A list of items matching the ISBN or ISSN of the search item.</returns>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown when no user is registered.</exception>
        /// <exception cref="ItemNotFoundException">Thrown when the item with the specified ISBN or ISSN couldn't be found.</exception>
        public List<AbstractItem> GetItemByIsbnOrIssn(AbstractItem searchItem, bool boughtItems)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            int count;
            List<AbstractItem> tmp;
            if (boughtItems)
            {
                if (searchItem.GetType() == typeof(Book))
                    tmp = _boughtItemsList.FindAll(x => x.GetType() == typeof(Book));
                else
                    tmp = _boughtItemsList.FindAll(x => x.GetType() == typeof(Journal));
                count = tmp.Count;
            }
            else
            {
                if (searchItem.GetType() == typeof(Book))
                    tmp = _borrowedItemsList.FindAll(x => x.GetType() == typeof(Book));
                else
                    tmp = _borrowedItemsList.FindAll(x => x.GetType() == typeof(Journal));
                count = tmp.Count;
            }
            List<AbstractItem> list = new List<AbstractItem>();
            for (int i = 0; i < count; i++)
            {
                if (tmp[i].Id.Equals(searchItem.Id))
                {
                    list.Add(tmp[i]);
                }
            }

            if (list.Count < 1)
                throw new ItemNotFoundException($"Item couldn't be found!");
            return list;
        }

        /// <summary>
        /// Sorts the list of items by title.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        /// <param name="boughtItems">True if sorting should be done on bought items, false for borrowed items.</param>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        public void SortByTitle(bool ascending, bool boughtItems)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            // Sort the list of items by title based on whether bought items are being sorted.
            if (boughtItems)
                _boughtItemsList.Sort((x, y) => x.Title.CompareTo(y.Title));
            else
                _borrowedItemsList.Sort((x, y) => x.Title.CompareTo(y.Title));
            if (!ascending)
            {
                if (boughtItems)
                    _boughtItemsList.Reverse();
                else
                    _borrowedItemsList.Reverse();
            }
        }

        /// <summary>
        /// Sorts the list of items by type and title.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        /// <param name="boughtItems">True if sorting should be done on bought items, false for borrowed items.</param>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        public void SortByType(bool ascending, bool boughtItems)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            // Sort the list of items by type and title based on whether bought items are being sorted.
            if (boughtItems)
            {
                _boughtItemsList.Sort((x, y) =>
                {
                    int result = x.Type.CompareTo(y.Type);

                    if (result == 0)
                        return x.Title.CompareTo(y.Title);

                    return result;
                });
            }
            else
            {
                _borrowedItemsList.Sort((x, y) =>
                {
                    int result = x.Type.CompareTo(y.Type);

                    if (result == 0)
                        return x.Title.CompareTo(y.Title);

                    return result;
                });
            }
            if (!ascending)
            {
                if (boughtItems)
                    _boughtItemsList.Reverse();
                else
                    _borrowedItemsList.Reverse();
            }
        }

        /// <summary>
        /// Sorts the list of items by category.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        /// <param name="boughtItems">True if sorting should be done on bought items, false for borrowed items.</param>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        public void SortByCategory(bool ascending, bool boughtItems)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            // Sort the list of items by category based on whether bought items are being sorted.
            if (boughtItems)
            {
                _boughtItemsList.Sort((x, y) =>
                {
                    int result = x.TheCategory.CompareTo(y.TheCategory);
                    if (result == 0)
                        return x.Title.CompareTo(y.Title);
                    return result;
                });
            }
            else
            {
                _borrowedItemsList.Sort((x, y) =>
                {
                    int result = x.TheCategory.CompareTo(y.TheCategory);
                    if (result == 0)
                        return x.Title.CompareTo(y.Title);
                    return result;
                });
            }

            if (!ascending)
            {
                if (boughtItems)
                    _boughtItemsList.Reverse();
                else
                    _borrowedItemsList.Reverse();
            }
        }

        /// <summary>
        /// Sorts the list of items by price.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        /// <param name="boughtItems">True if sorting should be done on bought items, false for borrowed items.</param>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        public void SortByPrice(bool ascending, bool boughtItems)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            // Sort the list of items by price based on whether bought items are being sorted.
            if (boughtItems)
            {
                _boughtItemsList.Sort((x, y) =>
                {
                    int result = x.Price.CompareTo(y.Price);
                    if (result == 0)
                        return x.Title.CompareTo(y.Title);
                    return result;
                });
            }
            else
            {
                _borrowedItemsList.Sort((x, y) =>
                {
                    int result = x.Price.CompareTo(y.Price);
                    if (result == 0)
                        return x.Title.CompareTo(y.Title);
                    return result;
                });
            }
            if (!ascending)
            {
                if (boughtItems)
                    _boughtItemsList.Reverse();
                else
                    _borrowedItemsList.Reverse();
            }
        }

        /// <summary>
        /// Adds an item to the appropriate list (bought or borrowed) and saves the data.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="boughtItem">True if the item is bought, false if it is borrowed.</param>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        public void Add(AbstractItem item, bool boughtItem)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            if (boughtItem)
                _boughtItemsList.Add(item);
            else
                _borrowedItemsList.Add(item);
            if (item.GetType() == typeof(Book))
            {
                if (boughtItem)
                    dal.SaveData(_boughtItemsList.OfType<Book>().ToList(), fileBoughtBooks);
                else
                    dal.SaveData(_borrowedItemsList.OfType<Book>().ToList(), fileBorrowedBooks);
            }
            else
            {
                if (boughtItem)
                    dal.SaveData(_boughtItemsList.OfType<Journal>().ToList(), fileBoughtJournals);
                else
                    dal.SaveData(_borrowedItemsList.OfType<Journal>().ToList(), fileBorrowedJournals);
            }
        }

        /// <summary>
        /// Removes an item from the appropriate list (bought or borrowed) and saves the data.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <param name="boughtItem">True if the item is bought, false if it is borrowed.</param>
        /// <exception cref="ManagerHasNoUserRegisteredException">Thrown if no user is registered.</exception>
        public void Remove(AbstractItem item, bool boughtItem)
        {
            if (User == null)
                throw new ManagerHasNoUserRegisteredException("No user is registered");

            if (boughtItem)
                _boughtItemsList.Remove(item);
            else
                _borrowedItemsList.Remove(item);
            if (item.GetType() == typeof(Book))
            {
                if (boughtItem)
                    dal.SaveData(_boughtItemsList.OfType<Book>().ToList(), fileBoughtBooks);
                else
                    dal.SaveData(_borrowedItemsList.OfType<Book>().ToList(), fileBorrowedBooks);
            }
            else
            {
                if (boughtItem)
                    dal.SaveData(_boughtItemsList.OfType<Journal>().ToList(), fileBoughtJournals);
                else
                    dal.SaveData(_borrowedItemsList.OfType<Journal>().ToList(), fileBorrowedJournals);
            }
        }
    }
}
