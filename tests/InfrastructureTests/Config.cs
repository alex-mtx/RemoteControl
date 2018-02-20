using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RC.Implementation.Storages;
using RC.JsonServices;

namespace InfrastructureTests
{
    [SetUpFixture]
    public class Config
    {
        [OneTimeSetUpAttribute]
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
                new BasicStorageSetup(AppContext.BaseDirectory, "Uk", true),
                new BasicStorageSetup(Path.GetDirectoryName(Path.GetDirectoryName(AppContext.BaseDirectory)), "Es", true),
                new BasicStorageSetup(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppContext.BaseDirectory))), "Ftp", true)
            };

            var jsonBody = Json.Serialize(defaultBasicStorageList);

            File.WriteAllText(fullPath, jsonBody);
        }
    }
}
