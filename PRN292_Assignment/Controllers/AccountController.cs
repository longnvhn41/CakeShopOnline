using CakeShop.DAO;
using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace CakeShop.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            string username = Request.Form["username"];
            string password = Request.Form["password"];
            AccountDAO adb = new AccountDAO();
            bool isLogin = adb.Login(username, password);
            if (isLogin && ModelState.IsValid)
            {
                UserDetail user = new UserDetailDAO().GetUserDetailByUsername(username);
                Session["user"] = user;
                Session["countLoadCartController"] = 0;
                CartDAO Cart = new CartDAO();
                List<Cart> lc = Cart.GetAllCartbyUserName(username);
                Session["cart"] = (lc != null) ? lc : null;
                if (lc != null)
                {
                    string txt = "";
                    foreach (Cart cart in lc)
                    {
                        if (txt.Trim().Length == 0)
                        {
                            txt = cart.productID + "";

                        }
                        else
                        {
                            txt = txt + "," + cart.productID;
                        }
                        if (cart.quantity > 1)
                        {
                            for (int i = 1; i < cart.quantity; i++)
                            {
                                txt = txt + "," + cart.productID;
                            }
                        }
                    }
                    HttpCookie c1 = new HttpCookie("cookieId", txt);
                    c1.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(c1);

                }
                List<UserDetail> listUD = new List<UserDetail>();
                UserDetailDAO ud = new UserDetailDAO();
                listUD = ud.GetUserDetail();
                Session["listUD"] = listUD;
                new CartDAO().deleteByUsername(username);
                return RedirectToAction("../Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Username or password is incorrect");
                return View();
            }

        }



        [HttpGet]
        public ActionResult Logout()
        {
            string txt = "";
            HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
            if (c_id != null)
            {
                txt += c_id.Value;
                c_id.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(c_id);
            }
            var list = Session["cart"] as List<Cart>;
            UserDetail user = Session["user"] as UserDetail;
            CartDAO cDao = new CartDAO();
            if (list != null)
            {
                foreach (Cart c in list)
                {
                    cDao.insertToCart(user.username, c.productID, c.quantity);
                }
            }

            foreach (string key in Request.Cookies.AllKeys)
            {
                HttpCookie c = Request.Cookies[key];
                c.Expires = DateTime.Now.AddSeconds(-1);
                Response.AppendCookie(c);
            }

            int loop1, loop2;
            HttpCookieCollection MyCookieColl;
            HttpCookie MyCookie;
            MyCookieColl = Request.Cookies;

            // Capture all cookie names into a string array.
            String[] arr1 = MyCookieColl.AllKeys;

            // Grab individual cookie objects by cookie name.
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                MyCookie = MyCookieColl[arr1[loop1]];
                Response.Write("Cookie: " + MyCookie.Name + "<br>");
            }
            Session.Clear();
            return RedirectToAction("../Home/Index");
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {

            string username = Request.Form["userName_register"];
            string password = Request.Form["userPassword_register"];
            string rePassword = Request.Form["reUserPassword_register"];
            if (password.Trim().Length < 8 || rePassword.Length < 8)
            {
                return RedirectToAction("../Account/Login", new { error = "Password must more  than 8 chars" });
            }
            AccountDAO adb = new AccountDAO();
            Account a = adb.GetUserByUsername(username);
            if (a != null)
            {
                return RedirectToAction("../Account/Login", new { error = "User already Exist" });
            }
            else
            {
                if (!password.Equals(rePassword))
                {
                    return RedirectToAction("../Account/Login", new { error = "Password and repassword must not match" });

                }
                else
                {
                    string fullName = Request.Form["fullname"];
                    string email = Request.Form["email"];
                    string phone = Request.Form["phone"];
                    Regex r = new Regex("^[0-9]+$");
                    if (!r.IsMatch(phone.Trim()))
                    {
                        return RedirectToAction("../Account/Login", new { error = "Phone must only 10 digits number" });
                    }
                    string address = Request.Form["address"];
                    //string image = Request.Form["image"];
                    adb.insertUserDetail(username, fullName, email, phone, address, "imgC/1.png");
                    adb.register(username, password);
                    return RedirectToAction("../Account/Login", new { error = "User create success" });
                }
            }
        }
        // GET: List for edit
        public ActionResult Index()
        {
            ProductCategoryDAO pcdb = new ProductCategoryDAO();
            List<ProductCategory> listPC = pcdb.GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;

            //PAGING
            int pageSize = 6;
            int index = 1;
            if (Request.Params["index"] != null)
            {
                index = Convert.ToInt32(Request.Params["index"]);
            }
            int size = 0;
            UserDetailDAO udb = new UserDetailDAO();
            size = udb.GetAccountSize();
            List<UserDetail> listU = udb.GetUserDetailByPage(index, pageSize);
            ViewData["ListAccount"] = listU;
            int numberPage = size / pageSize;
            if (size % pageSize != 0)
            {
                numberPage++;
            }
            ViewData["numberPage"] = numberPage;
            ViewData["index"] = index;
            ViewData["size"] = size;

            return View();
        }

        // GET: Profile for user
        public ActionResult DetailsForUser()
        {
            ProductCategoryDAO pcdb = new ProductCategoryDAO();
            List<ProductCategory> listPC = pcdb.GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            
            return View();
        }

        // GET: Profile for admin
        public ActionResult DetailsForAdmin(string id)
        {
            ProductCategoryDAO pcdb = new ProductCategoryDAO();
            List<ProductCategory> listPC = pcdb.GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
           
            Session["userChange"] = new UserDetailDAO().GetUserDetailByUsername(id);
            return View();
        }

        [HttpPost]
        public ActionResult changeType(FormCollection collection)
        {
            try
            {
                ProductCategoryDAO pcdb = new ProductCategoryDAO();
                List<ProductCategory> listPC = pcdb.GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                string username = Request.Form["username"];
                UserDetail u = new UserDetailDAO().GetUserDetailByUsername(username);
                // ViewData["UserDetail"] = new UserDetailDAO().GetUserDetailByUsername(username);
                Session["userChange"] = new UserDetailDAO().GetUserDetailByUsername(username);
                int type = Convert.ToInt32(Request.Form["type"]);
                AccountDAO acc = new AccountDAO();
                acc.ChangeType(username, type);
                return RedirectToAction("Index", "Account");
            }
            catch
            {
                return RedirectToAction("Index", "Account");
            }
        }
        // POST: Edit
        [HttpPost]
        public ActionResult EditForUser(FormCollection collection)
        {
            try
            {
                UserDetail user = Session["user"] as UserDetail;
                string phone = Request.Form["phone"];
                string fullname = Request.Form["fullname"];
                string email = Request.Form["email"];
                string address = Request.Form["address"];
                string img = user.imageLink;
                HttpPostedFileBase file = Request.Files["imageFile"];
                if (file != null && file.ContentLength != 0)
                {
                   img = UploadImage(file);
                }
                if (Request.Form["changePassword"] != null)
                {
                    string pass = Request.Form["newPassword"];
                    new AccountDAO().updatePassByUsername(user.username, pass);
                    new UserDetailDAO().updateProfile(user.username, fullname, address, phone, email, img);
                    Session["user"] = new UserDetailDAO().GetUserDetailByUsername(user.username);
                    Console.WriteLine(user.imageLink);
                }
                else
                {
                    new UserDetailDAO().updateProfile(user.username, fullname, address, phone, email, img);
                    Session["user"] = new UserDetailDAO().GetUserDetailByUsername(user.username);
                    Console.WriteLine(user.imageLink);
                }
                return RedirectToAction("DetailsForUser", "Account" , new { id = user.username});
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //Upload Image 
        public string UploadImage(HttpPostedFileBase file)
        {
            var allowedExtensions = new[] {".Jpg", ".png", ".jpg", "jpeg"};
            var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
            var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
            if (allowedExtensions.Contains(ext)) //check what type of extension  
            {
                string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                long elapsedTicks = DateTime.Now.Ticks - new DateTime(2001, 1, 1).Ticks;
                string myfile = name + "_" + elapsedTicks + ext; //appending the name with ticks 
                // store the file inside Content/img/ava
                var path = Path.Combine(Server.MapPath("~/Content/img/ava"), myfile);
                file.SaveAs(path);
                return myfile;
            }
            else
            {
                ViewData["error"] = "Please choose only Image file";
            }
            return null;
        }

        // Ban user
        [HttpGet]
        public ActionResult Ban(string id)
        {
            try
            {
                AccountDAO acc = new AccountDAO();
                acc.ChangeType(id, 4);

                return RedirectToAction("Index", "Account");
            }
            catch
            {
                return RedirectToAction("Index", "Account");
            }
        }
        // POST: Delete
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
