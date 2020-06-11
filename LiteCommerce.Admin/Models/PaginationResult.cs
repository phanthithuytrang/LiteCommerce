using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public abstract class PaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        /// <summary>
        /// Giá trị tìm kiếm
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                int p = RowCount / PageSize;
                if (RowCount / PageSize > 0)
                    p += 1;
                return p;
            }
        }
    }
}