using System;
using System.Text;
using com.hujun64.type;
using com.hujun64.util;

namespace com.hujun64.po
{
    /// <summary>
    ///Weather ��ժҪ˵��
    /// </summary>
    public class Weather
    {
        /*******************************
         * һά�ַ������� String()���ṹ���£� 
         * Array(0) = "ʡ�� ����/�� �����������⣩" 
         * Array(1) = "��ѯ������Ԥ����������" 
         * Array(2) = "��ѯ������Ԥ������ID" 
         * Array(3) = "������ʱ�� ��ʽ��yyyy-MM-dd HH:mm:ss" 
         * Array(4) = "��ǰ����ʵ�������¡�����/������ʪ��" 
         * Array(5) = "��һ�� ����������������ǿ��" 
         * Array(6) = "��һ�� ����������ָ��" 
         * Array(7) = "��һ�� �ſ� ��ʽ��M��d�� �����ſ�" 
         * Array(8) = "��һ�� ����" 
         * Array(9) = "��һ�� ����/����" 
         * Array(10) = "��һ�� ����ͼ�� 1" 
         * Array(11) = "��һ�� ����ͼ�� 2" 
         * Array(12) = "�ڶ��� �ſ� ��ʽ��M��d�� �����ſ�" 
         * Array(13) = "�ڶ��� ����" 
         * Array(14) = "�ڶ��� ����/����" 
         * Array(15) = "�ڶ��� ����ͼ�� 1"
         * Array(16) = "�ڶ��� ����ͼ�� 2" ...... ......
         * 
         * ÿһ��ĸ�ʽͬ��
         * Array(12) -- Array(16) ...... Array(n-4) = "���һ�� �ſ� ��ʽ��M��d�� �����ſ�" 
         * Array(n-3) = "���һ�� ����" 
         * Array(n-2) = "���һ�� ����/����" 
         * Array(n-1) = "���һ�� ����ͼ�� 1" 
         * Array(n) = "���һ�� ����ͼ�� 2"
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