using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class StateManager
{
    bool isProcessing = false;
    List<SavingMethod> savingMethods = new List<SavingMethod>(); // I choose factory pattern to make it easier to add more saving methods.

    int selectedMethod, expectedCardsAmount;
    static StateManager _instance;
    public static StateManager Instance { get {  // I choose singleton so it wont have more than one instance and will be accessible everywhere.

        if(_instance == null)
            _instance = new StateManager();

        return _instance;
    }}
    
    private StateManager()
    {
        savingMethods.Add(new PlayerPrefMethod());
        savingMethods.Add(new InMemoryMethod());

        selectedMethod = PlayerPrefs.GetInt("selectedMethod", 0);  // I choose to save and load the selection option from the PlayerPrefs because it is persisted.
    }

    public int GetCurrentMethod()
    {
        return selectedMethod;
    }

    public List<string> GetMethodsName()  // Names to be display when selecting saving system in the main menu
    {
        List<string> names = new List<string>();

        foreach(SavingMethod method in savingMethods)
        {
            names.Add(method.GetMethodName());
        }

        return names;
    }


    public void SelectSavingMethod(int method)
    {
        if(method != selectedMethod && method >= 0 && method < savingMethods.Count)
        {
            savingMethods[selectedMethod].ClearData();
            selectedMethod = method;
            PlayerPrefs.SetInt("selectedMethod", selectedMethod);  // I choose to save and load the selection option from the PlayerPrefs because it is persisted.
        }
    }

    public bool isProcessingState()
    {
        return isProcessing;
    }

    public void SetCardsAmount(int cardsAmount)
    {
        expectedCardsAmount = cardsAmount;
    }
    
    public void SaveState(StateData stateData)
    {
        isProcessing = true;

        stateData.cardsFacesIndexes_str = String.Join('-', stateData.cardsFacesIndexes);
        stateData.foundCards_str = String.Join("-", stateData.foundCards);

        savingMethods[selectedMethod].SaveState(stateData);

        isProcessing = false;
    }

    public StateData LoadState()
    {    
        isProcessing = true;

        StateData stateData = savingMethods[selectedMethod].LoadState();

        if(!isValidAfterLoad(stateData))
            stateData = null;

        isProcessing = false;

        return stateData;
    }


    bool isValidAfterLoad(StateData stateData) // Validating data after loading state and arranging it
    {
        List<string> cardsFacesData = new List<string>((stateData.cardsFacesIndexes_str.Split('-')));

        if(!String.IsNullOrEmpty(cardsFacesData[0]) && cardsFacesData.Count == expectedCardsAmount)
        {
            try
            {
                stateData.cardsFacesIndexes = cardsFacesData.Select(int.Parse).ToList();
            }
            catch(Exception e)
            {
                Debug.Log(e);
                return false;
            }


            if(!String.IsNullOrEmpty(stateData.foundCards_str))
            {
                stateData.foundCards =  new List<string>(stateData.foundCards_str.Split('-'));
            }
            else
                stateData.foundCards.Clear();   


            if(String.IsNullOrEmpty(stateData.firstCompareCard))
                stateData.firstCompareCard = null;
        }
        else
        {
            Debug.LogError("The cards amount has changed");
            savingMethods[selectedMethod].ClearData();
            return false;
        }

        return true;
    }

}
