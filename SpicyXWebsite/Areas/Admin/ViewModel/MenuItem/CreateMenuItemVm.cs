using System.ComponentModel.DataAnnotations;

namespace SpicyXWebsite.Areas.Admin.ViewModel
{
	public class CreateMenuItemVm
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Invalid name")]
        public string Name { get; set; } = null!;
		public IFormFile Photo { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
	}
}
