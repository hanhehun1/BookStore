using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
	public class Book
	{
		public int Id { get; set; }

		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		public int Pages { get; set; }
		public string Author { get; set; }
		public float Price { get; set; }
		public int CategoryID { get; set; }
		public Category Category { get; set; }
		public float Quantity { get; set; }

		[DisplayName("Upload Image")]
		public string ImageUrl { get; set; }


	}
}