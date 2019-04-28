using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARModelManager.UI.Models.ARJsonModel
{
   

    public class MethodTypeCalculation
    {
    }

    public class MethodTypeItem
    {
    }

    public class MethodsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 正向挖土 反向卸土
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 正向开挖反向卸土即挖土机向前进方向挖土，汽车停在后面装土，此开挖方式因挖掘机卸土时回转角大、汽车需倒车开入，运输不方便。只适合用于基坑宽度较小、深度较大的情况。
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GameObjectName { get; set; }
    }

    public class MethodPracticeItem
    {

        public int Index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
    }

    public class MethodPractice
    {
        /// <summary>
        /// 某基坑平面尺寸如上图所示，坑深5.5m，四边均按1:0.4的坡度放坡，抗深范围内箱形基础的体积为2000m3。基坑开挖的土方量面积为#m3。
        /// </summary>
        public string TextInfo { get; set; }
        public string TaskPicName { get; set; }

        public string TaskPicUrl { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string AnswerPicName { get; set; }
        /// <summary>
        /// 答案图片路径
        /// </summary>
        /// 
        public string AnswerPicUrl { get; set; }
        public bool IsShowAnswer { get; set; }


        public List<MethodPracticeItem> PracObjects
        {

            get;
            set;

        }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Answers { get; set; }
     
    }

    public class MethodTypeMethod
    {
        /// <summary>
        /// 
        /// </summary>
        public List<MethodsItem> Methods { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MethodPractice Practices { get; set; }
    }

    public class MethodTypeTable
    {
    }

    public class MethodTypeProcess
    {
    }

    public class ArModelMethod
    {
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 正铲挖土机作业方式
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图1-16
        /// </summary>
        public string CNID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aburl { get; set; }


        /// <summary>
        /// 物体名称
        /// </summary>
       public string AbObjectName { get; set; }



        public string SerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MethodTypeCalculation TypeCalculation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MethodTypeItem TypeItem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MethodTypeMethod TypeMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MethodTypeTable TypeTable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MethodTypeProcess TypeProcess { get; set; }
    }
}