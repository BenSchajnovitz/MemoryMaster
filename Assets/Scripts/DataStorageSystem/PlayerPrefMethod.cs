using UnityEngine;
using System;

class PlayerPrefMethod: SavingMethod
{
    public PlayerPrefMethod()
    {
        methodName = "PlayerPrefs";
    }

    public override bool SaveState(StateData stateData)
    {
        try
        {
            PlayerPrefs.SetFloat("timeLeft", stateData.timeLeft);
            PlayerPrefs.SetString("firstCompareCard",  stateData.firstCompareCard);
            PlayerPrefs.SetString("foundCards", stateData.foundCards_str);
            PlayerPrefs.SetInt("Score", stateData.score);
            PlayerPrefs.SetString("CardsFacesIndexes", stateData.cardsFacesIndexes_str);
            PlayerPrefs.SetString("isMuted", stateData.isMuted.ToString());
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return false;
        }

        return true;
    }
    public override StateData LoadState()
    {
        StateData data = new StateData();

        try
        {
            data.cardsFacesIndexes_str = PlayerPrefs.GetString("CardsFacesIndexes", "");
            data.score = PlayerPrefs.GetInt("Score", 0);
            data.firstCompareCard = PlayerPrefs.GetString("firstCompareCard", null); 
            data.timeLeft = PlayerPrefs.GetFloat("timeLeft", 0);     
            data.foundCards_str =  PlayerPrefs.GetString("foundCards", null);
            data.isMuted = (PlayerPrefs.GetString("isMuted", "False") == "True");  
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return null;
        }

        return data;
    }

    public override void ClearData()
    {
        PlayerPrefs.DeleteKey("timeLeft");
        PlayerPrefs.DeleteKey("firstCompareCard");
        PlayerPrefs.DeleteKey("foundCards");
        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.DeleteKey("CardsFacesIndexes");
        PlayerPrefs.DeleteKey("isMuted");
    }
}