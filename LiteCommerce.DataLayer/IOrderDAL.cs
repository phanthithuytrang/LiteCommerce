using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer
{
    public interface IOrderDAL
    {
        /// <summary>
        /// Hiển thị danh sách Order, có phân trang theo kích thước trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        List<Order> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// Đếm số lượng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin của 1 Order theo ID. Trả về Order lấy được.
        /// </summary>
        /// <param name="OrderID">ID của Order cần lấy thông tin</param>
        /// <returns></returns>
        Order Get(int OrderID);
        /// <summary>
        /// Bổ sung 1 Order. Hàm trả về ID của Order được bổ sung.
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data">Dữ liệu của Order cần bổ sung</param>
        /// <returns></returns>
        int Add(Order data);
        /// <summary>
        /// Cập nhật thông tin của 1 Order.
        /// Trả về true nếu cập nhật được, false nếu không cập nhật được.
        /// </summary>
        /// <param name="data">Thông tin đã sửa</param>
        /// <returns></returns>
        bool Update(Order data);
        /// <summary>
        /// Xóa các Order.
        /// Trả về số lượng Order đã được xóa
        /// </summary>
        /// <param name="OrderIDs">Danh sách các ID của các Order cần xóa</param>
        /// <returns></returns>
        int Delete(int[] OrderIDs);
    }
}
