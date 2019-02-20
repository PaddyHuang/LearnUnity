using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour {

    private Transform tipsPanel;
    private Button button;
    private Button exitBtn;

    private Vector3 targetPosition;
    private Vector3 originPosition;

    private Vector3 velocity = Vector3.zero;

    private bool moved;

	// Use this for initialization
	void Start () {
        tipsPanel = transform.Find("TipsPanel");
        button = transform.Find("Button").GetComponent<Button>();
        exitBtn = tipsPanel.Find("ExitBtn").GetComponent<Button>();

        originPosition = tipsPanel.position;
        targetPosition = tipsPanel.position - new Vector3(55, 0, 0);

        button.onClick.AddListener(delegate 
        {            
            moved = true;
        });

        exitBtn.onClick.AddListener(delegate
        {
            moved = false;
        });
    }	
    	
	void LateUpdate ()
    {
        if (moved)
        {
            tipsPanel.position = Vector3.SmoothDamp(tipsPanel.position, targetPosition, ref velocity, 1);
        }
        if (!moved)
        {
            tipsPanel.position = Vector3.SmoothDamp(tipsPanel.position, originPosition, ref velocity, 1);
        }
    }
}
