using BookStore.Models;
using System;
using System.Collections.Generic;

namespace BookStore.ViewModel
{
	public class OrderDetailViewModel
	{
		public string BookId { get; set; }
		public string UserId { get; set; }
		public Book Book { get; set; }
		public float Quantity { get; set; }
		public float TotalPrice { get; set; }
		public DateTime DateTime { get; set; }
		public OrderDetailViewModel()
		{
			DateTime = DateTime.Now;
		}
		public List<OrderDetail> OrderDetail { get; set; }

		public IEnumerable<User> User { get; set; }
	}
}