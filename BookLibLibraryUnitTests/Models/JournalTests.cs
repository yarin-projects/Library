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
    /// Test class for testing Journal class functionality.
    /// </summary>
    [TestClass()]
    public class JournalTests
    {
        /// <summary>
        /// Test method for constructing a Journal object with default constructor.
        /// </summary>
        [TestMethod()]
        public void JournalCtorTestEmpty()
        {
            Journal journal = new Journal() { Title = "a" };
            Assert.IsNotNull(journal);
        }

        /// <summary>
        /// Test method for constructing a Journal object with parameterized constructor.
        /// </summary>
        [TestMethod()]
        public void JournalCtorTest()
        {
            DateTime dateTime = DateTime.Now;
            var category = JournalCategories.Science;
            Journal journal = new Journal("a", dateTime, 1, category, Months.January, 20, "abc");
            Assert.IsTrue(journal.Title == "a" && journal.PublishDate == dateTime &&
                journal.Category == category && journal.Months == Months.January && journal.Price == 20 &&
                journal.ISSN == "abc");
        }

        /// <summary>
        /// Test method for copying journal details.
        /// </summary>
        [TestMethod()]
        public void CopyJournalDetailsTest()
        {
            DateTime dateTime = DateTime.Now;
            var category = JournalCategories.Science;
            Journal journal = new Journal("a", dateTime, 1, category, Months.January, 20, "abc");
            Journal j = journal.CopyJournalDetails();
            Assert.IsTrue(j.Title == journal.Title && j.PublishDate == journal.PublishDate && j.ISSN == journal.ISSN && 
                          j.Category == journal.Category && j.Months == journal.Months && j.Price == journal.Price);
        }

        /// <summary>
        /// Test method for recovering the original price of a journal.
        /// </summary>
        [TestMethod()]
        public void RecoverOriginalPriceTest()
        {
            Journal j = new Journal() { Price = 10, DiscountActive = true };
            double price = 10;
            j.RecoverOriginalPrice();
            double originalPrice = j.Price;
            Assert.IsTrue(price == originalPrice);
        }

        /// <summary>
        /// Test method for getting the original price of a journal.
        /// </summary>
        [TestMethod()]
        public void GetOriginalPriceTest()
        {
            Journal j = new Journal() { Price = 10, DiscountActive = true };
            double price = 10;
            Assert.IsTrue(price == j.GetOriginalPrice());
        }
    }
}