using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer
{
    public interface ICustomerDAL
    {
        /// <summary>
        /// Hiển thị danh sách Customer, phân trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Customer> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Đếm số lượng đã tìm kiếm được
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        Customer Get(string CustomerID);

        /// <summary>
        /// Bổ sung 1 Customer, Hàm trả về ID của Customer được bổ sung
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Customer data);

        /// <summary>
        /// Thêm một Customer,trả về kết quả true/false
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Customer data);

        /// <summary>
        /// Xóa một hoặc nhiều Customer
        /// Kết quả trả về true/false
        /// </summary>
        /// <param name="CustomerIDs"></param>
        /// <returns></returns>
        int Delete(string[] CustomerIDs);
    }
}

