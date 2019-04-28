using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARModelManager.UI.Models.ARJsonModel
{
    /// <summary>
    /// 计算类型
    /// </summary>
    public class TypeCalculation
    {
    }
    /// <summary>
    /// 介绍
    /// </summary>
    public class Introduce
    {

        public string Title { get; set; }
        /// <summary>
        /// 这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍里是介绍里是介绍这里是介绍这里是介绍
        /// </summary>
        public string Info { get; set; }


        public string Audio { get; set; }
    }
    /// <summary>
    /// 构建项
    /// </summary>
    public class StructItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 前轮
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GameObjectName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool LeftOrRight { get; set; }

        public string Audio { get; set; }
    }
    /// <summary>
    /// 构建内容
    /// </summary>
    public class Structs
    {
        /// <summary>
        /// 铲运机结构
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<StructItem> Struct { get; set; }
    }
    /// <summary>
    /// 尺寸项
    /// </summary>
    public class SizeItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 测试长度
        /// </summary>
        public string ObjcetName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FromGoName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ToGaName { get; set; }

        public string Audio { get; set; }
    }
    /// <summary>
    /// 尺寸集合
    /// </summary>
    public class Sizes
    {
        /// <summary>
        /// 尺寸
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SizeItem> Size { get; set; }
    }
    /// <summary>
    /// 视频
    /// </summary>
    public class Video
    {
        /// <summary>
        /// 铲运机测试
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 描述测试
        /// </summary>
        public string Info { get; set; }
    }


    public class TypeItemPractice
    {

        public string AnswerImageUrl { get; set; }
        public string PracticeTitle { get; set; }

        public string TaskPicName { get; set; }


        public string TaskPicUrl { get; set; }


        public string AnswerPicName { get; set; }


        public string AnswerPicUrl { get; set; }


        public bool IsShowAnswer { get; set; }



        public List<TypePracticeItem> PracObjects { get; set; }

    }


    public class TypePracticeItem
    {
        public int Index { get; set; }

        public string Info { get; set; }

        public string GameObjectName { get; set; }


        public bool LeftOrRight { get; set; }
    }
    /// <summary>
    /// 构建类型项
    /// </summary>
    public class TypeItem
    {
        /// <summary>
        /// 
        /// </summary>
        public Introduce Introduce { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Structs Structs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Sizes Sizes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Video Video { get; set; }


        public TypeItemPractice Practices { get; set; }
    }

    /// <summary>
    /// 方式类型
    /// </summary>
    public class TypeMethod
    {
    }
    /// <summary>
    /// 图标类型
    /// </summary>
    public class TypeTable
    {
    }
    /// <summary>
    /// 流程类型
    /// </summary>
    public class TypeProcess
    {
    }

    public class RootModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        public string AbObjectName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SerId { get; set; }
        /// <summary>
        /// 图1-11
        /// </summary>
        public string CNID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aburl { get; set; }
        /// <summary>
        /// 计算类结构
        /// </summary>
        public TypeCalculation TypeCalculation { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public TypeItem TypeItem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TypeMethod TypeMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TypeTable TypeTable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TypeProcess TypeProcess { get; set; }
    }

}