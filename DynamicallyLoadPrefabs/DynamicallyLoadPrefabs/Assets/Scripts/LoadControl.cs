using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class LoadControl : MonoBehaviour {

    [SerializeField] private Button loadResBtn, loadPathBtn, loadAbBtn;
    [SerializeField] private Button destroyAll;

    private GameObject sphere, cube, capsule;
    
    private AssetBundle ab;
    private string path = "Assets/AssetBundles/PC";
    
    // Use this for initialization
    void Start () {
        loadResBtn.onClick.AddListener(LoadResources);
        loadPathBtn.onClick.AddListener(LoadFromPath);
        loadAbBtn.onClick.AddListener(LoadFromAssetBundle);
        destroyAll.onClick.AddListener(DestroyAll);                        
    }

    /// <summary>
    /// 第一种方法，从Resources文件夹读取Prefab
    /// </summary>
    private void LoadResources()
    {
        sphere = Instantiate((GameObject)Resources.Load("Sphere"));
        // 设置父物体
        sphere.transform.parent = transform;
        
        Debug.Log("Loaded");
    }

    /// <summary>
    /// 第二种方法，绝对路径读取Prefab（这种方法局限性比较小，想怎么加载就怎么加载）
    /// </summary>
    private void LoadFromPath()
    {
        // 游戏运行中不能使用该方法。因为AssetDatabase是Editor的方法，所以它不能在构建中导入，不能在游戏中使用AssetDatabase。
        //cube = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Cube.prefab", typeof(GameObject)));
        AssetBundle ab = AssetBundle.LoadFromFile(string.Format("{0}/cube", path));
        cube = Instantiate(ab.LoadAsset<GameObject>("Cube"));
        cube.transform.parent = transform;
                
        ab.Unload(false);
    }

    private void LoadFromAssetBundle()
    {
        AssetBundle ab = AssetBundle.LoadFromFile(string.Format("{0}/capsule", path));
        capsule = Instantiate(ab.LoadAsset<GameObject>("Capsule"));
        capsule.transform.parent = transform;

        ab.Unload(false);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    private void DestroyAll()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
