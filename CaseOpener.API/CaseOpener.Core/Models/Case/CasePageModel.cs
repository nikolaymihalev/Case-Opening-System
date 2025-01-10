namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for page
    /// </summary>
    public class CasePageModel
    {
        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the case.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Image of the case.
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Price of the case.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Case's name
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
    }
}
