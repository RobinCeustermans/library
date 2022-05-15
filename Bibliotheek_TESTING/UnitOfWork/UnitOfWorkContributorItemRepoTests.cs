using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Bibliotheek_TESTING.UnitOfWork
{
    [TestFixture]
    class UnitOfWorkContributorItemRepoTests
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Get_ReturnsObservableCollectionOfTypeContributorItemFromContributorID()
        {
            //Arrange
            Contributor contributor = A.Fake<Contributor>();
            ObservableCollection<ContributorItem> contributorItems;

            //Act
            contributorItems = new ObservableCollection<ContributorItem>(unitOfWork.ContributorItemRepo.GetEntities(x => x.ContributorID == contributor.ContributorID));

            //Assert
            Assert.NotNull(contributorItems);
            Assert.IsInstanceOf<ObservableCollection<ContributorItem>>(contributorItems);
        }
    }
}
