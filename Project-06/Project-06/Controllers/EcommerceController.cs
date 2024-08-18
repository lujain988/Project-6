using Project_06.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_06.Controllers
{
    public class EcommerceController : Controller
    {
        private EcommerceEntities db = new EcommerceEntities();
        public ActionResult Index()

        {

            return View(db.Categories.ToList());
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User user)
        {
            if (user.PasswordHash != user.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
            }

            if (ModelState.IsValid)
            {
                var existUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existUser == null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists.");
                }
            }
            return View(user);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                ModelState.AddModelError("", "Password is required.");
                return View(user);
            }

            var existingUser = db.Users.FirstOrDefault(u => u.Email == user.Email && u.PasswordHash == user.PasswordHash);

            if (existingUser != null)
            {
                Session["UserEmail"] = existingUser.Email;
                Session["UserID"] = existingUser.ID;

                return RedirectToAction("Index", "Ecommerce");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(user);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login", "Ecommerce");
        }
        [HttpGet]
        public ActionResult Profile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost]
        public ActionResult Profile(User user)
        {
            if (ModelState.IsValid)
            {
                // Reload the user entity to ensure it's the latest version
                var userInDb = db.Users.Find(user.ID);

                if (userInDb == null)
                {
                    return HttpNotFound();
                }
                userInDb.FirstName = user.FirstName;
                userInDb.LastName = user.LastName;
                userInDb.Email = user.Email;
                userInDb.PasswordHash = user.PasswordHash;
                userInDb.PhoneNumber = user.PhoneNumber;

                db.SaveChanges();
                TempData["ProfileUpdated"] = true;
                return RedirectToAction("Profile");
            }
            return View(user);
        }



        [HttpGet]
        public ActionResult Shop(int? id)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            string categoryName = "All Products";

            List<Product> products;

            if (id == null)
            {
                products = db.Products.ToList();
            }
            else
            {
                products = db.Products.Where(p => p.CategoryID == id).ToList();
                var category = db.Categories.FirstOrDefault(c => c.ID == id);
                if (category != null)
                {
                    categoryName = category.CategoryName;
                }
            }

            ViewBag.CategoryName = categoryName;

            return View(products);
        }


        public ActionResult ShowMore(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View("ShowMore", product);
        }
        public ActionResult Cart()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Ecommerce");
            }

            int userID = (int)Session["UserID"];

            var cartItems = db.Carts
                              .Where(c => c.UserID == userID)
                              .Include(c => c.Product)
                              .ToList();

            return View(cartItems);
        }


        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Ecommerce");
            }
            int userID = (int)Session["UserID"];

            var existingCartItem = db.Carts.FirstOrDefault(c => c.UserID == userID && c.ProductID == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new Cart
                {
                    UserID = userID,
                    ProductID = productId,
                    Quantity = quantity
                };

                db.Carts.Add(cartItem);
            }

            db.SaveChanges();

            return RedirectToAction("Cart");
        }

        public ActionResult UpdateCart(int id, bool increment)
        {
            var cart = db.Carts.FirstOrDefault(c => c.ID == id);
            if (cart != null)
            {
                if (increment)
                {
                    cart.Quantity += 1;  
                }
                else
                {
                    cart.Quantity -= 1; 
                    if (cart.Quantity <= 0)
                    {
                        db.Carts.Remove(cart);  
                    }
                }

                db.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        public ActionResult DeleteCart(int id) {
        var cart = db.Carts.FirstOrDefault(x => x.ID == id);
            if (cart != null) {
                db.Carts.Remove(cart);
                db.SaveChanges();
            }
            return RedirectToAction("Cart");
        }



    }
}