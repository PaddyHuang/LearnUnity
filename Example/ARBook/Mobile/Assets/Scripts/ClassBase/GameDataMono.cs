using System.Collections.Generic;
using UnityEngine;

public class GameDataMono : MonoBehaviour
{
    #region Mono-Singleton

    private static GameDataMono instance;
    public static GameDataMono Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<GameDataMono>();
            return instance;
        }
    }

    #endregion

    //存储已经创建过的类型
    public Dictionary<string, GameObject> CreatedObjectCollProp => CreatedObjectColl;
    private Dictionary<string, GameObject> CreatedObjectColl = new Dictionary<string, GameObject>();

    private Queue<GameObject> PreGameObjectQueue = new Queue<GameObject>();

    //全局成功
    public bool CreateSuccessed { get; private set; }

    public GameObject CreatedObject => createdObject;

    private GameObject createdObject;

    void Start()
    {
        CreatedObjectColl = new Dictionary<string, GameObject>();
        PreGameObjectQueue = new Queue<GameObject>();
    }

    /// <summary>
    /// 保存创建的物体
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cgo"></param>
    public void SaveCreatedObject(string id, GameObject cgo)
    {
        if (!CreatedObjectCollProp.ContainsKey(id) && !ReferenceEquals(cgo, null))
        {
            createdObject = cgo;
            CreatedObjectCollProp.Add(id, cgo);

            PreGameObjectQueue.Enqueue(cgo);
        }

        if (PreGameObjectQueue.Count > 2)
        {
            GameObject rg = PreGameObjectQueue.Peek();
            string key = null;
            foreach (var r in CreatedObjectCollProp)
                if (rg == r.Value)
                    key = r.Key;
            if (key == null)
                return;

            CreatedObjectCollProp.Remove(key);

            Object.Destroy(PreGameObjectQueue.Dequeue());
        }
    }

    /// <summary>
    /// 设置当前创建的物体
    /// </summary>
    /// <param name="cg"></param>
    public void SetCurrentGameObject(GameObject cg)
    {
        createdObject = cg;
    }

    /// <summary> 
    /// 设空当前物体 </summary>
    public void SetCurrentNull()
    {
        createdObject = null;
        CreateSuccessed = false;
    }

    /// <summary> 
    /// 成功创建一个物体 </summary>
    public void CreatedSuccess()
    {
        CreateSuccessed = true;
    }
}