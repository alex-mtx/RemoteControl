using RC.Interfaces.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.Implementation.Storages
{
    public abstract class AbstractStorage<T> : IStorage<T> where T : SimpleStorageObject
    {
        protected readonly IStorageSetup _setup;

        public AbstractStorage(IStorageSetup setup)
        {
            _setup = setup;
        }
        public abstract IEnumerable<T> Contents();

    }
}
