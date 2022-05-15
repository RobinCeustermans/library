using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Bibliotheek_TESTING.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkUserRepoTest
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Ophalen_ReturnsObservableCollectionOfTypeUsers()
        {
            //Arrange
            ObservableCollection<User> Users;

            //Act
            Users = new ObservableCollection<User>(unitOfWork.UserRepo.GetEntities());

            //Assert
            Assert.NotNull(Users);
            Assert.IsInstanceOf<ObservableCollection<User>>(Users);
        }

        [Test]
        public void Get_ReturnsObservableCollectionOfTypeUserInterestedItemFromUserID()
        {
            //Arrange
            User user = A.Fake<User>();
            ObservableCollection<UserInterestedItem> UserInterestedItems;

            //Act
            UserInterestedItems = new ObservableCollection<UserInterestedItem>(unitOfWork.UserInterestedItemRepo.GetEntities(x => x.UserID == user.UserID));

            //Assert
            Assert.NotNull(UserInterestedItems);
            Assert.IsInstanceOf<ObservableCollection<UserInterestedItem>>(UserInterestedItems);
        }
    }
}
