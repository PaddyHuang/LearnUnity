using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARModelManager.UI.Models.ARJsonModel
{
    public class ProcessTypeCalculation
    {
    }



    public class ProcessTypeItem
    {
    }



    public class ProcessTypeMethod
    {
    }



    public class ProcessTypeTable
    {
    }



    public class AnimStatesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// （1）定位。深层搅拌机桩架到达指定桩位、对中
        /// </summary>
        public string StateInfo { get; set; }

    }



    public class ProcessPractices
    {
        /// <summary>
        /// 水泥土桩墙工程的施工工艺流程为
        /// </summary>
        public string TextInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TaskPicName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TaskPicUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AnswerPicName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AnswerPicUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsShowAnswer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> PracObjects { get; set; }


        public List<AnswerItemProcess> Answers
        {
            get;set;
        }
    }



    public class ProcessTypeProcess
    {
        /// <summary>
        /// 
        /// </summary>
        public string AbObjectName { get; set; }

        /// <summary>
        /// 0-15帧大声道,18-20帧545485
        /// </summary>
        public string Frames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<AnimStatesItem> AnimStates { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProcessPractices Practices { get; set; }

    }

    public class AnswerItemProcess
    {

        public int Index { get; set; }

        public string Text { get; set; }

    }

    public class ARModelProcess
    {


        public string SerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 水泥土桩墙工程施工工艺
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图1-18
        /// </summary>
        public string CNID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string aburl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AbObjectName { get; set; }

        /// <summary>
        /// 计算类
        /// </summary>
        public ProcessTypeCalculation TypeCalculation { get; set; }

        /// <summary>
        /// 物件类
        /// </summary>
        public ProcessTypeItem TypeItem { get; set; }

        /// <summary>
        /// 方式类
        /// </summary>
        public ProcessTypeMethod TypeMethod { get; set; }

        /// <summary>
        /// 图表类
        /// </summary>
        public ProcessTypeTable TypeTable { get; set; }

        /// <summary>
        /// 流程类
        /// </summary>
        public ProcessTypeProcess TypeProcess { get; set; }

    }

}