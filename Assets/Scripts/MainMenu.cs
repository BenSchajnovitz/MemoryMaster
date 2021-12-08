using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    ToggleGroup storageGroup;

    void Start() {
        storageGroup = GetComponentInChildren<ToggleGroup>();    
    }

    public void StartGame()
    {
        int savingMethodSelection = int.Parse(storageGroup.GetFirstActiveToggle().name);
        StateManager.Instance.SelectSavingMethod(savingMethodSelection);
        SceneManager.LoadScene("GameScene");
    }
}
