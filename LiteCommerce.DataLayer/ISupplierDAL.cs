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
    public interface ISupplierDAL
    {
        /// <summary>
        /// Hiển thị danh sách Supplier, phân trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Supplier> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Đếm số lượng đã tìm kiếm được
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
         int Count(string searchValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        Supplier Get(int supplierID);

        /// <summary>
        /// Bổ sung 1 supplier, Hàm trả về ID của supplier được bổ sung
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Supplier data);

        /// <summary>
        /// Thêm một supplier,trả về kết quả true/false
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Supplier data);

        /// <summary>
        /// Xóa một hoặc nhiều supplier
        /// Kết quả trả về true/false
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        int Delete(int[] supplierIDs);
    }
}
