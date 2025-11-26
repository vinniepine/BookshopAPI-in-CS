using System.ComponentModel.DataAnnotations;

namespace BookshopAPI.DTOs
{
    public class BookCreateDTO
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Author { get; set; } = string.Empty;
        [Required]
        public string Genre { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
