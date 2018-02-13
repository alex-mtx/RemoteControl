using System;
using System.Data;
using NUnit.Framework;
using RC.SQLiteServices;



namespace SQLiteServicesTests
{
    [TestFixture(TestOf =typeof(SQLiteConnectionFactory))]
    public class SQLiteConnectionFactoryTests
    {
        [Test]
        public void CreateConnection_When_Connection_String_Is_Correct_Should_Create_A_Connection()
        {
            var factory = new SQLiteConnectionFactory("Data Source=|DataDirectory|demo1.db;Version=3");
            IDbConnection conn = null;
            Assert.DoesNotThrow(()=> conn = factory.CreateDbConnection());
            Assert.DoesNotThrow(() => conn.Open());
            Assert.DoesNotThrow(() => conn.Close());

        }
    }
}
