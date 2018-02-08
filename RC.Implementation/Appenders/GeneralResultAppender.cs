using RC.Interfaces.Appenders;
using System.Collections.Generic;

namespace RC.Implementation.Appenders
{
    public class GeneralResultAppender : IResultAppender
    {
        private IEnumerable<IOutput> _outputs;
        public GeneralResultAppender(IEnumerable<IOutput> outputs) => _outputs = outputs;

        public virtual void Append<T>(T result)
        {
            foreach (var output in _outputs)
            {
                output.Send(result);
            } 
        }
    }
}
