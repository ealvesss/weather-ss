using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    public class WeatherController : ApiController
    {

        

        // GET: api/Weather/5
        public HttpResponseMessage Get(string country, string city)
        {
            WeatherParamIn paramIn = new WeatherParamIn();
            WeatherParamOut paramOut = new WeatherParamOut();

            paramIn.Country = country;
            paramIn.City = city;
            String notFound = "";

            if (WeatherClass.FakeDataBase == null) {
                WeatherClass.FillData();
            }


            try
            {
                paramOut = WeatherClass.ReturnWeather(paramIn);
            }
            catch (Exception ex) {
                notFound = ex.Message.ToString();
            }

            //Converting Obj to Json
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var result = notFound.Length > 0 ? notFound : serializer.Serialize(paramOut);

            return Request.CreateResponse(HttpStatusCode.OK,result, Configuration.Formatters.JsonFormatter);
        }

     
    }
}
