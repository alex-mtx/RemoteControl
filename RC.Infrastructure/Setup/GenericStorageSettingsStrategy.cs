using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.Implementation.Storages;
using RC.Interfaces.Infrastructure;
using RC.Interfaces.Storages;

namespace RC.Infrastructure.Setup
{
    public abstract class GenericStorageSettingsStrategy : IStorageSettingsStrategy
    {
        protected static List<BasicStorageSetup> _setups;

        public IStorageSetup GetSetup(Uri uri)
        {
            return BuildSetups().Single(x => x.Uri == uri);
        }

        public abstract List<BasicStorageSetup> BuildSetups();

    }
}
