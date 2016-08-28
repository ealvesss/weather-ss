using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WeatherApi.Common
{
    public class Util
    {
        private static String url_ = "http://www.accuweather.com/pt/dk/city/";

        // Aarhus
        //124594
        public HtmlDocument ReturnWeather(String url)
        {
            HtmlDocument lReturn = new HtmlDocument();
            StreamReader streamReader;
            System.Text.Encoding encoding = null;
            HttpWebResponse response = null;
            WebRequest request = null;
          
            try
            {
                request = WebRequest.Create(url);
                request.Method ="GET";
                response = (HttpWebResponse)request.GetResponse();
                encoding = System.Text.Encoding.GetEncoding("UTF-8");
                streamReader = new StreamReader(response.GetResponseStream(), encoding);

                lReturn.LoadHtml(HttpUtility.HtmlDecode(streamReader.ReadToEnd()));
                streamReader.Close();
                request = null;
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                streamReader = null;
                encoding = null;
                response = null;
                request = null;
            }
            return lReturn;
        }

    }
}