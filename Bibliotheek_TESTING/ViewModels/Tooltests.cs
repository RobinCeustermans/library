using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using The_Boys_Project.models;

namespace Bibliotheek_TESTING.ViewModels
{
    [TestFixture]
    public class ToolTests
    {
        [Test]
        public void Decryption_EncryptedPaswword_ReturnsDecryptedPassword()
        {
            //Arrange
            string password = "Test@123";
            string encryptedPassword = Tools.Encrypt(password);

            //Act
            string decrypectedPassword = Tools.Decrypt(encryptedPassword);

            //Assert
            Assert.AreEqual(password, decrypectedPassword);
        }
    }
}
