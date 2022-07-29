using System.Data.SqlClient;
using Dapper;
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

        [SetUp]
        public void OnSetup()
        {
            _repositoryMock = new Mock<IDapperRepository>();
            _dbController = new DbController(_repositoryMock.Object);
        }

        [Test]
        public void AddProduct_ProvidedValidProduct_ExecutesCorrectQuery()
        {
            var sql = @"INSERT INTO Product (Name, Description, Weight, Height, Width, Length) VALUES(@Name, @Description, @Weight, @Height, @Width, @Length)";
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
            var sql = @"INSERT INTO Product (Name, Description, Weight, Height, Width, Length) VALUES(@Name, @Description, @Weight, @Height, @Width, @Length)";
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
            var sql = $@"SELECT ProductId, [Name], [Description], [Weight], [Height], [Width], [Length] FROM [Product] WHERE [ProductId] = {productId};";
            var productList = new List<Product> { GetSingleTestProduct() };
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            _dbController.GetProduct(productId);

            _repositoryMock.Verify(r => r.Query<Product>(sql), Times.Once);
        }

        [Test]
        public void GetProduct_ProvidedValidId_ReturnsCorrectProduct()
        {
            var productId = 1;
            var sql = $@"SELECT ProductId, [Name], [Description], [Weight], [Height], [Width], [Length] FROM [Product] WHERE [ProductId] = {productId};";
            var productList = new List<Product> { GetSingleTestProduct() };
            _repositoryMock.Setup(r => r.Query<Product>(sql)).Returns(productList);

            var result = _dbController.GetProduct(productId);

            Assert.That(result, Is.EqualTo(productList[0]));
        }

        [Test]
        public void GetProduct_ProvidedInvalidId_ReturnsNull()
        {
            var validProductId = 0;
            var sql = $@"SELECT ProductId, [Name], [Description], [Weight], [Height], [Width], [Length] FROM [Product] WHERE [ProductId] = {validProductId};";
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
                UPDATE Product 
                SET [Name] = @Name,
                    [Description] = @Description,
                    [Weight] = @Weight,
                    [Height] = @Height,
                    [Width] = @Width,
                    [Length] = @Length 
                WHERE ProductId = {product.ProductId}";
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
                UPDATE Product 
                SET [Name] = @Name,
                    [Description] = @Description,
                    [Weight] = @Weight,
                    [Height] = @Height,
                    [Width] = @Width,
                    [Length] = @Length 
                WHERE ProductId = {product.ProductId}";
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
            Assert.That(() => _dbController.UpdateProduct(GetSingleTestProduct(), null), Throws.ArgumentNullException);
        }

        [Test]
        public void DeleteProduct_ProvidedValidProduct_ExecutesCorrectQuery()
        {
            var sql = @"DELETE FROM Product WHERE ProductId = @ProductId";
            var idToDelete = 1;
            var param = new { ProductId = idToDelete };
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            _dbController.DeleteProduct(idToDelete);

            _repositoryMock.Verify(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param))));
        }

        [Test]
        public void DeleteProduct_ProvidedValidProduct_OneRowAffected()
        {
            var sql = @"DELETE FROM Product WHERE ProductId = @ProductId";
            var idToDelete = 1;
            var param = new { ProductId = idToDelete };
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var affectedRows = _dbController.DeleteProduct(idToDelete);

            Assert.That(affectedRows, Is.EqualTo(1));
        }

        [Test]
        public void DeleteProduct_ProvidedInvalidId_ZeroRowsAffected()
        {
            var validProductIdToDelete = 0;
            var sql = @"DELETE FROM Product WHERE ProductId = @ProductId";
            var invalidProductIdToDelete = int.MaxValue;
            var param = new { ProductId = validProductIdToDelete };
            _repositoryMock.Setup(r => r.Execute<Product>(sql, It.Is<object>(o => o.JsonMatches(param)))).Returns(1);

            var result = _dbController.DeleteProduct(invalidProductIdToDelete);

            Assert.That(result, Is.EqualTo(0));
        }

        private static Product GetSingleTestProduct()
        {
            return new Product()
            {
                ProductId = 1,
                Name = "The Box",
                Description = "",
                Height = .5m,
                Weight = .5m,
                Width = .5m,
                Length = .5m
            };
        }

        [TearDown]
        public void OnTearDown()
        {
            _repositoryMock = null;
            _dbController = null;
            using (var conn = new SqlConnection(
                       "Data Source=EPGETBIW03B6;Initial Catalog=ORMFundamentals;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                conn.Query("DELETE FROM Product WHERE Name = 'The Box'");
            }
        }
    }
}