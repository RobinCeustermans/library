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
    class UnitOfWorkCategoryRepo
    {
        private IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();
        [Test]
        public void Get_ReturnsObservableCollectionOfTypeCategory()
        {
            //Arrange
            ObservableCollection<Category> categories;

            //Act
            categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities());

            //Assert
            Assert.NotNull(categories);
            Assert.IsInstanceOf<ObservableCollection<Category>>(categories);
        }

        [Test]
        public void Get_ReternObservableCollectionCategoryByNameCheckIfCollectionIsNothingButCategory()
        {
            //Arrange
            ObservableCollection<Category> categories;
            Category category = A.Fake<Category>();

            //Act
            categories = new ObservableCollection<Category>(unitOfWork.CategoryRepo.GetEntities(x => x.Name == category.Name));

            //Assert
            Assert.IsInstanceOf<ObservableCollection<Category>>(categories);
            Assert.IsNotInstanceOf<ObservableCollection<Contributor>>(categories);
            Assert.IsTrue(categories.Count.Equals(0));
        }


    }
}
