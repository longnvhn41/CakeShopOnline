using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CakeShop.Models;
namespace CakeShop.DAO
{
    public class CartDAO
    {
        public Boolean insertToCart(string username, int productid, int quantity)
        {
            string insert = "INSERT INTO Cart (username,productID,quantity) values('" + username + "','" + productid + "','" + quantity + "')";
            return new DataProvider().executeNonQuery(insert, "cart");
        }
        public Boolean deleteByUsername(string username)
        {
            string insert = "delete from cart where username ='" + username + "'";
            return new DataProvider().executeNonQuery(insert, "delete cart");
        }
        public List<Cart> GetAllCartbyUserName(string username)
        {
            string strSelect = "SELECT cartID, username, c.productID,productCategoryID, productName, price, c.quantity as CartQuantity, p.quantity as ProductQuantity FROM Cart c left join Product p on c.productID = p.productID where username = '" + username + "'";
            DataTable dt = new DataProvider().executeQuery(strSelect, "CartDAO GetAllCartbyUserName");
            List<Cart> ListPC = new List<Cart>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.AsEnumerable())
                {
                    Cart pc = new Cart()
                    {
                        cartID = r.IsNull("cartID") ? 0 : Convert.ToInt32(Convert.ToString(r["cartID"])),
                        username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                        productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                        quantity = r.IsNull("CartQuantity") ? 0 : Convert.ToInt32(Convert.ToString(r["CartQuantity"])),
                        Product = new Product()
                        {
                            productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                            productCategoryID = r.IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(r["productCategoryID"])),
                            productName = r.IsNull("productName") ? String.Empty : Convert.ToString(r["productName"]),
                            price = r.IsNull("price") ? 0 : float.Parse(Convert.ToString(r["price"])),
                            quantity = r.IsNull("ProductQuantity") ? 0 : Convert.ToInt32(Convert.ToString(r["ProductQuantity"]))
                        }
                    };
                    ListPC.Add(pc);
                    Console.WriteLine(pc.quantity);
                }
                return ListPC;
            }
            return null;
        }
    }
}