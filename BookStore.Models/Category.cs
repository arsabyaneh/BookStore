using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        [Range(1, 100, ErrorMessage = "The value must be in range 1 to 100!")]
        public int DisplayOder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
