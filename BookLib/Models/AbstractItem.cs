using System;

namespace BookLib.Models
{
    /// <summary>
    /// The AbstractItem class serves as a blueprint for items in a generic inventory system.
    /// </summary>
    public abstract class AbstractItem
    {
        /// <summary>
        /// Field to store the original price of the item.
        /// </summary>
        protected double _price;

        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the category of the item.
        /// </summary>
        public string TheCategory { get; set; }

        /// <summary>
        /// Gets or sets the Copies Owned number of the item.
        /// </summary>
        public int CopiesOwned { get; set; }

        /// <summary>
        /// Gets or sets the Id of the item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the return date of the item.
        /// </summary>
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// Gets or sets the price of the item. If a discount is active, returns the price after applying the discount.
        /// </summary>
        public double Price
        {
            get
            {
                if (DiscountActive)
                    return AfterDiscount();
                else
                    return _price;
            }
            set => _price = value;
        }

        /// <summary>
        /// Gets or sets the publish date of the item.
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the copy number of the item.
        /// </summary>
        public int CopyNum { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether a discount is active for the item.
        /// </summary>
        public bool DiscountActive { get; set; }

        /// <summary>
        /// Initializes a new instance of the AbstractItem class with the specified title, publish date, copy number, and price.
        /// </summary>
        /// <param name="title">The title of the item.</param>
        /// <param name="publishDate">The publish date of the item.</param>
        /// <param name="copyNum">The copy number of the item.</param>
        public AbstractItem(string title, DateTime publishDate, int copyNum)
        {
            Title = title;
            PublishDate = publishDate;
            CopyNum = copyNum;
            
        }

        /// <summary>
        /// Default constructor for the AbstractItem class.
        /// Required for the file handling.
        /// </summary>
        public AbstractItem() { }

        /// <summary>
        /// Protected virtual method that calculates the price after applying a discount. By default, it applies a 10% discount to the original price.
        /// </summary>
        /// <returns>The price of the book after discount.</returns>
        protected virtual double AfterDiscount() => _price * 0.9;
    }
}
