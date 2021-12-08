using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] Sprite backImgSprite;
    [SerializeField] AudioClip cardFlipSound, explosionSound;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] Image faceImg, frameImg;

    GameController gameControl;
    Sprite face;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.FindWithTag("GameBoard").GetComponent<GameController>();
        audioSource = GameObject.FindWithTag("SoundHandler").GetComponent<AudioSource>();

        if(backImgSprite != null)
            faceImg.sprite = backImgSprite;
        else
            Debug.LogError("No back sprite has been assigned");

        if(gameControl == null)
            Debug.LogError("Couldnt find gameboard");
    }
    
    public void SetFace(Sprite newFace)
    {
        face = newFace;
    }

    public void Explode()
    {
        explosionEffect.Play();

        if(explosionSound != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(explosionSound);

        SetVisibilty(false);
    }

    public void SetVisibilty(bool isVisible)
    {
        if(faceImg != null )
            faceImg.enabled = isVisible;
        if(frameImg != null )
            frameImg.enabled = isVisible;
    }

    public Sprite GetFace()
    {
        return face;
    }

    public void FaceUp()
    {
        faceImg.sprite = face;
    }

    public void FaceDown()
    {
        if(faceImg != null )
            faceImg.sprite = backImgSprite;
    }

    public bool isFacedUp()
    {
        return (faceImg.sprite != backImgSprite);
    }

    public void Flip()
    {
        if(cardFlipSound != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(cardFlipSound);

        if(isFacedUp())
            faceImg.sprite = backImgSprite;
        else
            faceImg.sprite = face;
    }
}
