using ORMDapper.Data;
using ORMDapper.Model;

namespace ORMDapper
{
    public class DbController : IDbController
    {
        private readonly IDapperRepository _repository;
        public DbController(IDapperRepository repository)
        {
            _repository = repository;
        }

        public int AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException();
            }

            var sql = @"INSERT INTO [Product] ([Name], [Description], [Weight], [Height], [Width], [Length]) VALUES(@Name, @Description, @Weight, @Height, @Width, @Length)";
            var param = new
                { product.Name, product.Description, product.Weight, product.Height, product.Width, product.Length };
            return _repository.Execute<Product>(sql, param);
        }

        public Product GetProduct(int id)
        {
            var sql = $@"SELECT [ProductId], [Name], [Description], [Weight], [Height], [Width], [Length] FROM [Product] WHERE [ProductId] = {id};";
            return _repository.Query<Product>(sql).FirstOrDefault();
        }

        public int UpdateProduct(Product product, Product updatedProduct)
        {
            if (product == null || updatedProduct == null)
            {
                throw new ArgumentNullException();
            }

            var sql = $@"
                UPDATE [Product] 
                SET [Name] = @Name,
                    [Description] = @Description,
                    [Weight] = @Weight,
                    [Height] = @Height,
                    [Width] = @Width,
                    [Length] = @Length 
                WHERE [ProductId] = {product.ProductId}";
            var param = new
            {
                updatedProduct.Name,
                updatedProduct.Description,
                updatedProduct.Weight,
                updatedProduct.Height,
                updatedProduct.Width,
                updatedProduct.Length
            };

            return _repository.Execute<Product>(sql, param);
        }

        public int DeleteProduct(int id)
        {
            var sql = @"DELETE FROM [Product] WHERE [ProductId] = @ProductId";
            var param = new { ProductId = id };
            return _repository.Execute<Product>(sql, param);
        }

        public int AddOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException();
            }

            var sql = @"INSERT INTO [Order] ([Status], [CreatedDate], [UpdatedDate], [ProductId]) VALUES(@Status, @CreatedDate, @UpdatedDate, @ProductId)";
            var param = new { order.OrderId, order.Status, order.CreatedDate, order.UpdatedDate, order.ProductId };
            return _repository.Execute<Order>(sql, param);
        }

        public Order GetOrder(int id)
        {
            var sql = $@"SELECT [OrderId], [Status], [CreatedDate], [UpdatedDate], [ProductId] FROM [Order] WHERE [OrderId] = {id};";
            return _repository.Query<Order>(sql).FirstOrDefault();
        }

        public int UpdateOrder(Order order, Order updatedOrder)
        {
            if (order == null || updatedOrder == null)
            {
                throw new ArgumentNullException();
            }

            var sql = $@"
                UPDATE [Order]
                SET [Status] = @Status,
                    [CreatedDate] = @CreatedDate,
                    [UpdatedDate] = @UpdatedDate,
                    [ProductId] = @ProductId
                WHERE [ProductId] = {order.OrderId}";
            var param = new
            {
                updatedOrder.Status,
                updatedOrder.CreatedDate,
                updatedOrder.UpdatedDate,
                updatedOrder.ProductId
            };

            return _repository.Execute<Order>(sql, param);
        }

        public int DeleteOrder(int id)
        {
            var sql = @"DELETE FROM [Order] WHERE [OrderId] = @OrderId";
            var param = new { OrderId = id };
            return _repository.Execute<Order>(sql, param);
        }

        public IEnumerable<Product> FetchAllProducts()
        {
            var sql = @"SELECT * FROM [Product]";
            return _repository.Query<Product>(sql);
        }

        public IEnumerable<Order> FetchOrders(int? month = null, int? year = null, OrderStatus status = OrderStatus.Null,
            Product? product = null, bool useStoredProcedure = false)
        {
            var orderParams = new Order
            {
                CreatedDate = DateTime.UnixEpoch,
                UpdatedDate = DateTime.UnixEpoch,
                Status = nameof(OrderStatus.Null),
                ProductId = -1
            };
            var query = _repository.QueryBuilder($@"SELECT [OrderId],[Status],[CreatedDate],[UpdatedDate],[ProductId] FROM [Order] WHERE 1=1");

            if (status != OrderStatus.Null)
            {
                query += $" AND [Status] = @Status";

                orderParams.Status = status.ToString();
            }

            if (month == null && year != null)
            {
                query += $" AND YEAR([CreatedDate]) = YEAR(@CreatedDate) OR YEAR([UpdatedDate]) = YEAR(@UpdatedDate)";

                orderParams.CreatedDate = new DateTime((int)year, 1, 1);
                orderParams.UpdatedDate = new DateTime((int)year, 1, 1);
            }
            else if (year == null && month != null)
            {
                query += $" AND MONTH([CreatedDate]) = MONTH(@CreatedDate) OR MONTH([UpdatedDate]) = MONTH(@UpdatedDate)";

                orderParams.CreatedDate = new DateTime(1900, (int)month, 1);
                orderParams.UpdatedDate = new DateTime(1900, (int)month, 1);
            }
            else if (year != null && month != null)
            {
                query += $" AND MONTH([CreatedDate]) = MONTH(@CreatedDate) OR MONTH([UpdatedDate]) = MONTH(@UpdatedDate)";
                query += $" AND YEAR([CreatedDate]) = YEAR(@CreatedDate) OR YEAR([UpdatedDate]) = YEAR(@UpdatedDate)";

                orderParams.CreatedDate = new DateTime((int)year, (int)month, 1);
                orderParams.UpdatedDate = new DateTime((int)year, (int)month, 1);
            }

            if (product != null)
            {
                query += $" AND [ProductId] = @ProductId";

                orderParams.ProductId = product.ProductId;
            }

            if (useStoredProcedure){}

            var param = new
            {
                orderParams.CreatedDate,
                orderParams.UpdatedDate,
                orderParams.Status,
                orderParams.ProductId
            };

            return _repository.Query<Order>(query.Sql, param);
        }

        public int DeleteOrders(int? month = null, int? year = null, OrderStatus status = OrderStatus.Null, Product? product = null,
            bool useStoredProcedure = false)
        {
            throw new NotImplementedException();
        }
    }
}