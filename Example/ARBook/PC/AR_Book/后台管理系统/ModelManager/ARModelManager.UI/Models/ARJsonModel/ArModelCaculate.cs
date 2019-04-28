using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARModelManager.UI.Models.ARJsonModel
{
    
        public class OverallInfo
        {
            /// <summary>
            /// 基坑概念
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 基坑是指长宽比小于或等于3的矩形土体
            /// </summary>
            public string Info { get; set; }
        }

        public class Calcul
        {

        private string _audio;
            /// <summary>
            /// Index
            /// </summary>
        public int Index { get; set; }
            /// <summary>
            /// 基坑上底的面积,m²
            /// </summary>
            public string Info { get; set; }
            /// <summary>
            /// LeftOrRight
            /// </summary>
            public bool LeftOrRight { get; set; }
            /// <summary>
            /// Ceng3
            /// </summary>
            public string GameObjectName { get; set; }
            /// <summary>
            /// A1
            /// </summary>
            public string GameObjectIndex { get; set; }
       
            public string Audio
            {
            get;
            set;
            }
        }

        public class TypeSizes
        {
            /// <summary>
            /// Index
            /// </summary>
            public int Index { get; set; }
            /// <summary>
            /// H
            /// </summary>
            public string Length { get; set; }
            /// <summary>
            /// Size1
            /// </summary>
            public string FromGoName { get; set; }
            /// <summary>
            /// Size2
            /// </summary>
            public string ToGaName { get; set; }

             public string Audio { get; set; }
    }

        public class Calculations
        {
            /// <summary>
            /// 土方量计算
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 基坑土方量计算公式：V=H/6(A1+4A0+A2)
            /// </summary>
            public string Info { get; set; }
            /// <summary>
            /// Calcul
            /// </summary>
            public List<Calcul> Calcul { get; set; }
            /// <summary>
            /// Sizes
            /// </summary>
            public List<TypeSizes> Sizes { get; set; }
        }



    public class AnswerItem {
        public int Index { get; set; }
        public string Answer { get; set; }
    }
        public class Practice
        {
            /// <summary>
            /// 某基坑平面尺寸如上图所示，坑深5.5m，四边均按1:0.4的坡度放坡，抗深范围内箱形基础的体积为2000m3。基坑开挖的土方量面积为#m3。
            /// </summary>
            public string TextInfo { get; set; }
            /// <summary>
            /// task
            /// </summary>
            public string TaskPicName { get; set; }
            /// <summary>
            /// 答题图片
            /// </summary>
            public string AnswerPicUrl { get; set; }
            /// <summary>
            /// 习题示意图
            /// </summary>
            public string TaskPicUrl { get; set; }
            /// <summary>
            /// answer
            /// </summary>
            public string AnswerPicName { get; set; }
            /// <summary>
            /// IsShowAnswer
            /// </summary>
            public bool IsShowAnswer { get; set; }


            public List<AnswerItem> Answers
            {
                    get;
                    set;
            }
        }

        public class CaculTypeCalculation
    {
            /// <summary>
            /// OverallInfo
            /// </summary>
            public OverallInfo OverallInfo { get; set; }
            /// <summary>
            /// Calculations
            /// </summary>
            public Calculations Calculations { get; set; }
            /// <summary>
            /// Practice
            /// </summary>
            public Practice Practices { get; set; }
        }

        public class CaculTypeItem
    {
        }

        public class CaculTypeMethod
        {
        }

        public class CaculTypeTable
    {
        }

        public class CaculTypeProcess
    {
        }

        public class RootARCaculate
        {
            /// <summary>
            /// JiSuan
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 基坑土方量计算
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 序号
            /// </summary>
            public string SerId { get; set; }
            /// <summary>
            /// 图1-12
            /// </summary>
            public string CNID { get; set; }
            /// <summary>
            /// /AssetBundles/testjisuan
            /// </summary>
            public string aburl { get; set; }
            /// <summary>
            /// JiSuanTestGo
            /// </summary>
            public string AbObjectName { get; set; }
            /// <summary>
            /// TypeCalculation
            /// </summary>
            public CaculTypeCalculation TypeCalculation { get; set; }
            /// <summary>
            /// TypeItem
            /// </summary>
            public CaculTypeItem TypeItem { get; set; }
            /// <summary>
            /// TypeMethod
            /// </summary>
            public CaculTypeMethod TypeMethod { get; set; }
            /// <summary>
            /// TypeTable
            /// </summary>
            public CaculTypeTable TypeTable { get; set; }
            /// <summary>
            /// TypeProcess
            /// </summary>
            public CaculTypeProcess TypeProcess { get; set; }
        }

    
}