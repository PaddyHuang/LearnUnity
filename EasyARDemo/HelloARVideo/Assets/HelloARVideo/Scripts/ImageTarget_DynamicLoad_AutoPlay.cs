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
    public class ImageTarget_DynamicLoad_AutoPlay : ImageTargetBehaviour
    {
        private string video = @"https://sightpvideo-cdn.sightp.com/sdkvideo/EasyARSDKShow201520.mp4";

        protected override void Awake()
        {
            base.Awake();
            TargetFound += OnTargetFound;
            TargetLost += OnTargetLost;
            TargetLoad += OnTargetLoad;
            TargetUnload += OnTargetUnload;
        }

        protected override void Start()
        {
            base.Start();
            LoadVideo();
        }

        public void LoadVideo()
        {
            GameObject subGameObject = Instantiate(Resources.Load("VideoPlayer", typeof(GameObject))) as GameObject;
            subGameObject.transform.parent = this.transform;
            subGameObject.transform.localPosition = new Vector3(0, 0.225f, 0);
            subGameObject.transform.localRotation = new Quaternion();
            subGameObject.transform.localScale = new Vector3(0.8f, 0.45f, 0.45f);

            VideoPlayerBaseBehaviour videoPlayer = subGameObject.GetComponent<VideoPlayerBaseBehaviour>();
            if (videoPlayer)
            {
                videoPlayer.Storage = StorageType.Absolute;
                videoPlayer.Path = video;
                videoPlayer.EnableAutoPlay = true;
                videoPlayer.EnableLoop = true;
                videoPlayer.Open();
            }
        }

        void OnTargetFound(TargetAbstractBehaviour behaviour)
        {
            Debug.Log("Found: " + Target.Id);
        }

        void OnTargetLost(TargetAbstractBehaviour behaviour)
        {
            Debug.Log("Lost: " + Target.Id);
        }

        void OnTargetLoad(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
        {
            Debug.Log("Load target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
        }

        void OnTargetUnload(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
        {
            Debug.Log("Unload target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
        }
    }
}
