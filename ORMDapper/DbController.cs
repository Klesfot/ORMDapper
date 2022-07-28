using ORMDapper.Data;
using ORMDapper.Model;

namespace ORMDapper
{
    public class DbController : IDbController
    {
        private IDapperRepository _repository;
        public DbController(IDapperRepository repository)
        {
            _repository = repository;
        }

        public int AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Product GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public int DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public int AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Product GetOrder(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public int DeleteOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> FetchAllProducts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> FetchOrders(int month = 0, OrderStatus status = OrderStatus.NotStarted, int year = 1980,
            Product? product = null, bool useStoredProcedure = false)
        {
            throw new NotImplementedException();
        }

        public int DeleteOrders(int month = 0, OrderStatus status = OrderStatus.NotStarted, int year = 1980, Product? product = null,
            bool useStoredProcedure = false)
        {
            throw new NotImplementedException();
        }
    }
}