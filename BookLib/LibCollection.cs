using BookLib.Exceptions;
using BookLib.Interfaces;
using BookLib.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BookLib
{
    /// <summary>
    /// The LibCollection class represents a collection of items (books and journals) in a library. It implements the ILibCollection interface and IEnumerable interface, providing methods to interact with the collection, such as adding, removing, and retrieving items.
    /// </summary>
    public class LibCollection : ILibCollection
    {
        /// <summary>
        /// fileBooks (string): The file path for storing books data in XML format.
        /// </summary>
        string fileBooks = "Data/books.xml";

        /// <summary>
        /// fileJournals (string): The file path for storing journals data in XML format.
        /// </summary>
        string fileJournals = "Data/journals.xml";

        /// <summary>
        /// An instance of the Repository class used for data access and serialization.
        /// </summary>
        readonly Repository dal = new Repository();

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count { get => _itemsList.Count; }

        /// <summary>
        /// Gets a static instance of the LibCollection class for initialization.
        /// </summary>
        public static LibCollection Init { get; } = new LibCollection();

        /// <summary>
        /// Gets the list of items in the collection.
        /// </summary>
        public List<AbstractItem> ItemsList { get => _itemsList; private set => _itemsList = value; }
        private List<AbstractItem> _itemsList;

        /// <summary>
        /// Initializes a new instance of the LibCollection class.
        /// Initializes the items list by loading data from XML files using the Repository.
        /// If loading data from the XML files fails (an exception is caught), it calls the SaveFirstTime() method to save initial data.
        /// </summary>
        private LibCollection()
        {
            // Initialize a new list to store AbstractItem objects.
            _itemsList = new List<AbstractItem>();

            try
            {
                // Attempt to load data from the book and journal files.
                LoadLibDataFromFile();
            }
            catch (Exception)
            {
                // If an exception occurs (e.g., files not found), create a "Data" directory and save initial data.
                Directory.CreateDirectory("Data");

                // Save initial data to files.
                SaveFirstTime();

                // Reload data after saving it for the first time.
                LoadLibDataFromFile();
            }
        }

        /// <summary>
        /// Saves initial data of books and journals to XML files if they don't exist.
        /// </summary>
        public void SaveFirstTime()
        {
            dal.SaveData(new List<Book>
            {
                new Book("Harry Potter-1", DateTime.Now.AddYears(-15), 2514, Guid.NewGuid().ToString().Remove(11),
                BookCategories.Fantasy, 94.2, 0.2),
                new Book("Harry Potter-2", DateTime.Now.AddYears(-12), 3, Guid.NewGuid().ToString().Remove(11),
                BookCategories.Romance, 25, 0.3) { DiscountActive = true },
                new Book("The Hobbit", DateTime.Now.AddYears(-5), 8, Guid.NewGuid().ToString().Remove(11),
                BookCategories.Adventure, 152.99, 0),
                new Book("Lord Of The Rings", DateTime.Now.AddYears(-33), 21, Guid.NewGuid().ToString().Remove(11),
                BookCategories.Horror, 43.7, 0.4),
            }, fileBooks);
            dal.SaveData(new List<Journal>
            {
                new Journal("EDS", DateTime.Now.AddYears(-2), 8, JournalCategories.History,
                Months.July|Months.April|Months.March, 41.11, Guid.NewGuid().ToString().Remove(11)),
                new Journal("JPE", DateTime.Now.AddYears(-53), 4, JournalCategories.Science, Months.January,
                27, Guid.NewGuid().ToString().Remove(11))  { DiscountActive = true },
                new Journal("SNA", DateTime.Now.AddYears(-1), 42, JournalCategories.Biography,
                Months.February|Months.December|Months.January|Months.June, 142.52, Guid.NewGuid().ToString().Remove(11)),
                new Journal("POB", DateTime.Now.AddYears(-22), 6542, JournalCategories.Memoir,
                Months.July|Months.April|Months.March|Months.February|Months.December|Months.January|Months.June|Months.October,
                2841.3, Guid.NewGuid().ToString().Remove(11)),
            }, fileJournals);
        }

        /// <summary>
        /// Reloads library data by clearing the existing items list and loading data from files.
        /// </summary>
        public void ReloadLibDataFromFile()
        {
            _itemsList.Clear();
            LoadLibDataFromFile();
        }

        /// <summary>
        /// Loads library data from files into the items list.
        /// </summary>
        private void LoadLibDataFromFile()
        {
            var books = dal.LoadData<List<Book>>(fileBooks);
            var journals = dal.LoadData<List<Journal>>(fileJournals);

            // Restore original price for books and journals if discount is active
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].DiscountActive)
                    books[i].RecoverOriginalPrice();
            }

            for (int i = 0; i < journals.Count; i++)
            {
                if (journals[i].DiscountActive)
                    journals[i].RecoverOriginalPrice();
            }

            // Add loaded books and journals to the items list
            _itemsList.AddRange(books);
            _itemsList.AddRange(journals);
        }

        /// <summary>
        /// Retrieves items from the collection that match the specified title.
        /// </summary>
        /// <param name="title">The title to search for.</param>
        /// <returns>List of AbstractItem objects</returns>
        /// <exception cref="ItemNotFoundException">Thrown if no items match the search criteria.</exception>
        public List<AbstractItem> GetItemsByTitle(string title)
        {
            List<AbstractItem> list = new List<AbstractItem>();
            for (int i = 0; i < Count; i++)
            {
                if (_itemsList[i].Title.ToLower().Contains(title.ToLower()))
                    list.Add(_itemsList[i]);
            }
            if (list.Count < 1)
                throw new ItemNotFoundException($"Item couldn't be found!");
            return list;
        }

        /// <summary>
        /// Retrieves items from the collection that match the specified AbstractItem based on title and price.
        /// </summary>
        /// <param name="searchItem">The AbstractItem to search for.</param>
        /// <returns>List of AbstractItem objects.</returns>
        /// <exception cref="ItemNotFoundException">Thrown if no items match the search criteria.</exception>
        public List<AbstractItem> GetItemsByItem(AbstractItem searchItem)
        {
            List<AbstractItem> list = new List<AbstractItem>();
            for (int i = 0; i < Count; i++)
            {
                if (_itemsList[i].Title.ToLower().Contains(searchItem.Title.ToLower()) &&
                    _itemsList[i].Price >= searchItem.Price)
                    list.Add(_itemsList[i]);
            }
            if (list.Count < 1)
                throw new ItemNotFoundException($"Item couldn't be found!");
            return list;
        }

        /// <summary>
        /// Retrieves a list of items filtered by a search item and a maximum price.
        /// </summary>
        /// <param name="searchItem">The item to search for.</param>
        /// <param name="maxPrice">The maximum price for filtering the items.</param>
        /// <returns>A list of items matching the search item and within the specified price range.</returns>
        /// <exception cref="ItemNotFoundException">Thrown if no items match the search criteria.</exception>
        public List<AbstractItem> GetItemsByItemToMaxPrice(AbstractItem searchItem, double maxPrice)
        {
            List<AbstractItem> list = new List<AbstractItem>();
            for (int i = 0; i < Count; i++)
            {
                if (_itemsList[i].Title.ToLower().Contains(searchItem.Title.ToLower()) &&
                    _itemsList[i].Price >= searchItem.Price && _itemsList[i].Price <= maxPrice)
                {
                    list.Add(_itemsList[i]);
                }
            }

            // If no items match the criteria, throw an exception.
            if (list.Count < 1)
                throw new ItemNotFoundException($"Item couldn't be found!");
            return list;
        }

        /// <summary>
        /// Retrieves items from the library by ISBN or ISSN of the search item.
        /// </summary>
        /// <param name="searchItem">The search item containing the ISBN or ISSN to search for.</param>
        /// <returns>A list of items matching the ISBN or ISSN of the search item.</returns>
        /// <exception cref="ItemNotFoundException">Thrown when the item with the specified ISBN or ISSN couldn't be found.</exception>
        public List<AbstractItem> GetItemByIsbnOrIssn(AbstractItem searchItem)
        {
            int count;
            List<AbstractItem> tmp;
            if (searchItem.GetType() == typeof(Book))
            {
                tmp = _itemsList.FindAll(x => x.GetType() == typeof(Book));
                count = tmp.Count;
            }
            else
            {
                tmp = _itemsList.FindAll(x => x.GetType() == typeof(Journal));
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
        public void SortByTitle(bool ascending)
        {
            // Sorting the items list by title using a lambda expression as the comparison delegate.
            _itemsList.Sort((x, y) => x.Title.CompareTo(y.Title));

            // Reversing the list if the sort order is descending.
            if (!ascending)
                _itemsList.Reverse();
        }

        /// <summary>
        /// Sorts the list of items by type and title.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        public void SortByType(bool ascending)
        {
            // Sort the items list first by type and then by title using a custom comparison.
            _itemsList.Sort((x, y) =>
            {
                // Compare the types of items.
                int result = x.Type.CompareTo(y.Type);

                // If types are the same, compare by title.
                if (result == 0)
                    return x.Title.CompareTo(y.Title);

                return result;
            });

            // Reverse the list if the sort order is descending.
            if (!ascending)
                _itemsList.Reverse();
        }


        /// <summary>
        /// Sorts the list of items by category.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        public void SortByCategory(bool ascending)
        {
            // Sorting the items list by category using a lambda expression as the comparison delegate.
            _itemsList.Sort((x, y) =>
            {
                int result = x.TheCategory.CompareTo(y.TheCategory);
                if (result == 0)
                    return x.Title.CompareTo(y.Title);
                return result;
            });

            // Reversing the list if the sort order is descending.
            if (!ascending)
                _itemsList.Reverse();
        }

        /// <summary>
        /// Sorts the list of items by price.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        public void SortByPrice(bool ascending)
        {
            // Sorting the items list by price using a lambda expression as the comparison delegate.
            _itemsList.Sort((x, y) =>
            {
                int result = x.Price.CompareTo(y.Price);
                if (result == 0)
                    return x.Title.CompareTo(y.Title);
                return result;
            });

            // Reversing the list if the sort order is descending.
            if (!ascending)
                _itemsList.Reverse();
        }

        /// <summary>
        /// Adds an item to the internal list of items and saves the updated list to the appropriate file based on the type of the added item.
        /// </summary>
        /// <param name="item">The item to add to the list.</param>
        public void Add(AbstractItem item)
        {
            // Add the item to the internal list of items.
            _itemsList.Add(item);

            // Check if the added item is of type Book.
            if (item.GetType() == typeof(Book))
            {
                // If it's a Book, extract all Books from the internal list and save them to the book file.
                dal.SaveData(_itemsList.OfType<Book>().ToList(), fileBooks);
            }
            else
            {
                // If it's not a Book (assumed to be a Journal in this case),
                // extract all Journals from the internal list and save them to the journal file.
                dal.SaveData(_itemsList.OfType<Journal>().ToList(), fileJournals);
            }
        }


        /// <summary>
        /// Removes the specified item from the internal list of items and saves the updated list to the appropriate file based on the type of the removed item.
        /// </summary>
        /// <param name="item">The item to remove from the list.</param>
        public void Remove(AbstractItem item)
        {
            // Remove the specified item from the internal list of items.
            _itemsList.Remove(item);

            // Check if the removed item is of type Book.
            if (item.GetType() == typeof(Book))
            {
                // If it's a Book, extract all Books from the internal list and save them to the book file.
                dal.SaveData(_itemsList.OfType<Book>().ToList(), fileBooks);
            }
            else
            {
                // If it's not a Book (assumed to be a Journal in this case),
                // extract all Journals from the internal list and save them to the journal file.
                dal.SaveData(_itemsList.OfType<Journal>().ToList(), fileJournals);
            }
        }
    }
}
