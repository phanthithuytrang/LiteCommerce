using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer
{
    public interface IAttributeDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Attributes> List();
    }
}
