using System.ComponentModel.DataAnnotations;

namespace ProductFF.Dtos
{
    public class CategoryDto
    {
        public record AddCategory(
                    [Required(ErrorMessage = "Category name is required.")]
            [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
            string Name,

                    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
            string Description
                );

        public record ViewCategory(
            int Id,
            string Name,
            string Description
        );

    }
}
