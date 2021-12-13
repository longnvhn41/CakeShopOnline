using CakeShop.DAO;
using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CakeShop.Controllers
{
    public class OrderController : Controller
    {
        // GET: List Order for customer
        public ActionResult IndexForCustomer()
        {
            try
            {
                UserDetail user = Session["user"] as UserDetail;
                if (user != null)
                {
                    List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                    ViewData["ProductCategoryList"] = listPC;
                    //PAGING
                    int pageSize = 6;
                    int index = 1;
                    if (Request.Params["index"] != null)
                    {
                        index = Convert.ToInt32(Request.Params["index"]);
                    }
                    int size = 0;
                    //List<Order> listO = new List<Order>();
                    OrderDAO odb = new OrderDAO();
                    size = odb.getOrderByNameSize(user.username);
                    List<Order> listO = odb.GetOrderByUsername(index, pageSize, user.username);
                    ViewData["OrderList"] = listO;
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
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: List Order for admin
        public ActionResult IndexForAdmin()
        {
            try
            {
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;

                //PAGING
                int pageSize = 6;
                int index = 1;
                if (Request.Params["index"] != null)
                {
                    index = Convert.ToInt32(Request.Params["index"]);
                }
                int size = 0;
                //List<Order> listO = new List<Order>();
                OrderDAO odb = new OrderDAO();
                size = odb.getOrderSize();
                List<Order> listO = odb.GetOrder(index, pageSize);
                ViewData["OrderList"] = listO;
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
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Order detail for customer
        public ActionResult DetailsForCustomer(string id)
        {
            List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            ViewData["Order"] = new OrderDAO().getOrderByID(id);
            List<OrderDetail> listOD = new OrderDetailDAO().getOrderDetailByOrderID(id);
            ViewData["OrderDetail"] = (listOD == null) ? null : listOD;
            return View();
        }

        // GET: Order detail for admin
        public ActionResult DetailsForAdmin(string id)
        {
            List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            ViewData["Order"] = new OrderDAO().getOrderByID(id);
            List<OrderDetail> listOD = new OrderDetailDAO().getOrderDetailByOrderID(id);
            ViewData["OrderDetail"] = (listOD == null) ? null : listOD;
            return View();
        }

        // GET: Create order for customer
        public ActionResult CreateForCustomer()
        {
            List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            return View();
        }

        //
        public ActionResult CreateOrderForAdmin()
        {
            try
            {

                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                string username = (Session["userDetail"] as UserDetail).username;
                string address = Request.Form["address"];
                string phone = Request.Form["phone"];
                Regex r = new Regex("^[0-9]+$");
                if (!r.IsMatch(phone.Trim()))
                {
                    return RedirectToAction("../Order/CreateForCustomer", new { errorString = "Phone must only 10 digits number" });
                }
                string email = Request.Form["email"];
                OrderDAO order = new OrderDAO();
                order.insertOrder(username, address, phone, "request");
                List<Cart> list = Session["cart"] as List<Cart>;
                int id = order.getLastOrderID();
                if (list != null)
                {
                    foreach (Cart cart in list)
                    {
                        order.insertOrderdetail(id, cart.productID, cart.quantity);
                    }
                }
                Session["cart"] = null;
                HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
                if (c_id != null)
                {
                    c_id.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(c_id);
                }
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: Create order for admin
        public ActionResult CreateForAdmin()
        {
            try
            {

                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                string username = Request.Params["username"];
                UserDetailDAO ud = new UserDetailDAO();
                UserDetail userdetail=  ud.GetUserDetailByUsername(username);
                Session["userDetail"] = userdetail;
                //PAGING
                
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Create order
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string username = (Session["user"] as UserDetail).username;
                string address = Request.Form["address"];
                string phone = Request.Form["phone"];
                Regex r = new Regex("^[0-9]+$");
                if (!r.IsMatch(phone.Trim()))
                {
                    return RedirectToAction("../Order/CreateForCustomer", new { errorString = "Phone must only 10 digits number" });
                }
                string email = Request.Form["email"];
                OrderDAO order = new OrderDAO();
                order.insertOrder(username, address, phone, "request");
                List<Cart> list = Session["cart"] as List<Cart>;
                int id = order.getLastOrderID();
                if (list != null)
                {
                    foreach (Cart cart in list)
                    {
                        order.insertOrderdetail(id, cart.productID, cart.quantity);
                    }
                }

                Session["cart"] = null;
                HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
                if (c_id != null)
                {
                    c_id.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(c_id);
                }

                return RedirectToAction("IndexForCustomer", "Order");

            }
            catch
            {
                return RedirectToAction("IndexForCustomer", "Order");
            }
        }

        // POST: Edit
        [HttpGet]
        public ActionResult EditForAdmin(string id, FormCollection collection)
        {
            try
            {
                string status = Request.Params["newStatus"];
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                if (new OrderDAO().UpdateStatus(id, status))
                {
                    return RedirectToAction("IndexForAdmin", "Order");
                }
                return RedirectToAction("IndexForAdmin", "Order");
            }
            catch
            {
                return RedirectToAction("IndexForAdmin", "Order");
            }
        }

        [HttpPost]
        public ActionResult EditForAdmin()
        {
            try
            {
                string status = Request.Form["newStatus"];
                string oid = Request.Form["id"];
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                if (new OrderDAO().UpdateStatus(oid, status))
                {
                    return RedirectToAction("DetailsForAdmin", "Order", new { ID = oid });
                }
                return RedirectToAction("IndexForAdmin", "Order");
            }
            catch
            {
                return RedirectToAction("IndexForAdmin", "Order");
            }
        }

        // POST: Edit
        [HttpGet]
        public ActionResult EditForCustomer(string id, FormCollection collection)
        {
            try
            {
                string status = Request.Params["newStatus"];
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                if (new OrderDAO().UpdateStatus(id, status))
                {
                    return RedirectToAction("IndexForCustomer", "Order");
                }
                return RedirectToAction("IndexForCustomer", "Order");
            }
            catch
            {
                return RedirectToAction("IndexForCustomer", "Order");
            }
        }

        // POST: Delete
        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            try
            {
                if (new OrderDAO().Delete(Request.Form["oid"]))
                {
                    List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                    ViewData["ProductCategoryList"] = listPC;
                    ViewData["Order"] = new OrderDAO().getOrderByID(Request.Params["oid"]);
                    return RedirectToAction("../Order/IndexForAdmin");
                }
                else
                {
                    ViewData["error"] = "Delete order fail";
                    return RedirectToAction("../Order/IndexForAdmin");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
