using NUnit.Framework;
using RC.Data;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using System;

namespace DataTests
{
    [TestFixture]
    public class TableNameMappingTests
    {
        [TestCase(typeof(CmdParametersSet))]
        [TestCase(typeof(StorageCmdParamSet))]
        public void Should_Map_CmdParametersSet_And_Its_Derived_Classes_To_Table_Name(Type type)
        {
            var expectedName = "[CmdParametersSets]";
            var actualName = TableNameMapping.TableName(type);

            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void Should_Throws_ArgumentException_When_Type_Is_Not_Mapped()
        {
            Assert.Throws<ArgumentException>(() => TableNameMapping.TableName(typeof(Exception)));
        }
    }
}
