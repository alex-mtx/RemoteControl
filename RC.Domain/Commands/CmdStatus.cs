namespace RC.Domain.Commands
{
    public enum CmdStatus
    {
        Unknown = 0,
        AwaitingForExecution = 1,
        Executed = 2,
        Malformed = 3,
        ResultedInError = 4
    }
}
