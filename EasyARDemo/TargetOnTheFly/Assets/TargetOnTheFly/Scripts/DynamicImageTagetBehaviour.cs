//=============================================================================================================================
//
// Copyright (c) 2015-2017 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using UnityEngine;
using EasyAR;

namespace Sample
{
    public class DynamicImageTagetBehaviour : ImageTargetBehaviour
    {
        private GameObject subGameObject;

        protected override void Awake()
        {
            base.Awake();
            subGameObject = Instantiate(Resources.Load("EasyAR", typeof(GameObject))) as GameObject;
            subGameObject.transform.parent = transform;
        }
    }
}
