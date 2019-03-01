using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuaFramework
{
    public class LoadSceneManager : Manager
    {
        private void Awake()
        {
            Util.CallMethod("CtrlManager", "InitScene", SceneManager.GetActiveScene().name);                   
        }

        /// <summary>
        /// 通过Index加载场景
        /// </summary>
        /// <param name="index"></param>
        public static void LoadSceneByIndex(int index)
        {
            SceneManager.LoadScene(index);
        }

        /// <summary>
        /// 通过Name加载场景
        /// </summary>
        /// <param name="name"></param>
        public static void LoadSceneByName(string name)
        {
            SceneManager.LoadScene(name);
        }

    }
}