using System.Net;
using System.IO;
using System.Text;

/// <summary>
/// POST网络请求
/// </summary>
public class HTTPNetTool
{
    /// <summary>
    /// 网络请求主方法
    /// </summary>
    /// <returns>返回json结果，可以通过下面Deserialize 方法转换成对象直接解析数据。</returns>
    /// <param name="url”网络接口</param>
    /// <param name="param">参数字符串，通过下面的Serialize方法，由类转换得到。</param>
    public static string PostMoths(string url, string param)
    {
        string strURL = url;
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(strURL);
        request.Timeout = 200000;
        request.Method = "POST";
        request.ContentType = "application/json;charset=UTF-8";
        string paraUrlCoded = param;
        byte[] payload;
        payload = Encoding.UTF8.GetBytes(paraUrlCoded);
        request.ContentLength = payload.Length;
        Stream writer = request.GetRequestStream();
        writer.Write(payload, 0, payload.Length);
        writer.Close();

        HttpWebResponse response;
        response = (HttpWebResponse) request.GetResponse();
        Stream s = response.GetResponseStream();
        string StrDate = "";
        string strValue = "";
        StreamReader Reader = new StreamReader(s, Encoding.UTF8);
        while ((StrDate = Reader.ReadLine()) != null)
        {
            strValue += StrDate + "\r\n";
        }
        return strValue;
    }

    ///// <summary>
    ///// 把json字符串转成对象
    ///// </summary>
    ///// <typeparam name="T">对象</typeparam>
    ///// <param name="data">json字符串</param> 
    //public static T Deserialize<T>(string data)
    //{
    //	System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
    //	return json.Deserialize<T>(data);
    //}

    ///// <summary>
    ///// 把对象转成json字符串
    ///// </summary>
    ///// <param name="o">对象</param>
    ///// <returns>json字符串</returns>
    //public static string Serialize(object o)
    //{
    //	System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //	System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
    //	json.Serialize(o, sb);
    //	return sb.ToString();
    //}
}