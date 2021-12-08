using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] Sprite soundOn, soundOff;

    Image icon;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        icon = GetComponent<Image>();
    }

    public void ToggleSound(bool isMuted)
    {
        if(isMuted)
            icon.sprite = soundOff;
        else
            icon.sprite = soundOn;
            
        audioSource.mute = isMuted;
    }

}
