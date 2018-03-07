using RC.Domain.Storages;
using RC.Interfaces.Storages;
using System.Collections.Generic;

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

        public IStorageSetup Info()
        {
            return _setup;
        }
    }
}
