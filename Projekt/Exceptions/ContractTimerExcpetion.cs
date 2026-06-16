namespace Projekt.Exceptions;

public class ContractTimerExcpetion : Exception
{
    public ContractTimerExcpetion()
    {
    }

    public ContractTimerExcpetion(string? message) : base(message)
    {
    }
}