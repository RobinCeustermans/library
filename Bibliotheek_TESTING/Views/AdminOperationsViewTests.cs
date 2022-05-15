using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bibliotheek_TESTING.Views
{
    [TestFixture]
    public class AdminOperationsViewTests
    {
        string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        string WpfAppId = @"C:\School2\Project-Agile-Gevorderde\backup\2021-GD-Bibliotheek-The_Boys\The_Boys_Project\bin\Debug\The_Boys_Project.exe";

        WindowsDriver<WindowsElement> session;

        WindowsElement AccountviewBtn, txtUserEmail, txtUserPassword, loginBtn;

        [SetUp]
        public void Setup()
        {
            if (session == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", WpfAppId);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
                AccountviewBtn = session.FindElementByAccessibilityId("AccountviewBtn");
            }
        }

        [Test]
        public void LogIn()
        {
            // Act
            AccountviewBtn.Click();
            txtUserEmail = session.FindElementByAccessibilityId("txtUserEmail");
            txtUserPassword = session.FindElementByAccessibilityId("txtUserPassword");
            loginBtn = session.FindElementByAccessibilityId("loginBtn");
            LoginAsHeadAdmin();
            // Assert
            //Assert.AreEqual("r0739214@student.thomasmore.be", txtEmail.Text);
        }
        public void LoginAsHeadAdmin()
        {
            txtUserEmail.SendKeys("Jos@gmail.com");
            txtUserPassword.SendKeys("Test@123");
            loginBtn.Click();
        }
    }
}
