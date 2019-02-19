using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Utils
{
    public class UtilsFiles
    {
        public static string GetFileHash(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                
                int len = (int) fs.Length;
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();

                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(data);

                StringBuilder sb = new StringBuilder();

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e.Message);
                return "";
            }
        }
    }

    public class UtilsDownload
    {
        /// <summary> 
        /// 下载多张图片 </summary>
        public static IEnumerator DownloadImages(string[] urls, Action<List<Sprite>> result)
        {
            //todo:错误码
            if (urls.Length < 1)
                yield break;

            if (urls.Contains(String.Empty))
                yield break;

            List<Sprite> sps = new List<Sprite>();

            for (int i = 0; i < urls.Length; i++)
            {
                string noUniStr = urls[i];

                WWW www = new WWW(noUniStr);

                while (!www.isDone)
                {
                    yield return null;
                }

                if (!www.isDone || !String.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("Load Failed");
                    result(null); // Pass null result.
                    yield break;
                }

                Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);

                // assign the downloaded image to sprite
                www.LoadImageIntoTexture(texture);
                Rect rec = new Rect(0, 0, texture.width, texture.height);
                Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

                //split name
                string[] spg = noUniStr.Split('/');

                spriteToUse.name = spg[spg.Length - 1];

                sps.Add(spriteToUse);

                www.Dispose();
            }

            result(sps);
        }
    }

    public class UtilsSearch
    {
        /// <summary>
        /// 按名字递归查找所有子物体
        /// </summary>
        /// <param name="paTarget">查找的父物体</param>
        /// <param name="goName">需要查找的名字</param>
        /// <returns></returns>
        public static GameObject RecursiveFindChild(Transform paTarget, string goName)
        {
            var go = paTarget.Find(goName);
            if (ReferenceEquals(go, null))
            {
                GameObject g;
                foreach (Transform child in paTarget)
                {
                    g = RecursiveFindChild(child, goName);
                    if (!ReferenceEquals(g, null))
                        return g.gameObject;
                }
            }

            return go?.gameObject;
        }

        /// <summary> 
        /// 在所有子物体中查找组件 </summary>
        public static T FindInChild<T>(GameObject go) where T : Component
        {
            if (go == null)
                return null;
            var comp = go.GetComponent<T>();

            if (comp != null)
                return comp;

            foreach (Transform c in go.transform)
            {
                comp = c.GetComponent<T>();
                if (comp != null)
                    break;

                var hasChild = c.childCount;

                if (hasChild != 0)
                    comp = FindInChild<T>(c.gameObject);
            }

            return comp;
        }

        /// <summary> 
        /// 在父物体中查找组件 </summary>
        public static T FindInParents<T>(GameObject go) where T : Component
        {
            if (go == null)
                return null;
            var comp = go.GetComponent<T>();

            if (comp != null)
                return comp;

            var t = go.transform.parent;
            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }

            return comp;
        }

        /// <summary>
        /// 递归查找子物体/获取子物体组件
        /// </summary> 
        public static Transform FindChildByName(Transform parent, string name)
        {
            Transform child = null;
            child = parent.Find(name);
            if (child != null)
                return child;
            Transform grandchild = null;
            for (int i = 0; i < parent.childCount; i++)
            {
                grandchild = FindChildByName(parent.GetChild(i), name);
                if (grandchild != null)
                    return grandchild;
            }
            return null;
        }

        public static T FindComponentChild<T>(Transform parent, string name) where T : Component
        {
            Transform child = null;
            child = FindChildByName(parent, name);
            if (child != null)
                return child.GetComponent<T>();
            return null;
        }        
    }

    public class AssetBundleLoader
    {
        public static GameObject LoadAssetBundles(string path, string name)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            return ab.LoadAsset<GameObject>(name);
        }

        public static void UnLoadAll()
        {
            AssetBundle.UnloadAllAssetBundles(true);
        }

    }
}