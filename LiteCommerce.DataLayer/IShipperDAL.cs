using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayer
{
    public interface IShipperDAL
    {
        List<Shipper> List(int page, int pageSize, string searchValue);
        int Count(string searchValue);
        Shipper Get(int ShipperID);
        int Add(Shipper data);
        bool Update(Shipper data);
        int Delete(int[] ShipperIDs);
    }
}
