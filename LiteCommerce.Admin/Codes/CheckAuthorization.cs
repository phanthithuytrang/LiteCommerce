using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Codes
{
    public static class CheckAuthorization
    {
        public static string CheckAuthorizationDATA(string text)
        {
            try
            {
                string[] infos = text.Split('|');
                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].Equals(WebUserRoles.STAFF_DATA))
                        return WebUserRoles.STAFF_DATA;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
        public static string CheckAuthorizationSale(string text)
        {
            try
            {
                string[] infos = text.Split('|');
                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].Equals(WebUserRoles.STAFF_SALE))
                        return WebUserRoles.STAFF_SALE;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
        public static string CheckAuthorizationAcc(string text)
        {
            try
            {
                string[] infos = text.Split('|');
                for (int i = 0; i < infos.Length; i++)
                {
                    if (infos[i].Equals(WebUserRoles.STAFF_ACCOUNT))
                        return WebUserRoles.STAFF_ACCOUNT;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}