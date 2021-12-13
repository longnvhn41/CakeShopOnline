using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CakeShop.DAO
{
    public class OrderDAO
    {
        public int getOrderSize()
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM [Order]", "OrderDAO getOrderSize").Rows[0].Field<int>(0);
        }
        internal int getLastOrderID()
        {
            return new DataProvider().executeQuery("SELECT top 1 [orderID]FROM [PRN292].[dbo].[Order] order by orderID desc", "get last order id").Rows[0].Field<int>(0);
        }
        public bool insertOrder(string username, string address, string phone, string staus)
        {
            DateTime d = DateTime.Now;
            return new DataProvider().executeNonQuery("INSERT INTO [dbo].[Order]([username],[deliverAddress],[phone],[createdOn],[status])VALUES('" + username + "','" + address + "','" + phone + "', GETUTCDATE(),'" + staus + "')", "insert order ");
        }
        public bool insertOrderdetail(int id, int productID, int Quantity)
        {
            return new DataProvider().executeNonQuery("INSERT INTO [dbo].[OrderDetail]([orderID],[productID],[quantity])VALUES('" + id + "','" + productID + "','" + Quantity + "')", "insert order detail ");
        }
        public int getOrderHiddenSize()
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM [Order] WHERE status like '%Hidden'", "OrderDAO getOrderSize").Rows[0].Field<int>(0);
        }

        public int getOrderNoteHiddenSize()
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM [Order] WHERE status like '%Hidden'", "OrderDAO getOrderSize").Rows[0].Field<int>(0);
        }

        internal List<Order> GetOrder(int index, int size)
        {
            try
            {
                string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY OrderID DESC) AS [Row], * FROM [Order]) SELECT * FROM X WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ")";
                DataTable dt = new DataProvider().executeQuery(strSelect, "OrderDAO GetOrder");
                List<Order> listO = new List<Order>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.AsEnumerable())
                    {
                        Order o = new Order()
                        {
                            orderID = r.IsNull("orderID") ? 0 : Convert.ToInt32(Convert.ToString(r["orderID"])),
                            username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                            discountCode = r.IsNull("discountCode") ? String.Empty : Convert.ToString(r["discountCode"]),
                            deliverAddress = r.IsNull("deliverAddress") ? String.Empty : Convert.ToString(r["deliverAddress"]),
                            phone = r.IsNull("phone") ? String.Empty : Convert.ToString(r["phone"]),
                            createdOn = DateTime.Parse(r["createdOn"].ToString()),
                            status = r.IsNull("status") ? String.Empty : Convert.ToString(r["status"])
                        };
                        listO.Add(o);
                    }
                    return listO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        internal List<Order> GetOrderHidden(int index, int size)
        {
            try
            {
                string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY OrderID DESC) AS [Row], * FROM [Order] WHERE status like '%Hidden' ) SELECT * FROM X WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ")";
                DataTable dt = new DataProvider().executeQuery(strSelect, "OrderDAO GetOrder");
                List<Order> listO = new List<Order>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.AsEnumerable())
                    {
                        Order o = new Order()
                        {
                            orderID = r.IsNull("orderID") ? 0 : Convert.ToInt32(Convert.ToString(r["orderID"])),
                            username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                            discountCode = r.IsNull("discountCode") ? String.Empty : Convert.ToString(r["discountCode"]),
                            deliverAddress = r.IsNull("deliverAddress") ? String.Empty : Convert.ToString(r["deliverAddress"]),
                            phone = r.IsNull("phone") ? String.Empty : Convert.ToString(r["phone"]),
                            createdOn = DateTime.Parse(r["createdOn"].ToString()),
                            status = r.IsNull("status") ? String.Empty : Convert.ToString(r["status"])
                        };
                        listO.Add(o);
                    }
                    return listO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        internal List<Order> GetOrderNotHidden(int index, int size)
        {
            try
            {
                string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY OrderID DESC) AS [Row], * FROM [Order] WHERE status not like '%Hidden') SELECT * FROM X WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ")";
                DataTable dt = new DataProvider().executeQuery(strSelect, "OrderDAO GetOrder");
                List<Order> listO = new List<Order>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.AsEnumerable())
                    {
                        Order o = new Order()
                        {
                            orderID = r.IsNull("orderID") ? 0 : Convert.ToInt32(Convert.ToString(r["orderID"])),
                            username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                            discountCode = r.IsNull("discountCode") ? String.Empty : Convert.ToString(r["discountCode"]),
                            deliverAddress = r.IsNull("deliverAddress") ? String.Empty : Convert.ToString(r["deliverAddress"]),
                            phone = r.IsNull("phone") ? String.Empty : Convert.ToString(r["phone"]),
                            createdOn = DateTime.Parse(r["createdOn"].ToString()),
                            status = r.IsNull("status") ? String.Empty : Convert.ToString(r["status"])
                        };
                        listO.Add(o);
                    }
                    return listO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        internal int getOrderByNameSize(string username)
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM [Order] WHERE username = '" + username + "'", "OrderDAO getOrderSize").Rows[0].Field<int>(0);
        }

        internal int getOrderNotHiddenByNameSize(string username)
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM [Order] WHERE username = '" + username + "' and status not like '%Hidden'", "OrderDAO getOrderSize").Rows[0].Field<int>(0);
        }
        internal int getOrderHiddenByNameSize(string username)
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM [Order] WHERE username = '" + username + "' and status like '%Hidden'", "OrderDAO getOrderSize").Rows[0].Field<int>(0);
        }

        internal List<Order> GetOrderByUsername(int index, int size, string username)
        {
            try
            {
                string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY OrderID DESC) AS [Row], * FROM [Order] WHERE username = '" + username + "') SELECT * FROM X WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ")";
                DataTable dt = new DataProvider().executeQuery(strSelect, "OrderDAO GetOrder");
                List<Order> listO = new List<Order>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.AsEnumerable())
                    {
                        Order o = new Order()
                        {
                            orderID = r.IsNull("orderID") ? 0 : Convert.ToInt32(Convert.ToString(r["orderID"])),
                            username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                            discountCode = r.IsNull("discountCode") ? String.Empty : Convert.ToString(r["discountCode"]),
                            deliverAddress = r.IsNull("deliverAddress") ? String.Empty : Convert.ToString(r["deliverAddress"]),
                            phone = r.IsNull("phone") ? String.Empty : Convert.ToString(r["phone"]),
                            createdOn = DateTime.Parse(r["createdOn"].ToString()),
                            status = r.IsNull("status") ? String.Empty : Convert.ToString(r["status"])
                        };
                        listO.Add(o);
                    }
                    return listO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        internal List<Order> GetOrderNotHiddenByUsername(int index, int size, string username)
        {
            try
            {
                string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY OrderID DESC) AS [Row], * FROM [Order] WHERE username = '" + username + "' and status not like '%Hidden') SELECT * FROM X WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ")";
                DataTable dt = new DataProvider().executeQuery(strSelect, "OrderDAO GetOrder");
                List<Order> listO = new List<Order>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.AsEnumerable())
                    {
                        Order o = new Order()
                        {
                            orderID = r.IsNull("orderID") ? 0 : Convert.ToInt32(Convert.ToString(r["orderID"])),
                            username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                            discountCode = r.IsNull("discountCode") ? String.Empty : Convert.ToString(r["discountCode"]),
                            deliverAddress = r.IsNull("deliverAddress") ? String.Empty : Convert.ToString(r["deliverAddress"]),
                            phone = r.IsNull("phone") ? String.Empty : Convert.ToString(r["phone"]),
                            createdOn = DateTime.Parse(r["createdOn"].ToString()),
                            status = r.IsNull("status") ? String.Empty : Convert.ToString(r["status"])
                        };
                        listO.Add(o);
                    }
                    return listO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }


        internal List<Order> GetOrderHiddenByUsername(int index, int size, string username)
        {
            try
            {
                string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY OrderID DESC) AS [Row], * FROM [Order] WHERE username = '" + username + "' and status like '%Hidden') SELECT * FROM X WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ")";
                DataTable dt = new DataProvider().executeQuery(strSelect, "OrderDAO GetOrder");
                List<Order> listO = new List<Order>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.AsEnumerable())
                    {
                        Order o = new Order()
                        {
                            orderID = r.IsNull("orderID") ? 0 : Convert.ToInt32(Convert.ToString(r["orderID"])),
                            username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                            discountCode = r.IsNull("discountCode") ? String.Empty : Convert.ToString(r["discountCode"]),
                            deliverAddress = r.IsNull("deliverAddress") ? String.Empty : Convert.ToString(r["deliverAddress"]),
                            phone = r.IsNull("phone") ? String.Empty : Convert.ToString(r["phone"]),
                            createdOn = DateTime.Parse(r["createdOn"].ToString()),
                            status = r.IsNull("status") ? String.Empty : Convert.ToString(r["status"])
                        };
                        listO.Add(o);
                    }
                    return listO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        internal bool Delete(string id)
        {
            if (!(new OrderDetailDAO().Delete(id)))
                return false;
            return new DataProvider().executeNonQuery("DELETE FROM [dbo].[Order] WHERE orderID = " + id, "OrderDAO Delete");
        }
        internal Order getOrderByID(string id)
        {
            DataTable dt = new DataProvider().executeQuery("Select * from [Order] WHERE orderID = " + id, "OrderDAO getOrderByID");
            if (dt.Rows.Count > 0)
            {
                return new Order()
                {
                    orderID = dt.Rows[0].IsNull("orderID") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["orderID"])),
                    username = dt.Rows[0].IsNull("username") ? String.Empty : Convert.ToString(dt.Rows[0]["username"]),
                    //discountCode = dt.Rows[0].IsNull("discountCode") ? String.Empty : Convert.ToString(dt.Rows[0]["discountCode"]),
                    deliverAddress = dt.Rows[0].IsNull("deliverAddress") ? String.Empty : Convert.ToString(dt.Rows[0]["deliverAddress"]),
                    phone = dt.Rows[0].IsNull("phone") ? String.Empty : Convert.ToString(dt.Rows[0]["phone"]),
                    createdOn = DateTime.Parse(dt.Rows[0]["createdOn"].ToString()),
                    status = dt.Rows[0].IsNull("status") ? String.Empty : Convert.ToString(dt.Rows[0]["status"])
                };
            }
            return null;

        }

        internal bool UpdateStatus(string id, string status)
        {
            return new DataProvider().executeNonQuery("UPDATE[dbo].[Order] SET[status] = '" + status + "' WHERE OrderID = " + id, "OrderDAO UpdateStatus");
        }
    }
}