using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SaveAndLoadTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void SaveAndLoad()
    {
        StateData stateData = new StateData();
        stateData.cardsFacesIndexes = new List<int>(){0, 3, 5, 6, 2, 1, 3, 7, 1, 4, 0, 5, 2, 4, 6, 7};
        stateData.firstCompareCard = "Card9";
        stateData.foundCards = new List<string>(){"Card6", "Card2", "Card4", "Card8"};
        stateData.score = 3;
        stateData.timeLeft = 14.567f;
        stateData.isMuted = true;

        StateData oldStateData = new StateData();
        oldStateData.cardsFacesIndexes = stateData.cardsFacesIndexes;
        oldStateData.firstCompareCard = stateData.firstCompareCard;
        oldStateData.foundCards = stateData.foundCards;
        oldStateData.score = stateData.score;
        oldStateData.timeLeft = stateData.timeLeft;
        oldStateData.isMuted = stateData.isMuted;
        
        StateManager.Instance.SetCardsAmount(stateData.cardsFacesIndexes.Count);

        StateManager.Instance.SaveState(stateData);

        stateData = StateManager.Instance.LoadState();

        
        Assert.AreEqual(oldStateData.cardsFacesIndexes, stateData.cardsFacesIndexes);
        Assert.AreEqual(oldStateData.firstCompareCard, stateData.firstCompareCard);
        Assert.AreEqual(oldStateData.foundCards, stateData.foundCards);
        Assert.AreEqual(oldStateData.score, stateData.score);
        Assert.AreEqual(oldStateData.timeLeft, stateData.timeLeft);
        Assert.AreEqual(oldStateData.isMuted, stateData.isMuted);
    }

}
