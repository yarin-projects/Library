using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Models.Tests
{
    /// <summary>
    /// Test class for testing Book class functionality.
    /// </summary>
    [TestClass()]
    public class BookTests
    {

        /// <summary>
        /// Test method for constructing a Book object with default constructor.
        /// </summary>
        [TestMethod()]
        public void BookTestCtorEmpty()
        {
            Book book = new Book() { Title = "a" };
            Assert.IsNotNull(book);
        }

        /// <summary>
        /// Test method for constructing a Book object with parameterized constructor.
        /// </summary>
        [TestMethod()]
        public void BookTestCtor()
        {
            DateTime dateTime = DateTime.Now;
            var category = BookCategories.Horror;
            Book book = new Book("a", dateTime, 1,"aa",category, 20, 0.2);
            Assert.IsTrue(book.Title == "a" && book.PublishDate == dateTime &&
                          book.Category == category && book.Price == 20 &&
                          book.ISBN == "aa");
        }

        /// <summary>
        /// Test method for copying book details.
        /// </summary>
        [TestMethod()]
        public void CopyBookDetailsTest()
        {
            DateTime dateTime = DateTime.Now;
            var category = BookCategories.Romance;
            Book book = new Book("a", dateTime, 1, "asd", category, 20, 0.2);
            Book b = book.CopyBookDetails();
            Assert.IsTrue(b.Title == book.Title && b.PublishDate == book.PublishDate && b.ISBN == book.ISBN &&
                          b.Category == book.Category && b.Discount == book.Discount && b.Price == book.Price);
        }

        /// <summary>
        /// Test method for recovering the original price of a book.
        /// </summary>
        [TestMethod()]
        public void RecoverOriginalPriceTest()
        {
            Book b = new Book() { Price = 10, DiscountActive = true, Discount = 0.1 };
            double price = 10;
            b.RecoverOriginalPrice();
            double originalPrice = b.Price;
            Assert.IsTrue(price == originalPrice);
        }

        /// <summary>
        /// Test method for getting the original price of a book.
        /// </summary>
        [TestMethod()]
        public void GetOriginalPriceTest()
        {
            Book b = new Book() { Price = 10, DiscountActive = true, Discount = 0.1 };
            double price = 10;
            Assert.IsTrue(price == b.GetOriginalPrice());
        }
    }
}