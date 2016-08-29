using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Xml;

namespace WeatherApi.Models
{
    public class XmlValidatorClass
    {

        public String Validate(HttpRequestMessage request) {
            var doc = new XmlDocument();
            

            try
            {
                doc.Load(request.Content.ReadAsStreamAsync().Result);
            }
            catch (XmlException) {
                throw new XmlException("Malformed XML");
          
            }


            string jsonText = JsonConvert.SerializeXmlNode(doc);


            return jsonText;
  
        }

    }
}