using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles_Android")]
    static void BuildAllAssetBundlesMB()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles",BuildAssetBundleOptions.None,BuildTarget.Android);
        //BuildPipeline.BuildAssetBundles("Assets/AssetBundles/PC",BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);
    }  
    
    [MenuItem("Assets/Build AssetBundles_PC")]
    static void BuildAllAssetBundlesPC()
    {
        //BuildPipeline.BuildAssetBundles("Assets/AssetBundles",BuildAssetBundleOptions.None,BuildTarget.Android);
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/PC",BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);
    }
}