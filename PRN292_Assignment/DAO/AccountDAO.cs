using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CakeShop.DAO
{
    public class AccountDAO
    {
        public bool Login(string user, string pass)
        {
            string strSelect = "SELECT * FROM Account WHERE username = '" + user + "' AND password = '" + pass + "'";
            if (new DataProvider().executeQuery(strSelect, "AccountDAO Login").Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public Boolean register(string user, string pass)
        {
            string insert = "INSERT INTO Account (username,Password,type) values('" + user + "','" + pass + "','" + 3 + "')";
            return new DataProvider().executeNonQuery(insert, "register");
        }
        public Boolean insertUserDetail(string username, string fullname, string email, string phone, string address, string link)
        {
            string insert = "INSERT INTO UserDetail (username,fullname,email,phone,address,imageLink) values('" + username + "','" + fullname + "','" + email + "','" + phone + "','" + address + "','" + link + "')";
            return new DataProvider().executeNonQuery(insert, "AccountDAO insertUserDetail");
        }
        public Account GetUserByUsername(string user)
        {
            string strSelect = "SELECT * FROM Account WHERE username = '" + user + "'";
            DataTable dt = new DataProvider().executeQuery(strSelect, "AccountDAO Login");
            if (dt.Rows.Count > 0)
            {
                return new Account()
                {
                    username = dt.Rows[0].IsNull("username") ? String.Empty : Convert.ToString(dt.Rows[0]["username"]),
                    password = dt.Rows[0].IsNull("password") ? String.Empty : Convert.ToString(dt.Rows[0]["password"]),
                    type = dt.Rows[0].IsNull("type") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["type"]))
                };
            }
            return null;
        }

        internal bool updatePassByUsername(string username, string pass)
        {
            return new DataProvider().executeNonQuery("UPDATE [dbo].[Account] SET [password] = '" + pass + "' WHERE [username] = '" + username + "'", "AccountDAO updatePassByUsername");
        }
        public bool ChangeType(string username, int type)
        {
            string strupdate = "update Account set type =" + type + "where username ='" + username + "'";
            return new DataProvider().executeNonQuery(strupdate, "change type");
        }
    }
}