using System.Collections.Generic;
class InMemoryMethod: SavingMethod
{
    private StateData stateData;

    public InMemoryMethod()
    {
        methodName = "In Memory";
    }

    StateData GetClonedData(StateData data)
    {
        StateData stateData = new StateData();

        stateData.foundCards = new List<string>(data.foundCards);
        stateData.foundCards_str = data.foundCards_str; 
        stateData.cardsFacesIndexes = new List<int>(data.cardsFacesIndexes);
        stateData.cardsFacesIndexes_str = data.cardsFacesIndexes_str;
        stateData.firstCompareCard = data.firstCompareCard;
        stateData.timeLeft = data.timeLeft;
        stateData.score = data.score;
        stateData.isMuted = data.isMuted;

        return stateData;
    }
    
    public override bool SaveState(StateData data)
    {
        stateData = GetClonedData(data);
        return true;
    }
    public override StateData LoadState()
    {
        return GetClonedData(stateData);
    }

    public override void ClearData()
    {
        stateData = null;
    }
}