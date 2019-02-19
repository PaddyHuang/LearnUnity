using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

    AndroidJavaClass unity;
    AndroidJavaObject currentActivity;

    private Button singlePointShowBtn;
    private Button multiPointsShowBtn;
    private Button sceneryArShowBtn;
    private Button buildingArShowBtn;
    private Button exploreArShowBtn;

    private void Awake()
    {
        transform.Find("MainActivityInit").GetComponent<Button>().onClick.AddListener(InitMainActivity);
        singlePointShowBtn = transform.Find("SinglePointShow").GetComponent<Button>();
        multiPointsShowBtn = transform.Find("MultiPointsShow").GetComponent<Button>();
        sceneryArShowBtn = transform.Find("SceneryArShow").GetComponent<Button>();
        buildingArShowBtn = transform.Find("BuildingArShow").GetComponent<Button>();
        exploreArShowBtn = transform.Find("ExploreArShow").GetComponent<Button>();
    }

    // Use this for initialization
    void Start () {
        unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        singlePointShowBtn.onClick.AddListener(delegate () { OnClick(singlePointShowBtn.name); });
        multiPointsShowBtn.onClick.AddListener(delegate () { OnClick(multiPointsShowBtn.name); });
        sceneryArShowBtn.onClick.AddListener(delegate () { OnClick(sceneryArShowBtn.name); });
        buildingArShowBtn.onClick.AddListener(delegate () { OnClick(buildingArShowBtn.name); });
        exploreArShowBtn.onClick.AddListener(delegate () { OnClick(exploreArShowBtn.name); });
    }

    public void InitMainActivity()
    {       
        currentActivity.Call("init");
    }

    public void OnClick(string id)
    {
        switch (id)
        {
            case ConstantValue.singlePointShow:
                currentActivity.Call("onClick", ConstantValue.singlePointShow);
                break;
            case ConstantValue.multiPointsShow:
                currentActivity.Call("onClick", ConstantValue.multiPointsShow);
                break;
            case ConstantValue.sceneryArShow:
                currentActivity.Call("onClick", ConstantValue.sceneryArShow);
                break;
            case ConstantValue.buildingArShow:
                currentActivity.Call("onClick", ConstantValue.buildingArShow);
                break;
            case ConstantValue.exploreArShow:
                currentActivity.Call("onClick", ConstantValue.exploreArShow);
                break;
            default:
                break;
        }
    }

}
