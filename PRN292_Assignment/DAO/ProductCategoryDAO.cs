using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CakeShop.DAO
{
    public class ProductCategoryDAO
    {
        public List<ProductCategory> GetProductCategory()
        {
            string strSelect = "SELECT * FROM ProductCategory ";
            DataTable dt = new DataProvider().executeQuery(strSelect, "ProductCategoryDAO GetProductCategory");
            List<ProductCategory> ListPC = new List<ProductCategory>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.AsEnumerable())
                {
                    ProductCategory pc = new ProductCategory()
                    {
                        productCategoryID = r.IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(r["productCategoryID"])),
                        productCategoryName = r.IsNull("productCategoryName") ? String.Empty : Convert.ToString(r["productCategoryName"])
                    };
                    ListPC.Add(pc);
                }
                return ListPC;
            }
            return null;
        }

        internal bool Insert(string productCategoryName)
        {
            return new DataProvider().executeNonQuery("INSERT INTO [dbo].[ProductCategory]([productCategoryName]) VALUES('" + productCategoryName + "')", "ProductCategoryDAO Insert");
        }

        internal int GetProductCategoryByName(string productCategoryName)
        {
            DataTable dt = new DataProvider().executeQuery("SELECT productCategoryID FROM ProductCategory WHERE productCategoryName = '" + productCategoryName + "'", "ProductCategoryDAO GetProductCategoryByName");
            if (dt.Rows.Count > 0)
                return dt.Rows[0].Field<int>(0);
            else
                return 0;
        }
    }
}