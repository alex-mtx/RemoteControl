using NUnit.Framework;
using RC.Interfaces.Commands;
using RC.Interfaces.Receivers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationTests.Receivers
{
    [TestFixture]
    public class CmdReceiverTests
    {
        [Test]
        public void TestMethod()
        {
            var cmdMock = new Moq.Mock<ICmd>();
            cmdMock.Setup(x => x.Run()).Callback(() => { Debug.WriteLine("Run"); });

            var receiver = new DummyCmdReceiver(cmdMock.Object);
            receiver.Observable.CollectionChanged += Observable_CollectionChanged;
        }

        private void Observable_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void RunCommand(ICmd cmd)
        {

        }

        private class DummyCmdReceiver : ICmdReceiver<ICmd>
        {
            private ICmd _cmd;

            public DummyCmdReceiver(ICmd cmd)
            {
                _cmd = cmd;
            }

            public ObservableCollection<ICmd> Observable { get; private set; }
            public ICmd Receive()
            {
                return _cmd;
            }

            public void ReceiveAndPublish()
            {
                //... fetch the cmd from anywhere....
                Observable.Add(_cmd);
            }
        }
    }
}
