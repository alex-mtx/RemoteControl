namespace RC.Interfaces.Appenders
{
    public interface IOutput
    {
        void Send<T>(T data);
    }
}
