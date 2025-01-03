using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductFF.Model
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        
        [StringLength(100 , ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; }
        [StringLength(400 , ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
