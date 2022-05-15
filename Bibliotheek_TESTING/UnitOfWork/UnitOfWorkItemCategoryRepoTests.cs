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
    class UnitOfWorkItemCategoryRepoTests
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void Get_ReturnsObservableCollectionOfTypeItemCategoryFromItemID()
        {
            //Arrange
            Item item = A.Fake<Item>();
            ObservableCollection<ItemCategory> itemCategories;

            //Act
            itemCategories = new ObservableCollection<ItemCategory>(unitOfWork.ItemCategoryRepo.GetEntities(x => x.ItemID == item.ItemID));

            //Assert
            Assert.NotNull(itemCategories);
            Assert.IsInstanceOf<ObservableCollection<ItemCategory>>(itemCategories);
        }

    }
}
