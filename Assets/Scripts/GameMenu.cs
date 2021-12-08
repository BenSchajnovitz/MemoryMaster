using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] Text title;

    public void SetText(string text)
    {
        title.text = text;
    }    
}
