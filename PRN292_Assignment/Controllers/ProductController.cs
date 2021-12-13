using ExcelDataReader;
using OfficeOpenXml;
using CakeShop.DAO;
using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Configuration;

//using Excel = Microsoft.Office.Interop.Excel;
namespace CakeShop.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                conString = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.Product";

                        //[OPTIONAL]: Map the Excel columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("productID", "productID");
                        sqlBulkCopy.ColumnMappings.Add("productCategoryID", "productCategoryID");
                        sqlBulkCopy.ColumnMappings.Add("productName", "productName");
                        sqlBulkCopy.ColumnMappings.Add("price", "price");
                        sqlBulkCopy.ColumnMappings.Add("quantity", "quantity");
                        //sqlBulkCopy.ColumnMappings.Add("productDetailID", "productDetailID");
                        //sqlBulkCopy.ColumnMappings.Add("origin", "origin");
                        //sqlBulkCopy.ColumnMappings.Add("ingredients", "ingredients");
                        //sqlBulkCopy.ColumnMappings.Add("netWeight", "netWeight");
                        //sqlBulkCopy.ColumnMappings.Add("description", "description");
                        //sqlBulkCopy.ColumnMappings.Add("imageLink", "imageLink");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }
            new ProductDAO().DeleteDuplicate();
            return View();
        }

        [HttpPost]
        public ActionResult Search()
        {
            try
            {
                string keyword = Request.Form["keyword"];
                ProductCategoryDAO pcdb = new ProductCategoryDAO();
                List<ProductCategory> listPC = pcdb.GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;

                ProductDetailDAO pddb = new ProductDetailDAO();


                //PAGING
                int pageSize = 6;
                int index = 1;
                if (Request.Params["index"] != null)
                {
                    index = Convert.ToInt32(Request.Params["index"]);
                }
                int size = 0;
                List<ProductDetail> listP = new List<ProductDetail>();

                if (keyword == null)
                {
                    size = pddb.GetProductSize();
                    List<ProductDetail> listPD = pddb.GetProduct(index, pageSize);
                    ViewData["ProductList"] = listPD;
                    ViewData["type"] = "Product";
                    List<int> list = pddb.GetPrice();
                    ViewData["min"] = (list == null) ? 0 : Convert.ToInt32(list[0]);
                    ViewData["max"] = (list == null) ? 0 : Convert.ToInt32(list[1]);
                }
                else
                {
                    size = pddb.GetProductByNameSize(keyword);
                    List<ProductDetail> listPD = pddb.GetProductByName(keyword, index, pageSize);
                    ViewData["ProductList"] = listPD;
                    ViewData["type"] = "ProductName";
                    ViewData["keyword"] = keyword;
                    List<int> list = pddb.GetPriceByName(keyword);
                    ViewData["min"] = (list == null) ? 0 : Convert.ToInt32(list[0]);
                    ViewData["max"] = (list == null) ? 0 : Convert.ToInt32(list[1]);
                }

                int numberPage = size / pageSize;
                if (size % pageSize != 0)
                {
                    numberPage++;
                }
                ViewData["numberPage"] = numberPage;
                ViewData["index"] = index;
                ViewData["size"] = size;

                return View("~/Views/Product/IndexForCustomer.cshtml");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: List product for customer
        [HttpGet]
        public ActionResult IndexForCustomer(string id)
        {
            try
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
                List<ProductDetail> listP = new List<ProductDetail>();

                ProductDetailDAO pddb = new ProductDetailDAO();
                if (id == null)
                {
                    size = pddb.GetProductSize();
                    List<ProductDetail> listPD = pddb.GetProduct(index, pageSize);
                    ViewData["ProductList"] = listPD;
                    ViewData["type"] = "Product";
                    List<int> list = pddb.GetPrice();
                    ViewData["min"] = (list == null) ? 0 : Convert.ToInt32(list[0]);
                    ViewData["max"] = (list == null) ? 0 : Convert.ToInt32(list[1]);
                }
                else
                {
                    size = pddb.GetProductByCIDSize(id);
                    List<ProductDetail> listPD = pddb.GetProductByCID(id, index, pageSize);
                    ViewData["ProductList"] = listPD;
                    ViewData["type"] = "ProductCID";
                    ViewData["id"] = id;
                    List<int> list = pddb.GetPriceByCID(id);
                    ViewData["min"] = (list == null) ? 0 : Convert.ToInt32(list[0]);
                    ViewData["max"] = (list == null) ? 0 : Convert.ToInt32(list[1]);
                }

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

        // GET: List product for admin
        [HttpGet]
        public ActionResult IndexForAdmin()
        {
            try
            {
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

                //PAGING
                int pageSize = 6;
                int index = 1;
                if (Request.Params["index"] != null)
                {
                    index = Convert.ToInt32(Request.Params["index"]);
                }
                int size = 0;
                List<ProductDetail> listP = new List<ProductDetail>();
                ProductDetailDAO pddb = new ProductDetailDAO();
                size = pddb.GetProductSize();
                List<ProductDetail> listPD = pddb.GetProduct(index, pageSize);
                ViewData["ProductList"] = listPD;
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
        int ids = 0;

        // GET: Product detail for customer
        [HttpGet]
        public ActionResult DetailsForCustomer(string id)
        {
            try
            {

                UserDetail u = Session["user"] as UserDetail;
                CommetDAO cmtDAO = new CommetDAO();

                if (Convert.ToInt32(id) != 0)
                {
                    ids = Convert.ToInt32(id);
                }
                List<Comment> listCmt = cmtDAO.getcommentbyproductid(ids + "");
                string cmt = Request["comment"];
                if (cmt != null && u != null)
                {
                    string s = u.Account.username + id + cmt;
                    cmtDAO.insertCmt(u.Account.username, ids, cmt);
                }
                Session["listCmt"] = listCmt;
                ProductCategoryDAO pcdb = new ProductCategoryDAO();
                List<ProductCategory> listPC = pcdb.GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;

                ProductDetailDAO pddb = new ProductDetailDAO();
                ProductDetail pd = pddb.GetProductByID(ids + "");
                ViewData["ProductDetail"] = pd;
                Session["ids"] = id;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Product detail for admin
        [HttpGet]
        public ActionResult DetailsForAdmin(string id)
        {
            try
            {
                ProductDetail pd = new ProductDetailDAO().GetProductByID(id);
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductDetail"] = pd;
                ViewData["ProductCategoryList"] = listPC;

                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Create product
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;

                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Create product
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                ProductDetail pd = new ProductDetail();
                Product p = new Product();
                Console.WriteLine(Request.Form["category"]);
                //On Hoac Null
                if (Request.Form["category"] == null)
                {
                    p.productCategoryID = Convert.ToInt32(Request.Form["oldCategory"]);
                }
                else
                {
                    if (!new ProductCategoryDAO().Insert(Request.Form["newCategory"]))
                    {
                        ViewData["error"] = "Insert new category fail";
                        return View();
                    }
                    else
                    {
                        int categoryID = Convert.ToInt32(new ProductCategoryDAO().GetProductCategoryByName(Request.Form["newCategory"]));
                        if (categoryID == 0)
                        {
                            ViewData["error"] = "Not found this category";
                            return View();
                        }
                        else
                        {
                            p.productCategoryID = categoryID;
                        }
                    }
                }

                p.productName = Request.Form["productName"];
                p.price = float.Parse(Request.Form["price"]);
                p.quantity = Convert.ToInt32(Request.Form["quantity"]);
                pd.Product = p;
                pd.origin = Request.Form["origin"];
                pd.ingredients = Request.Form["ingredients"];
                pd.netWeight = Convert.ToInt32(Request.Form["netWeight"]);
                pd.description = Request.Form["description"];

                HttpPostedFileBase file = Request.Files["imageFile"];
                string imageLink = UploadImage(file);
                if (imageLink == null)
                {
                    ViewData["error"] = "Please choose only Image file";
                    return View();
                }
                else
                {
                    pd.imageLink = imageLink;
                }

                if (new ProductDetailDAO().Insert(pd))
                {
                    return RedirectToAction("../Product/IndexForAdmin");
                }
                else
                {
                    ViewData["error"] = "Create fail";
                    return View();
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //Upload Image 
        public string UploadImage(HttpPostedFileBase file)
        {
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
            var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
            var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
            if (allowedExtensions.Contains(ext)) //check what type of extension  
            {
                string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                long elapsedTicks = DateTime.Now.Ticks - new DateTime(2001, 1, 1).Ticks;
                string myfile = name + "_" + elapsedTicks + ext; //appending the name with ticks 
                                                                 // store the file inside Content/img/product/details 
                var path = Path.Combine(Server.MapPath("~/Content/img/product/details"), myfile);
                file.SaveAs(path);
                return myfile;
            }
            else
            {
                ViewData["error"] = "Please choose only Image file";
            }
            return null;
        }

        //POST: Create category
        [HttpPost]
        public ActionResult CreateCategory(HttpPostedFileBase file)
        {
            try
            {
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;

                string name = Request.Form["name"];
                if (new ProductCategoryDAO().Insert(name))
                {
                    return RedirectToAction("../Product/IndexForAdmin");
                }
                else
                {
                    ViewData["error"] = "Insert new category fail";
                    return RedirectToAction("../Product/IndexForAdmin");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Edit
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                ProductDetail pd = new ProductDetail();
                Product p = new Product();

                //On Hoac Null
                p.productID = Convert.ToInt32(Request.Form["productID"]);
                pd.productID = Convert.ToInt32(Request.Form["productID"]);
                if (Request.Form["category"] == null)
                {
                    p.productCategoryID = Convert.ToInt32(Request.Form["oldCategory"]);
                }
                else
                {
                    if (!new ProductCategoryDAO().Insert(Request.Form["newCategory"]))
                    {
                        ViewData["error"] = "Insert new category fail";
                        return RedirectToAction("../Product/DetailsForAdmin", new { id = pd.productID });
                    }
                    else
                    {
                        int categoryID = Convert.ToInt32(new ProductCategoryDAO().GetProductCategoryByName(Request.Form["newCategory"]));
                        if (categoryID == 0)
                        {
                            ViewData["error"] = "Not found this category";
                            return RedirectToAction("../Product/DetailsForAdmin", new { id = pd.productID });
                        }
                        else
                        {
                            p.productCategoryID = categoryID;
                        }
                    }
                }

                p.productName = Request.Form["productName"];
                if (Request.Form["price"].Contains('.'))
                    p.price = float.Parse(Request.Form["price"].Replace('.', ','));
                else
                    p.price = float.Parse(Request.Form["price"]);
                p.quantity = Convert.ToInt32(Request.Form["quantity"]);
                pd.Product = p;

                pd.origin = Request.Form["origin"];
                pd.ingredients = Request.Form["ingredients"];
                pd.netWeight = Convert.ToInt32(Request.Form["netWeight"]);
                pd.description = Request.Form["description"];
                HttpPostedFileBase file = Request.Files["imageFile"];
                bool isChangeImage = false;
                if (file != null && file.ContentLength != 0)
                {
                    isChangeImage = true;
                    pd.imageLink = UploadImage(file);
                }

                if (new ProductDetailDAO().Update(pd, isChangeImage))
                {
                    return RedirectToAction("../Product/IndexForAdmin");
                }
                else
                {
                    ViewData["error"] = "Update fail";
                    return RedirectToAction("../Product/DetailsForAdmin", new { id = pd.productID });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Delete
        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            try
            {
                List<ProductCategory> listPC = new ProductCategoryDAO().GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;
                if (new ProductDAO().Delete(Request.Params["pid"]))
                {
                    return RedirectToAction("../Product/IndexForAdmin");
                }
                else
                {
                    ViewData["error"] = "Delete product fail";
                    return RedirectToAction("../Product/IndexForAdmin");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult insertCmt()
        {
            try
            {
                string id = Session["ids"] as String;
                UserDetail u = Session["user"] as UserDetail;
                CommetDAO cmtDAO = new CommetDAO();
                List<Comment> listCmt = cmtDAO.getcommentbyproductid(id);
                string cmt = Request.Form["comment"];
                if (cmt != null && u != null)
                {
                    cmtDAO.insertCmt(u.Account.username, Convert.ToInt32(id), cmt);
                }
                Session["listCmt"] = listCmt;
                ProductCategoryDAO pcdb = new ProductCategoryDAO();
                List<ProductCategory> listPC = pcdb.GetProductCategory();
                ViewData["ProductCategoryList"] = listPC;

                ProductDetailDAO pddb = new ProductDetailDAO();
                ProductDetail pd = pddb.GetProductByID(id);
                ViewData["ProductDetail"] = pd;
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


    }
}
