using CakeShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CakeShop.DAO
{
    public class UserDetailDAO
    {
        public UserDetail GetUserDetailByUsername(string user)
        {
            string strSelect = "SELECT ud.username, a.password, a.type, ud.fullname, ud.email, ud.phone, ud.address, ud.imageLink FROM UserDetail ud join Account a on ud.username = a.username where a.username = '" + user + "'";
            DataTable dt = new DataProvider().executeQuery(strSelect, "UserDetailDAO GetUserDetailByUsername");
            if (dt.Rows.Count > 0)
            {
                return new UserDetail()
                {
                    username = dt.Rows[0].IsNull("username") ? String.Empty : Convert.ToString(dt.Rows[0]["username"]),
                    fullname = dt.Rows[0].IsNull("fullname") ? String.Empty : Convert.ToString(dt.Rows[0]["fullname"]),
                    email = dt.Rows[0].IsNull("email") ? String.Empty : Convert.ToString(dt.Rows[0]["email"]),
                    phone = dt.Rows[0].IsNull("phone") ? String.Empty : Convert.ToString(dt.Rows[0]["phone"]),
                    address = dt.Rows[0].IsNull("address") ? String.Empty : Convert.ToString(dt.Rows[0]["address"]),
                    imageLink = dt.Rows[0].IsNull("imageLink") ? String.Empty : Convert.ToString(dt.Rows[0]["imageLink"]),
                    Account = new Account()
                    {
                        username = dt.Rows[0].IsNull("username") ? String.Empty : Convert.ToString(dt.Rows[0]["username"]),
                        password = dt.Rows[0].IsNull("password") ? String.Empty : Convert.ToString(dt.Rows[0]["password"]),
                        type = dt.Rows[0].IsNull("type") ? 0 : Convert.ToInt32(Convert.ToString(dt.Rows[0]["type"]))
                    }

                };
            }
            return null;
        }

        internal List<UserDetail> GetUserDetail()
        {
            string strSelect = "SELECT ud.username, a.password, a.type, ud.fullname, ud.email, ud.phone, ud.address, ud.imageLink FROM UserDetail ud join Account a on ud.username = a.username";
            DataTable dt = new DataProvider().executeQuery(strSelect, "UserDetailDAO GetUserDetailByUsername");
            if (dt.Rows.Count > 0)
            {
                List<UserDetail> listU = new List<UserDetail>();
                foreach (DataRow r in dt.AsEnumerable())
                {
                    UserDetail ud = new UserDetail()
                    {
                        username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                        fullname = r.IsNull("fullname") ? String.Empty : Convert.ToString(r["fullname"]),
                        email = r.IsNull("email") ? String.Empty : Convert.ToString(r["email"]),
                        phone = r.IsNull("phone") ? String.Empty : Convert.ToString(r["phone"]),
                        address = r.IsNull("address") ? String.Empty : Convert.ToString(r["address"]),
                        imageLink = r.IsNull("imageLink") ? String.Empty : Convert.ToString(r["imageLink"]),
                        Account = new Account()
                        {
                            username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                            password = r.IsNull("password") ? String.Empty : Convert.ToString(r["password"]),
                            type = r.IsNull("type") ? 0 : Convert.ToInt32(Convert.ToString(r["type"]))
                        }
                    };
                    listU.Add(ud);
                }
                return listU;
            }
            return null;
        }

        internal int GetAccountSize()
        {
            return new DataProvider().executeQuery("SELECT COUNT(*) FROM UserDetail ud join Account a on ud.username = a.username", "UserDetailDAO GetAccountSize").Rows[0].Field<int>(0);
        }

        internal List<UserDetail> GetUserDetailByPage(int index, int size)
        {
            string strSelect = "WITH X AS(SELECT ROW_NUMBER() OVER (ORDER BY [type] DESC) AS [Row], ud.username, a.password, a.type, ud.fullname, ud.email, ud.phone, ud.address, ud.imageLink FROM UserDetail ud join Account a on ud.username = a.username ) SELECT * FROM X  WHERE [Row] BETWEEN (" + index + " * " + size + " - (" + size + " - 1)) AND (" + index + " * " + size + ")";
            DataTable dt = new DataProvider().executeQuery(strSelect, "UserDetailDAO GetAccount");
            if (dt.Rows.Count > 0)
            {
                List<UserDetail> listU = new List<UserDetail>();
                foreach (DataRow r in dt.AsEnumerable())
                {
                    UserDetail ud = new UserDetail()
                    {
                        username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                        fullname = r.IsNull("fullname") ? String.Empty : Convert.ToString(r["fullname"]),
                        email = r.IsNull("email") ? String.Empty : Convert.ToString(r["email"]),
                        phone = r.IsNull("phone") ? String.Empty : Convert.ToString(r["phone"]),
                        address = r.IsNull("address") ? String.Empty : Convert.ToString(r["address"]),
                        imageLink = r.IsNull("imageLink") ? String.Empty : Convert.ToString(r["imageLink"]),
                        Account = new Account()
                        {
                            username = r.IsNull("username") ? String.Empty : Convert.ToString(r["username"]),
                            password = r.IsNull("password") ? String.Empty : Convert.ToString(r["password"]),
                            type = r.IsNull("type") ? 0 : Convert.ToInt32(Convert.ToString(r["type"]))
                        }
                    };
                    listU.Add(ud);
                }
                return listU;
            }
            return null;
        }

        internal bool updateProfile(string username, string fullname, string address, string phone, string email, string img)
        {
            return new DataProvider().executeNonQuery("UPDATE [dbo].[UserDetail] SET [fullname] = '" + fullname + "',[email] = '" + email + "',[phone] = '" + phone + "',[address] = '" + address + "',[imageLink] = '" + img + "' WHERE [username] = '" + username + "'", "UserDetailDAO updateProfile");
        }
    }
}