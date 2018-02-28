using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientReceiverResultTests
{
    [TestFixture(Category = "ClientReceiverResult")]
    public class ClientReceiverResultTest
    {

        [Test]
        [Ignore("Missing Implementation and behavior")]
        public void Should_Deserialize_Json_Result_To_Specified_Type()
        {
            var person = new Person()
            {
                Name = "Joaquim",
                Age = 30
            };


            var personJsonString = JsonConvert.SerializeObject(person);

            var json = new Json<Person>();
            Type type = typeof(Person);
            var actualPerson = json.Deserialize(personJsonString);

            //actualPerson = JsonConvert.DeserializeObject(personJsonString, expected);

            Assert.AreEqual(person.Age, actualPerson.Age);

        }

        private class Person
        {
            public Person()
            {
            }

            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class Json<T>
        {
            public Json()
            {
            }

            internal T Deserialize(string personJsonString)
            {
                return JsonConvert.DeserializeObject<T>(personJsonString);
            }
        }
    }
}
