using BookStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private ApplicationUser _user;
		private ApplicationDbContext _context;
		private UserManager<ApplicationUser> _usermanager;
		public AdminController()
		{
			_user = new ApplicationUser();
			_context = new ApplicationDbContext();
			_usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
		}
		// GET: Admin
		public ActionResult Index()
		{

			return View();
		}
		// list user 
		public ActionResult UserManage(string searchString)
		{
			var user = _context.User.ToList();
			if (!String.IsNullOrWhiteSpace(searchString))
			{
				user = _context.User
			 .Where(t => t.Email.Contains(searchString))
			 .ToList();
			}
			return View(user);
		}

		//delete User
		public ActionResult DeleteUser(string id)
		{
			var removeUser = _context.Users.SingleOrDefault(t => t.Id == id);
			_context.Users.Remove(removeUser);
			_context.SaveChanges();
			return RedirectToAction("");
		}
		//update user
		[HttpGet]
		public ActionResult UpdateUser(string id)
		{
			var user = _context.Users
					.OfType<User>()
					.SingleOrDefault(t => t.Id == id);
			var updateUser = new User()
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				FullName = user.FullName,
				Address = user.Address
			};
			return View(updateUser);
		}
		[HttpPost]
		public ActionResult UpdateUser(User detailUser)
		{
			var userID = _context.Users.OfType<User>().FirstOrDefault(t => t.Id == detailUser.Id);
			userID.UserName = detailUser.UserName;
			userID.FullName = detailUser.FullName;
			userID.Address = detailUser.Address;
			userID.PhoneNumber = detailUser.PhoneNumber;
			_context.SaveChanges();
			return RedirectToAction("UserManage", "Admin");
		}
		// change password by admin
		[HttpGet]
		public ActionResult ChangePassword(string id)
		{
			var user = _context.Users.FirstOrDefault(model => model.Id == id);
			var changePasswordViewModel = new AdminChangePasswordViewModel()
			{
				UserId = user.Id
			};

			return View(changePasswordViewModel);
		}
		[HttpPost]
		public ActionResult ChangePassword(AdminChangePasswordViewModel model)
		{
			var user = _context.Users.SingleOrDefault(t => t.Id == model.UserId);
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("Validation", "Some thing is wrong");
				return View(model);
			}
			if (user.PasswordHash != null)
			{
				_usermanager.RemovePassword(user.Id);
			}
			_usermanager.AddPassword(user.Id, model.NewPassword);
			return RedirectToAction("UserManage", "Admin");
		}
		//manage category
		public ActionResult CategoryView(string searchString)
		{
			var categories = _context.Category.ToList();
			if (!String.IsNullOrWhiteSpace(searchString))
			{
				categories = _context.Category
			 .Where(t => t.CategoryName.Contains(searchString))
			 .ToList();
			}
			return View(categories);
		}
		//tao category
		[HttpGet]
		public ActionResult CreateCategory()
		{
			return View();
		}
		[HttpPost]
		public ActionResult CreateCategory(Category category)
		{
			var create_category = new Category() { CategoryName = category.CategoryName };
			if (_context.Category.Any(t => t.CategoryName == category.CategoryName))
			{
				ModelState.AddModelError("Validation", "Existed before");
				return View(category);
			}
			_context.Category.Add(create_category);
			_context.SaveChanges();
			return RedirectToAction("CategoryView");
		}
		//xoa category
		public ActionResult DeleteCategory(int id)
		{
			var removeCategory = _context.Category.SingleOrDefault(t => t.Id == id);
			_context.Category.Remove(removeCategory);
			_context.SaveChanges();
			return RedirectToAction("CategoryView");
		}
		//chinh sau category
		[HttpGet]
		public ActionResult EditCategory(int id)
		{
			var category = _context.Category.SingleOrDefault(t => t.Id == id);
			var categories = new Category()
			{
				Id = id,
				CategoryName = category.CategoryName
			};

			return View(categories);
		}
		[HttpPost]
		public ActionResult EditCategory(Category viewModel)
		{
			var category = _context.Category.SingleOrDefault(t => t.Id == viewModel.Id);
			category.CategoryName = viewModel.CategoryName;

			_context.SaveChanges();
			return RedirectToAction("CategoryView", "Admin");
		}
		//book manage
		public ActionResult BookManage(string searchString)
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
		//tao course
		[HttpGet]
		public ActionResult CreateBook()
		{
			var courseCategory = new ViewModel.CategoryBookViewModel()
			{
				Category = _context.Category.ToList(),
			};
			return View(courseCategory);
		}
		[HttpPost]
		public ActionResult CreateBook(ViewModel.CategoryBookViewModel categoryBookModel)
		{
			var new_book = new Book()
			{
				Title = categoryBookModel.Book.Title,
				Description = categoryBookModel.Book.Description,
				Author = categoryBookModel.Book.Author,
				Pages = categoryBookModel.Book.Pages,
				ImageUrl = categoryBookModel.Book.ImageUrl,
				Quantity = categoryBookModel.Book.Quantity,
				CategoryID = categoryBookModel.Id,
				Price = categoryBookModel.Book.Price
			};
			_context.Book.Add(new_book);
			_context.SaveChanges();
			return RedirectToAction("BookManage");
		}

		//xóa course
		public ActionResult DeleteBook(int id)
		{
			var removeBook = _context.Book.SingleOrDefault(t => t.Id == id);
			_context.Book.Remove(removeBook);
			_context.SaveChanges();
			return RedirectToAction("BookManage");
		}

		//edit course
		public ActionResult EditBook(int id)
		{
			var book = _context.Book.SingleOrDefault(t => t.Id == id);
			var books = new ViewModel.CategoryBookViewModel()
			{
				Id = id,
				Book = book,
				Category = _context.Category.ToList()
			};
			return View(books);
		}
		[HttpPost]
		public ActionResult EditBook(ViewModel.CategoryBookViewModel viewModel)
		{
			var book = _context.Book.SingleOrDefault(t => t.Id == viewModel.Id);
			book.Title = viewModel.Book.Title;
			book.CategoryID = viewModel.Book.CategoryID;
			book.Description = viewModel.Book.Description;
			book.Pages = viewModel.Book.Pages;
			book.Price = viewModel.Book.Price;
			book.Author = viewModel.Book.Author;
			book.ImageUrl = viewModel.Book.ImageUrl;
			book.Quantity = viewModel.Book.Quantity;
			_context.SaveChanges();
			return RedirectToAction("BookManage", "Admin");
		}
		//detail course
		public ActionResult DetailBook(int id)
		{
			var book = new ViewModel.CategoryBookViewModel();
			book.Id = id;
			book.Book = _context.Book.Include(t => t.Category).SingleOrDefault(t => t.Id == id);
			return View(book);
		}
		public ActionResult OrderList(string searchString)
		{

			var book = _context.OrderDetail.Include(t => t.Book.Category).Include(t => t.User).OrderByDescending(t => t.Datetime).ToList();
			if (!String.IsNullOrWhiteSpace(searchString))
			{
				book = _context.OrderDetail
				.Where(t => t.Book.Title.Contains(searchString))
				.Include(t => t.Book.Category)

				.ToList();
			}
			return View(book);
		}
		public ActionResult OrderDetail(int id)
		{
			var orderDetail = _context.OrderDetail.Include(t => t.User).Include(t => t.Book).SingleOrDefault(t => t.Id == id);
			return View(orderDetail);
		}

	}
}