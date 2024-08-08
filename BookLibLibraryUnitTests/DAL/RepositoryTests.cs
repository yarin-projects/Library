using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLib.Models;

namespace BookLib.Tests
{
    /// <summary>
    /// Test class for Repository functionality.
    /// </summary>
    [TestClass()]
    public class RepositoryTests
    {
        private LibCollection _collection = LibCollection.Init;

        /// <summary>
        /// Test method for loading and reloading library data.
        /// </summary>
        [TestMethod()]
        public void LoadAndLoadDataTest()
        {
            // Clear existing items list and add new items
            _collection.ItemsList.Clear();
            _collection.Add(new Book());
            _collection.Add(new Book());
            _collection.Add(new Journal());

            // Reload library data from files
            _collection.ReloadLibDataFromFile();

            // Assert that the count of items in the collection is as expected
            Assert.IsTrue(_collection.Count == 3);
        }
    }
}