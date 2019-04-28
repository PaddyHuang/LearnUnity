using UnityEngine;

/// <summary>
/// 存储游戏数据的地方
/// </summary>
public class GameData
{
    #region Instance

    public static GameData Instance { get; } = new GameData();

    #endregion

    public float ScaleFactor
    {
        get
        {
            if (scaleFactor < 0)
            {
                scaleFactor = Screen.width / 1920f; //初始化，第一次读取时赋值
            }

            return scaleFactor;
        }
    }

    private float scaleFactor = -1;

    public readonly float AudioInterval = 2;

    ////存储已经创建过的类型
    //private Dictionary<string, GameObject> CreatedObjectColl = new Dictionary<string, GameObject>();
    //public Dictionary<string, GameObject> CreatedObjectCollProp
    //{
    //    get { return CreatedObjectColl ?? (CreatedObjectColl = new Dictionary<string, GameObject>()); }
    //    private set { CreatedObjectColl = value; }
    //}

    //private Queue<GameObject> PreGameObjectQueue = new Queue<GameObject>();
    //private Queue<GameObject> PreGameObjectQueueProp
    //{
    //    get { return PreGameObjectQueue ?? (PreGameObjectQueue = new Queue<GameObject>()); }
    //}

    ////全局静态变量结束
    //public GameObject CreatedObject => createdObject;

    //private GameObject createdObject;

    //public void SaveCreatedObject(string id, GameObject cgo)
    //{
    //    if (!CreatedObjectCollProp.ContainsKey(id) && !ReferenceEquals(cgo, null))
    //    {
    //        createdObject = cgo;
    //        CreatedObjectCollProp.Add(id, cgo);

    //        PreGameObjectQueueProp.Enqueue(cgo);
    //    }

    //    if (PreGameObjectQueueProp.Count > 2)
    //    {
    //        GameObject rg = PreGameObjectQueueProp.Peek();
    //        string key = null;
    //        foreach (var r in CreatedObjectCollProp)
    //            if (rg == r.Value)
    //                key = r.Key;
    //        if (key == null)
    //            return;

    //        CreatedObjectCollProp.Remove(key);

    //        Object.Destroy(PreGameObjectQueueProp.Dequeue());
    //    }
    //}

    //public void SetCurrentGameObject(GameObject cg)
    //{
    //    createdObject = cg;
    //}

    //public void SetCurrentNull()
    //{
    //    createdObject = null;
    //    CreateSuccessed = false;
    //}

    //public void OnDestroy()
    //{
    //    CreateSuccessed = false;
    //    if (CreatedObjectCollProp != null)
    //    {
    //        CreatedObjectCollProp.Clear();
    //        CreatedObjectCollProp = null;
    //    }

    //    createdObject = null;
    //    CreateSuccessed = false;
    //}
}