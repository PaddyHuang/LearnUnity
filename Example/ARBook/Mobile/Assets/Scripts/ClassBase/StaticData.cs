using UnityEngine;

public static class StaticData
{
    public static string UserName;
    public static string UserId;

    /// <summary>
    /// Book Info Class
    /// </summary>
    public static class BookInfo
    {
        public static string currentBookID;

        public const string JianZhuJiChuGongChengName = "JianZhuJiChuGongCheng";            // 建筑基础工程
        public const string JianZhuJiChuGongChengID = "430d638c9e2246ad8b01124605d98d3c";

        public const string ChanPinShouCeName = "ChanPinShouCe";                            // 行星科学
        public const string ChanPinShouCeID = "fc84801321374309b0cca774ea7a0e83";             
    }
    
    /// <summary>
    /// Components Name String
    /// </summary>
    public const string HomeSceneStr = "HomeScene";    
    public const string ComponentIntroduceName = "ComponentIntroduce(Clone)";
    public const string CanvasMainTag = "CanvasMain";
    public const string LightSystemTag = "LightSystem";
    public const string LightSourceName = "Directional Light";
    public const string ButtonShowName = "ButtonShow";
    public const string ButtonPauseName = "ButtonPause";
    public const string ButonReplayName = "ButtonReplay";
    public const string BackButtonName = "BackBtn";
    public const string MainToggleName = "MainToggle";
    public const string PracToggleName = "PracToggle";
    public const string ToggleContentName = "TogglesContent";

    public const string RecPostUrl = "http://39.108.107.2:8080/ARModel/GetModelInfo";
    public const string AnswerPostUrl = "http://39.108.107.2:8080/ARModel/SubmitAnswer";
    public const string animeName = "Take 001";

    public static string AssetBundlePath
    {
        get
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            return Application.dataPath + "/Packages";
#endif

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        return  Application.persistentDataPath + "/Packages";
#endif
        }
    }

    public const string LocalJsonName = "LocalData.json";
}