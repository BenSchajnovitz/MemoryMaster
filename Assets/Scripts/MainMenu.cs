using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

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
        SceneManager.LoadScene(1);
        //EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/GameScene.unity", new LoadSceneParameters());
    }
}
