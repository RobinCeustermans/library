using Bibliotheek_DAL;
using Bibliotheek_DAL.UnitsOfWork;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using The_Boys_Project.ViewModels;

namespace Bibliotheek_TESTING.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkItemRepo
    {
        public IUnitOfWork unitOfWork = A.Fake<IUnitOfWork>();

        [Test]
        public void get_ReturnObservableCollectionItemFromItemID()
        {
            //Arrange
            Item item = A.Fake<Item>();
            ObservableCollection<Item> items;

            //
            items = new ObservableCollection<Item>(unitOfWork.ItemRepo.GetEntities(x => x.ItemID == item.ItemID));

            //Assert
            Assert.NotNull(items);
            Assert.IsInstanceOf<ObservableCollection<Item>>(items);
        }


    }
}
