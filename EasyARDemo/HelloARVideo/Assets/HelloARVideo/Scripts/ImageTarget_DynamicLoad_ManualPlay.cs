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
    public class ImageTarget_DynamicLoad_ManualPlay : ImageTargetBehaviour
    {
        private bool loaded;
        private bool found;
        private System.EventHandler videoReayEvent;
        private VideoPlayerBaseBehaviour videoPlayer;
        private string video = "transparentvideo.mp4";

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
            videoReayEvent = OnVideoReady;
            base.Start();
            LoadVideo();
        }

        public void LoadVideo()
        {
            GameObject subGameObject = Instantiate(Resources.Load("TransparentVideo", typeof(GameObject))) as GameObject;
            subGameObject.transform.parent = this.transform;
            subGameObject.transform.localPosition = new Vector3(0, 0.1f, 0);
            subGameObject.transform.localRotation = new Quaternion();
            subGameObject.transform.localScale = new Vector3(0.5f, 0.2f, 0.3154205f);

            videoPlayer = subGameObject.GetComponent<VideoPlayerBaseBehaviour>();
            if (videoPlayer)
            {
                videoPlayer.Storage = StorageType.Assets;
                videoPlayer.Path = video;
                videoPlayer.EnableAutoPlay = false;
                videoPlayer.EnableLoop = true;
                videoPlayer.Type = VideoPlayerBaseBehaviour.VideoType.TransparentSideBySide;
                videoPlayer.VideoReadyEvent += videoReayEvent;
                videoPlayer.Open();
            }
        }

        public void UnLoadVideo()
        {
            if (!videoPlayer)
                return;
            videoPlayer.VideoReadyEvent -= videoReayEvent;
            videoPlayer.Close();
            loaded = false;
        }

        void OnVideoReady(object sender, System.EventArgs e)
        {
            Debug.Log("Load video success");
            VideoPlayerBaseBehaviour player = sender as VideoPlayerBaseBehaviour;
            loaded = true;
            if (player && found)
                player.Play();
        }

        void OnTargetFound(TargetAbstractBehaviour behaviour)
        {
            found = true;
            if (videoPlayer && loaded)
                videoPlayer.Play();
            Debug.Log("Found: " + Target.Id);
        }

        void OnTargetLost(TargetAbstractBehaviour behaviour)
        {
            found = false;
            if (videoPlayer && loaded)
                videoPlayer.Pause();
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
