using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Weather : MonoBehaviour {

    private const string KEY = "3d01bb5391eb4ad796420cc68647d849";
    private string LOCATION = "xixiangtang";
    AssetBundle ab;

    WeatherInfo weather_info;
    AirQuality airQuality;
        
    // 全景页--简版天气信息
    public Text location_simple;
    public Text tmp_simple;
    public Text date_simple;
    public Image image_simple;

    // 天气页--实况天气信息
    public Text location_now;
    public Text tmp_now;
    public Text cond_txt_now;
    public Image image_now;
    public Text TmpMax_Min_now;

    // 天气页--空气质量
    public Text text_air;
    
    // 天气页--逐小时天气信息
    //  Hour1
    public Text time_hour1;
    public Image image_hour1;
    public Text temp_hour1;
    //  Hour2
    public Text time_hour2;
    public Image image_hour2;
    public Text temp_hour2;
    //  Hour3
    public Text time_hour3;
    public Image image_hour3;
    public Text temp_hour3;
    //  Hour4
    public Text time_hour4;
    public Image image_hour4;
    public Text temp_hour4;
    //  Hour5
    public Text time_hour5;
    public Image image_hour5;
    public Text temp_hour5;
    //  Hour6
    public Text time_hour6;
    public Image image_hour6;
    public Text temp_hour6;

    // 天气页--未来7天天气信息
    // Day1
    public Text text_day1;
    public Image image_day1;
    public Text temp_day1;
    // Day2
    public Text text_day2;
    public Image image_day2;
    public Text temp_day2;
    // Day3
    public Text text_day3;
    public Image image_day3;
    public Text temp_day3;
    // Day4
    public Text text_day4;
    public Image image_day4;
    public Text temp_day4;
    // Day5
    public Text text_day5;
    public Image image_day5;
    public Text temp_day5;
    // Day6
    public Text text_day6;
    public Image image_day6;
    public Text temp_day6;
    // Day7
    public Text text_day7;
    public Image image_day7;
    public Text temp_day7;

    // Use this for initialization
    void Start () {    
        StartCoroutine(UIAssignment());
	}
	
    IEnumerator UIAssignment()
    {
        if (weather_info == null)
        {
            yield return GetWeatherJson();
        }
        if (airQuality == null)
        {
            yield return GetAirJson();
        }
        if (ab == null)
        {
            yield return LoadWeatherAssets();
        }

        // 全景页--简版天气信息
        location_simple.text = weather_info.HeWeather6[0].basic.location;
        tmp_simple.text = string.Format("{0}°C", weather_info.HeWeather6[0].now.tmp);

        string[] date = weather_info.HeWeather6[0].update.loc.Split('-', ' ');
        date_simple.text = string.Format("{0}/{1}", date[1], date[2]);                
        ChangeWeatherImage(image_simple, cond_txt_now.text);

        // 天气页--实况天气信息
        location_now.text = weather_info.HeWeather6[0].basic.location;
        tmp_now.text = string.Format("{0}°", weather_info.HeWeather6[0].now.tmp);
        cond_txt_now.text = weather_info.HeWeather6[0].now.cond_txt;
        ChangeWeatherImage(image_now, cond_txt_now.text);
        TmpMax_Min_now.text = string.Format("{0}°/{1}°", weather_info.HeWeather6[0].daily_forecast[0].tmp_max, weather_info.HeWeather6[0].daily_forecast[0].tmp_min);

        // 天气页--逐小时天气信息
        // Hour1          
        time_hour1.text = "Now";
        ChangeWeatherImage(image_hour1, weather_info.HeWeather6[0].now.cond_txt);
        temp_hour1.text = weather_info.HeWeather6[0].now.tmp;
        //Hour2
        time_hour2.text = weather_info.HeWeather6[0].hourly[0].time.Remove(0, 11);
        ChangeWeatherImage(image_hour2, weather_info.HeWeather6[0].hourly[0].cond_txt);        
        temp_hour2.text = weather_info.HeWeather6[0].hourly[0].tmp;
        // Hour3
        time_hour3.text = weather_info.HeWeather6[0].hourly[1].time.Remove(0, 11);
        ChangeWeatherImage(image_hour3, weather_info.HeWeather6[0].hourly[1].cond_txt);
        temp_hour3.text = weather_info.HeWeather6[0].hourly[1].tmp;
        // Hour4
        time_hour4.text = weather_info.HeWeather6[0].hourly[2].time.Remove(0, 11);
        ChangeWeatherImage(image_hour4, weather_info.HeWeather6[0].hourly[2].cond_txt);
        temp_hour4.text = weather_info.HeWeather6[0].hourly[2].tmp;
        // Hour5
        time_hour5.text = weather_info.HeWeather6[0].hourly[3].time.Remove(0, 11);
        ChangeWeatherImage(image_hour5, weather_info.HeWeather6[0].hourly[3].cond_txt);
        temp_hour5.text = weather_info.HeWeather6[0].hourly[3].tmp;
        // Hour6
        time_hour6.text = weather_info.HeWeather6[0].hourly[4].time.Remove(0, 11);
        ChangeWeatherImage(image_hour6, weather_info.HeWeather6[0].hourly[4].cond_txt);
        temp_hour6.text = weather_info.HeWeather6[0].hourly[4].tmp;

        // 天气页--未来7天天气信息
        // Day1
        text_day1.text = "今天";
        ChangeWeatherImage(image_day1, weather_info.HeWeather6[0].daily_forecast[0].cond_txt_d);
        temp_day1.text = string.Format("{0}°/{1}°", weather_info.HeWeather6[0].daily_forecast[0].tmp_max, weather_info.HeWeather6[0].daily_forecast[0].tmp_min);
        // Day2
        text_day2.text = "明天";
        ChangeWeatherImage(image_day2, weather_info.HeWeather6[0].daily_forecast[1].cond_txt_d);        
        temp_day2.text = string.Format("{0}°/{1}°", weather_info.HeWeather6[0].daily_forecast[1].tmp_max, weather_info.HeWeather6[0].daily_forecast[1].tmp_min);
        // Day3
        text_day3.text = weather_info.HeWeather6[0].daily_forecast[2].date.Remove(0, 5);
        ChangeWeatherImage(image_day3, weather_info.HeWeather6[0].daily_forecast[2].cond_txt_d);        
        temp_day3.text = string.Format("{0}°/{1}°", weather_info.HeWeather6[0].daily_forecast[2].tmp_max, weather_info.HeWeather6[0].daily_forecast[2].tmp_min);
        // Day4
        text_day4.text = weather_info.HeWeather6[0].daily_forecast[3].date.Remove(0, 5);
        ChangeWeatherImage(image_day4, weather_info.HeWeather6[0].daily_forecast[3].cond_txt_d);
        temp_day4.text = string.Format("{0}°/{1}°", weather_info.HeWeather6[0].daily_forecast[3].tmp_max, weather_info.HeWeather6[0].daily_forecast[3].tmp_min);
        // Day5
        text_day5.text = weather_info.HeWeather6[0].daily_forecast[4].date.Remove(0, 5);
        ChangeWeatherImage(image_day5, weather_info.HeWeather6[0].daily_forecast[4].cond_txt_d);
        temp_day5.text = string.Format("{0}°/{1}°", weather_info.HeWeather6[0].daily_forecast[4].tmp_max, weather_info.HeWeather6[0].daily_forecast[4].tmp_min);
        // Day6
        text_day6.text = weather_info.HeWeather6[0].daily_forecast[5].date.Remove(0, 5);
        ChangeWeatherImage(image_day6, weather_info.HeWeather6[0].daily_forecast[5].cond_txt_d);
        temp_day6.text = string.Format("{0}°/{1}°", weather_info.HeWeather6[0].daily_forecast[5].tmp_max, weather_info.HeWeather6[0].daily_forecast[5].tmp_min);
        // Day7
        text_day7.text = weather_info.HeWeather6[0].daily_forecast[6].date.Remove(0, 5);
        ChangeWeatherImage(image_day7, weather_info.HeWeather6[0].daily_forecast[6].cond_txt_d);
        temp_day7.text = string.Format("{0}°/{1}°", weather_info.HeWeather6[0].daily_forecast[6].tmp_max, weather_info.HeWeather6[0].daily_forecast[6].tmp_min);

        // 天气页--空气质量
        text_air.text = string.Format("{0}·{1}", airQuality.HeWeather6[0].air_now_city.aqi, airQuality.HeWeather6[0].air_now_city.qlty);
    }

    // 改变天气图标
    private void ChangeWeatherImage(Image image, string cond_txt)
    {
        if (cond_txt == "多云")
        {
            image.sprite = ab.LoadAsset<Sprite>("duoyun_da");
            //image.sprite = Resources.Load<Sprite>("Weather/duoyun_da");
        }
        else if (cond_txt == "晴")
        {
            image.sprite = ab.LoadAsset<Sprite>("qing_zhong");
            //image.sprite = Resources.Load<Sprite>("Weather/qing_zhong");
        }
        else if (cond_txt == "多云转晴")
        {
            image.sprite = ab.LoadAsset<Sprite>("duoyunzhuanqing_da");
            //image.sprite = Resources.Load<Sprite>("Weather/duoyunzhuanqing_da");
        }
        else
        {
            image.sprite = ab.LoadAsset<Sprite>("duoyun_da");
            //image.sprite = Resources.Load<Sprite>("Weather/duoyun_da");
        }
    }

    IEnumerator LoadWeatherAssets()
    {
        string path = @"Assets/AssetBundles/weathericons.ab";
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
        yield return request;
        ab = request.assetBundle;
    }

    IEnumerator GetWeatherJson()
    {
        string url = string.Format("https://free-api.heweather.com/s6/weather?location={0}&key={1}", LOCATION, KEY);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Done!");
            }
            //Debug.Log(www.downloadHandler.text);
            weather_info = GetData<WeatherInfo>(www.downloadHandler.text);                        
        }
    }

    IEnumerator GetAirJson()
    {
        string url = string.Format("https://free-api.heweather.com/s6/air/now?location=nanning&key={0}", KEY);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Done!");
            }
            //Debug.Log(www.downloadHandler.text);
            airQuality = GetData<AirQuality>(www.downloadHandler.text);           
        }
    }

    // LitJson解析返回的Json
    public static T GetData<T>(string text) where T : class
    {
        T t = JsonMapper.ToObject<T>(text);
        //T t = JsonConvert.DeserializeObject<T>(text);
        return t;
    }

    // Json内部数据类--天气信息
    public class basic
    {
        public string cid { get; set; }
        public string location { get; set; }
        public string parent_city { get; set; }
        public string admin_area { get; set; }
        public string cnty { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string tz { get; set; }
    }

    public class update
    {
        public string loc { get; set; }
        public string utc { get; set; }
    }

    public class now
    {
        public string cloud { get; set; }
        public string cond_code { get; set; }
        public string cond_txt { get; set; }
        public string fl { get; set; }
        public string hum { get; set; }
        public string pcpn { get; set; }
        public string pres { get; set; }
        public string tmp { get; set; }
        public string vis { get; set; }
        public string wind_deg { get; set; }
        public string wind_dir { get; set; }
        public string wind_sc { get; set; }
        public string wind_spd { get; set; }
    }

    public class daily_forecast
    {
        public string cond_code_d { get; set; }
        public string cond_code_n { get; set; }
        public string cond_txt_d { get; set; }
        public string cond_txt_n { get; set; }
        public string date { get; set; }
        public string hum { get; set; }
        public string mr { get; set; }
        public string ms { get; set; }
        public string pcpn { get; set; }
        public string pop { get; set; }
        public string pres { get; set; }
        public string sr { get; set; }
        public string ss { get; set; }
        public string tmp_max { get; set; }
        public string tmp_min { get; set; }
        public string uv_index { get; set; }
        public string vis { get; set; }
        public string wind_deg { get; set; }
        public string wind_dir { get; set; }
        public string wind_sc { get; set; }
        public string wind_spd { get; set; }
    }

    public class hourly
    {
        public string cloud { get; set; }
        public string cond_code { get; set; }
        public string cond_txt { get; set; }
        public string dew { get; set; }
        public string hum { get; set; }
        public string pop { get; set; }
        public string pres { get; set; }
        public string time { get; set; }
        public string tmp { get; set; }
        public string wind_deg { get; set; }
        public string wind_dir { get; set; }
        public string wind_sc { get; set; }
        public string wind_spd { get; set; }
    }

    public class lifestyle
    {
        public string type { get; set; }
        public string brf { get; set; }
        public string txt { get; set; }
    }

    public class heWeather6
    {
        public basic basic { get; set; }
        public update update { get; set; }
        public string status { get; set; }
        public now now { get; set; }
        public List<daily_forecast> daily_forecast { get; set; }
        public List<hourly> hourly { get; set; }
        public List<lifestyle> lifestyle { get; set; }
    }

    public class WeatherInfo
    {
        public List<heWeather6> HeWeather6 { get; set; }
    }

    // Json内部数据类--空气质量
    public class air_now_city
    {
        public string aqi { get; set; }
        public string co { get; set; }
        public string main { get; set; }
        public string no2 { get; set; }
        public string o3 { get; set; }
        public string pm10 { get; set; }
        public string pm25 { get; set; }
        public string pub_time { get; set; }
        public string qlty { get; set; }
        public string so2 { get; set; }
    }

    public class air_now_station
    {
        public string air_sta { get; set; }
        public string aqi { get; set; }
        public string asid { get; set; }
        public string co { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string main { get; set; }
        public string no2 { get; set; }
        public string o3 { get; set; }
        public string pm10 { get; set; }
        public string pm25 { get; set; }
        public string pub_time { get; set; }
        public string qlty { get; set; }
        public string so2 { get; set; }
    }
       
    public class HeWeather6
    {
        public air_now_city air_now_city { get; set; }
        public List<air_now_station> air_now_station { get; set; }
        public basic basic { get; set; }
        public string status { get; set; }
        public update update { get; set; }
    }

    public class AirQuality
    {
        public List<HeWeather6> HeWeather6 { get; set; }
    }

}
