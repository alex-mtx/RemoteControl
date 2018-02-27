using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RC.Implementation.Storages;
using RC.JsonServices;

namespace IntegrationTests
{
    [SetUpFixture]
    public class Config
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, "jsonsettings.json");
            if (!File.Exists(fullPath))
            {
                var fs = File.Create(fullPath);
                fs.Close();
            }

            List<BasicStorageSetup> defaultBasicStorageList = new List<BasicStorageSetup>()
            {
                new BasicStorageSetup(AppDomain.CurrentDomain.BaseDirectory, "Uk", true),
                new BasicStorageSetup(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)), "Es", true),
                new BasicStorageSetup(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))), "Ftp", true)
            };

            var jsonBody = Json.Serialize(defaultBasicStorageList);

            File.WriteAllText(fullPath, jsonBody);
        }
    }
}
