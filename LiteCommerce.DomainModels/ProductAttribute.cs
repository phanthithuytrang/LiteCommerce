using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    public class ProductAttribute
    {
        public int AttributeID { get; set; }
        public int ProductID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValues { get; set; }
        public int DisplayOrder { get; set; }
    }
}
