using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CakeShop.DAO
{
    public class ProductDAO
    {
        public bool Insert(Product p)
        {
            string strExecute = "INSERT INTO [dbo].[Product]([productCategoryID],[productName],[price],[quantity]) VALUES (" + p.productCategoryID + ",'" + p.productName + "','" + ((p.price.ToString().Contains(',')) ? p.price.ToString().Replace(',', '.') : p.price.ToString()) + "'," + p.quantity + ")";
            return new DataProvider().executeNonQuery(strExecute, "ProductDAO Insert");
        }

        internal int getProductByName(string productName)
        {
            DataTable dt = new DataProvider().executeQuery("SELECT productID FROM Product WHERE productName = '" + productName + "'", "ProductDAO getProductByName");
            if (dt.Rows.Count > 0)
                return dt.Rows[0].Field<int>(0);
            else
                return 0;
        }

        internal bool Delete(string id)
        {
            if (!(new ProductDetailDAO().Delete(id)))
                return false;
            return new DataProvider().executeNonQuery("DELETE FROM [dbo].[Product] WHERE productID = " + id, "ProductDAO Delete");
        }

        internal bool Update(Product product)
        {
            return new DataProvider().executeNonQuery("UPDATE [dbo].[Product] SET [productCategoryID] = " + product.productCategoryID + ",[productName] = '" + product.productName + "',[price] = '" + ((product.price.ToString().Contains(',')) ? product.price.ToString().Replace(',', '.') : product.price.ToString()) + "',[quantity] = " + product.quantity + " WHERE [productID] = " + product.productID, "ProductDAO Update");
        }

        public Product GetProductByID(string id)
        {
            string strSelect = "select * from Product  WHERE productID = " + id;
            DataTable dt = new DataProvider().executeQuery(strSelect, "ProductDAO GetProductByID");
            if (dt.Rows.Count > 0)
            {
                return new Product()
                {
                    productID = dt.Rows[0].IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["productID"])),
                    productCategoryID = dt.Rows[0].IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["productCategoryID"])),
                    productName = dt.Rows[0].IsNull("productName") ? String.Empty : Convert.ToString(dt.Rows[0]["productName"]),
                    price = dt.Rows[0].IsNull("price") ? 0 : float.Parse(Convert.ToString(dt.Rows[0]["price"])),
                    quantity = dt.Rows[0].IsNull("quantity") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["quantity"]))
                };
            }
            return null;
        }


        public List<Product> GetProduct()
        {
            string strSelect = "select * from Product";
            DataTable dt = new DataProvider().executeQuery(strSelect, "ProductDAO GetProductByID");
            if (dt.Rows.Count > 0)
            {
                List<Product> list = new List<Product>();
                foreach (DataRow r in dt.AsEnumerable())
                {
                    list.Add(new Product()
                    {
                        productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                        productCategoryID = r.IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(r["productCategoryID"])),
                        productName = r.IsNull("productName") ? String.Empty : Convert.ToString(r["productName"]),
                        price = r.IsNull("price") ? 0 : float.Parse(Convert.ToString(r["price"])),
                        quantity = r.IsNull("quantity") ? 0 : Convert.ToInt32(Convert.ToString(r["quantity"]))
                    });
                    return list;
                }
            }
            return null;
        }

        internal bool DeleteDuplicate()
        {
            return new DataProvider().executeNonQuery("WITH cte AS (SELECT * ,ROW_NUMBER() OVER (PARTITION BY ProductName ORDER BY ProductID desc) row_num FROM Product) DELETE FROM cte WHERE row_num > 1;", "");
        }
    }
}