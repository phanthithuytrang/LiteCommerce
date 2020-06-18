using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// lưu thông tinh liên qua đến tài khoản đăng nhập hệ thống
    /// </summary>
    public class UserAccount
    {
        public string UserID { get; set; }
        /// <summary>
        /// Tên đầy đủ của User
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Tên file ảnh của User
        /// </summary>
        public string Photo { get; set; }

        public string Title { get; set; }

        public string Roles { get; set; }

    }
}
