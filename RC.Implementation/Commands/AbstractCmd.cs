using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;
using System.Collections.Generic;

namespace RC.Implementation.Commands
{
    public abstract class AbstractCmd<TResult>: ICmd
    {
        protected readonly IResultAppender _resultAppender;
        private IDictionary<string, string> _params = new Dictionary<string, string>();
        public virtual IDictionary<string, string> Params
        {
            get { return _params; }
            set { _params = value; }
        }

        public AbstractCmd(IResultAppender resultAppender) => _resultAppender = resultAppender;
       
        protected virtual void AppendResult(TResult result)
        {
            _resultAppender.Append(result);
        }

        public void Run()
        {
            var result = RunCommand();
            AppendResult(result);
        }

        protected abstract TResult RunCommand();
    }
}
