using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabControl : MonoBehaviour {

    // Use this for initialization
    void Start() {
        // 全景按钮监听
        Toggle quanJingToggle = FindChild<Toggle>(transform, "QuanJing");
        quanJingToggle.onValueChanged.AddListener((bool value) => OnValueChange(quanJingToggle, value));
        // 导航按钮监听
        Toggle daoHangToggle = FindChild<Toggle>(transform, "DaoHang");
        daoHangToggle.onValueChanged.AddListener((bool value) => OnValueChange(daoHangToggle, value));
        // 识楼按钮监听
        Toggle shiLouToggle = FindChild<Toggle>(transform, "ShiLou");
        shiLouToggle.onValueChanged.AddListener((bool value) => OnValueChange(shiLouToggle, value));
        // 天气按钮监听
        Toggle tianQiToggle = FindChild<Toggle>(transform, "TianQi");
        tianQiToggle.onValueChanged.AddListener((bool value) => OnValueChange(tianQiToggle, value));               
    }

    void OnValueChange(Toggle toggle, bool value)
    {
        Text quanJingLabel = FindChild<Text>(transform, "quanJIngLabel");

        if (toggle.isOn)
        {
            quanJingLabel.color = new Color(59 / 256f, 152 / 256f, 229 / 256f);
            //quanJingLabel.text = string.Format("<color=#2598e5>全景</color>");
        }
        else
        {
            quanJingLabel.color = new Color(171 / 256f, 171 / 256f, 171 / 256f);
            //quanJingLabel.text = string.Format("<color=#ababab>全景</color>");
        }
    }

    //void OnDaoHangSelected(Toggle toggle, bool value)
    //{
    //    Text daoHangLabel = FindChild<Text>(transform, "daoHangLabel");
    //    if (toggle.isOn)
    //    {
    //        daoHangLabel.text = string.Format("<color=#2598e5>导航</color>");
    //    }
    //    else
    //    {
    //        daoHangLabel.text = string.Format("<color=#ababab>导航</color>");
    //    }
    //}

    //void OnShiLouSelected(Toggle toggle, bool value)
    //{
    //    Text shiLouLabel = FindChild<Text>(transform, "shiLouLabel");
    //    if (toggle.isOn)
    //    {
    //        shiLouLabel.text = string.Format("<color=#2598e5>识楼</color>");
    //    }
    //    else
    //    {
    //        shiLouLabel.text = string.Format("<color=#ababab>识楼</color>");
    //    }
    //}

    //void OnTianQiSelected(Toggle toggle, bool value)
    //{
    //    Text tianQiLabel = FindChild<Text>(transform, "tianQiLabel");
    //    if (toggle.isOn)
    //    {
    //        tianQiLabel.text = string.Format("<color=#2598e5>天气</color>");
    //    }
    //    else
    //    {
    //        tianQiLabel.text = string.Format("<color=#ababab>天气</color>");
    //    }
    //}


    #region // 递归获取子物体及其组件    
    private static T FindChild<T>(Transform parent, string name) where T : Component
    {
        Transform child = FindChild(parent, name);
        if (child != null)
        {
            return child.GetComponent<T>();
        }
        return null;
    }

    // 递归查找子对象
    private static  Transform FindChild(Transform parent, string name)
    {
        // 在当前层级查找子对象
        Transform child = parent.Find(name);
        if (child != null)
        {
            return child;
        }
        // 查找子对象的子对象
        Transform grantChild = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            grantChild = FindChild(parent.GetChild(i), name);
            if (grantChild != null)
            {
                return grantChild;
            }
        }
        return null;
    }
    #endregion
}
