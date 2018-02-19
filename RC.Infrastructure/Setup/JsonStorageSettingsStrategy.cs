using System;
using System.Collections.Generic;
using System.Configuration;
using RC.Implementation.Storages;
using RC.Interfaces.Setup;
using RC.Interfaces.Storages;
using System.Linq;
using System.IO;

namespace RC.Infrastructure.Setup
{
    public class JsonStorageSettingsStrategy : IStorageSettingsStrategy
    {
        private static string _jsonFile;
        private static List<BasicStorageSetup> _setups;
        private static JsonStorageSettingsStrategy _instance = new JsonStorageSettingsStrategy();

        private JsonStorageSettingsStrategy()
        {

        }
        static JsonStorageSettingsStrategy()
        {
            var path = ConfigurationManager.AppSettings["ConfigurationJsonStorageSettingsStrategy"];
            FullPath(path);
            DeserializeAllSetups();
        }

        public static JsonStorageSettingsStrategy Instance { get { return _instance; } }
        public IStorageSetup GetSetup(Uri uri)
        {
           return _setups.Where(x => x.Uri == uri && x.Active).Single();
        }

        private static void DeserializeAllSetups()
        {
            _setups = JsonServices.Json.DeserializeFromFile<List<BasicStorageSetup>>(_jsonFile);
        }

        private static void FullPath(string fileName)
        {
            _jsonFile = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)).LocalPath;
        }
    }
}