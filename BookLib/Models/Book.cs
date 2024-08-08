using System;

namespace BookLib.Models
{
    /// <summary>
    /// Represents a book item in the library. Inherits from the AbstractItem class.
    /// </summary>
    public class Book : AbstractItem
    {
        /// <summary>
        /// Gets or sets the International Standard Book Number (ISBN) of the book.
        /// </summary>
        public string ISBN { get; set; }

        /// <summary>
        /// Gets or sets the category of the book.
        /// </summary>
        public BookCategories Category { get; set; }

        /// <summary>
        /// Gets or sets the discount applied to the book.
        /// </summary>
        public double Discount { get; set; }

        /// <summary>
        /// Initializes a new instance of the Book class with the specified properties.
        /// </summary>
        /// <param name="title">The title of the book.</param>
        /// <param name="publishDate">The publish date of the book.</param>
        /// <param name="copyNum">The copy number of the book.</param>
        /// <param name="isbn">The ISBN of the book.</param>
        /// <param name="category">The category of the book.</param>
        /// <param name="price">The price of the book.</param>
        /// <param name="discount">The discount applied to the book.</param>
        public Book(string title, DateTime publishDate, int copyNum, string isbn, BookCategories category,
            double price, double discount) : base(title, publishDate, copyNum)
        {
            Category = category;
            TheCategory = category.ToString();
            ISBN = isbn;
            Discount = discount;
            Type = "Book";
            _price = price;
            Id = isbn;
        }

        /// <summary>
        /// Creates a new Book object and initializes its properties with the values from the current Book object.
        /// </summary>
        /// <returns>Book object</returns>
        public Book CopyBookDetails()
        {
            return new Book()
            {
                Title = Title,
                PublishDate = PublishDate,
                ISBN = ISBN,
                Category = Category,
                TheCategory = Category.ToString(),
                Type = "Book",
                Id = ISBN,
                _price = _price,
                Discount = Discount,
                DiscountActive = DiscountActive
            };
        }

        /// <summary>
        /// Initializes a new instance of the Book class with default values.
        /// Required for the file handling.
        /// </summary>
        public Book() { }

        /// <summary>
        /// Calculates the price of the book after applying the discount.
        /// </summary>
        /// <returns>The price of the book after discount.</returns>
        protected override double AfterDiscount() => _price * (1 - Discount);

        /// <summary>
        /// Recalculates the original price of an item based on its current price and discount percentage.
        /// </summary>
        public void RecoverOriginalPrice() => _price = _price / (1 - Discount);

        /// <summary>
        /// Calculates and returns the original price of an item based on its current price and discount percentage.
        /// </summary>
        /// <returns>The calculated original price as a double value.</returns>
        public double GetOriginalPrice() => Price / (1 - Discount);
    }
}
