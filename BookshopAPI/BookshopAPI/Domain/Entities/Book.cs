using System.ComponentModel.DataAnnotations;

namespace BookshopAPI.Domain.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Author { get; set; } = string.Empty;
        [Required]
        public string Genre { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Price must be >= 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be >= 0")]
        public int Stock { get; set; }
    }
}
