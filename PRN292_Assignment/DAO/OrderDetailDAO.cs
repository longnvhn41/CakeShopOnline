using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CakeShop.DAO
{
    public class OrderDetailDAO
    {
        internal bool Delete(string id)
        {
            return new DataProvider().executeNonQuery("DELETE FROM [dbo].[OrderDetail] WHERE orderID = " + id, "OrderDetailDAO Delete");
        }

        internal List<OrderDetail> getOrderDetailByOrderID(string id)
        {
            string str = "select orderDetailID, orderID, od.productID, od.quantity as OrderQuantity, productCategoryID, productName, price, p.quantity as ProductQuantity from OrderDetail od left join Product p on od.productID = p.productID WHERE orderID = " + id;
            DataTable dt = new DataProvider().executeQuery(str, "OrderDetailDAO getOrderDetailByOrderID");
            if (dt.Rows.Count > 0)
            {
                List<OrderDetail> listOD = new List<OrderDetail>();
                foreach (DataRow r in dt.AsEnumerable())
                {
                    OrderDetail od = new OrderDetail()
                    {
                        Product = new Product()
                        {
                            productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                            productCategoryID = r.IsNull("productCategoryID") ? 0 : Convert.ToInt32(Convert.ToString(r["productCategoryID"])),
                            productName = r.IsNull("productName") ? String.Empty : Convert.ToString(r["productName"]),
                            price = r.IsNull("price") ? 0 : float.Parse(Convert.ToString(r["price"])),
                            quantity = r.IsNull("ProductQuantity") ? 0 : Convert.ToInt32(Convert.ToString(r["ProductQuantity"]))
                        },
                        orderDetailID = r.IsNull("orderDetailID") ? 0 : Convert.ToInt32(Convert.ToString(r["orderDetailID"])),
                        orderID = r.IsNull("orderID") ? 0 : Convert.ToInt32(Convert.ToString(r["orderID"])),
                        productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                        quantity = r.IsNull("OrderQuantity") ? 0 : Convert.ToInt32(Convert.ToString(r["OrderQuantity"])),
                    };
                    listOD.Add(od);
                }
                return listOD;
            }
            return null;
        }
    }
}