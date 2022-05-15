using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class OrderDetail
    {
		public class OrderDetail
		{
			public int Id { get; set; }
			public int BookID { get; set; }
			public Book Book { get; set; }
			public string UserID { get; set; }
			public User User { get; set; }
			public float TotalPrice { get; set; }
			public float Quantity { get; set; }
			public DateTime Datetime { get; set; }
			public OrderDetail()
			{
				Datetime = DateTime.Now;
			}
		}
}