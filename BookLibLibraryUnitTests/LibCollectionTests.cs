using BookLib.Exceptions;
using BookLib.Models;
using BookLib.UserManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace BookLib.Tests
{
    /// <summary>
    /// Test class for testing LibCollection class functionality.
    /// </summary>
    [TestClass()]
    public class LibCollectionTests
    {
        private LibCollection _collection = LibCollection.Init;

        /// <summary>
        /// Test method for constructing a LibCollection object.
        /// </summary>
        [TestMethod()]
        public void LibCollectionCtorTest()
        {
            Assert.IsNotNull(_collection.ItemsList);
        }

        /// <summary>
        /// Test method for reloading library data from files.
        /// </summary>
        [TestMethod()]
        public void ReloadLibDataFromFileTest()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "a" });
            _collection.ReloadLibDataFromFile();
            Assert.IsTrue(_collection.ItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for retrieving items by title from the library.
        /// </summary>
        [TestMethod()]
        public void GetItemsByTitleTest()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "b" });
            List<AbstractItem> list = _collection.GetItemsByTitle("b");
            Assert.IsTrue(list[0].Title == "b");
        }

        /// <summary>
        /// Test method for retrieving items by an item object from the library.
        /// </summary>
        [TestMethod()]
        public void GetItemsByItemTest()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "c", Price = 20 });
            List<AbstractItem> list = _collection.GetItemsByItem(new Book() { Title = "c", Price = 20 });
            Assert.IsTrue(list[0].Title == "c" && list[0].Price == 20);
        }

        /// <summary>
        /// Test method for retrieving items by an item object and maximum price from the library.
        /// </summary>
        [TestMethod()]
        public void GetItemsByItemToMaxPriceTest()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "d", Price = 25 });
            List<AbstractItem> list = _collection.GetItemsByItemToMaxPrice(new Book() { Title = "d", Price = 5 }, 30);
            Assert.IsTrue(list[0].Title == "d" && list[0].Price == 25);
        }

        /// <summary>
        /// Test method for retrieving items by ISBN or ISSN from the library.
        /// </summary>
        [TestMethod()]
        public void GetItemByIsbnOrIssnTest()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "e", ISBN = "abc", Id = "abc" });
            List<AbstractItem> list = _collection.GetItemByIsbnOrIssn(new Book() { ISBN = "abc", Id = "abc" });
            Assert.IsTrue(list[0].Id == "abc");
        }

        /// <summary>
        /// Test method for sorting items by title in ascending order.
        /// </summary>
        [TestMethod()]
        public void SortByTitleTestAscending()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "b" });
            _collection.Add(new Book() { Title = "d" });
            _collection.Add(new Book() { Title = "c" });
            _collection.Add(new Book() { Title = "a" });
            _collection.SortByTitle(true);
            Assert.IsTrue(_collection.ItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for sorting items by title in descending order.
        /// </summary>
        [TestMethod()]
        public void SortByTitleTestDescending()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "b" });
            _collection.Add(new Book() { Title = "d" });
            _collection.Add(new Book() { Title = "c" });
            _collection.Add(new Book() { Title = "a" });
            _collection.SortByTitle(false);
            Assert.IsFalse(_collection.ItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for sorting items by type in ascending order.
        /// </summary>
        [TestMethod()]
        public void SortByTypeTestAscending()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Journal() { Title = "c", Type = "Journal" });
            _collection.Add(new Book() { Title = "b", Type = "Book" });
            _collection.Add(new Book() { Title = "d", Type = "Book" });
            _collection.SortByType(true);
            Assert.IsTrue(_collection.ItemsList[0].Type == "Book");
        }

        /// <summary>
        /// Test method for sorting items by type in descending order.
        /// </summary>
        [TestMethod()]
        public void SortByTypeTestDescending()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Journal() { Title = "c", Type = "Journal" });
            _collection.Add(new Book() { Title = "b", Type = "Book" });
            _collection.Add(new Book() { Title = "d", Type = "Book" });
            _collection.SortByType(false);
            Assert.IsFalse(_collection.ItemsList[0].Type == "Book");
        }

        /// <summary>
        /// Test method for sorting items by category in ascending order.
        /// </summary>
        [TestMethod()]
        public void SortByCategoryTestAscending()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Journal()
            {
                Title = "c",
                Category = JournalCategories.History,
                TheCategory = JournalCategories.History.ToString()
            });
            _collection.Add(new Book()
            {
                Title = "b",
                Category = BookCategories.Adventure,
                TheCategory = BookCategories.Adventure.ToString()
            });
            _collection.Add(new Book()
            {
                Title = "d",
                Category = BookCategories.Romance,
                TheCategory = BookCategories.Romance.ToString()
            });
            _collection.SortByCategory(true);
            Assert.IsTrue(_collection.ItemsList[0].TheCategory == BookCategories.Adventure.ToString());
        }

        /// <summary>
        /// Test method for sorting items by category in descending order.
        /// </summary>
        [TestMethod()]
        public void SortByCategoryTestDescending()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Journal()
            {
                Title = "c",
                Category = JournalCategories.History,
                TheCategory = JournalCategories.History.ToString()
            });
            _collection.Add(new Book()
            {
                Title = "b",
                Category = BookCategories.Adventure,
                TheCategory = BookCategories.Adventure.ToString()
            });
            _collection.Add(new Book()
            {
                Title = "d",
                Category = BookCategories.Romance,
                TheCategory = BookCategories.Romance.ToString()
            });
            _collection.SortByCategory(false);
            Assert.IsFalse(_collection.ItemsList[0].TheCategory == BookCategories.Adventure.ToString());
        }

        /// <summary>
        /// Test method for sorting items by price in ascending order.
        /// </summary>
        [TestMethod()]
        public void SortByPriceTestAscending()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "c", Price = 15 });
            _collection.Add(new Book() { Title = "b", Price = 10 });
            _collection.Add(new Book() { Title = "a", Price = 5 });
            _collection.Add(new Book() { Title = "d", Price = 20 });
            _collection.SortByPrice(true);
            Assert.IsTrue(_collection.ItemsList[0].Price == 5);
        }

        /// <summary>
        /// Test method for sorting items by price in descending order.
        /// </summary>
        [TestMethod()]
        public void SortByPriceTestDescending()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "c", Price = 15 });
            _collection.Add(new Book() { Title = "b", Price = 10 });
            _collection.Add(new Book() { Title = "a", Price = 5 });
            _collection.Add(new Book() { Title = "d", Price = 20 });
            _collection.SortByPrice(false);
            Assert.IsFalse(_collection.ItemsList[0].Price == 5);
        }

        /// <summary>
        /// Test method for adding an item to the collection.
        /// </summary>
        [TestMethod()]
        public void AddTest()
        {
            _collection.ItemsList.Clear();
            _collection.Add(new Book() { Title = "a"});
            Assert.IsTrue(_collection.ItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for removing an item from the collection.
        /// </summary>
        [TestMethod()]
        public void RemoveTest()
        {
            _collection.ItemsList.Clear();
            Book b = new Book() { Title = "a" };
            _collection.Add(b);
            _collection.Add(new Book() { Title = "b" });
            _collection.Remove(b);
            Assert.IsTrue(_collection.Count == 1);
        }

        /// <summary>
        /// Tests whether an ItemNotFoundException is thrown when attempting to get items by title with a specified title that does not exist in the collection.
        /// </summary>
        [TestMethod()]
        public void ItemNotFoundExceptionTest()
        {
            _collection.ItemsList.Clear();
            Assert.ThrowsException<ItemNotFoundException>(() => _collection.GetItemsByTitle("a"));
        }

        /// <summary>
        /// Tests saving the library data for the first time.
        /// </summary>
        [TestMethod()]
        public void SaveFirstTimeTest()
        {
            _collection.ItemsList.Clear();
            File.Delete("Data/books.xml");
            File.Delete("Data/journals.xml");
            _collection.SaveFirstTime();
            _collection.ReloadLibDataFromFile();
            Assert.IsTrue(_collection.Count == 8);
        }
    }
}