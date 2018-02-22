using System;
using System.Collections.Generic;
using System.Configuration;
using RC.Implementation.Storages;
using RC.Interfaces.Infrastructure;
using RC.Interfaces.Storages;
using System.Linq;
using System.IO;
using RC.JsonServices;

namespace RC.Infrastructure.Setup
{
    public class JsonStorageSettingsStrategy : GenericStorageSettingsStrategy
    {
        private static string _jsonFileName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonFileName">Can be relative or absolute path. if relative it takes the currentDirectory of app</param>
        public JsonStorageSettingsStrategy(string jsonFileName)
        {
            _jsonFileName = jsonFileName;
        }

        public override List<BasicStorageSetup> BuildSetups()
        {
            return _setups ?? (_setups = JsonServices.Json.DeserializeFromFile<List<BasicStorageSetup>>(_jsonFileName));
        }
    }
}