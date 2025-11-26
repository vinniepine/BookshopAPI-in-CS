using System.ComponentModel.DataAnnotations;

namespace BookshopAPI.DTOs
{
    public class BookUpdateDTO
    {
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Author { get; set; } = null!;

        [Required]
        public string Genre { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
