using System.ComponentModel.DataAnnotations;

namespace SpicyXWebsite.Areas.Admin.ViewModel
{
    public class UpdateMenuItemVm
    {
        [Required]
        [MaxLength(50,ErrorMessage ="Invalid name")]
        public string Name { get; set; }
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
