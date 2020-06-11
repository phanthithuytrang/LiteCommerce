using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Kiểm tra UN và Pass có hợp lệ hay không
        /// Nếu hợp lệ thì trả về thông tin của User
        /// Ngược lại thì trả về giá trị NULL
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        UserAccount Authorize(string userName, string password);
    }
}
