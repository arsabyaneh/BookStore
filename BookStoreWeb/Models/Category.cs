using System.ComponentModel.DataAnnotations;

namespace BookStoreWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int DisplayOder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
