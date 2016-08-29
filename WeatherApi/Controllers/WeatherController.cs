using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using System.Xml;
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

        [HttpPost]
        [Route("api/Weather/XmlValidate")]
        public HttpResponseMessage XmlValidate(HttpRequestMessage request) {

            HttpStatusCode code = HttpStatusCode.OK;
            XmlValidatorClass validator = new XmlValidatorClass();
            String lReturn="";
            try
            {
                lReturn = validator.Validate(request);

            }
            catch (XmlException ex) {
                code = HttpStatusCode.BadRequest;
                lReturn = ex.Message.ToString();
            }

            return Request.CreateResponse(code,lReturn,Configuration.Formatters.JsonFormatter);
        }
     
    }
}
