abstract class SavingMethod
{
    protected string methodName;
    public abstract bool SaveState(StateData stateData);
    public abstract StateData LoadState();
    public abstract void ClearData();
    public string GetMethodName()
    {
        return methodName;
    }
}