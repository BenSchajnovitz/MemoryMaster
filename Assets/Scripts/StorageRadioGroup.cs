using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StorageRadioGroup : MonoBehaviour
{
    [SerializeField] Toggle toggleObj;
    List<Toggle> toggles = new List<Toggle>();
    ToggleGroup storageGroup;
    int selectedMethod;

    // Start is called before the first frame update
    void Start()
    {
        storageGroup = GetComponent<ToggleGroup>();
        InitiateRadioButtons();
    }

    void InitiateRadioButtons()
    {
        List<string> methodsName = StateManager.Instance.GetMethodsName();

        for(int i=0; i < methodsName.Count; i++)
        {
            Toggle toggle = Instantiate(toggleObj);
            toggle.group = storageGroup;
            toggle.name = ""+i;
            toggle.GetComponentInChildren<Text>().text = methodsName[i];
            toggles.Add(toggle);
            toggle.transform.SetParent(gameObject.transform, false);
        }

        selectedMethod = StateManager.Instance.GetCurrentMethod();
        toggles[selectedMethod].isOn = true;
    }
}
