using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RC.JsonServices;

namespace JsonServicesTests
{
    [TestFixture(Category = "JsonServicesTests")]
    public class JsonTests
    {
        private const string _fileName = "Test.json";

        [SetUp]
        protected void SetUp()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
            if (!File.Exists(filePath))
            {
                var fs = File.Create(filePath);
                fs.Close();
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
        }


        [Test]
        public void Should_Deserialize_Object_From_File_With_AbsolutePath()
        {
            //Arrange
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);

            //Act & Assert
            Assert.DoesNotThrow(() => Json.DeserializeFromFile<object>(filePath));
        }

        [Test]
        public void Should_Deserialize_Object_From_File_With_RelativePath()
        {
            //Arrange & Act & Assert
            Assert.DoesNotThrow(() => Json.DeserializeFromFile<object>(_fileName));
        }
    }
}
