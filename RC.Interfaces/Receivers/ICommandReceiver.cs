namespace RC.Interfaces.Receivers
{
    public interface ICommandReceiver<out T>
    {
        T Receive();
    }
}