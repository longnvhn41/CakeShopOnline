using CakeShop.DAO;
using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace CakeShop.Controllers
{
    public class HomeController : Controller
    {
        //Home
        public ActionResult Index()
        {
            List<ProductDetail> listPD = new ProductDetailDAO().GetProduct();
            ViewData["ProductList"] = listPD;
            ProductCategoryDAO pcdb = new ProductCategoryDAO();
            List<ProductCategory> listPC = pcdb.GetProductCategory();
            ViewData["ProductCategoryList"] = listPC;
            return View();
        }
    }
}
