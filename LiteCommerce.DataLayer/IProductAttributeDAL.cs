using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer
{
    public interface IProductAttributeDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<ProductAttribute> List(int productID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(ProductAttribute data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(ProductAttribute data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeIDs"></param>
        /// <returns></returns>
        int Delete(int[] attributeIDs);
        ProductAttribute Get(int attributeID);
    }
}
