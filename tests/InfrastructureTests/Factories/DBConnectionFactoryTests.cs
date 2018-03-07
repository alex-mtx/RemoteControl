using NUnit.Framework;
using RC.Data;

namespace InfrastructureTests.Factories
{
    [TestFixture(Category = "Factories",TestOf = typeof(DBConnectionFactory))]
    public class DBConnectionFactoryTests
    {
        [Test]
        public void CreateDbConnection_Should_Create_A_Real_Connection()
        {
            var factory = new DBConnectionFactory("System.Data.SQlClient", @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;");

            Assert.DoesNotThrow(() => { 
                using (var connection = factory.CreateDbConnection())
                {
                    connection.Open();
                }
            });
        }
    }
}
