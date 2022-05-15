using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace Bibliotheek_TESTING.Views
{
    [TestFixture]
    public class CUDBookFairViewTests
    {

        //string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        //string WpfAppId = @"";

        //WindowsDriver<WindowsElement> session;

        //WindowsElement bookFairName, bookFairLocation, bookFairDescription;


        //[SetUp]
        //public void SetUp()
        //{
        //    if (session == null)
        //    {
        //        var appiumOptions = new AppiumOptions();
        //        appiumOptions.AddAdditionalCapability("app", WpfAppId);
        //        session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);


        //        bookFairName = session.FindElementByAccessibilityId("bookFairName");
        //        bookFairLocation = session.FindElementByAccessibilityId("bookFairLocation");
        //        bookFairDescription = session.FindElementByAccessibilityId("bookFairDescription");
        //    }
        //}


        //[Test]
        //public void AddBookFairInfo()
        //{
        //    // Act 
        //    bookFairName.SendKeys("De Boekenneuzen");
        //    bookFairLocation.SendKeys("Thomas More Geel");
        //    bookFairDescription.SendKeys("De Boekenneuzen was een jaarlijkse boekenbeurs in de Belgische stad Geel. Deze werd voor het eerst georganiseerd in december 1950 in de stadsfeestzaal aan de kerk. De beurs werd gehouden tijdens de maand december te Thomas More Geel en werd georganiseerd door de beroepsvereniging The Boys");
            
            
        //    Assert.AreEqual("", )
        
        
        //}


        //[TearDown]
        //public void TearDown()
        //{
        //    bookFairName.Clear();
        //    bookFairLocation.Clear();
        //    bookFairDescription.Clear();
        //}


        //[OneTimeTearDown]
        //public void CloseSession()
        //{
        //    session.Close();
        //}


    }
}
