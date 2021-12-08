using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class StateData
{
    public float timeLeft;
    public string firstCompareCard; // The first faced up card name when trying to match his pair 
    public List<string> foundCards;
    public string foundCards_str;
    public int score;
    public bool isMuted;
    public List<int> cardsFacesIndexes; // Contains data for each card's face image.
    public string cardsFacesIndexes_str; // Contains data for each card's face image.

    public StateData()
    {
        foundCards = new List<string>();
        foundCards_str = null; 
        cardsFacesIndexes = new List<int>();
        cardsFacesIndexes_str = null;
        firstCompareCard = null;
        timeLeft = 0;
        score = 0;
        isMuted = false;
    }
}
