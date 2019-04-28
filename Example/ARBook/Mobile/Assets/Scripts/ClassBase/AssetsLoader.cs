using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public enum TypeEnum
{
    WuJian,
    JiSuan,
    FangShi,
    TuBiao,
    LiuCheng
}

public class AssetsLoader
{
    /// <summary> 
    /// 开始解析json文件 </summary>
    public static ObjectData LoadServerJsonData(string json)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        JsonData data = JsonMapper.ToObject(json);

        string typeStr = data["Type"].ToString();

        //大类解析
        ObjectData objectData;
        //大类型识别
        TypeEnum t = (TypeEnum) Enum.Parse(typeof(TypeEnum), typeStr);
        switch (t)
        {
            case TypeEnum.WuJian:
                objectData = new TypeItem(data["TypeItem"]);
                break;
            case TypeEnum.JiSuan:
                objectData = new TypeCalculation(data["TypeCalculation"]);
                break;
            case TypeEnum.FangShi:
                objectData = new TypeMethod(data["TypeMethod"]);
                break;
            case TypeEnum.TuBiao:
                objectData = new TypeTable(data["TypeTable"]);
                break;
            case TypeEnum.LiuCheng:
                objectData = new TypeProcess(data["TypeProcess"]);
                break;
            default: return null;
        }

        objectData.Type = t;
        objectData.BookID = data["BookID"].ToString();
        objectData.BookName = data["BookName"].ToString();
        objectData.Name = data["Name"].ToString();
        objectData.CNID = data["CNID"].ToString();
        objectData.aburl = data["aburl"].ToString();
        objectData.aburlPC = data["aburlPC"].ToString();
        //Debug.Log(objectData.BookID);
        //sw.Stop();
        //TimeSpan ts2 = sw.Elapsed;
        //UnityEngine.Debug.Log(ts2.TotalMilliseconds);

        return objectData;
    }
}

public static class TypeData
{
    public const string Symbol = "#";
    public const string Title = "Title";

    public static class Calcul
    {
        public const string OverallInfo = "OverallInfo";
        public const string Calculations = "Calculations";
        public const string Practices = "Practices";
    }

    public static class Item
    {
        public const string AbObjectName = "AbObjectName";
        public const string Introduce = "Introduce";
        public const string Structs = "Structs";
        public const string Sizes = "Sizes";
        public const string Video = "Video";
        public const string Practices = "Practices";
    }

    public static class Table
    {
        public const string Effect = "Effect";
        public const string Practices = "Practices";
    }

    public static class Method
    {
        public const string Methods = "Methods";
        public const string Practices = "Practices";
    }

    public static class Process
    {
        public const string AnimStates = "AnimStates";
        public const string Frames = "Frames";
        public const string Practices = "Practices";
    }
}

public class ObjectData
{
    public TypeEnum Type;
    public string BookID;
    public string BookName;
    public string Name;
    public string CNID;
    public string aburl;
    public string aburlPC;
}

#region 计算类型

public class TypeCalculation : ObjectData
{
    public readonly Dictionary<string, string> CompomentsTitle = new Dictionary<string, string>();

    public readonly List<CalculationSingle> CalcultionsList = new List<CalculationSingle>();
    public readonly List<CalculSizes> CalculSizesList = new List<CalculSizes>();

    public CalculOverallInfo CalculOverallInfo;

    public CalculPracticeData m_CalculPracticeData;

    public string CalculComponentInfo;

    public TypeCalculation(JsonData jsonData)
    {
        //Creat基本信息
        CalculOverallInfo = new CalculOverallInfo
        {
            Info = jsonData[TypeData.Calcul.OverallInfo]["Info"].ToString(),
            Audio = jsonData[TypeData.Calcul.OverallInfo]["Audio"].ToString()
        };
        CompomentsTitle.Add(TypeData.Calcul.OverallInfo,
            jsonData[TypeData.Calcul.OverallInfo][TypeData.Title].ToString());

        //Creat计算小类
        int calCount = jsonData[TypeData.Calcul.Calculations]["Calcul"].Count;
        if (calCount != 0)
        {
            CompomentsTitle.Add(TypeData.Calcul.Calculations,
                jsonData[TypeData.Calcul.Calculations][TypeData.Title].ToString());

            CalculComponentInfo = jsonData[TypeData.Calcul.Calculations]["Info"].ToString();
            for (int i = 0; i < calCount; i++)
            {
                CalculationSingle cls = new CalculationSingle(jsonData[TypeData.Calcul.Calculations]["Calcul"][i]);

                CalcultionsList.Add(cls);
            }
        }

        //计算小类中的size子类
        int sizeCount = jsonData[TypeData.Calcul.Calculations]["Sizes"].Count;
        if (sizeCount != 0)
        {
            for (int i = 0; i < sizeCount; i++)
            {
                CalculSizes ccs = new CalculSizes(jsonData[TypeData.Calcul.Calculations]["Sizes"][i]);
                CalculSizesList.Add(ccs);
            }
        }

        m_CalculPracticeData = new CalculPracticeData(jsonData[TypeData.Calcul.Practices]);
    }
}

public class CalculOverallInfo
{
    //介绍信息
    public string Info;
    public string Audio;
}

public class CalculationSingle
{
    //序号
    public int Index;

    //描述
    public string Info;

    //游戏物体名字
    public string GameObjectName;

    //左或右
    public readonly bool LeftOrRight; //默认为false=右边

    //游戏物体名称标识,如A0
    public string GameObjectIndex;

    public string AudioUrl;

    public CalculationSingle(JsonData jd)
    {
        Index = int.Parse(jd["Index"].ToString());
        Info = jd["Info"].ToString();
        GameObjectName = jd["GameObjectName"].ToString();
        GameObjectIndex = jd["GameObjectIndex"].ToString();
        LeftOrRight = (bool) jd["LeftOrRight"];
        AudioUrl = jd["Audio"].ToString();
    }
}

public class CalculSizes
{
    public readonly int Index; //序号
    public readonly string Length; //长度

    public readonly string FromGoName; //起始游戏物体的名字
    public readonly string ToGoName; //终点游戏物体的名字
    public string AudioUrl;

    /// <summary> 
    /// 构造函数 </summary>
    public CalculSizes(JsonData jd)
    {
        Index = int.Parse(jd["Index"].ToString());
        Length = jd["Length"].ToString();
        FromGoName = jd["FromGoName"].ToString();
        ToGoName = jd["ToGaName"].ToString();
        AudioUrl = jd["Audio"].ToString();
    }
}

public class CalculPracticeData
{
    public string TextInfo;
    public string TaskPicName;
    public string TaskPicUrl;
    public string AnswerPicName;
    public string AnswerPicUrl;
    public bool IsShowAnswer;

    //public float[] Answers;

    public Dictionary<int, float> AnswersColl = new Dictionary<int, float>();

    public CalculPracticeData(JsonData jd)
    {
        //TextInfo = jd["TextInfo"].ToString();
        //todo:待服务器解决
        TextInfo = jd["TextInfo"].ToString().TrimStart().TrimEnd();

        TaskPicName = jd["TaskPicName"].ToString();
        TaskPicUrl = jd["TaskPicUrl"].ToString();
        AnswerPicName = jd["AnswerPicName"].ToString();
        AnswerPicUrl = jd["AnswerPicUrl"].ToString();
        IsShowAnswer = bool.Parse(jd["IsShowAnswer"].ToString());

        //构建正确答案
        for (int i = 0; i < jd["Answers"].Count; i++)
        {
            int index = int.Parse(jd["Answers"][i]["Index"].ToString());
            float answer = float.Parse(jd["Answers"][i]["Answer"].ToString());
            AnswersColl.Add(index, answer);
        }
    }
}

public struct CalculPost
{
    public int Index;
    public string Input;
    public bool IsRight;
}

#endregion

#region 物件类型

/// <summary> 
/// 物件类型 </summary>
public class TypeItem : ObjectData
{
    public readonly Dictionary<string, string> CompomentsTitle = new Dictionary<string, string>();

    public Introduce m_Introduce;
    public ItemPracticeData m_ItemPracticeData;

    public readonly List<StructSingle> StructsList = new List<StructSingle>();
    public readonly List<SizeSingle> SizesList = new List<SizeSingle>();
    public readonly List<VideoSigle> VideoList = new List<VideoSigle>();

    public TypeItem(JsonData jsonData)
    {
        //Creat介绍
        m_Introduce = new Introduce
        {
            IntroduceInfo = jsonData[TypeData.Item.Introduce]["Info"].ToString(),
            AudioUrl = jsonData[TypeData.Item.Introduce]["Audio"].ToString()
        };

        CompomentsTitle.Add(TypeData.Item.Introduce, jsonData[TypeData.Item.Introduce][TypeData.Title].ToString());

        //Creat结构
        int structCount = jsonData[TypeData.Item.Structs]["Struct"].Count;
        if (structCount != 0)
        {
            CompomentsTitle.Add(TypeData.Item.Structs, jsonData[TypeData.Item.Structs][TypeData.Title].ToString());
            for (int i = 0; i < structCount; i++)
            {
                StructSingle ss = new StructSingle(jsonData[TypeData.Item.Structs]["Struct"][i]);
                StructsList.Add(ss);
            }
        }

        //Creat尺寸
        int sizeCount = jsonData[TypeData.Item.Sizes]["Size"].Count;
        if (sizeCount != 0)
        {
            CompomentsTitle.Add(TypeData.Item.Sizes, jsonData[TypeData.Item.Sizes][TypeData.Title].ToString());

            for (int i = 0; i < jsonData[TypeData.Item.Sizes]["Size"].Count; i++)
            {
                SizeSingle ss = new SizeSingle(jsonData[TypeData.Item.Sizes]["Size"][i]);
                SizesList.Add(ss);
            }
        }

        //Creat movie
        var movie = jsonData[TypeData.Item.Video]["Url"];
        if (movie != null)
        {
            CompomentsTitle.Add(TypeData.Item.Video, jsonData[TypeData.Item.Video][TypeData.Title].ToString());

            VideoSigle ss = new VideoSigle(jsonData[TypeData.Item.Video]);
            VideoList.Add(ss);
        }

        //Creat Practice
        m_ItemPracticeData = new ItemPracticeData
        {
            AnswerImageUrl = jsonData[TypeData.Item.Practices]["AnswerImageUrl"].ToString(),
            PracTitle = jsonData[TypeData.Item.Practices]["PracticeTitle"].ToString(),
            IsShowAnswer = Boolean.Parse(jsonData[TypeData.Item.Practices]["IsShowAnswer"].ToString())
        };

        int pracCount = jsonData[TypeData.Item.Practices]["PracObjects"].Count;
        if (pracCount != 0)
        {
            for (int i = 0; i < pracCount; i++)
            {
                ItemPracticeData.PracticeData pd =
                    new ItemPracticeData.PracticeData(jsonData[TypeData.Item.Practices]["PracObjects"][i]);

                m_ItemPracticeData.PracticeDatas.Add(pd);
            }
        }
        else //如果为空,则取用structjson 结构
        {
            for (int i = 0; i < structCount; i++)
            {
                ItemPracticeData.PracticeData pd =
                    new ItemPracticeData.PracticeData(jsonData[TypeData.Item.Structs]["Struct"][i]);

                m_ItemPracticeData.PracticeDatas.Add(pd);
            }
        }
    }
}

public class Introduce
{
    public class MainShow
    {
        public string Info;
        public AudioClip Audio { get; private set; } //音频属性

        public void SetAudio(AudioClip ac)
        {
            Audio = ac;
        }
    }

//用于展示的结构
    public class Structs
    {
        public string Info;
        public UITextSingleCreator UITextSingle;
        public AudioClip Audio { get; private set; }

        public void SetAudio(AudioClip ac)
        {
            Audio = ac;
        }
    }

//用于展示的尺寸
    public class Sizes
    {
        public string Info;
        public UISizeSingleCreator UISizeSingle;
        public AudioClip Audio { get; private set; }

        public void SetAudio(AudioClip ac)
        {
            Audio = ac;
        }
    }

    public string IntroduceInfo;

    public string AudioUrl;
}

/// <summary> 
/// FromJsonData
/// 物件-结构
///  </summary>
public class StructSingle
{
    public readonly int Index; //序号
    public readonly string Info; //描述信息
    public readonly string GameObjectName; //
    public readonly bool LeftOrRight; //默认为false=右边
    public readonly string CombinedStr; //组合后的字符串，如5-尾架
    public readonly string AudioUrl;

    /// <summary> 
    /// 构造函数 </summary>
    public StructSingle(JsonData jd)
    {
        Index = int.Parse(jd["Index"].ToString());
        Info = jd["Info"].ToString();
        GameObjectName = jd["GameObjectName"].ToString();
        LeftOrRight = (bool) jd["LeftOrRight"];
        CombinedStr = Index + "-" + Info;
        AudioUrl = jd["Audio"].ToString();
    }
}

/// <summary> 
/// FromJsonData
/// 物件-尺寸
///  </summary>
public class SizeSingle
{
    public readonly int Index; //序号
    public readonly string Length; //长度
    public readonly string ObjcetName;
    public readonly string FromGoName; //起始游戏物体的名字
    public readonly string ToGoName; //终点游戏物体的名字
    public readonly string AudioUrl;

    /// <summary> 
    /// 构造函数 </summary>
    public SizeSingle(JsonData jd)
    {
        Index = int.Parse(jd["Index"].ToString());
        Length = jd["Length"].ToString();
        FromGoName = jd["FromGoName"].ToString();
        ToGoName = jd["ToGaName"].ToString();
        ObjcetName = jd["ObjcetName"].ToString();
        AudioUrl = jd["Audio"].ToString();
    }
}

/// <summary> 
/// FromJsonData
/// 物件-视频
///  </summary>
public class VideoSigle
{
    public string Url;
    public string Info;

    public VideoSigle(JsonData jd)
    {
        Url = jd["Url"].ToString();
        Info = jd["Info"].ToString();
    }
}

/// <summary> 
/// FromJsonData
/// 物件-练习
///  </summary>
public class ItemPracticeData
{
    public struct PracticeData
    {
        public readonly int Index; //序号
        public readonly string Info; //描述信息
        public readonly string GameObjectName; //
        public readonly bool LeftOrRight; //默认为false=右边

        /// <summary> 
        /// 构造函数 </summary>
        public PracticeData(JsonData jd)
        {
            Index = int.Parse(jd["Index"].ToString());
            //Info = jd["Info"].ToString();
            //todo：待服务器解决
            Info = jd["Info"].ToString().TrimStart().TrimEnd();
            GameObjectName = jd["GameObjectName"].ToString();
            LeftOrRight = (bool) jd["LeftOrRight"];
        }
    }

    public List<PracticeData> PracticeDatas = new List<PracticeData>();

    public string AnswerImageUrl;
    public string PracTitle;
    public bool IsShowAnswer;
}

public class ItemPost
{
    //public int Index;
    public string RightAnswer;
    public string UserAnswer;
    public bool IsRight;
}

#endregion

#region 方式类型

public class TypeMethod : ObjectData
{
    public readonly List<MethodSigle> MethodList = new List<MethodSigle>();
    public MethodPracticeData m_MethodPracticeData;

    public TypeMethod(JsonData jsonData)
    {
        //Creat 图表
        int methodsCount = jsonData[TypeData.Method.Methods].Count;
        if (methodsCount != 0)
        {
            for (int i = 0; i < methodsCount; i++)
            {
                MethodSigle ss = new MethodSigle(jsonData[TypeData.Method.Methods][i]);
                MethodList.Add(ss);
            }
        }

        m_MethodPracticeData = new MethodPracticeData(jsonData[TypeData.Method.Practices]);
    }
}

public class MethodSigle
{
    public string Title;
    public int Index; //序号索引
    public string Info; //文字信息
    public string GameObjectName; //游戏内物体名字
    public string AudioUrl; //音频链接

    public MethodSigle(JsonData jd)
    {
        Index = int.Parse(jd["Index"].ToString());
        Info = jd["Info"].ToString();
        GameObjectName = jd["GameObjectName"].ToString();
        Title = jd["Title"].ToString();
        AudioUrl = jd["Audio"].ToString();
    }
}

public class MethodPracticeData
{
    public string TextInfo;
    public string TaskPicName;
    public string TaskPicUrl;
    public string AnswerPicName;
    public string AnswerPicUrl;
    public bool IsShowAnswer;

    //public List<string> practiceUrls = new List<string>();
    public Dictionary<int, string> PracticeUrls = new Dictionary<int, string>();
    public List<int> Answers = new List<int>();

    public MethodPracticeData(JsonData jd)
    {
        //TextInfo = jd["TextInfo"].ToString();
        //todo:待服务器解决
        TextInfo = jd["TextInfo"].ToString().TrimStart().TrimEnd();
        TaskPicName = jd["TaskPicName"].ToString();
        TaskPicUrl = jd["TaskPicUrl"].ToString();
        AnswerPicName = jd["AnswerPicName"].ToString();
        AnswerPicUrl = jd["AnswerPicUrl"].ToString();
        IsShowAnswer = (bool) jd["IsShowAnswer"];

        for (int i = 0; i < jd["PracObjects"].Count; i++)
        {
            PracticeUrls.Add(int.Parse(jd["PracObjects"][i]["Index"].ToString()), jd["PracObjects"][i]["Url"].ToString());
        }

        for (int i = 0; i < jd["Answers"].Count; i++)
        {
            Answers.Add(int.Parse(jd["Answers"][i].ToString()));
        }
    }
}

public struct MethodPost
{
    public int Selected;
}

#endregion

#region 图表类型

public class TypeTable : ObjectData
{
    public readonly Dictionary<string, string> CompomentsTitle = new Dictionary<string, string>();

    public readonly List<TableSingle> TablesList = new List<TableSingle>();

    public TablePracticeData m_TablePracticeData;

    public TypeTable(JsonData jsonData)
    {
        //Creat 图表
        int tableCount = jsonData[TypeData.Table.Effect].Count;
        if (tableCount != 0)
        {
            for (int i = 0; i < tableCount; i++)
            {
                string titleStr = jsonData[TypeData.Table.Effect][i]["Title"].ToString();
                CompomentsTitle.Add(titleStr, titleStr);

                TableSingle ss = new TableSingle(jsonData[TypeData.Table.Effect][i]);
                TablesList.Add(ss);
            }
        }

        m_TablePracticeData = new TablePracticeData(jsonData[TypeData.Table.Practices]);
    }
}

public class TableSingle
{
    // 因为服务器已经返回谁激活，所以此处暂不解析
    // public string Key; //返回的CNIDkey 用以判断谁激活
    public bool IsOn; //是否激活
    public string BackgroundPicUrl; //曲线背景图片的名字
    public string DynamicPicUrl; //曲线图片的名字
    public string GameObjectName; //游戏物体名字
    public string AudioUrl; //音频地址

    public TableSingle(JsonData jd)
    {
        //Key = jd["Key"].ToString();
        IsOn = Boolean.Parse(jd["IsOn"].ToString());
        BackgroundPicUrl = jd["BackgroundUrl"].ToString();
        DynamicPicUrl = jd["DynamicPicUrl"].ToString();
        GameObjectName = jd["GameObjectName"].ToString();
        AudioUrl = jd["Audio"].ToString();
    }
}

public class TableUnityData
{
    public GameObject[] AnimGameObjects;
    //public Sprite[] AnimSprite;
}

public class TablePracticeData
{
    public string TextInfo;
    public string TaskPicName;
    public string TaskPicUrl;
    public string AnswerPicName;
    public string AnswerPicUrl;
    public bool IsShowAnswer;

    public Dictionary<int, string> AnswersColl = new Dictionary<int, string>();

    public TablePracticeData(JsonData jd)
    {
        //todo:待服务器解决
        //TextInfo = jd["TextInfo"].ToString();
        TextInfo = jd["TextInfo"].ToString().TrimStart().TrimEnd();
        TaskPicName = jd["TaskPicName"].ToString();
        TaskPicUrl = jd["TaskPicUrl"].ToString();
        AnswerPicName = jd["AnswerPicName"].ToString();
        AnswerPicUrl = jd["AnswerPicUrl"].ToString();
        IsShowAnswer = bool.Parse(jd["IsShowAnswer"].ToString());

        for (int i = 0; i < jd["Answers"].Count; i++)
        {
            int index = int.Parse(jd["Answers"][i]["Index"].ToString());
            string text = jd["Answers"][i]["Answer"].ToString();
            AnswersColl.Add(index, text);
        }
    }
}

public class TablePost
{
    public int Index;
    public string RightAnswer;
    public string UserAnswer;
    public bool IsRight;
}

#endregion

#region 流程类型

public class TypeProcess : ObjectData
{
    public readonly List<ProcessSingle> PsList = new List<ProcessSingle>();
    public readonly string Frames;
    public readonly ProcessPracticeData m_ProcessPracticeData;

    public TypeProcess(JsonData jsonData)
    {
//Creat StateInfo
        int psCount = jsonData[TypeData.Process.AnimStates].Count;
        if (psCount != 0)
        {
            for (int i = 0; i < psCount; i++)
            {
                ProcessSingle ps = new ProcessSingle(jsonData[TypeData.Process.AnimStates][i]);
                PsList.Add(ps);
            }
        }

        Frames = jsonData[TypeData.Process.Frames].ToString();
        m_ProcessPracticeData = new ProcessPracticeData(jsonData[TypeData.Process.Practices]);
    }
}

public class ProcessSingle
{
    public int Index;
    public string Info;
    public string AudioUrl;

    public ProcessSingle(JsonData jd)
    {
        Index = int.Parse(jd["Index"].ToString());
        Info = jd["StateInfo"].ToString();
        AudioUrl = jd["Audio"].ToString();
    }
}

public class ProcessPracticeData
{
    public Dictionary<int, string> AnswersColl = new Dictionary<int, string>();

    public string[] PracticesGroup;
    public string TextInfo;
    public string TaskPicName;
    public string TaskPicUrl;
    public string AnswerPicName;
    public string AnswerPicUrl;
    public bool IsShowAnswer;

    public ProcessPracticeData(JsonData jd)
    {
        // //todo:待服务器解决
        //TextInfo = jd["TextInfo"].ToString();
        TextInfo = jd["TextInfo"].ToString().TrimStart().TrimEnd();
        TaskPicName = jd["TaskPicName"].ToString();
        TaskPicUrl = jd["TaskPicUrl"].ToString();
        AnswerPicName = jd["AnswerPicName"].ToString();
        AnswerPicUrl = jd["AnswerPicUrl"].ToString();
        IsShowAnswer = bool.Parse(jd["IsShowAnswer"].ToString());
        PracticesGroup = new string[jd["PracObjects"].Count];
        for (int i = 0; i < jd["PracObjects"].Count; i++)
            PracticesGroup[i] = jd["PracObjects"][i].ToString();

        for (int i = 0; i < jd["Answers"].Count; i++)
        {
            int idx = int.Parse(jd["Answers"][i]["Index"].ToString());
            string str = jd["Answers"][i]["Text"].ToString();
            AnswersColl.Add(idx, str);
        }
    }
}

public class ProcessPost
{
    public int Index;
    public string UserAnswer;
    public string RightAnswer;
    public bool IsRight;
}

#endregion