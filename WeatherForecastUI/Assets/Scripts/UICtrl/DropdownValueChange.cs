using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownValueChange : MonoBehaviour {

    public Dropdown dropdown;
    public InputField inputField;
    public Text label;

    private bool isDropdownFirstEnable = true;

    // Use this for initialization
    void Start () {
        label.text = string.Empty;
        inputField.text = string.Empty;
        dropdown.enabled = false;
        
        EventTriggerListener.Get(dropdown.gameObject).onClick = OnDropdownClick;        
        EventTriggerListener.Get(inputField.gameObject).onClick = OnInputFieldClick;        
    }
	
    private void OnDropdownClick(GameObject gameObject)
    {
        inputField.text = string.Empty;
        inputField.placeholder.GetComponent<Text>().text = string.Empty;

        if (isDropdownFirstEnable)
        {
            dropdown.enabled = true;
            isDropdownFirstEnable = false;            
        }       
    }

    private void OnInputFieldClick(GameObject gameObject)
    {
        label.text = string.Empty;
        inputField.placeholder.GetComponent<Text>().text = string.Empty;
    }   
}
