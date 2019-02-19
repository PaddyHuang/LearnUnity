using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

public class GetMemoryClass : MonoBehaviour
{
    private long avaliableMemory;
    private long totalMemory;
    private GUIStyle fontStyle;
    void Start()
    {
        //获取当前系统
        //SystemInfo.operatingSystem;

        fontStyle = new GUIStyle();
        fontStyle.normal.background = null;
        fontStyle.normal.textColor = new Color(1, 1, 1);
        fontStyle.fontSize = 40;
    }

    void OnGUI()
    {
        GUILayout.Label(GetMemoryStatus(), fontStyle);       
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        //系统内存总量
        public ulong dwTotalPhys;
        //系统可用内存
        public ulong dwAvailPhys;
        public ulong dwTotalPageFile;
        public ulong dwAvailPageFile;
        public ulong dwTotalVirtual;
        public ulong dwAvailVirtual;
    }

    [DllImport("kernel32")]
    public static extern string GlobalMemoryStatus(ref MEMORY_INFO meminfo);
    //  [DllImport("User32")]
    //  public static extern void GetWindowThreadProgressld (IntPtr hwnd,out int id);
    
        /// <summary>
    /// 检测内存是否溢出
    /// </summary>
    /// <returns></returns>
    private string GetMemoryStatus()
    {
        MEMORY_INFO MemInfo;
        MemInfo = new MEMORY_INFO();
        GlobalMemoryStatus(ref MemInfo);

        avaliableMemory = Convert.ToInt64(MemInfo.dwAvailPhys.ToString()) / 1024 / 1024;
        totalMemory = Convert.ToInt64(MemInfo.dwTotalPhys.ToString()) / 1024 / 1024;

        var freeMemory = String.Format("FreeMomory: {0}/{1} MB", avaliableMemory, totalMemory);

        //Debug.Log(freeMemory);

        return freeMemory;

        //print("FreeMemory:" + Convert.ToString(avaliableMb) + " MB");
        //if (avaliableMb < 200)
        //{
        //    Debug.Log("内存不足！");
        //    //弹出内存警告
        //}
        //else
        //{
        //    Debug.Log("可以使用");
        //    //自动取消内存警告
        //    Debug.Log(Environment.WorkingSet.ToString());

        //}
    }
    
}

