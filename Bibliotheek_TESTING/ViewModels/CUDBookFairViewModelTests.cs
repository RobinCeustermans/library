using Bibliotheek_DAL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bibliotheek_TESTING.ViewModels
{
    [TestFixture]
    public class CUDBookFairViewModelTests
    {
        
        [Test]
        public void AddValueToBookFair_ValueIsNotEmpty_GivenValuesIsEqualToValue()
        {
            //Arrange
            BookFair bookFair = new BookFair();

            //Act
            bookFair.Name = "De bezige boeken";
            bookFair.Location = "Turnhout";
            bookFair.Description = "Een groot spektakel voor jong en oud, kom dat zien kom dat zien!!!";

            //Assert
            Assert.AreEqual("De bezige boeken", bookFair.Name);
            Assert.AreEqual("Turnhout", bookFair.Location);
            Assert.AreEqual("Een groot spektakel voor jong en oud, kom dat zien kom dat zien!!!", bookFair.Description);
        }
    }
}
