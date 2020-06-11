using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer
{
    public interface IEmployeeDAL
    {
        /// <summary>
        /// Hiển thị danh sách Employee, phân trang và có thể tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Employee> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Đếm số lượng đã tìm kiếm được
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        Employee Get(int EmployeeID);

        /// <summary>
        /// Bổ sung 1 Employee, Hàm trả về ID của Employee được bổ sung
        /// Nếu lỗi, hàm trả về giá trị nhỏ hơn hoặc bằng 0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Employee data);

        /// <summary>
        /// Thêm một Employee,trả về kết quả true/false
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Employee data);

        /// <summary>
        /// Xóa một hoặc nhiều Employee
        /// Kết quả trả về true/false
        /// </summary>
        /// <param name="EmployeeIDs"></param>
        /// <returns></returns>
        int Delete(int[] EmployeeIDs);
    }
}
