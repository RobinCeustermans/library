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
    public class UnitOfWorkBookFairRepoTest
    {

        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Get_ReturnsObservableCollectionOfTypeBookFair()
        {
            //Arrange
            ObservableCollection<BookFair> BookFairs;

            //Act
            BookFairs = new ObservableCollection<BookFair>(unitOfWork.BookFairRepo.GetEntities());

            //Assert
            Assert.NotNull(BookFairs);
            Assert.IsInstanceOf<ObservableCollection<BookFair>>(BookFairs);
        }

        [Test]
        public void Get_UserBookFairFromBookFairID_ReturnsObservableCollectionOfTypeUserBookFair()
        {
            //Arrange
            BookFair bookFair = A.Fake<BookFair>();
            ObservableCollection<UserBookFair> UserBookFairs;

            //Act
            UserBookFairs = new ObservableCollection<UserBookFair>(unitOfWork.UserBookFairRepo.GetEntities(x => x.BookFairID == bookFair.BookFairID));

            //Assert
            Assert.NotNull(UserBookFairs);
            Assert.IsInstanceOf<ObservableCollection<UserBookFair>>(UserBookFairs);
        }
    }
}
