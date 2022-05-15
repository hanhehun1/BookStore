using BookStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
	[Authorize(Roles = "User")]
	public class UserController : Controller
	{
		private ApplicationUser _user;
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _usermanager;

		private ApplicationUserManager _userManager;
		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}
		public UserController()
		{
			_user = new ApplicationUser();
			_context = new ApplicationDbContext();
			_usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}
		// GET: User
		//updaet trainee
		[HttpGet]
		public ActionResult Checkout(int id)
		{
			var assignModel = new ViewModel.OrderDetailViewModel()
			{
				Book = _context.Book.Include(t => t.Category).SingleOrDefault(t => t.Id == id),

			};


			return View(assignModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Checkout(ViewModel.OrderDetailViewModel model)
		{

			var userId = User.Identity.GetUserId();
			var userName = User.Identity.GetUserName();
			var orderDetail = new OrderDetail()
			{
				UserID = userId,
				Datetime = model.DateTime,
				BookID = model.Book.Id,
				Quantity = model.Quantity,
				TotalPrice = model.Quantity * model.Book.Price
			};
			// tru di so luong da co 
			Book update = _context.Book.ToList().Find(u => u.Id == orderDetail.BookID);
			update.Quantity -= orderDetail.Quantity;


			_context.OrderDetail.Add(orderDetail);
			_context.SaveChanges();
			//gui mail
			await UserManager.SendEmailAsync(orderDetail.UserID, "Order Information", "Dear \"" + userName + "\". Your order information is Book Name: \"" + orderDetail.Book.Title + "\"  , Author:  \"" + orderDetail.Book.Author + "\" , Price:  \"" + orderDetail.Book.Price + "\"., Quantity:  \"" + orderDetail.Quantity + "\" , Total Price:  \"" + orderDetail.TotalPrice + "\". And Thanks for choosing us!");
			return RedirectToAction("OrderList", "User");
		}
		//update price

		public ActionResult OrderList(string searchString)
		{
			var userId = User.Identity.GetUserId();
			var book = _context.OrderDetail.Include(t => t.Book.Category).OrderByDescending(t => t.Datetime).Where(t => t.UserID == userId).ToList();
			if (!String.IsNullOrWhiteSpace(searchString))
			{
				book = _context.OrderDetail
				.Where(t => t.Book.Title.Contains(searchString))
				.Include(t => t.Book.Category)
				.ToList();
			}
			return View(book);
		}
		public ActionResult MyProfile()
		{
			//get user id dang dang nhap
			var userid = User.Identity.GetUserId();
			// dem tat ca cac order by user id
			var orderCount = _context.OrderDetail.Where(t => t.UserID == userid).ToList();
			ViewBag.TotalOrder = orderCount.Count();

			var user = _context.Users.OfType<User>().SingleOrDefault(t => t.Id == userid);
			return View(user);
		}
		public ActionResult OrderDetail(int id)
		{
			var orderDetail = _context.OrderDetail.Include(t => t.User).Include(t => t.Book).SingleOrDefault(t => t.Id == id);
			return View(orderDetail);
		}

	}
}