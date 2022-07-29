using ORMDapper.Model;

namespace ORMDapper
{
    public interface IDbController
    {
        int AddProduct(Product product);
        Product GetProduct(int id);
        int UpdateProduct(Product product, Product updatedProduct);
        int DeleteProduct(int id);
        int AddOrder(Order order);
        Product GetOrder(object param);
        int UpdateOrder(Order order, Order updatedOrder);
        int DeleteOrder(int id);
        IEnumerable<Product> FetchAllProducts();

        IEnumerable<Order> FetchOrders(int month = 0, OrderStatus status = OrderStatus.NotStarted, int year = 1980,
            Product? product = null, bool useStoredProcedure = false);

        int DeleteOrders(int month = 0, OrderStatus status = OrderStatus.NotStarted, int year = 1980,
            Product? product = null, bool useStoredProcedure = false);
    }
}