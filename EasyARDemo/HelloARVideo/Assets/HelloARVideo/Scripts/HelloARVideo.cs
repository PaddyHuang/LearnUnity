//=============================================================================================================================
//
// Copyright (c) 2015-2018 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using UnityEngine;
using EasyAR;

namespace Sample
{
    public class HelloARVideo : MonoBehaviour
    {
        private const string title = "Please enter KEY first!";
        private const string boxtitle = "===PLEASE ENTER YOUR KEY HERE===";
        private const string keyMessage = ""
            + "Steps to create the key for this sample:\n"
            + "  1. login www.easyar.com\n"
            + "  2. create app with\n"
            + "      Name: HelloARVideo (Unity)\n"
            + "      Bundle ID: cn.easyar.samples.unity.helloarvideo\n"
            + "  3. find the created item in the list and show key\n"
            + "  4. replace all text in TextArea with your key";

        private ImageTarget_DynamicLoad_ManualPlay target;

        private void Awake()
        {
            if (FindObjectOfType<EasyARBehaviour>().Key.Contains(boxtitle))
            {
#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog(title, keyMessage, "OK");
#endif
                Debug.LogError(title + " " + keyMessage);
            }
        }

        private void Start()
        {
            if (!target)
            {
                GameObject go = new GameObject("ImageTarget-TransparentVideo-DynamicLoad");
                target = go.AddComponent<ImageTarget_DynamicLoad_ManualPlay>();
            }
            target.ActiveTargetOnStart = false;
            target.Bind(ARBuilder.Instance.ImageTrackerBehaviours[0]);
            target.SetupWithImage("idback.jpg", EasyAR.StorageType.Assets, "idback", new Vector2(8.56f, 5.4f));
        }

        private void OnDestory()
        {
            if (!target)
                return;
            target.Bind(null);
        }
    }
}
