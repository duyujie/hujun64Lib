using System;
using System.Text;
using com.hujun64.type;
using com.hujun64.util;

namespace com.hujun64.po
{
    /// <summary>
    ///Weather 的摘要说明
    /// </summary>
    public class Weather
    {
        /*******************************
         * 一维字符串数组 String()，结构如下： 
         * Array(0) = "省份 地区/洲 国家名（国外）" 
         * Array(1) = "查询的天气预报地区名称" 
         * Array(2) = "查询的天气预报地区ID" 
         * Array(3) = "最后更新时间 格式：yyyy-MM-dd HH:mm:ss" 
         * Array(4) = "当前天气实况：气温、风向/风力、湿度" 
         * Array(5) = "第一天 空气质量、紫外线强度" 
         * Array(6) = "第一天 天气和生活指数" 
         * Array(7) = "第一天 概况 格式：M月d日 天气概况" 
         * Array(8) = "第一天 气温" 
         * Array(9) = "第一天 风力/风向" 
         * Array(10) = "第一天 天气图标 1" 
         * Array(11) = "第一天 天气图标 2" 
         * Array(12) = "第二天 概况 格式：M月d日 天气概况" 
         * Array(13) = "第二天 气温" 
         * Array(14) = "第二天 风力/风向" 
         * Array(15) = "第二天 天气图标 1"
         * Array(16) = "第二天 天气图标 2" ...... ......
         * 
         * 每一天的格式同：
         * Array(12) -- Array(16) ...... Array(n-4) = "最后一天 概况 格式：M月d日 天气概况" 
         * Array(n-3) = "最后一天 气温" 
         * Array(n-2) = "最后一天 风力/风向" 
         * Array(n-1) = "最后一天 天气图标 1" 
         * Array(n) = "最后一天 天气图标 2"
         * 
         *************************************/

        public string cityName;
        public string reportDatetime;
        public string todayWeather;

        private readonly int dayMin = 1, dayMax = 5;
        private DayWeather[] weatherDayArray=null;



        private readonly int indexCityName = 1, indexReportDatetime = 3,indexTodaysWeather=4, indexFirstDay = 7;
        private readonly int relativeSummary = 0, relativeTemperature = 1, relativeWind = 2, relativeIcon1 = 3, relativeIcon2 = 4;
        private readonly string iconPath = "images/weather/";

        public class DayWeather
        {
            public string summary;
            public string temperature;
            public string wind;
            public string icon1;
            public string icon2;
            public int relativeDay;
            

            public string ToHtml()
            {
                return ToHtml(true, false);
            }
            public string ToHtml(bool isWithDate)
            {
                return ToHtml(isWithDate, false);
            }
            public string ToHtml(bool isWithDate,bool isLineBreak)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(UtilHtml.GetFullImageUrl(icon1));
                sb.Append(UtilHtml.GetFullImageUrl(icon2));
                if (!isWithDate && !string.IsNullOrEmpty(summary))
                    sb.Append(summary.Substring(summary.IndexOf(" ")));
                else
                    sb.Append(summary);

                sb.Append(" ");
                sb.Append(temperature);

                if(isLineBreak)
                    sb.Append("<br />");
                else
                    sb.Append(" ");
                
                sb.Append(wind);

                return sb.ToString();
            }
        }

        public Weather()
        {
        }
        public Weather(string[] weatherStringArray)
        {
            if (weatherStringArray == null || weatherStringArray.Length < indexTodaysWeather)
                return;

            cityName = weatherStringArray[indexCityName];
            reportDatetime = weatherStringArray[indexReportDatetime];
            todayWeather = weatherStringArray[indexTodaysWeather];


            weatherDayArray = new DayWeather[dayMax];

            for (int iDay = dayMin-1; iDay < dayMax; iDay++)
            {
                try
                {
                    int thisIndexDayBegin = iDay + indexFirstDay;

                    DayWeather dayWeather = new DayWeather();
                    dayWeather.relativeDay = iDay;
                    dayWeather.summary = weatherStringArray[thisIndexDayBegin + relativeSummary];
                    dayWeather.temperature = weatherStringArray[thisIndexDayBegin + relativeTemperature];
                    dayWeather.wind = weatherStringArray[thisIndexDayBegin + relativeWind];
                    dayWeather.icon1 = iconPath + weatherStringArray[thisIndexDayBegin + relativeIcon1];
                    dayWeather.icon2 = iconPath + weatherStringArray[thisIndexDayBegin + relativeIcon2];


                    weatherDayArray[iDay] = dayWeather;
                }
                catch (System.IndexOutOfRangeException)
                {
                    break;
                }
            }



        }

        public DayWeather GetDayWeather(int dayIndex)
        {
            if (weatherDayArray != null && weatherDayArray.Length > dayIndex)
                return weatherDayArray[dayIndex];
            else
                return new DayWeather();
        }
    }
}