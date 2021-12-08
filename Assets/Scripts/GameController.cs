using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject cardObj;
    [SerializeField] List<Sprite> cardsfaces;
    [SerializeField] GameMenu gameMenu;
    [SerializeField] Button saveBtn, loadBtn;
    [SerializeField] float popupDelay = 1f; // Time before ending popup appears
    [SerializeField] float explosionDelay = 1f; // Time before card is explode (to show the player the pair he found)
    [SerializeField] float mistakeDelay = 0.5f; // Time to pause the game before flipping back the cards after a mistake

    bool isButtonsDisabled = false, isGameActive = false;
    
    Timer timer;
    Text scoreText;
    SoundHandler soundHandler;
    StateData stateData = new StateData();
    
    Dictionary<string, CardController> cards = new Dictionary<string, CardController>();

    // Start is called before the first frame update
    void Start()
    {
        if(cardsfaces.Count <= 0 || cardsfaces.Count > 8) // This game is designed to have maximum amount of 8 pairs, any additional cards wont be viewed currectly
        {
            Debug.LogError("Invalid cards faces amount.");
            return;
        }

        scoreText = GameObject.FindWithTag("Score").GetComponent<Text>();
        soundHandler = GameObject.FindWithTag("SoundHandler").GetComponent<SoundHandler>();
        timer = GetComponent<Timer>();
        ApplyListenersToUIButtons();
        InitiateCards();
        RestartGame();
    }

    void ApplyListenersToUIButtons()
    {
        soundHandler.GetComponent<Button>().onClick.AddListener(()=> ToggleSound());
        saveBtn.onClick.AddListener(()=> SaveState());
        loadBtn.onClick.AddListener(()=> LoadState());
        gameMenu.GetComponentInChildren<Button>().onClick.AddListener(()=> RestartGame());
    }


    // The cards must contain exactly one pair of each given card image and shuffled.
    // This method creates a temporary array of indexes that represents a card image, then duplicate 
    // the array in order to make sure we have a pair for each image, applying the image to the actual 
    // card randomly and then remove it from the array.
    void ShuffleCardsFaces()
    {
        stateData.cardsFacesIndexes.Clear();
        System.Random rnd = new System.Random();
        List<int> facesIndexes = Enumerable.Range(0, cardsfaces.Count).ToList();
        facesIndexes.AddRange(facesIndexes);

        for(int i=0; i < cards.Count; i++) // Didnt use foreach because the order is important and this is a dictionary
        {
            int faceIndex = facesIndexes[rnd.Next(0, facesIndexes.Count)];
            cards["Card"+i].SetVisibilty(true);
            cards["Card"+i].SetFace(cardsfaces[faceIndex]);
            cards["Card"+i].FaceDown();
            stateData.cardsFacesIndexes.Add(faceIndex);
            facesIndexes.Remove(faceIndex);
        }
    }

    void InitiateCards() 
    {
        for(int i=0; i < cardsfaces.Count*2; i++)  // x2 for the pairs
        {
            GameObject card = Instantiate(cardObj);
            card.name = "Card" + i;
            card.transform.SetParent(gameObject.transform, false);
            CardController cardControl = card.GetComponent<CardController>();
            Button cardButton = card.GetComponent<Button>();
            cardButton.onClick.AddListener(() => onCardClick(cardControl));
            cards.Add(card.name, cardControl);
        }

        StateManager.Instance.SetCardsAmount(cards.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameActive)
        {
            stateData.timeLeft = timer.GetTime();
            if(stateData.timeLeft <= 0)
            {
                GameOver(false);
            }
        }
    }

    IEnumerator FoundPair(string firstCard, string secondCard)
    {
        yield return new WaitForSeconds(explosionDelay);
        cards[firstCard].Explode();
        cards[secondCard].Explode();
    }

    IEnumerator ResumeAfterMistake(string secondCompareCard)
    {
        yield return new WaitForSeconds(mistakeDelay);

        cards[stateData.firstCompareCard].Flip();
        cards[secondCompareCard].Flip();
        stateData.firstCompareCard = null;
        timer.ResumeTimer();
        isButtonsDisabled = false;
    }

    void ShowEndingScreen()
    {
        gameMenu.gameObject.SetActive(true);
    }

    void GameOver(bool hasWon)
    {
        timer.StopTimer();
        isGameActive = false;
        isButtonsDisabled = true;

        if(hasWon)
        {
            gameMenu.SetText("You won");
            stateData.score += Mathf.CeilToInt(timer.GetTime());  // User get points as the number of seconds remain everytime he wins
        }
        else
            gameMenu.SetText("Game Over");
        
        Invoke("ShowEndingScreen", popupDelay);
    }

    void ToggleSound()
    {
        stateData.isMuted = !stateData.isMuted;
        soundHandler.ToggleSound(stateData.isMuted);
    }
    
    public void RestartGame()
    {
        gameMenu.gameObject.SetActive(false);
        ShuffleCardsFaces();
        stateData.foundCards.Clear();
        stateData.firstCompareCard = null;
        timer.RestartTimer();
        scoreText.text = stateData.score.ToString();
        isButtonsDisabled = false;
        isGameActive = true;
    }

    public void SaveState()
    {
        if(StateManager.Instance.isProcessingState()) { return; }

        isGameActive = false;
        isButtonsDisabled = true;

        StateManager.Instance.SaveState(stateData);

        isGameActive = true;
        isButtonsDisabled = false;
    }

    public void LoadState()
    {
        if(StateManager.Instance.isProcessingState()) { return; }

        isGameActive = false;
        isButtonsDisabled = true;
    
        stateData = StateManager.Instance.LoadState();

        if(stateData == null) { return; }

        for(int i=0; i < stateData.cardsFacesIndexes.Count; i++)
        {
            int faceIndex = stateData.cardsFacesIndexes[i];
            cards["Card"+i].SetVisibilty(true);
            cards["Card"+i].SetFace(cardsfaces[faceIndex]);
            cards["Card"+i].FaceDown(); // foundCards array and firstCompareCard will flip back the right cards. 
        }

        foreach(string cardName in stateData.foundCards)
            cards[cardName].SetVisibilty(false);
        
        if(stateData.firstCompareCard != null)
            cards[stateData.firstCompareCard].FaceUp();

        timer.SetTime(stateData.timeLeft);
        scoreText.text = stateData.score.ToString();
        soundHandler.ToggleSound(stateData.isMuted);


        isGameActive = true;
        isButtonsDisabled = false;
    }

    public void onCardClick(CardController card)
    {
        if(isButtonsDisabled) { return; }

        if(!card.isFacedUp())
        {
            if(stateData.firstCompareCard != null)
            {
                card.Flip();
                if(cards[stateData.firstCompareCard].GetFace() == card.GetFace())
                {
                    StartCoroutine(FoundPair(stateData.firstCompareCard, card.name));

                    stateData.foundCards.Add(stateData.firstCompareCard);
                    stateData.foundCards.Add(card.name);
                    stateData.firstCompareCard = null;

                    if(stateData.foundCards.Count == cards.Count)
                        GameOver(true);
                }
                else
                {
                    isButtonsDisabled = true;
                    timer.StopTimer();
                    StartCoroutine(ResumeAfterMistake(card.name));
                }
            }
            else
            {
                stateData.firstCompareCard = card.name;
                card.Flip();
            }
        }
        
    }
}
