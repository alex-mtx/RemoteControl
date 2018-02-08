namespace RC.Interfaces.Appenders
{
    public interface IResultAppender
    {
        void Append<T>(T result);
    }
}
