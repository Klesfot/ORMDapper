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
        Order GetOrder(int id);
        int UpdateOrder(Order order, Order updatedOrder);
        int DeleteOrder(int id);
        IEnumerable<Product> FetchAllProducts();

        IEnumerable<Order> FetchOrders(int? month = null, int? year = null, OrderStatus status = OrderStatus.Null,
            Product? product = null, bool useStoredProcedure = false);

        int DeleteOrders(int? month = null, int? year = null, OrderStatus status = OrderStatus.Null,
            Product? product = null, bool useStoredProcedure = false);
    }
}