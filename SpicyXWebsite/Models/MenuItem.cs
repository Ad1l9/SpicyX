using SpicyXWebsite.Models.Base;

namespace SpicyXWebsite.Models
{
	public class MenuItem:BaseNameableModel
	{
		public decimal Price { get; set; }
		public string ImageUrl { get; set; }
		public string Description { get; set; }

	}
}
