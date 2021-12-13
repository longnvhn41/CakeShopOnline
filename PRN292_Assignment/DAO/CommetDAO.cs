using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CakeShop.Models;
namespace CakeShop.DAO
{
    public class CommetDAO
    {
        public bool insertCmt(string username, int productID, string content)
        {
            DateTime d = DateTime.Now;
            return new DataProvider().executeNonQuery("INSERT INTO [dbo].[Comment]([username],[productID],[createdOn],[content]) VALUES('" + username + "','" + productID + "', GETUTCDATE(),'" + content + "')", "insert commet ");
        }
        public List<Comment> getcommentbyproductid(string productid)
        {
            string strSelect = "SELECT * from comment where productid = " + productid + "";
            DataTable dt = new DataProvider().executeQuery(strSelect, "getcommet");
            List<Comment> ListPC = new List<Comment>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.AsEnumerable())
                {
                    Comment pc = new Comment()
                    {
                        commentID = r.IsNull("commentID") ? 0 : Convert.ToInt32(Convert.ToString(r["commentID"])),
                        username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                        productID = r.IsNull("productID") ? 0 : Convert.ToInt32(Convert.ToString(r["productID"])),
                        createdOn = DateTime.Parse(r["createdOn"].ToString()),
                        content = r.IsNull("content") ? String.Empty : Convert.ToString(r["content"])
                    };
                    ListPC.Add(pc);
                    
                }
                return ListPC;
            }
            return null;
        }
    }
   
}