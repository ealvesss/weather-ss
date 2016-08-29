using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherApi.Controllers;
using WeatherApi.Models;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Xml;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void NoDataFound()
        {
            WeatherParamIn paramIn = new WeatherParamIn();
            String lReturn = "";
            paramIn.Country = "nonono";
            paramIn.City = "nono";

            try
            {

                WeatherParamOut obj = WeatherClass.ReturnWeather(paramIn);
            }
            catch (Exception ex)
            {
                lReturn = ex.Message.ToString();
            }
            Assert.AreEqual(lReturn, "Data not Found");

        }

        [TestMethod]
        public void DataOk()
        {

            WeatherParamIn paramIn = new WeatherParamIn();
            WeatherParamOut paramOut = new WeatherParamOut();

            paramIn.Country = "denmark";
            paramIn.City = "Aarhus";


            String notFound = "";
            if (WeatherClass.FakeDataBase == null)
            {
                WeatherClass.FillData();
            }

            try
            {
                paramOut = WeatherClass.ReturnWeather(paramIn);
            }
            catch (Exception ex)
            {
                notFound = ex.Message.ToString();
            }

            Assert.AreNotEqual(paramOut.City, "");
            Assert.AreNotEqual(paramOut.Country, "");
            Assert.AreNotEqual(paramOut.dtTimeSearch, "");
            Assert.AreNotEqual(paramOut.Temp, "");
            Assert.AreNotEqual(paramOut.Cond, "");
        }


        [TestMethod]
        public void ValidXML()
        {
            XmlValidatorClass xmlValid = new XmlValidatorClass();
            HttpRequestMessage request = new HttpRequestMessage();


            request.Content = new StringContent("<html>Teste</html>");

            String data = xmlValid.Validate(request);


            Assert.IsFalse(data.Length == 0);
        }

        [TestMethod]
        public void InvalidXML()
        {
            XmlValidatorClass xmlValid = new XmlValidatorClass();
            HttpRequestMessage request = new HttpRequestMessage();
            String data="";

            request.Content = new StringContent("<html>Teste</html>#####");

            try
            {
               data = xmlValid.Validate(request);
            }
            catch (XmlException ex) {
                data = ex.Message.ToString();
            }

            Assert.AreEqual(data, "Malformed XML");
        }
    }
}
