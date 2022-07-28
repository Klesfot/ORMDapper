using ORMDapper.Model;

namespace ORMDapper
{
    public interface IDbController
    {
        int AddProduct(Product product);
        Product GetProduct(int id);
        int UpdateProduct(Product product);
        int DeleteProduct(Product product);
        int AddOrder(Order order);
        Product GetOrder(int id);
        int UpdateOrder(Order order);
        int DeleteOrder(Order order);
        IEnumerable<Product> FetchAllProducts();

        IEnumerable<Order> FetchOrders(int month = 0, OrderStatus status = OrderStatus.NotStarted, int year = 1980,
            Product? product = null, bool useStoredProcedure = false);

        int DeleteOrders(int month = 0, OrderStatus status = OrderStatus.NotStarted, int year = 1980,
            Product? product = null, bool useStoredProcedure = false);
    }
}