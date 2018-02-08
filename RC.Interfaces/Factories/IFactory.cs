namespace RC.Interfaces.Factories
{
    public interface IFactory<out TReturn, TType>
    {
        TReturn Create(TType type);
    }
}