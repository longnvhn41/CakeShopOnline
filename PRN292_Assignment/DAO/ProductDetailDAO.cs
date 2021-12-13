using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CakeShop.DAO
{
    public class ProductDetailDAO
    {
        public List<ProductDetail> GetProduct(int index, int size)
        {
            string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY pd.productID DESC) AS [Row], p.productID,[productDetailID], [productCategoryID], [productName], [price], [quantity],[origin], [ingredients], [netWeight], [description], [imageLink] FROM Product p join ProductDetail pd on p.productID = pd.productID ) SELECT * FROM X WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ") ORDER BY productID DESC";
            DataTable dt = new DataProvider().executeQuery(strSelect, "ProductDetailDAO GetProduct");
            List<ProductDetail> ListP = new List<ProductDetail>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.AsEnumerable())
                {
                    ProductDetail p = new ProductDetail()
                    {
                        productDetailID = r.IsNull("productDetailID") ? 0 : Convert.ToInt32(Convert.ToString(r["productDetailID"])),
                        productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                        origin = r.IsNull("origin") ? String.Empty : Convert.ToString(r["origin"]),
                        ingredients = r.IsNull("ingredients") ? String.Empty : Convert.ToString(r["ingredients"]),
                        netWeight = r.IsNull("netWeight") ? 0 : Convert.ToInt32(Convert.ToString(r["netWeight"])),
                        description = r.IsNull("description") ? String.Empty : Convert.ToString(r["description"]),
                        imageLink = r.IsNull("imageLink") ? String.Empty : Convert.ToString(r["imageLink"]),
                        Product = new Product()
                        {
                            productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                            productCategoryID = r.IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(r["productCategoryID"])),
                            productName = r.IsNull("productName") ? String.Empty : Convert.ToString(r["productName"]),
                            price = r.IsNull("price") ? 0 : float.Parse(Convert.ToString(r["price"])),
                            quantity = r.IsNull("quantity") ? 0 : Convert.ToInt32(Convert.ToString(r["quantity"]))
                        }
                    };
                    ListP.Add(p);
                }
                return ListP;
            }
            return null;
        }

        public List<ProductDetail> GetProduct()
        {
            string strSelect = "SELECT p.productID,[productDetailID], [productCategoryID], [productName], [price], [quantity],[origin], [ingredients], [netWeight], [description], [imageLink] FROM Product p join ProductDetail pd on p.productID = pd.productID";
            DataTable dt = new DataProvider().executeQuery(strSelect, "ProductDetailDAO GetProduct");
            List<ProductDetail> ListP = new List<ProductDetail>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.AsEnumerable())
                {
                    ProductDetail p = new ProductDetail()
                    {
                        productDetailID = r.IsNull("productDetailID") ? 0 : Convert.ToInt32(Convert.ToString(r["productDetailID"])),
                        productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                        origin = r.IsNull("origin") ? String.Empty : Convert.ToString(r["origin"]),
                        ingredients = r.IsNull("ingredients") ? String.Empty : Convert.ToString(r["ingredients"]),
                        netWeight = r.IsNull("netWeight") ? 0 : Convert.ToInt32(Convert.ToString(r["netWeight"])),
                        description = r.IsNull("description") ? String.Empty : Convert.ToString(r["description"]),
                        imageLink = r.IsNull("imageLink") ? String.Empty : Convert.ToString(r["imageLink"]),
                        Product = new Product()
                        {
                            productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                            productCategoryID = r.IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(r["productCategoryID"])),
                            productName = r.IsNull("productName") ? String.Empty : Convert.ToString(r["productName"]),
                            price = r.IsNull("price") ? 0 : float.Parse(Convert.ToString(r["price"])),
                            quantity = r.IsNull("quantity") ? 0 : Convert.ToInt32(Convert.ToString(r["quantity"]))
                        }
                    };
                    ListP.Add(p);
                }
                return ListP;
            }
            return null;
        }

        public int GetProductSize()
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM Product p join ProductDetail pd on p.productID = pd.productID", "ProductDetailDAO GetProductSize").Rows[0].Field<int>(0);
        }

        public List<ProductDetail> GetProductByCID(string id, int index, int size)
        {
            string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER(ORDER BY pd.productID DESC) AS[Row], p.productID,[productDetailID], [productCategoryID], [productName], [price], [quantity],[origin], [ingredients], [netWeight], [description], [imageLink] FROM Product p join ProductDetail pd on p.productID = pd.productID WHERE productCategoryID = " + id + ") SELECT* FROM X WHERE[Row] BETWEEN(" + index + " * " + size + " - (" + size + " - 1)) AND(" + index + " * " + size + ") ORDER BY productID DESC";
            DataTable dt = new DataProvider().executeQuery(strSelect, "ProductDetailDAO GetProductByCID");
            List<ProductDetail> ListP = new List<ProductDetail>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.AsEnumerable())
                {
                    ProductDetail p = new ProductDetail()
                    {
                        productDetailID = r.IsNull("productDetailID") ? 0 : Convert.ToInt32(Convert.ToString(r["productDetailID"])),
                        productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                        origin = r.IsNull("origin") ? String.Empty : Convert.ToString(r["origin"]),
                        ingredients = r.IsNull("ingredients") ? String.Empty : Convert.ToString(r["ingredients"]),
                        netWeight = r.IsNull("netWeight") ? 0 : Convert.ToInt32(Convert.ToString(r["netWeight"])),
                        description = r.IsNull("description") ? String.Empty : Convert.ToString(r["description"]),
                        imageLink = r.IsNull("imageLink") ? String.Empty : Convert.ToString(r["imageLink"]),
                        Product = new Product()
                        {
                            productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                            productCategoryID = r.IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(r["productCategoryID"])),
                            productName = r.IsNull("productName") ? String.Empty : Convert.ToString(r["productName"]),
                            price = r.IsNull("price") ? 0 : float.Parse(Convert.ToString(r["price"])),
                            quantity = r.IsNull("quantity") ? 0 : Convert.ToInt32(Convert.ToString(r["quantity"]))
                        }
                    };
                    ListP.Add(p);
                }
                return ListP;
            }
            return null;
        }


        public int GetProductByCIDSize(string id)
        {
            return new DataProvider().executeQuery("SELECT  COUNT(*) FROM Product p join ProductDetail pd on p.productID = pd.productID WHERE productCategoryID = " + id, "ProductDetailDAO GetProductByCIDSize").Rows[0].Field<int>(0);
        }

        public List<ProductDetail> GetProductByName(string keyword, int index, int size)
        {
            string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY pd.productID DESC) AS [Row], p.productID,[productDetailID], [productCategoryID], [productName], [price], [quantity],[origin], [ingredients], [netWeight], [description], [imageLink] FROM Product p join ProductDetail pd on p.productID = pd.productID WHERE productName like '%" + keyword + "%') SELECT * FROM X WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ") ORDER BY productID DESC";
            DataTable dt = new DataProvider().executeQuery(strSelect, "ProductDetailDAO GetProductByName");
            List<ProductDetail> ListP = new List<ProductDetail>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.AsEnumerable())
                {
                    ProductDetail p = new ProductDetail()
                    {
                        productDetailID = r.IsNull("productDetailID") ? 0 : Convert.ToInt32(Convert.ToString(r["productDetailID"])),
                        productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                        origin = r.IsNull("origin") ? String.Empty : Convert.ToString(r["origin"]),
                        ingredients = r.IsNull("ingredients") ? String.Empty : Convert.ToString(r["ingredients"]),
                        netWeight = r.IsNull("netWeight") ? 0 : Convert.ToInt32(Convert.ToString(r["netWeight"])),
                        description = r.IsNull("description") ? String.Empty : Convert.ToString(r["description"]),
                        imageLink = r.IsNull("imageLink") ? String.Empty : Convert.ToString(r["imageLink"]),
                        Product = new Product()
                        {
                            productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                            productCategoryID = r.IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(r["productCategoryID"])),
                            productName = r.IsNull("productName") ? String.Empty : Convert.ToString(r["productName"]),
                            price = r.IsNull("price") ? 0 : float.Parse(Convert.ToString(r["price"])),
                            quantity = r.IsNull("quantity") ? 0 : Convert.ToInt32(Convert.ToString(r["quantity"]))
                        }
                    };
                    ListP.Add(p);
                }
                return ListP;
            }
            return null;
        }
        public int GetProductByNameSize(string keyword)
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM Product p join ProductDetail pd on p.productID = pd.productID WHERE productName like '%" + keyword + "%'", "ProductDetailDAO GetProductByNameSize").Rows[0].Field<int>(0);
        }

        public List<int> GetPrice()
        {
            DataTable dt = new DataProvider().executeQuery("SELECT MIN(price) as [min], MAX(price) as [max] FROM Product p join ProductDetail pd on p.productID = pd.productID", "ProductDetailDAO GetPrice");
            List<int> list = new List<int>();
            if (dt.Rows.Count > 0)
            {
                list.Add(dt.Rows[0].IsNull("min") ? 0 : Convert.ToInt32(Math.Floor(Double.Parse(Convert.ToString(dt.Rows[0]["min"])))));
                list.Add(dt.Rows[0].IsNull("max") ? 0 : Convert.ToInt32(Math.Ceiling(Double.Parse(Convert.ToString(dt.Rows[0]["max"])))));
                return list;
            }
            return null;
        }

        public List<int> GetPriceByCID(string id)
        {
            DataTable dt = new DataProvider().executeQuery("SELECT MIN(price) as [min], MAX(price) as [max] FROM Product p join ProductDetail pd on p.productID = pd.productID WHERE productCategoryID = " + id, "ProductDetailDAO GetPriceByCID");
            List<int> list = new List<int>();
            if (dt.Rows.Count > 0)
            {
                list.Add(dt.Rows[0].IsNull("min") ? 0 : Convert.ToInt32(Math.Floor(Double.Parse(Convert.ToString(dt.Rows[0]["min"])))));
                list.Add(dt.Rows[0].IsNull("max") ? 0 : Convert.ToInt32(Math.Ceiling(Double.Parse(Convert.ToString(dt.Rows[0]["max"])))));
                return list;
            }
            return null;
        }

        public List<int> GetPriceByName(string keyword)
        {
            DataTable dt = new DataProvider().executeQuery("SELECT MIN(price) as [min], MAX(price) as [max] FROM Product p join ProductDetail pd on p.productID = pd.productID WHERE productName like '%" + keyword + "%'", "ProductDetailDAO GetPriceByName");
            List<int> list = new List<int>();
            if (dt.Rows.Count > 0)
            {
                list.Add(dt.Rows[0].IsNull("min") ? 0 : Convert.ToInt32(Math.Floor(Double.Parse(Convert.ToString(dt.Rows[0]["min"])))));
                list.Add(dt.Rows[0].IsNull("max") ? 0 : Convert.ToInt32(Math.Ceiling(Double.Parse(Convert.ToString(dt.Rows[0]["max"])))));
                return list;
            }
            return null;
        }

        public ProductDetail GetProductByID(string id)
        {
            string strSelect = "SELECT p.productID,[productDetailID], [productCategoryID], [productName], [price], [quantity],[origin], [ingredients], [netWeight], [description], [imageLink] FROM Product p join ProductDetail pd on p.productID = pd.productID WHERE p.productID = " + id;
            DataTable dt = new DataProvider().executeQuery(strSelect, "ProductDetailDAO GetProductByID");
            if (dt.Rows.Count > 0)
            {
                return new ProductDetail()
                {
                    productDetailID = dt.Rows[0].IsNull("productDetailID") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["productDetailID"])),
                    productID = dt.Rows[0].IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["productID"])),
                    origin = dt.Rows[0].IsNull("origin") ? String.Empty : Convert.ToString(dt.Rows[0]["origin"]),
                    ingredients = dt.Rows[0].IsNull("ingredients") ? String.Empty : Convert.ToString(dt.Rows[0]["ingredients"]),
                    netWeight = dt.Rows[0].IsNull("netWeight") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["netWeight"])),
                    description = dt.Rows[0].IsNull("description") ? String.Empty : Convert.ToString(dt.Rows[0]["description"]),
                    imageLink = dt.Rows[0].IsNull("imageLink") ? String.Empty : Convert.ToString(dt.Rows[0]["imageLink"]),
                    Product = new Product()
                    {
                        productID = dt.Rows[0].IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["productID"])),
                        productCategoryID = dt.Rows[0].IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["productCategoryID"])),
                        productName = dt.Rows[0].IsNull("productName") ? String.Empty : Convert.ToString(dt.Rows[0]["productName"]),
                        price = dt.Rows[0].IsNull("price") ? 0 : float.Parse(Convert.ToString(dt.Rows[0]["price"])),
                        quantity = dt.Rows[0].IsNull("quantity") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["quantity"]))
                    }
                };
            }
            return null;
        }

        public bool Insert(ProductDetail pd)
        {
            if (!(new ProductDAO().Insert(pd.Product)))
                return false;
            int productID = new ProductDAO().getProductByName(pd.Product.productName);
            if (productID == 0)
                return false;
            string strExecute = "INSERT INTO [dbo].[ProductDetail]([productID],[origin],[ingredients],[netWeight],[description],[imageLink]) " +
                "VALUES(" + productID + ",'" + pd.origin + "','" + pd.ingredients + "'," + pd.netWeight + ",'" + pd.description + "','" + pd.imageLink + "')";
            return new DataProvider().executeNonQuery(strExecute, "ProductDetailDAO Insert");
        }

        public bool Delete(string id)
        {
            return new DataProvider().executeNonQuery("DELETE FROM [dbo].[ProductDetail] WHERE productID = " + id, "ProductDetailDAO Delete");
        }

        internal bool Update(ProductDetail pd, bool isChangeImage)
        {
            if (!(new ProductDAO().Update(pd.Product)))
                return false;
            string strExecute = "";
            if (isChangeImage)
                strExecute = "UPDATE [dbo].[ProductDetail] SET [origin] = '" + pd.origin + "',[ingredients] = '" + pd.ingredients + "',[netWeight] = " 
                    + pd.netWeight + ",[description] = '" + pd.description + "', [imageLink] = '" + pd.imageLink + "' WHERE [productID] = " + pd.productID;
            else 
                strExecute = "UPDATE [dbo].[ProductDetail] SET [origin] = '" + pd.origin + "',[ingredients] = '" + pd.ingredients + "',[netWeight] = " 
                    + pd.netWeight + ",[description] = '" + pd.description + "' WHERE [productID] = " + pd.productID;
            return new DataProvider().executeNonQuery(strExecute, "ProductDetailDAO Insert");
        }
    }
}