using UnityEngine;
using UnityEngine.UI;

public class SelectTemplate : MonoBehaviour
{
    public Button SelectButton;
    public Text StepText;

    private Text selectText;

    //返回属性
    public string GetInputInfo => selectText?.text;

    void Start()
    {
        selectText = SelectButton.GetComponentInChildren<Text>();
    }
}