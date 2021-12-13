using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CakeShop.DAO;
using CakeShop.Models;
namespace CakeShop.Controllers
{
    public class CartController : Controller
    {
        DataTable cart = new DataTable();
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                try
                {
                    Session["countLoadCartController"] = Convert.ToInt32(Session["countLoadCartController"]) + 1;
                    List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                    ViewData["ProductCategoryList"] = listPC;
                    List<Cart> list = new List<Cart>();
                    ProductDAO dao = new ProductDAO();
                    string txt = "";
                    Console.WriteLine(Session["cart"]);
                    HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
                    if (c_id != null)
                    {
                        txt += c_id.Value;
                    }
                    HttpCookie c1 = new HttpCookie("cookieId", txt);
                    c1.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(c1);
                    string[] txtList = txt.Split(',');
                    foreach (string s in txtList)
                    {
                        Product p = dao.GetProductByID(s);
                        Cart c = new Cart();
                        if (p != null)
                        {
                            c.productID = Convert.ToInt32(s);
                            c.quantity = 1;
                            c.Product = p;
                            list.Add(c);
                        }
                    }
                    // dồn các product lại 
                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        // list product 
                        int count = 1;
                        for (int j = i + 1; j < list.Count; j++)
                        {
                            if (list[i].productID == list[j].productID)
                            {
                                count++;
                                list.RemoveAt(j);
                                j--;
                                list[i].quantity = count;
                            }
                        }
                    }
                    float total = 0;
                    foreach (Cart c in list)
                    {
                        total += float.Parse(c.quantity.ToString()) * c.Product.price;
                    }
                    double finalPrice = total;
                    Session["cart"] = list;
                    Session["total"] = total;
                    Session["finalPrice"] = finalPrice;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult IndexForAdmin()
        {
            if (Session["user"] != null)
            {
                try
                {
                    string username = Request.Params["username"];
                    UserDetailDAO ud = new UserDetailDAO();
                    UserDetail userdetail = ud.GetUserDetailByUsername(username);
                    if (userdetail == null)
                    {

                        userdetail = new UserDetail();
                        userdetail.fullname = "";
                        userdetail.address = "";
                        userdetail.address = "";
                        userdetail.email = "";
                        

                    }
                    Session["userDetail"] = userdetail;

                    Session["countLoadCartController"] = Convert.ToInt32(Session["countLoadCartController"]) + 1;
                    List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                    ViewData["ProductCategoryList"] = listPC;
                    List<Cart> list = new List<Cart>();
                    ProductDAO dao = new ProductDAO();
                    string txt = "";
                    Console.WriteLine(Session["cart"]);
                    HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
                    if (c_id != null)
                    {
                        txt += c_id.Value;
                    }
                    HttpCookie c1 = new HttpCookie("cookieId", txt);
                    c1.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(c1);
                    string[] txtList = txt.Split(',');
                    foreach (string s in txtList)
                    {
                        Product p = dao.GetProductByID(s);
                        Cart c = new Cart();
                        if (p != null)
                        {
                            c.productID = Convert.ToInt32(s);
                            c.quantity = 1;
                            c.Product = p;
                            list.Add(c);
                        }
                    }
                    // dồn các product lại 
                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        // list product 
                        int count = 1;
                        for (int j = i + 1; j < list.Count; j++)
                        {
                            if (list[i].productID == list[j].productID)
                            {
                                count++;
                                list.RemoveAt(j);
                                j--;
                                list[i].quantity = count;
                            }
                        }
                    }
                    float total = 0;
                    foreach (Cart c in list)
                    {
                        total += float.Parse(c.quantity.ToString()) * c.Product.price;
                    }
                    double finalPrice = total;
                    Session["cart"] = list;
                    Session["total"] = total;
                    Session["finalPrice"] = finalPrice;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return RedirectToAction("CreateForAdmin", "Order");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        // return it self
        [HttpGet]
        public ActionResult InsertToCart(string id)
        { 
            int length = 1;
            //string id = Request.Params["id"];
            if (Request.Params["num"] != null){
                length = Convert.ToInt32(Request.Params["num"]);
            }
            List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;

            string txt = "";
            // get  list of cookies 
            HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
            //
            if (c_id != null)
            {
                txt += c_id.Value;
                c_id.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c_id);
            }
            for(int i = 0; i < length; i++)
            {
                if (txt.Trim().Length == 0)
                {
                    txt = id;
                }
                else
                {
                    txt += "," + id;
                }
            }
            HttpCookie c1 = new HttpCookie("cookieId", txt);
            c1.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(c1);
            return RedirectToAction("../Product/IndexForCustomer");
        }


        [HttpGet]
        public ActionResult Delete(string id)
        {
            List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
            String txt = "";
            //get list of id : 1,2,3...
            if (c_id != null)
            {
                txt += c_id.Value;
                c_id.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c_id);
            }

            string[] ids = txt.Split(',');
            string txtOutPut = "";
            int check = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i].Equals(id))
                {
                    check++;
                }
                // bo qua id dau tien
                if (check != 1)
                {
                    if (txtOutPut.Trim().Length == 0)
                    {
                        txtOutPut = ids[i];
                    }
                    else
                    {
                        txtOutPut = txtOutPut + "," + ids[i];
                    }
                }
                else
                {
                    check++;
                }
            }
            if (txtOutPut.Trim().Length != 0)
            {
                HttpCookie c1 = new HttpCookie("cookieId", txtOutPut);
                c1.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(c1);
            }
            return RedirectToAction("../Cart/Index");
        }
        [HttpGet]
        public ActionResult Insert(string id)
        {
            List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            Console.WriteLine(id);
            //string id = Request.Params["id"];
            string txt = "";
            // get  list of cookies 
            HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
            //
            if (c_id != null)
            {
                txt += c_id.Value;
                c_id.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(c_id);
            }
            if (txt.Trim().Length == 0)
            {
                txt = id;
            }
            else
            {
                txt += "," + id;
            }
            HttpCookie c1 = new HttpCookie("cookieId", txt);
            c1.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(c1);
            return RedirectToAction("../Cart/Index");
        }
        [HttpGet]
        public ActionResult DeleteForAdmin(string id)
        {
            List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
            String txt = "";
            //get list of id : 1,2,3...
            if (c_id != null)
            {
                txt += c_id.Value;
                c_id.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c_id);
            }

            string[] ids = txt.Split(',');
            string txtOutPut = "";
            int check = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i].Equals(id))
                {
                    check++;
                }
                // bo qua id dau tien
                if (check != 1)
                {
                    if (txtOutPut.Trim().Length == 0)
                    {
                        txtOutPut = ids[i];
                    }
                    else
                    {
                        txtOutPut = txtOutPut + "," + ids[i];
                    }
                }
                else
                {
                    check++;
                }
            }
            if (txtOutPut.Trim().Length != 0)
            {
                HttpCookie c1 = new HttpCookie("cookieId", txtOutPut);
                c1.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(c1);
            }
            return RedirectToAction("../Cart/IndexForAdmin");
        }
        [HttpGet]
        public ActionResult InsertForAdmin(string id)
        {
            List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            Console.WriteLine(id);
            //string id = Request.Params["id"];
            string txt = "";
            // get  list of cookies 
            HttpCookie c_id = HttpContext.Request.Cookies.Get("cookieId");
            //
            if (c_id != null)
            {
                txt += c_id.Value;
                c_id.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(c_id);
            }
            if (txt.Trim().Length == 0)
            {
                txt = id;
            }
            else
            {
                txt += "," + id;
            }
            HttpCookie c1 = new HttpCookie("cookieId", txt);
            c1.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(c1);
            return RedirectToAction("../Cart/IndexForAdmin");
        }
    
}
}
