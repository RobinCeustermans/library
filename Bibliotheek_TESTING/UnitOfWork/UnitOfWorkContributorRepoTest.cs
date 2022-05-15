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
    public class UnitOfWorkContributorRepoTest
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Get_ReturnsObservableCollectionOfTypeContributor()
        {
            //Arrange
            ObservableCollection<Contributor> contributors;

            //Act
            contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities());

            //Assert
            Assert.NotNull(contributors);
            Assert.IsInstanceOf<ObservableCollection<Contributor>>(contributors);
        }

        [Test]
        public void Get_ReternObservableCollectionContributorsByName()
        {
            //Arrange
            ObservableCollection<Contributor> contributors;
            Contributor contributor = A.Fake<Contributor>();

            //Act
            contributors = new ObservableCollection<Contributor>(unitOfWork.ContributorRepo.GetEntities(x => x.Name == contributor.Name));

            //Assert
            Assert.IsInstanceOf<ObservableCollection<Contributor>>(contributors);
            Assert.IsTrue(contributors.Count.Equals(0));
        }

    }
}
