using System;
using UnityEngine;
using UnityEngine.UI;

public enum PracMainSwitch
{
    Practice,
    Main
}

public abstract class CreatorBase : MonoBehaviour
{
    [SerializeField] protected Transform togglesParent;
    [SerializeField] protected Transform togglePrefab;
    [SerializeField] protected ToggleGroup toggleGroup;

    private event Action<PracMainSwitch> OnSwichPanelHandler;
    public event Action<PracMainSwitch> OnSwichPanel
    {
        add
        {
            OnSwichPanelHandler -= value;
            OnSwichPanelHandler += value;
        }
        remove { OnSwichPanelHandler -= value; }
    }

    /// <summary> 
    /// 申请一个toggle </summary>
    protected void ApplyForOneToggle(GameObject fo, string toggleName, Action<bool> actFunc = null, bool isOn = false)
    {
        var to = Instantiate(togglePrefab.gameObject);
        to.transform.SetParent(togglesParent);
        to.transform.localPosition = to.transform.localEulerAngles = Vector3.zero;
        to.transform.localScale = Vector3.one;

        Toggle tg = to.GetComponent<Toggle>();
        if (!ReferenceEquals(tg, null))
        {
            //加入事件
            tg.group = toggleGroup;

            tg.onValueChanged.AddListener(b =>
            {
                if (fo.activeInHierarchy && b)
                    return;

                fo.SetActive(b);

                actFunc?.Invoke(b);                
            });

            tg.isOn = isOn;
            
            //更改名字
            Text tgt = tg.GetComponentInChildren<Text>();
            if (!ReferenceEquals(tgt, null))
                tgt.text = toggleName;
        }

        to.SetActive(false);
        to.SetActive(true);

        togglePrefab.gameObject.SetActive(false);
    }

    public abstract void CreatPractice(ObjectData od);

    protected virtual void LoadCommonFunc(GameObject bdGo)
    {
        //注册激活时的方法，防止再次激活时无法再控制物体
        MainGameObject mgo = transform.root.GetComponent<MainGameObject>();
        if (mgo == null)
        {
            Debug.LogError("找不到根MainGameObject类");
            return;
        }

        mgo.OnFound += () => ObjectControl.SetTarget(bdGo.transform);
        //激活结束
    }

    protected virtual void OnPracClick()
    {
        OnSwichPanelHandler?.Invoke(PracMainSwitch.Practice);
    }

    protected virtual void OnMainClick()
    {
        OnSwichPanelHandler?.Invoke(PracMainSwitch.Main);
    }
}