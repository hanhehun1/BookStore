using BookStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.Controllers
{
	public class HomeController : Controller
	{
		private ApplicationUser _user;
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _usermanager;
		public HomeController()
		{
			_user = new ApplicationUser();
			_context = new ApplicationDbContext();
			_usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}
		public ActionResult Index(string searchString)
		{
			var book = _context.Book.Include(t => t.Category).ToList();
			if (!String.IsNullOrWhiteSpace(searchString))
			{
				book = _context.Book
				.Where(t => t.Title.Contains(searchString))
				.Include(t => t.Category)
				.ToList();
			}
			return View(book);
		}
		public ActionResult DetailBook(int id)
		{
			var detailBook = _context.Book.Include(t => t.Category).SingleOrDefault(t => t.Id == id);
			return View(detailBook);
		}
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}
		public ActionResult Help()
		{


			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}