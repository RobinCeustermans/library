using Bibliotheek_DAL.UnitsOfWork;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bibliotheek_DAL
{
    [AddINotifyPropertyChangedInterface]
    public partial class User : IDataErrorInfo
    {
        public override string ToString()
        {
            return this.Name + " " + this.Surname;
        }
        public string MembershipStartDateToString
        {
            get
            {
                if (MembershipStartDate.HasValue)
                {
                    return MembershipStartDate.Value.ToShortDateString();
                }
                return "";
            }
        }

        public string FullAddress
        {
            get
            {
                return $"{StreetName} {HouseNumber}, {PostalCode} {Country}";
            }
        }

        public string FullName
        {
            get
            {
                return this.Name + " " + this.Surname;
            }
        }

        public string Status
        {
            get
            {
                return IsActive ? "Actief" : "Inactief";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Email))
                {
                    return IsValidEmail(Email);
                }
                else if (columnName == nameof(Name) && string.IsNullOrWhiteSpace(Name))
                {
                    return "Vul een naam in.";
                }
                else if (columnName == nameof(Surname) && string.IsNullOrWhiteSpace(Surname))
                {
                    return "Vul een voornaam in.";
                }
                else if (columnName == nameof(Password))
                {
                    return ValidatePassword(Password);
                }
                else if (columnName == nameof(StreetName) && string.IsNullOrWhiteSpace(StreetName))
                {
                    return "Vul een straat in.";
                }
                else if (columnName == nameof(PostalCode) && string.IsNullOrWhiteSpace(PostalCode))
                {
                    return "Vul een postcode in.";
                }
                else if (columnName == nameof(Country) && string.IsNullOrWhiteSpace(Country))
                {
                    return "Duid een land aan";
                }
                else if (columnName == nameof(HouseNumber) && string.IsNullOrWhiteSpace(HouseNumber))
                {
                    return "Vul een huisnummer in.";
                }
                return "";
            }
        }
    public bool IsGeldig()
        {
            return string.IsNullOrWhiteSpace(Error);
        }
        public string ValidatePassword(string Password)
        {
            string error = "";
            string specialChars = "!@#$%^&*()_+-=[]|{};:<>?,.";
            if (Password != null)
            {
                if (!Password.Any(c => !Char.IsLetterOrDigit(c)))
                {
                    error += "\nJe wachtwoord moet een speciaal teken bevatten\n";
                }
                if (!Password.Any(c => char.IsDigit(c)))
                {
                    error += "\nJe wachtwoord moet een cijfer bevatten\n";
                }
                if (!Password.Any(c => char.IsLetter(c)))
                {
                    error += "\nJe wachtwoord moet een letter bevatten\n";
                }
                if (Password.Length < 7)
                {
                    error += "\nJe wachtwoord moet minstens 7 karakters lang zijn";
                }
            }
            else error += "Vul een wachtwoord in";
            return error;
        }
        public static string IsValidEmail(string email)
        {
            string errormessage = "vul een geldig email-adres in";
            if (string.IsNullOrWhiteSpace(email)) return "Vul een email-adres in";

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return errormessage;
            }
            catch (ArgumentException e)
            {
                return errormessage;
            }

            try
            {
                if (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)) == false) return errormessage;
            }
            catch (RegexMatchTimeoutException)
            {
                return errormessage;
            }
            return "";
        }

        public override bool Equals(object obj)
        {
            if (obj is User user)
            {
                if (Email == user.Email || UserID == user.UserID)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return -506688385 + EqualityComparer<string>.Default.GetHashCode(Email);
        }

        public string Error
        {
            get
            {
                string foutmeldingen = "";

                foreach (var item in this.GetType().GetProperties()) //reflection 
                {
                    if (item.CanRead)
                    {
                        string fout = this[item.Name];
                        if (!string.IsNullOrWhiteSpace(fout))
                        {
                            foutmeldingen += fout + Environment.NewLine;
                        }
                    }
                }
                return foutmeldingen;
            }
        }

        public void UserFines()
        {
            // function to be called on user login, admin checking the list of fines or admin checking user's account

            UnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
            List<UserItem> userItems = unitOfWork.UserItemRepo.GetEntities(x => x.UserID == this.UserID, x => x.User.Fines).ToList();

            foreach (var userItem in userItems)
            {
                // check if item is borrowed (and not just reserved or already returned)
                if (userItem.BorrowedDate != null && userItem.ReturnedDate == null)
                {
                    // check if overdue
                    if (DateTime.Now > userItem.DueDate)
                    {
                        // check if fine for specific item already exists
                        Fine existingFine = null;
                        if (userItem.User.Fines != null)
                        {
                            existingFine = userItem.User.Fines
                                .Where(x => x.UserID == this.UserID &&
                                x.ItemID == userItem.ItemID &&
                                !x.IsPaid).FirstOrDefault();
                        }

                        if (existingFine != null)
                        {
                            // update amount
                            int daysOverdue = (DateTime.Now - userItem.DueDate).Value.Days;

                            // first week is a flat amount, after that it scales by time overdue
                            existingFine.Amount = (decimal)(1 + Math.Max(0, daysOverdue - 7) * 0.50 / 7);
                            unitOfWork.FineRepo.EditEntity(existingFine);
                        }
                        else
                        {
                            // create new fine
                            Fine newFine = new Fine()
                            {
                                ItemID = userItem.ItemID,
                                UserID = userItem.UserID,
                                Amount = (decimal)(1 + Math.Max(0, (DateTime.Now - userItem.DueDate).Value.Days - 7) * 0.50 / 7),
                                IsPaid = false
                            };

                            unitOfWork.FineRepo.AddEntity(newFine);
                        }
                    }
                }
                else continue;
            }
            unitOfWork.Save();

            unitOfWork.Dispose();
        }

        public decimal TotalFine
        {
            get
            {
                decimal amount = 0;

                // this.Fines.Where(x => x.UserID == UserID && !x.IsPaid).ToList().ForEach(x => amount += x.Amount);
                IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
                unitOfWork.FineRepo.GetEntities(x => x.UserID == UserID && !x.IsPaid).ToList().ForEach(x => amount += x.Amount);

                unitOfWork.Dispose();

                return amount;
            }
        }

        public string OverdueBooks
        {
            get
            {
                string output = "";

                IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());
                List<Fine> fines = unitOfWork.FineRepo.GetEntities(x =>
                x.UserID == UserID &&
                !x.IsPaid,
                x => x.Item).ToList();

                foreach (var fine in fines)
                {
                    output += fine.Item.Title;
                    if (fine != fines.LastOrDefault()) output += Environment.NewLine;
                }

                return output;
            }
        }


        public void CheckMembership()
        {
            if (MembershipTypeID == 2)
            {
                if (new DateTime(MembershipStartDate.Value.Year + 1, MembershipStartDate.Value.Month, MembershipStartDate.Value.Day) < DateTime.Now)
                {
                    IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());

                    MembershipTypeID = 1;
                    this.MembershipType = unitOfWork.MembershipTypeRepo.GetEntities(x => x.MembershipTypeID == MembershipTypeID).FirstOrDefault();
                    MembershipStartDate = null;

                    unitOfWork.UserRepo.EditEntity(this);
                    unitOfWork.Save();
                    unitOfWork.Dispose();
                }
            }
        }
    }
}

