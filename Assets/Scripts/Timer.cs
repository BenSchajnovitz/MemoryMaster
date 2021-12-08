using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float secondsCountdown = 30f;

    float currentTime;
    bool isTimerActive = false;
    Text timeText;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject textObj = GameObject.FindWithTag("TimeLeft");

        if(textObj != null)
            timeText = textObj.GetComponent<Text>();
        else
            Debug.LogError("Couldnt find TimeLeft object");
    }

    // Update is called once per frame
    void Update()
    {
        if(isTimerActive)
        {
            if(currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                timeText.text = Mathf.CeilToInt(currentTime).ToString(); // Because we want to view the time in integers
            }
            else
                isTimerActive = false;
        }
    }
    public void ResumeTimer()
    {
        isTimerActive = true;
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }

    public void RestartTimer()
    {
        currentTime = secondsCountdown;
        timeText.text = Mathf.Ceil(currentTime).ToString();
        isTimerActive = true;
    }

    public float GetTime()
    {
        return currentTime;
    }

    public void SetTime(float time)
    {
        currentTime = time;
    }
}
