using RC.Implementation.Storages;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;
using RC.Interfaces.Receivers;

namespace RC.Implementation.Receivers
{
    public class BaseStorageListingCmdReceiver : ICmdReceiver<ICmd>
    {
        private readonly IFactory<ICmd, StorageListingCmdType> _factory;

        public BaseStorageListingCmdReceiver(IFactory<ICmd, StorageListingCmdType> factory)
        {
            _factory = factory;
        }

        public virtual ICmd Receive()
        {
            return _factory.Create(StorageListingCmdType.LocalFileSystem);
        }

       
    }
}