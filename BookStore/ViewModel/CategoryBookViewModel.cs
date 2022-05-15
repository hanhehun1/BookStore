using BookStore.Models;
using System.Collections.Generic;

namespace BookStore.ViewModel
{
	public class CategoryBookViewModel
	{
		public Book Book { get; set; }
		public int Id { get; set; }
		public IEnumerable<Category> Category { get; set; }
	}
}