using BookLib.Exceptions;
using BookLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Interfaces
{
    /// <summary>
    /// The ILibCollection interface defines a contract for a collection that manages items of type AbstractItem. It provides properties to access the collection and its count, as well as methods for adding, removing, and retrieving items from the collection.
    /// </summary>
    public interface ILibCollection
    {
        /// <summary>
        /// Gets the list of AbstractItem objects contained in the collection.
        /// </summary>
        List<AbstractItem> ItemsList { get; }

        /// <summary>
        /// Gets the number of items currently stored in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds the specified AbstractItem to the collection.
        /// </summary>
        /// <param name="item">The AbstractItem to be added to the collection.</param>
        void Add(AbstractItem item);

        /// <summary>
        /// Removes the specified AbstractItem from the collection.
        /// </summary>
        /// <param name="item">The AbstractItem to be removed from the collection.</param>
        void Remove(AbstractItem item);

        /// <summary>
        /// Retrieves a list of AbstractItem objects from the collection that match the specified title.
        /// </summary>
        /// <param name="title">The title to search for.</param>
        /// <returns>List of AbstractItem objects</returns>
        List<AbstractItem> GetItemsByTitle(string title);

        /// <summary>
        /// Retrieves a list of AbstractItem objects from the collection that match the specified AbstractItem based on equality comparison.
        /// </summary>
        /// <param name="searchItem">The AbstractItem to search for.</param>
        /// <returns>List of AbstractItem objects</returns>
        List<AbstractItem> GetItemsByItem(AbstractItem searchItem);

        /// <summary>
        /// Retrieves a list of items filtered by a search item and a maximum price.
        /// </summary>
        /// <param name="searchItem">The item to search for.</param>
        /// <param name="maxPrice">The maximum price for filtering the items.</param>
        /// <returns>A list of items matching the search item and within the specified price range.</returns>
        List<AbstractItem> GetItemsByItemToMaxPrice(AbstractItem searchItem, double maxPrice);

        /// <summary>
        /// Sorts the list of items by title.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        void SortByTitle(bool ascending);

        /// <summary>
        /// Sorts the list of items by type and title.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        void SortByType(bool ascending);

        /// <summary>
        /// Sorts the list of items by category.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        void SortByCategory(bool ascending);

        /// <summary>
        /// Sorts the list of items by price.
        /// </summary>
        /// <param name="ascending">True if sorting should be done in ascending order, false otherwise.</param>
        void SortByPrice(bool ascending);
    }
}
