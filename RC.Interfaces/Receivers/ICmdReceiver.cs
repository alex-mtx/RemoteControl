namespace RC.Interfaces.Receivers
{
    public interface ICmdReceiver<out T>
    {
        T Receive();
    }
}