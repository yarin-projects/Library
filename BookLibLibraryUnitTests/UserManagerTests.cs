
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookLib.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLib.Models;
using System.IO;
using BookLib.Exceptions;

namespace BookLib.Tests
{
    /// <summary>
    /// Unit tests for the UserManager class.
    /// </summary>
    [TestClass()]
    public class UserManagerTests
    {
        private UserManager _userManager = UserManager.Init;

        /// <summary>
        /// Helper method to clear data and add a new user for testing
        /// </summary>
        private void ClearManagerDataAndAddNewUser()
        {
            _userManager.ClearUserData();
            _userManager.AddUser(new User("Unit", "Test"));
            _userManager.DeleteUserAccount();
            _userManager.AddUser(new User("Unit", "Test"));
        }

        /// <summary>
        /// Test method for adding a user to the system
        /// </summary>
        [TestMethod()]
        public void AddUserTest()
        {
            _userManager.ClearUserData();
            _userManager.AddUser(new User("Unit", "Test"));
            Assert.IsTrue(_userManager.User.Username == "Unit" && _userManager.User.Password == "Test");
        }

        /// <summary>
        /// Test method for clearing user data
        /// </summary>
        [TestMethod()]
        public void ClearUserDataTest()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.DeleteUserAccount();
            Assert.IsNull(_userManager.User);
        }

        /// <summary>
        /// Test method for deleting a user account
        /// </summary>
        [TestMethod()]
        public void DeleteUserAccountTest()
        {
            ClearManagerDataAndAddNewUser();
            string path = "Data/Users/" + _userManager.UserPath;
            _userManager.DeleteUserAccount();
            Assert.IsFalse(Directory.Exists(path));
        }

        /// <summary>
        /// Test method for retrieving bought items by title
        /// </summary>
        [TestMethod()]
        public void GetItemsByTitleTest_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "b" }, true);
            List<AbstractItem> list = _userManager.GetItemsByTitle("b", true);
            Assert.IsTrue(list[0].Title == "b");
        }

        /// <summary>
        /// Test method for retrieving borrowed items by title
        /// </summary>
        [TestMethod()]
        public void GetItemsByTitleTest_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "c" }, false);
            List<AbstractItem> list = _userManager.GetItemsByTitle("c", false);
            Assert.IsTrue(list[0].Title == "c");
        }

        /// <summary>
        /// Test method for retrieving bought items by item object
        /// </summary>
        [TestMethod()]
        public void GetItemsByItemTest_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "c", Price = 20 }, true);
            List<AbstractItem> list = _userManager.GetItemsByItem(new Book() { Title = "c", Price = 20 }, true);
            Assert.IsTrue(list[0].Title == "c" && list[0].Price == 20);
        }

        /// <summary>
        /// Test method for retrieving borrowed items by item object
        /// </summary>
        [TestMethod()]
        public void GetItemsByItemTest_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "c", Price = 20 }, false);
            List<AbstractItem> list = _userManager.GetItemsByItem(new Book() { Title = "c", Price = 20 }, false);
            Assert.IsTrue(list[0].Title == "c" && list[0].Price == 20);
        }

        /// <summary>
        /// Test method for retrieving bought items by item object and maximum price
        /// </summary>
        [TestMethod()]
        public void GetItemsByItemToMaxPriceTest_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "d", Price = 25 }, true);
            List<AbstractItem> list = _userManager.GetItemsByItemToMaxPrice(new Book() { Title = "d", Price = 5 }, 30, true);
            Assert.IsTrue(list[0].Title == "d" && list[0].Price == 25);
        }

        /// <summary>
        /// Test method for retrieving borrowed items by item object and maximum price
        /// </summary>
        [TestMethod()]
        public void GetItemsByItemToMaxPriceTest_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "d", Price = 25 }, false);
            List<AbstractItem> list = _userManager.GetItemsByItemToMaxPrice(new Book() { Title = "d", Price = 5 }, 30, false);
            Assert.IsTrue(list[0].Title == "d" && list[0].Price == 25);
        }

        /// <summary>
        /// Test method for retrieving bought items by ISBN or ISSN
        /// </summary>
        [TestMethod()]
        public void GetItemByIsbnOrIssnTest_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "e", ISBN = "abc", Id = "abc" }, true);
            List<AbstractItem> list = _userManager.GetItemByIsbnOrIssn(new Book() { ISBN = "abc", Id = "abc" }, true);
            Assert.IsTrue(list[0].Id == "abc");
        }

        /// <summary>
        /// Test method for retrieving borrowed items by ISBN or ISSN
        /// </summary>
        [TestMethod()]
        public void GetItemByIsbnOrIssnTest_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "e", ISBN = "abc", Id = "abc" }, false);
            List<AbstractItem> list = _userManager.GetItemByIsbnOrIssn(new Book() { ISBN = "abc", Id = "abc" }, false);
            Assert.IsTrue(list[0].Id == "abc");
        }

        /// <summary>
        /// Test method for sorting bought items by title in ascending order
        /// </summary>
        [TestMethod()]
        public void SortByTitleTest_Ascending_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "b" }, true);
            _userManager.Add(new Book() { Title = "d" }, true);
            _userManager.Add(new Book() { Title = "c" }, true);
            _userManager.Add(new Book() { Title = "a" }, true);
            _userManager.SortByTitle(true, true);
            Assert.IsTrue(_userManager.BoughtItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for sorting bought items by title in descending order
        /// </summary>
        [TestMethod()]
        public void SortByTitleTest_Descending_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "b" }, true);
            _userManager.Add(new Book() { Title = "d" }, true);
            _userManager.Add(new Book() { Title = "c" }, true);
            _userManager.Add(new Book() { Title = "a" }, true);
            _userManager.SortByTitle(false, true);
            Assert.IsFalse(_userManager.BoughtItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for sorting borrowed items by title in ascending order
        /// </summary>
        [TestMethod()]
        public void SortByTitleTest_Ascending_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "b" }, false);
            _userManager.Add(new Book() { Title = "d" }, false);
            _userManager.Add(new Book() { Title = "c" }, false);
            _userManager.Add(new Book() { Title = "a" }, false);
            _userManager.SortByTitle(true, false);
            Assert.IsTrue(_userManager.BorrowedItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for sorting borrowed items by title in descending order
        /// </summary>
        [TestMethod()]
        public void SortByTitleTest_Descending_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "b" }, false);
            _userManager.Add(new Book() { Title = "d" }, false);
            _userManager.Add(new Book() { Title = "c" }, false);
            _userManager.Add(new Book() { Title = "a" }, false);
            _userManager.SortByTitle(false, false);
            Assert.IsFalse(_userManager.BorrowedItemsList[0].Title == "a");
        }

        /// <summary>
        ///  Test method for sorting bought items by type in ascending order
        /// </summary>
        [TestMethod()]
        public void SortByTypeTest_Ascending_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Journal() { Title = "c", Type = "Journal" }, true);
            _userManager.Add(new Book() { Title = "b", Type = "Book" }, true);
            _userManager.Add(new Book() { Title = "d", Type = "Book" }, true);
            _userManager.SortByType(true, true);
            Assert.IsTrue(_userManager.BoughtItemsList[0].Type == "Book");
        }

        /// <summary>
        /// Test method for sorting bought items by type in descending order
        /// </summary>
        [TestMethod()]
        public void SortByTypeTest_Descending_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Journal() { Title = "c", Type = "Journal" }, true);
            _userManager.Add(new Book() { Title = "b", Type = "Book" }, true);
            _userManager.Add(new Book() { Title = "d", Type = "Book" }, true);
            _userManager.SortByType(false, true);
            Assert.IsFalse(_userManager.BoughtItemsList[0].Type == "Book");
        }

        /// <summary>
        /// Test method for sorting borrowed items by type in ascending order
        /// </summary>
        [TestMethod()]
        public void SortByTypeTest_Ascending_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Journal() { Title = "c", Type = "Journal" }, false);
            _userManager.Add(new Book() { Title = "b", Type = "Book" }, false);
            _userManager.Add(new Book() { Title = "d", Type = "Book" }, false);
            _userManager.SortByType(true, false);
            Assert.IsTrue(_userManager.BorrowedItemsList[0].Type == "Book");
        }

        /// <summary>
        /// Test method for sorting borrowed items by type in descending order
        /// </summary>
        [TestMethod()]
        public void SortByTypeTest_Descending_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Journal() { Title = "c", Type = "Journal" }, false);
            _userManager.Add(new Book() { Title = "b", Type = "Book" }, false);
            _userManager.Add(new Book() { Title = "d", Type = "Book" }, false);
            _userManager.SortByType(false, false);
            Assert.IsFalse(_userManager.BorrowedItemsList[0].Type == "Book");
        }

        /// <summary>
        /// Test method for sorting bought items by category in ascending order
        /// </summary>
        [TestMethod()]
        public void SortByCategoryTest_Ascending_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Journal()
            {
                Title = "c",
                Category = JournalCategories.History,
                TheCategory = JournalCategories.History.ToString()
            }, true);
            _userManager.Add(new Book()
            {
                Title = "b",
                Category = BookCategories.Adventure,
                TheCategory = BookCategories.Adventure.ToString()
            }, true);
            _userManager.Add(new Book()
            {
                Title = "d",
                Category = BookCategories.Romance,
                TheCategory = BookCategories.Romance.ToString()
            }, true);
            _userManager.SortByCategory(true, true);
            Assert.IsTrue(_userManager.BoughtItemsList[0].TheCategory == BookCategories.Adventure.ToString());
        }

        /// <summary>
        /// Test method for sorting bought items by category in descending order
        /// </summary>
        [TestMethod()]
        public void SortByCategoryTest_Descending_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Journal()
            {
                Title = "c",
                Category = JournalCategories.History,
                TheCategory = JournalCategories.History.ToString()
            }, true);
            _userManager.Add(new Book()
            {
                Title = "b",
                Category = BookCategories.Adventure,
                TheCategory = BookCategories.Adventure.ToString()
            }, true);
            _userManager.Add(new Book()
            {
                Title = "d",
                Category = BookCategories.Romance,
                TheCategory = BookCategories.Romance.ToString()
            }, true);
            _userManager.SortByCategory(false, true);
            Assert.IsFalse(_userManager.BoughtItemsList[0].TheCategory == BookCategories.Adventure.ToString());
        }

        /// <summary>
        /// Test method for sorting borrowed items by category in ascending order
        /// </summary>
        [TestMethod()]
        public void SortByCategoryTest_Ascending_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Journal()
            {
                Title = "c",
                Category = JournalCategories.History,
                TheCategory = JournalCategories.History.ToString()
            }, false);
            _userManager.Add(new Book()
            {
                Title = "b",
                Category = BookCategories.Adventure,
                TheCategory = BookCategories.Adventure.ToString()
            }, false);
            _userManager.Add(new Book()
            {
                Title = "d",
                Category = BookCategories.Romance,
                TheCategory = BookCategories.Romance.ToString()
            }, false);
            _userManager.SortByCategory(true, false);
            Assert.IsTrue(_userManager.BorrowedItemsList[0].TheCategory == BookCategories.Adventure.ToString());
        }

        /// <summary>
        /// Test method for sorting borrowed items by category in descending order
        /// </summary>
        [TestMethod()]
        public void SortByCategoryTest_Descending_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Journal()
            {
                Title = "c",
                Category = JournalCategories.History,
                TheCategory = JournalCategories.History.ToString()
            }, false);
            _userManager.Add(new Book()
            {
                Title = "b",
                Category = BookCategories.Adventure,
                TheCategory = BookCategories.Adventure.ToString()
            }, false);
            _userManager.Add(new Book()
            {
                Title = "d",
                Category = BookCategories.Romance,
                TheCategory = BookCategories.Romance.ToString()
            }, false);
            _userManager.SortByCategory(false, false);
            Assert.IsFalse(_userManager.BorrowedItemsList[0].TheCategory == BookCategories.Adventure.ToString());
        }

        /// <summary>
        /// Test method for sorting bought items by price in ascending order
        /// </summary>
        [TestMethod()]
        public void SortByPriceTest_Ascending_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "c", Price = 15 }, true);
            _userManager.Add(new Book() { Title = "b", Price = 10 }, true);
            _userManager.Add(new Book() { Title = "a", Price = 5 }, true);
            _userManager.Add(new Book() { Title = "d", Price = 20 }, true);
            _userManager.SortByPrice(true, true);
            Assert.IsTrue(_userManager.BoughtItemsList[0].Price == 5);
        }

        /// <summary>
        /// Test method for sorting bought items by price in descending order
        /// </summary>
        [TestMethod()]
        public void SortByPriceTest_Descending_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "c", Price = 15 }, true);
            _userManager.Add(new Book() { Title = "b", Price = 10 }, true);
            _userManager.Add(new Book() { Title = "a", Price = 5 }, true);
            _userManager.Add(new Book() { Title = "d", Price = 20 }, true);
            _userManager.SortByPrice(false, true);
            Assert.IsFalse(_userManager.BoughtItemsList[0].Price == 5);
        }

        /// <summary>
        /// Test method for sorting borrowed items by price in ascending order
        /// </summary>
        [TestMethod()]
        public void SortByPriceTest_Ascending_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "c", Price = 15 }, false);
            _userManager.Add(new Book() { Title = "b", Price = 10 }, false);
            _userManager.Add(new Book() { Title = "a", Price = 5 }, false);
            _userManager.Add(new Book() { Title = "d", Price = 20 }, false);
            _userManager.SortByPrice(true, false);
            Assert.IsTrue(_userManager.BorrowedItemsList[0].Price == 5);
        }

        /// <summary>
        /// Test method for sorting borrowed items by price in descending order
        /// </summary>
        [TestMethod()]
        public void SortByPriceTest_Descending_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "c", Price = 15 }, false);
            _userManager.Add(new Book() { Title = "b", Price = 10 }, false);
            _userManager.Add(new Book() { Title = "a", Price = 5 }, false);
            _userManager.Add(new Book() { Title = "d", Price = 20 }, false);
            _userManager.SortByPrice(false, false);
            Assert.IsFalse(_userManager.BorrowedItemsList[0].Price == 5);
        }

        /// <summary>
        /// Test method for adding bought items
        /// </summary>
        [TestMethod()]
        public void AddTest_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "a" }, true);
            Assert.IsTrue(_userManager.BoughtItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for adding borrowed items
        /// </summary>
        [TestMethod()]
        public void AddTest_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            _userManager.Add(new Book() { Title = "a" }, false);
            Assert.IsTrue(_userManager.BorrowedItemsList[0].Title == "a");
        }

        /// <summary>
        /// Test method for removing bought items
        /// </summary>
        [TestMethod()]
        public void RemoveTest_BoughtItems()
        {
            ClearManagerDataAndAddNewUser();
            Book b = new Book() { Title = "a" };
            _userManager.Add(b, true);
            _userManager.Add(new Book() { Title = "b" }, true);
            _userManager.Remove(b, true);
            Assert.IsTrue(_userManager.CountBought == 1);
        }

        /// <summary>
        /// Test method for removing borrowed items
        /// </summary>
        [TestMethod()]
        public void RemoveTest_BorrowedItems()
        {
            ClearManagerDataAndAddNewUser();
            Book b = new Book() { Title = "a" };
            _userManager.Add(b, false);
            _userManager.Add(new Book() { Title = "b" }, false);
            _userManager.Remove(b, false);
            Assert.IsTrue(_userManager.CountBorrowed == 1);
        }

        /// <summary>
        /// Tests whether a ManagerHasNoUserRegisteredException is thrown when attempting to sort users by title when the manager has no registered users.
        /// </summary>
        [TestMethod()]
        public void ManagerHasNoUserRegisteredExceptionTest()
        {
            _userManager.ClearUserData();
            Assert.ThrowsException<ManagerHasNoUserRegisteredException>(() => _userManager.SortByTitle(true, true));
        }

        /// <summary>
        /// Tests whether a UserAlreadyInManagerException is thrown when attempting to add a user that already exists in the manager.
        /// </summary>
        [TestMethod()]
        public void UserAlreadyInManagerExceptionTest()
        {
            ClearManagerDataAndAddNewUser();
            Assert.ThrowsException<UserAlreadyInManagerException>(() => _userManager.AddUser(new User("a", "a")));
        }

        /// <summary>
        /// Tests whether an ItemNotFoundException is thrown when attempting to get items by title with a specified title that does not exist in the user manager.
        /// </summary>
        [TestMethod()]
        public void ItemNotFoundExceptionTest()
        {
            ClearManagerDataAndAddNewUser();
            Assert.ThrowsException<ItemNotFoundException>(() => _userManager.GetItemsByTitle("a", true));
        }
    }
}