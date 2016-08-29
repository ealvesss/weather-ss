using HtmlAgilityPack;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using WeatherApi.Common;

namespace WeatherApi.Models
{
    public static class WeatherClass 
    {

        public static List<Country> FakeDataBase { get; set; }

        public static void GetByName(string city)
        {

            try
            {


            }


            catch (Exception) {
                throw;
            }

            //return null;    
        }

        

            public static WeatherParamOut ReturnWeather(WeatherParamIn paramIn) {

               Util util = new Util();
               WeatherParamOut lReturn = new WeatherParamOut();

            try
            {

                var country = (Country)FakeDataBase.Where(x => x.Desc.ToLower().Contains(paramIn.Country.ToLower())).FirstOrDefault();
                var city = (City)country.Regions.SelectMany(x => x.Cities).FirstOrDefault(y => y.Desc.ToLower() == paramIn.City.ToLower());

                HtmlDocument doc = util.ReturnWeather("http://www.accuweather.com/en/" + country.Id + "/city/" + city.Id_I + "/weather-forecast/" + city.Id_II);

                HtmlNode teste = doc.DocumentNode.SelectSingleNode("//li[@id='feed-main']//div[@class='info']");


                lReturn.Cond = teste.SelectSingleNode("//span[@class='cond']").InnerText;
                lReturn.Temp = teste.SelectSingleNode("//strong[@class='temp']").InnerText;
                lReturn.Country = country.Desc;
                lReturn.City = city.Desc;
                lReturn.dtTimeSearch = DateTime.Now.ToString();


                //Converting Obj to Json
                // JavaScriptSerializer serializer = new JavaScriptSerializer();
                // serializer.MaxJsonLength = Int32.MaxValue;

            }
            catch (NullReferenceException)
            {
                throw new Exception("Data not Found");
            }
            catch (ArgumentNullException) {
                throw new Exception("Data not Found");
            }
                return lReturn;
           }


        public static void  FillData()
        {
            Util util = new Util();
            HtmlDocument doc = new HtmlDocument();
            List<Country> countries = new List<Country>();
            String url = "http://www.accuweather.com/en/browse-locations/eur";
            doc = util.ReturnWeather(url);
           
            #if DEBUG
                TelemetryConfiguration.Active.DisableTelemetry = true;
            #endif
           
            //Parsing the country [in this case, Europe]
            doc.DocumentNode.SelectNodes("//div[@class='info']/h6").AsParallel()
               .WithDegreeOfParallelism(doc.DocumentNode.SelectNodes("//div[@class='info']/h6").Count())
               .ForAll(x =>
                            {
                                Country countryObj = new Country();
                                countryObj.Regions = new List<Region>();                               

                                if (!x.InnerText.Trim().Contains("Europe Weather"))
                                {

                                    countryObj.Desc = x.InnerText.Trim();
                                    countryObj.Id = x.LastChild.Attributes["href"].Value.Split(new char[] { '/' })[6];

                                    HtmlDocument regionDoc = util.ReturnWeather(url + "/" + countryObj.Id);

                                    //Parsing the Regions of the Countries
                                    foreach (HtmlNode region in regionDoc.DocumentNode.SelectNodes("//div[@class='info']/h6"))
                                    {
                                        Region regionObj = new Region();
                                        regionObj.Cities = new List<City>();

                                        if (!region.InnerText.Trim().Contains(region.InnerText.Trim().Replace(" Weather", "") + " Weather"))
                                        {
                                            regionObj.Desc = region.InnerText.Trim();
                                            regionObj.Id = region.LastChild.Attributes["href"].Value.Split(new char[] { '/' })[7];

                                            HtmlDocument cityDoc = util.ReturnWeather(url + "/" + countryObj.Id + "/" + regionObj.Id);

                                            try
                                            {
                                                //Parsing Cities
                                                foreach (HtmlNode city in cityDoc.DocumentNode.SelectNodes("//div[@class='info']/h6"))
                                                {
                                                    if (!city.InnerText.Trim().Contains(city.InnerText.Trim().Replace(" Weather", "") + " Weather"))
                                                    {
                                                        City cityObj = new City();
                                                        cityObj.Desc = city.InnerText.Trim();
                                                        cityObj.Id_I = city.LastChild.Attributes["href"].Value.Split(new char[] { '/' })[6];
                                                        cityObj.Id_II = city.LastChild.Attributes["href"].Value.Split(new char[] { '/' })[8];
                                                        regionObj.Cities.Add(cityObj);
                                                    }
                                                }
                                            }
                                            catch (NullReferenceException)
                                            {
                                                continue;
                                            }
                                            countryObj.Regions.Add(regionObj);
                                        }
                                    }
                                    countries.Add(countryObj);
                                }

                            });


            

            FakeDataBase = countries;
        }
    }
 }

public class Country
{
    public String Desc { get; set; }
    public String Id { get; set; }
    public List<Region> Regions { get; set; }
}
    public class Region {
        public String Desc { get; set;}
        public String Id { get; set; }
        public List<City> Cities { get; set; }

    }

    public class City {
        public String Desc { get; set; }
        public String Id_I { get; set; }
        public String Id_II { get; set; }
}


    public class WeatherParamIn {
       public String Country { get; set; }
       public String City { get; set; }
    }

    public class WeatherParamOut
    {
        public String Country { get; set; }
        public String City { get; set; }
        public String Cond { get; set; }
        public String Temp { get; set; }
        public String dtTimeSearch { get; set; }
    }


