namespace BookLib.Models
{
    /// <summary>
    /// The SortDirection class provides constants representing sorting directions, specifically ascending and descending order.
    /// </summary>
    public static class SortDirection
    {
        private const string ascending = " ↓ ";
        private const string descending = " ↑ ";

        /// <summary>
        ///  Provides access to the ascending sorting symbol.
        /// </summary>
        public static string Ascending => ascending;

        /// <summary>
        ///  Provides access to the descending sorting symbol.
        /// </summary>
        public static string Descending => descending;
    }
}
