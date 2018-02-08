using System.Collections.Generic;
using RC.Implementation.Appenders;
using RC.Infrastructure.Setup;
using RC.Interfaces.Appenders;

namespace RC.Infrastructure
{
    public class ResultAppenderManager
    {
        private static ResultAppenderManager _instance = null;
        private static readonly object _lock = new object();
        private static IEnumerable<IOutput> _outputs;

        public IResultAppender ResultAppender { get; private set; }
        private ResultAppenderManager()
        {
            _outputs = AppenderOutputSetup.RegisteredOutputs();
        }

        public static ResultAppenderManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null)
                        return _instance;
                    _instance = new ResultAppenderManager();
                    _instance.ResultAppender = new GeneralResultAppender(_outputs);
                    return _instance;
                }
            }
        }
    }
}
