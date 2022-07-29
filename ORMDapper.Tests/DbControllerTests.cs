using System.Data.SqlClient;
using DapperQueryBuilder;
using Moq;
using ORMDapper.Data;
using ORMDapper.Model;
using ORMDapper.Tests.Helpers;

#pragma warning disable CS8618

namespace ORMDapper.Tests
{
    [TestFixture]
    public class Tests
    {
        private IDbController _dbController;
        private Mock<IDapperRepository> _repositoryMock;
        private SqlConnection _fakeSqlConnection;

        [SetUp]
        public void OnSetup()
        {
            _fakeSqlConnection = new SqlConnection("Data Source=EPGETBIW03B7;Initial Catalog=ORMFundamentals;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            _repositoryMock = new Mock<IDapperRepository>();
            _dbController = new DbController(_repositoryMock.Object);
        }

        [Test]
        public void AddProduct_ProvidedValidProduct_ExecutesCorrectQuery()
        {
            var sql = @"INSERT INTO [Product] ([Name], [Description], [Weight], [Height], [Width], [Length]) VALUES(@Name, @Description, @Weight, @Height, @Width, @Length)";
            var product = GetSingleTestProduct();
            var param = new
            { product.Name, product.Description, product.Weight, product.Height, product.Width, product.Length };
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            _dbController.AddProduct(product);

            _repositoryMock.Verify(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param))));
        }

        [Test]
        public void AddProduct_ProvidedValidProduct_OneRowAffected()
        {
            var sql = @"INSERT INTO [Product] ([Name], [Description], [Weight], [Height], [Width], [Length]) VALUES(@Name, @Description, @Weight, @Height, @Width, @Length)";
            var product = GetSingleTestProduct();
            var param = new
            { product.Name, product.Description, product.Weight, product.Height, product.Width, product.Length };
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var affectedRows = _dbController.AddProduct(product);

            Assert.That(affectedRows, Is.EqualTo(1));
        }

        [Test]
        public void AddProduct_ProvidedInvalidProduct_ThrowsArgumentNullException()
        {
            Assert.That(() => _dbController.AddProduct(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetProduct_ProvidedValidId_QueriesForCorrectProduct()
        {
            var productId = 0;
            var sql = $@"SELECT [ProductId], [Name], [Description], [Weight], [Height], [Width], [Length] FROM [Product] WHERE [ProductId] = {productId};";
            var productList = new List<Product> { GetSingleTestProduct() };
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            _dbController.GetProduct(productId);

            _repositoryMock.Verify(r => r.Query<Product>(sql), Times.Once);
        }

        [Test]
        public void GetProduct_ProvidedValidId_ReturnsCorrectProduct()
        {
            var productId = 0;
            var sql = $@"SELECT [ProductId], [Name], [Description], [Weight], [Height], [Width], [Length] FROM [Product] WHERE [ProductId] = {productId};";
            var productList = new List<Product> { GetSingleTestProduct() };
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            var result = _dbController.GetProduct(productId);

            Assert.That(result, Is.EqualTo(productList[0]));
        }

        [Test]
        public void GetProduct_ProvidedInvalidId_ReturnsNull()
        {
            var validProductId = 0;
            var sql = $@"SELECT [ProductId], [Name], [Description], [Weight], [Height], [Width], [Length] FROM [Product] WHERE [ProductId] = {validProductId};";
            var invalidProductId = int.MaxValue;
            var productList = new List<Product> { GetSingleTestProduct() };
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            var result = _dbController.GetProduct(invalidProductId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateProduct_ProvidedValidProductModels_ExecutesCorrectQuery()
        {
            var product = GetSingleTestProduct();
            var updatedProduct = product;
            updatedProduct.Name = "The Container";
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
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            _dbController.UpdateProduct(product, updatedProduct);

            _repositoryMock.Verify(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param))), Times.Once);
        }

        [Test]
        public void UpdateProduct_ProvidedValidProductModels_OneRowAffected()
        {
            var product = GetSingleTestProduct();
            var updatedProduct = product;
            updatedProduct.Name = "The Container";
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
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var affectedRows = _dbController.UpdateProduct(product, updatedProduct);

            Assert.That(affectedRows, Is.EqualTo(1));
        }

        [Test]
        public void UpdateProduct_ProvidedNullProductModel_ThrowsArgumentNullException()
        {
            Assert.That(() => _dbController.UpdateProduct(null, GetSingleTestProduct()), Throws.ArgumentNullException);
        }

        [Test]
        public void UpdateProduct_ProvidedNullUpdatedProductModel_ThrowsArgumentNullException()
        {
            Assert.That(() => _dbController.UpdateProduct(GetSingleTestProduct(), null), Throws.ArgumentNullException);
        }

        [Test]
        public void DeleteProduct_ProvidedValidProduct_ExecutesCorrectQuery()
        {
            var sql = @"DELETE FROM [Product] WHERE [ProductId] = @ProductId";
            var idToDelete = 0;
            var param = new { ProductId = idToDelete };
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            _dbController.DeleteProduct(idToDelete);

            _repositoryMock.Verify(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param))));
        }

        [Test]
        public void DeleteProduct_ProvidedValidProduct_OneRowAffected()
        {
            var sql = @"DELETE FROM [Product] WHERE [ProductId] = @ProductId";
            var idToDelete = 0;
            var param = new { ProductId = idToDelete };
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var affectedRows = _dbController.DeleteProduct(idToDelete);

            Assert.That(affectedRows, Is.EqualTo(1));
        }

        [Test]
        public void DeleteProduct_ProvidedInvalidId_ZeroRowsAffected()
        {
            var validProductIdToDelete = 0;
            var sql = @"DELETE FROM [Product] WHERE [ProductId] = @ProductId";
            var invalidProductIdToDelete = int.MaxValue;
            var param = new { ProductId = validProductIdToDelete };
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var result = _dbController.DeleteProduct(invalidProductIdToDelete);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void AddOrder_ProvidedValidOrder_ExecutesCorrectQuery()
        {
            var sql = @"INSERT INTO [Order] ([Status], [CreatedDate], [UpdatedDate], [ProductId]) VALUES(@Status, @CreatedDate, @UpdatedDate, @ProductId)";
            var order = GetSingleTestOrder();
            var param = new { order.OrderId, order.Status, order.CreatedDate, order.UpdatedDate, order.ProductId };
            InsertSingleProductIntoMockRepository();
            _repositoryMock.Setup(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            _dbController.AddOrder(order);

            _repositoryMock.Verify(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param))));
        }

        [Test]
        public void AddOrder_ProvidedValidOrder_OneRowAffected()
        {
            var sql = @"INSERT INTO [Order] ([Status], [CreatedDate], [UpdatedDate], [ProductId]) VALUES(@Status, @CreatedDate, @UpdatedDate, @ProductId)";
            var order = GetSingleTestOrder();
            var param = new { order.OrderId, order.Status, order.CreatedDate, order.UpdatedDate, order.ProductId };
            InsertSingleProductIntoMockRepository();
            _repositoryMock.Setup(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var affectedRows = _dbController.AddOrder(order);

            Assert.That(affectedRows, Is.EqualTo(1));
        }

        [Test]
        public void AddOrder_ProvidedInvalidOrder_ThrowsArgumentNullException()
        {
            Assert.That(() => _dbController.AddOrder(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetOrder_ProvidedValidId_QueriesForCorrectOrder()
        {
            var orderId = 0;
            var sql = $@"SELECT [OrderId], [Status], [CreatedDate], [UpdatedDate], [ProductId] FROM [Order] WHERE [OrderId] = {orderId};";
            var orderList = new List<Order> { GetSingleTestOrder() };
            _repositoryMock.Setup(r => r.Query<Order>(sql)).Returns(orderList);

            _dbController.GetOrder(orderId);

            _repositoryMock.Verify(r => r.Query<Order>(sql), Times.Once);
        }

        [Test]
        public void GetOrder_ProvidedValidId_ReturnsCorrectOrder()
        {
            var orderId = 0;
            var sql = $@"SELECT [OrderId], [Status], [CreatedDate], [UpdatedDate], [ProductId] FROM [Order] WHERE [OrderId] = {orderId};";
            var orderList = new List<Order> { GetSingleTestOrder() };
            _repositoryMock.Setup(r => r.Query<Order>(sql)).Returns(orderList);
            
            var result = _dbController.GetOrder(orderId);

            Assert.That(result, Is.EqualTo(orderList[0]));
        }

        [Test]
        public void GetOrder_ProvidedInvalidId_ReturnsNull()
        {
            var validOrderId = 0;
            var sql = $@"SELECT [OrderId], [Status], [CreatedDate], [UpdatedDate], [ProductId] FROM [Order] WHERE [OrderId] = {validOrderId};";
            var invalidOrderId = int.MaxValue;
            var orderList = new List<Order> { GetSingleTestOrder() };
            _repositoryMock.Setup(r => r.Query<Order>(sql)).Returns(orderList);

            var result = _dbController.GetOrder(invalidOrderId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateOrder_ProvidedValidOrderModels_ExecutesCorrectQuery()
        {
            var order = GetSingleTestOrder();
            var updatedOrder = order;
            updatedOrder.Status = nameof(OrderStatus.InProgress);
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
            _repositoryMock.Setup(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);
            
            _dbController.UpdateOrder(order, updatedOrder);

            _repositoryMock.Verify(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param))), Times.Once);
        }

        [Test]
        public void UpdateOrder_ProvidedValidOrderModels_OneRowAffected()
        {
            var order = GetSingleTestOrder();
            var updatedOrder = order;
            updatedOrder.Status = nameof(OrderStatus.InProgress);
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
            _repositoryMock.Setup(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var affectedRows = _dbController.UpdateOrder(order, updatedOrder);

            Assert.That(affectedRows, Is.EqualTo(1));
        }

        [Test]
        public void UpdateOrder_ProvidedNullOrderModel_ThrowsArgumentNullException()
        {
            Assert.That(() => _dbController.UpdateOrder(null, GetSingleTestOrder()), Throws.ArgumentNullException);
        }

        [Test]
        public void UpdateOrder_ProvidedNullUpdatedOrderModel_ThrowsArgumentNullException()
        {
            Assert.That(() => _dbController.UpdateOrder(GetSingleTestOrder(), null), Throws.ArgumentNullException);
        }

        [Test]
        public void DeleteOrder_ProvidedValidOrder_ExecutesCorrectQuery()
        {
            var sql = @"DELETE FROM [Order] WHERE [OrderId] = @OrderId";
            var idToDelete = 0;
            var param = new { OrderId = idToDelete };
            _repositoryMock.Setup(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);
            
            _dbController.DeleteOrder(idToDelete);

            _repositoryMock.Verify(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param))));
        }

        [Test]
        public void DeleteOrder_ProvidedValidOrder_OneRowAffected()
        {
            var sql = @"DELETE FROM [Order] WHERE [OrderId] = @OrderId";
            var idToDelete = 0;
            var param = new { OrderId = idToDelete };
            _repositoryMock.Setup(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var affectedRows = _dbController.DeleteOrder(idToDelete);

            Assert.That(affectedRows, Is.EqualTo(1));
        }

        [Test]
        public void DeleteOrder_ProvidedInvalidId_ZeroRowsAffected()
        {
            var validOrderIdToDelete = 0;
            var sql = @"DELETE FROM [Order] WHERE [OrderId] = @OrderId";
            var invalidOrderIdToDelete = int.MaxValue;
            var param = new { OrderId = validOrderIdToDelete };
            _repositoryMock.Setup(r => r.Execute<Order>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var result = _dbController.DeleteOrder(invalidOrderIdToDelete);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void FetchAllProducts_ProductsPresentInRepo_QueriesAllProducts()
        {
            var sql = @"SELECT * FROM [Product]";
            var productList = GetTestProductList();
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            _dbController.FetchAllProducts();

            _repositoryMock.Verify(r => r.Query<Product>(sql));
        }

        [Test]
        public void FetchAllProducts_ProductsPresentInRepo_ReturnsAllProductsInRepo()
        {
            var sql = @"SELECT * FROM [Product]";
            var productList = GetTestProductList();
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            var result = _dbController.FetchAllProducts();

            Assert.That(result, Is.EqualTo(productList));
        }

        [Test]
        public void FetchAllProducts_OneProductPresentInRepo_ReturnsOneProductInRepo()
        {
            var sql = @"SELECT * FROM [Product]";
            var productList = new List<Product>(){GetSingleTestProduct()};
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            var result = _dbController.FetchAllProducts();

            Assert.That(result, Is.EqualTo(productList));
        }

        [Test]
        public void FetchAllProducts_NoProductsInRepo_ReturnsListWithZeroElements()
        {
            var sql = @"SELECT * FROM [Product]";
            var productList = new List<Product>();
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            var result = _dbController.FetchAllProducts();

            Assert.That(result.Count(), Is.EqualTo(productList.Count));
        }

        [Test]
        public void FetchOrders_AllConditionFieldsFilled_QueriesCorrectParameters()
        {
            var orderList = GetTestOrderList();
            FormattableString sql =
                $@"SELECT [OrderId],[Status],[CreatedDate],[UpdatedDate],[ProductId] FROM [Order] WHERE 1=1";
            var param = new
            {
                CreatedDate = new DateTime(1799, 7, 1),
                UpdatedDate = new DateTime(1799, 7, 1),
                Status = OrderStatus.NotStarted.ToString(),
                ProductId = 0
            };
            var queryBuilder = new QueryBuilder(_fakeSqlConnection, sql);
            _repositoryMock.Setup(r => r.QueryBuilder(It.IsAny<FormattableString>())).Returns(queryBuilder);
            _repositoryMock.Setup(r => r.Query<Order>(It.IsAny<string>(), It.Is<object>(o => o.JsonMatches(param)))).Returns(orderList);

            _dbController.FetchOrders(7, 1799, OrderStatus.NotStarted, GetSingleTestProduct());

            _repositoryMock.Verify(r => r.Query<Order>(It.IsAny<string>(), It.Is<object>(o => o.JsonMatches(param))), Times.Once);
        }

        [Test]
        public void FetchOrders_AllConditionFieldsFilled_ReturnsCorrectOrders()
        {
            var orderList = GetTestOrderList();
            FormattableString sql =
                $@"SELECT [OrderId],[Status],[CreatedDate],[UpdatedDate],[ProductId] FROM [Order] WHERE 1=1";
            var param = new
            {
                CreatedDate = new DateTime(1799, 7, 1),
                UpdatedDate = new DateTime(1799, 7, 1),
                Status = OrderStatus.NotStarted.ToString(),
                ProductId = 0
            };
            var queryBuilder = new QueryBuilder(_fakeSqlConnection, sql);
            _repositoryMock.Setup(r => r.QueryBuilder(It.IsAny<FormattableString>())).Returns(queryBuilder);
            _repositoryMock.Setup(r => r.Query<Order>(It.IsAny<string>(), It.Is<object>(o => o.JsonMatches(param)))).Returns(orderList);

            var result = _dbController.FetchOrders(7, 1799, OrderStatus.NotStarted, GetSingleTestProduct());

            Assert.That(result, Is.EqualTo(orderList));
        }

        private void InsertSingleProductIntoMockRepository()
        {
            var productId = 0;
            var sql = $@"SELECT ProductId, [Name], [Description], [Weight], [Height], [Width], [Length] FROM [Product] WHERE [ProductId] = {productId};";
            var productList = new List<Product> { GetSingleTestProduct() };
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);
        }

        private static Product GetSingleTestProduct()
        {
            return new Product()
            {
                ProductId = 0,
                Name = "The Box",
                Description = "",
                Height = .5m,
                Weight = .5m,
                Width = .5m,
                Length = .5m
            };
        }

        private static IEnumerable<Product> GetTestProductList()
        {
            var productList = new List<Product>();
            var product0 = GetSingleTestProduct();
            var product1 = GetSingleTestProduct();
            product1.ProductId = 1;
            productList.Add(product0);
            productList.Add(product1);
            return productList;
        }

        private static Order GetSingleTestOrder()
        {
            return new Order()
            {
                OrderId = 0,
                Status = nameof(OrderStatus.NotStarted),
                CreatedDate = DateTime.UnixEpoch,
                UpdatedDate = null,
                ProductId = 0
            };
        }

        private static IEnumerable<Order> GetTestOrderList()
        {
            var orderList = new List<Order>();
            var order0 = GetSingleTestOrder();
            var order1 = GetSingleTestOrder();
            order0.UpdatedDate = DateTime.Today;
            order1.UpdatedDate = DateTime.Today;
            order1.Status = nameof(OrderStatus.InProgress);
            orderList.Add(order0);
            orderList.Add(order1);
            return orderList;
        }
    }
}