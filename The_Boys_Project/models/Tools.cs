using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace The_Boys_Project.models
{
    public static class Tools
    {
        public static ObservableCollection<string> GetCountries()
        {
            //Creating list
            List<string> cultureList = new List<string>();
            //getting the specific CultureInfo from CultureInfo class
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                //creating the object of RegionInfo class
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                //adding each country Name into the Dictionary
                if (!(cultureList.Contains(getRegionInfo.DisplayName)))
                {
                    cultureList.Add(getRegionInfo.DisplayName);
                }
            }
            cultureList.Sort();
            return new ObservableCollection<string>(cultureList);
        }

        public static void SendMail(User recipient, string mailtype, string subject = "", BookFair bookFair = null)
        {
            try
            {
                var fromAddress = new MailAddress("cejsmets@gmail.com", "De Bib");
                var toAddress = new MailAddress(recipient.Email, recipient.ToString());
                const string fromPassword = "uocvjfcjawkrmift";
                string body = MailBodyBuilder(mailtype, bookFair).Replace("#user#", recipient.Name);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress.Address, toAddress.Address, subject, body))
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public static void SendMail(List<User> recipients, string mailtype, string subject = "", BookFair bookFair = null)
        {
            try
            {
                var fromAddress = new MailAddress("cejsmets@gmail.com", "De Bib");
                const string fromPassword = "uocvjfcjawkrmift";
                string body = MailBodyBuilder(mailtype, bookFair);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                foreach (var recipient in recipients)
                {
                    var toAddress = new MailAddress(recipient.Email, recipient.ToString());

                    using (var message = new MailMessage(fromAddress.ToString(), toAddress.ToString(), subject, body.Replace("#user#", recipient.Name)))
                    {
                        message.IsBodyHtml = true;
                        smtp.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private static string MailBodyBuilder(string mailtype, BookFair bookFair = null)
        {
            try
            {
                IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
                string output = unitOfWork.EmailTextRepo.GetEntities(x => x.Description == mailtype).FirstOrDefault().HTMLString;
                unitOfWork.Dispose();
                // fill in bookfair details for bookfair mails only
                if (bookFair != null)
                {
                    output = output
                        .Replace("#bookfair#", bookFair.Name)
                        .Replace("#startdate#", bookFair.StartDate.ToShortDateString())
                        .Replace("#enddate#", bookFair.EndDate.ToShortDateString())
                        .Replace("#description#", bookFair.Description);

                    if (bookFair.Location != null)
                        output = output.Replace("#location#", bookFair.Location);
                }
                return output;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private const string SecurityKey = "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";
        public static string Decrypt(string CipherText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            //Convert and return the decrypted data/byte into string format.
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string Encrypt(string PlainText)
        {
            // Getting the bytes of Input String.
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}

