using RC.Interfaces.Appenders;
using System.Diagnostics;

namespace RC.Implementation.Appenders
{
    public class DebugConsoleOutput : IOutput
    {
        public void Send<T>(T data)
        {
            Debug.WriteLine(JsonServices.Json.Serialize(data));
        }
    }
}
