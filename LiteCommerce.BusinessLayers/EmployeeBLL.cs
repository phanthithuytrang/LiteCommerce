using LiteCommerce.DataLayer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public class EmployeeBLL
    {
        public static void Initialize(string connectionString)
        {
            EmployeeDB = new DataLayer.SqlServer.EmployeeDAL(connectionString);          
        }
        private static IEmployeeDAL EmployeeDB { get; set; }

        public static List<Employee> ListOfEmployees(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = EmployeeDB.Count(searchValue);
            return EmployeeDB.List(page, pageSize, searchValue);
        }

        public static Employee GetEmployee(int EmployeeID)
        {
            return EmployeeDB.Get(EmployeeID);
        }
        public static int AddEmployee(Employee data)
        {
            return EmployeeDB.Add(data);
        }
        public static bool UpdateEmployee(Employee data)
        {
            return EmployeeDB.Update(data);
        }
        public static int DeleteEmployees(int[] EmployeeIDs)
        {
            return EmployeeDB.Delete(EmployeeIDs);
        }
        public static bool ChangePassword(int userID, string password)
        {
            return EmployeeDB.ChangePassword(userID, password);
        }
    }
}
