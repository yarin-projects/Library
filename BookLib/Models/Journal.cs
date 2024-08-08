using System;

namespace BookLib.Models
{
    /// <summary>
    /// Represents a journal item in the library. Inherits from the AbstractItem class.
    /// </summary>
    public class Journal : AbstractItem
    {
        /// <summary>
        /// Gets or sets the months in which the journal is published.
        /// </summary>
        public Months Months { get; set; }

        /// <summary>
        /// Gets or sets the category of the journal.
        /// </summary>
        public JournalCategories Category { get; set; }

        /// <summary>
        /// Gets or sets the International Standard Serial Number (ISSN) of the journal.
        /// </summary>
        public string ISSN { get; set; }

        /// <summary>
        /// Initializes a new instance of the Journal class with the specified properties.
        /// </summary>
        /// <param name="title">The title of the journal.</param>
        /// <param name="publishDate">The publish date of the journal.</param>
        /// <param name="copyNum">The copy number of the journal.</param>
        /// <param name="category">The category of the journal.</param>
        /// <param name="months">The months in which the journal is published.</param>
        /// <param name="price">The price of the journal.</param>
        /// <param name="issn">The ISSN of the journal.</param>
        public Journal(string title, DateTime publishDate, int copyNum, JournalCategories category, Months months,
            double price, string issn) : base(title, publishDate, copyNum)
        {
            Category = category;
            TheCategory = category.ToString();
            Months = months;
            ISSN = issn;
            Type = "Journal";
            _price = price;
            Id = issn;
        }

        /// <summary>
        /// Initializes a new instance of the Journal class with default values.
        /// Required for file handling.
        /// </summary>
        public Journal() { }

        /// <summary>
        /// Creates a new Journal object and initializes its properties with the values from the current Journal object.
        /// </summary>
        /// <returns>Journal object</returns>
        public Journal CopyJournalDetails()
        {
            return new Journal()
            {
                Title = Title,
                PublishDate = PublishDate,
                ISSN = ISSN,
                Category = Category,
                Months = Months,
                TheCategory = Category.ToString(),
                Type = "Journal",
                Id = ISSN,
                _price = _price,
                DiscountActive = DiscountActive
            };
        }

        /// <summary>
        /// Recalculates the original price of an item by dividing its current price by the complement of the discount percentage.
        /// </summary>
        public void RecoverOriginalPrice() => _price = _price / (1 - 0.1);

        /// <summary>
        /// Calculates the original price of an item by dividing its current price by the complement of the discount percentage.
        /// </summary>
        /// <returns>The calculated original price as a double value.</returns>
        public double GetOriginalPrice() => Price / (1 - 0.1);
    }
}
