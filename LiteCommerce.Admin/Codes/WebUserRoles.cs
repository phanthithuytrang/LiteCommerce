using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
      /// <summary>
      /// Định nghĩa danh sách các Role của user
      /// </summary>
      public class WebUserRoles
      {
            /// <summary>
            /// Không xác định
            /// </summary>
            public const string ANONYMOUS = "anonymous";
            /// <summary>
            /// Nhân viên bán hàng
            /// </summary>
            public const string STAFF_SALE = "staff_sale";
            /// <summary>
            /// Nhân viên quản lí dữ liệu
            /// </summary>
            public const string STAFF_DATA = "staff_data";
            /// <summary>
            /// Nhân viên quản lí tài khoản
            /// </summary>
            public const string STAFF_ACCOUNT = "staff_account";
            /// <summary>
            /// Quản trị hệ thống
            /// </summary>
            public const string ADMINISTRATOR = "administrator";   
            
           
            
      }
}