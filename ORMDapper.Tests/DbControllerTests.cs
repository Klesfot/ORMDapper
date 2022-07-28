using Moq;
using ORMDapper.Data;
using ORMDapper.Model;
#pragma warning disable CS8618

namespace ORMDapper.Tests
{
    [TestFixture]
    public class Tests
    {
        private IDbController _dbController;
        private Mock<IDapperRepository> _repositoryMock;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IDapperRepository>();
            _dbController = new DbController(_repositoryMock.Object);
        }
    }
}