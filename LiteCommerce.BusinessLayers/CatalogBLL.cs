using LiteCommerce.DataLayer;
using LiteCommerce.DataLayer.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// Các chức năng xử lí nghiệp vụ liên quan đến quản lí dữ liệu chung của hệ thống
    /// như : nhà cung cấp, khách hàng, mặt hàng, ...
    /// </summary>
    public class CatalogBLL
    {
        /// <summary>
        /// Hàm phải được gọi để khởi tạo chức năng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            SupplierDB = new DataLayer.SqlServer.SupplierDAL(connectionString);
            CustomerDB = new DataLayer.SqlServer.CustomerDAL(connectionString);
            ShipperDB = new DataLayer.SqlServer.ShipperDAL(connectionString);
            CategoryDB = new DataLayer.SqlServer.CategoryDAL(connectionString);
            ProductDB = new DataLayer.SqlServer.ProductDAL(connectionString);
            CountryDB = new DataLayer.SqlServer.CountryDAL(connectionString);
            AttributeDB = new DataLayer.SqlServer.AttributeDAL(connectionString);
            ProductAttributeDB = new DataLayer.SqlServer.ProductAttributeDAL(connectionString);
        }


        #region Khai báo các thuộc tính giao tiếp vs DAL
        private static ISupplierDAL SupplierDB { get; set; }

        private static ICustomerDAL CustomerDB { get; set; }

        private static IShipperDAL ShipperDB { get; set; }

        private static ICategoryDAL CategoryDB { get; set; }

        private static IProductDAL ProductDB { get; set; }

        private static ICountryDAL CountryDB { get; set; }

        private static IAttributeDAL AttributeDB { get; set; }

        private static IProductAttributeDAL ProductAttributeDB { get; set; }
        #endregion

        #region Khai báo các chức năng xử lí nghiệp vụ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = SupplierDB.Count(searchValue);
            return SupplierDB.List(page, pageSize, searchValue);
        }

        public static List<Supplier> ListOfSuppliers()
        {
            return SupplierDB.List(1,-1,"");
        }

        public static List<Customer> ListOfCustomer(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = CustomerDB.Count(searchValue);
            return CustomerDB.List(page, pageSize, searchValue);
        }

        public static List<Shipper> ListOfShippers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = ShipperDB.Count(searchValue);
            return ShipperDB.List(page, pageSize, searchValue);
        }

        public static List<Category> ListOfCategory(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = CategoryDB.Count(searchValue);
            return CategoryDB.List(page, pageSize, searchValue);
        }

        public static List<Category> ListOfCategorys()
        {
            return CategoryDB.List(1, -1, "");
        }

        public static List<Product> ListOfProducts(int page, int pageSize, string searchValue, out int rowCount, string category, string supplier)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = ProductDB.Count(searchValue, category, supplier);
            return ProductDB.List(page, pageSize, searchValue, category, supplier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Product> ListOfProducts()
        {
            return ProductDB.List(1, -1, "", "", "");
        }


        /// <summary>
        /// Lấy thông tin 1 supplier dựa vào DB
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier GetSupplier (int supplierID)
        {
            return SupplierDB.Get(supplierID);
        }
        /// <summary>
        /// Bổ sung 1 supplier, hàm trả về 1 ID của supplier được bổ sung 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return SupplierDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin của 1 supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return SupplierDB.Update(data);
        }
        /// <summary>
        /// Xóa (các) supplier dựa vào id
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        public static int DeleteSuppliers(int[] supplierIDs)
        {
            return SupplierDB.Delete(supplierIDs);
        }

        public static Category GetCategory(int CategoryID)
        {
            return CategoryDB.Get(CategoryID);
        }
        public static int AddCategory(Category data)
        {
            return CategoryDB.Add(data);
        }
        public static bool UpdateCategory(Category data)
        {
            return CategoryDB.Update(data);
        }
        public static int DeleteCategories(int[] CategoryIDs)
        {
            return CategoryDB.Delete(CategoryIDs);
        }

        public static Shipper GetShipper(int ShipperID)
        {
            return ShipperDB.Get(ShipperID);
        }
        public static int AddShipper(Shipper data)
        {
            return ShipperDB.Add(data);
        }
        public static bool UpdateShipper(Shipper data)
        {
            return ShipperDB.Update(data);
        }
        public static int DeleteShippers(int[] ShipperIDs)
        {
            return ShipperDB.Delete(ShipperIDs);
        }

        public static Customer GetCustomer(string CustomerID)
        {
            return CustomerDB.Get(CustomerID);
        }
        public static int AddCustomer(Customer data)
        {
            return CustomerDB.Add(data);
        }
        public static bool UpdateCustomer(Customer data)
        {
            return CustomerDB.Update(data);
        }
        public static int DeleteCustomers(string[] CustomerIDs)
        {
            return CustomerDB.Delete(CustomerIDs);
        }

        public static Product GetProduct(int productID)
        {
            return ProductDB.Get(productID);
        }
        public static int AddProduct(Product data)
        {
            return ProductDB.Add(data);
        }
        public static bool UpdateProduct(Product data)
        {
            return ProductDB.Update(data);
        }
        public static bool DeleteProduct(int[] productIDs)
        {
            return ProductDB.Delete(productIDs);
        }

        public static List<Country> ListOfCountries()
        {
            return CountryDB.List();
        }

        public static List<Attributes> ListOfAttributes()
        {
            return AttributeDB.List();
        }
        public static List<ProductAttribute> ListOfProductAttribute(int productID)
        {
            return ProductAttributeDB.List(productID);
        }
        public static int AddProductAttribute(ProductAttribute data)
        {
            return ProductAttributeDB.Add(data);
        }
        public static bool UpdateProductAttribute(ProductAttribute data)
        {
            return ProductAttributeDB.Update(data);
        }
        public static int DeleteProductAttributes(int[] attributeIDs)
        {
            return ProductAttributeDB.Delete(attributeIDs);
        }
        public static ProductAttribute GetProductAttribute(int attributeID)
        {
            return ProductAttributeDB.Get(attributeID);
        }
        #endregion
    }
}
