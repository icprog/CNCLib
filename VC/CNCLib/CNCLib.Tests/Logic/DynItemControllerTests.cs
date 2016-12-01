﻿////////////////////////////////////////////////////////
/*
  This file is part of CNCLib - A library for stepper motors.

  Copyright (c) 2013-2016 Herbert Aitenbichler

  CNCLib is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  CNCLib is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
  http://www.gnu.org/licenses/
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNCLib.Logic;
using System.Linq;
using NSubstitute;
using Framework.Tools.Dependency;
using CNCLib.Logic.Client;
using CNCLib.ServiceProxy;
using CNCLib.Logic.Contracts.DTO;

namespace CNCLib.Tests.Logic
{
	[TestClass]
	public class DynItemControllerTests : CNCUnitTest
	{
		private TInterface CreateMock<TInterface>() where TInterface : class, IDisposable
        {
			TInterface srv = Substitute.For<TInterface>();
            Dependency.Container.RegisterInstance(srv);
            return srv;
		}


		[TestMethod]
		public void GetItemNone()
		{
			var srv = CreateMock<IItemService>();

			var itemEntity = new Item[0];
			srv.GetAll().Returns(itemEntity);

			var ctrl = new DynItemController();

			var all = ctrl.GetAll().ConfigureAwait(false).GetAwaiter().GetResult().ToArray();
			Assert.AreEqual(true, all.Length == 0);
		}

		[TestMethod]
		public void GetItemAll()
		{
			var srv = CreateMock<IItemService>();

			var itemEntity = new Item[2]
			{
				new Item() { ItemID=1,Name="Test1" },
				new Item() { ItemID=2,Name="Test2" },
			};
			srv.GetAll().Returns(itemEntity);

			var ctrl = new DynItemController();

			var all = ctrl.GetAll().ConfigureAwait(false).GetAwaiter().GetResult().ToArray();
			Assert.AreEqual(2, all.Count());
			Assert.AreEqual(1, all.FirstOrDefault().ItemID);
			Assert.AreEqual("Test1", all.FirstOrDefault().Name);
		}

		[TestMethod]
		public void GetAllType()
		{
			var srv = CreateMock<IItemService>();

			var itemEntity = new Item[2]
			{
				new Item() { ItemID=1,Name="Test1" },
				new Item() { ItemID=2,Name="Test2" },
			};
			srv.GetByClassName("System.String,mscorlib").Returns(itemEntity);

			var ctrl = new DynItemController();

			var all = ctrl.GetAll(typeof(string)).ConfigureAwait(false).GetAwaiter().GetResult();

			Assert.AreEqual(2, all.Count());
			Assert.AreEqual(1, all.FirstOrDefault().ItemID);
			Assert.AreEqual("Test1", all.FirstOrDefault().Name);
		}

		[TestMethod]
		public void GetItem()
		{
			var srv = CreateMock<IItemService>();
			srv.Get(1).Returns(new Item() { ItemID = 1, Name = "Test1" });

			var ctrl = new DynItemController();

			var all = ctrl.Get(1).ConfigureAwait(false).GetAwaiter().GetResult();

			Assert.AreEqual(1, all.ItemID);
			Assert.AreEqual("Test1", all.Name);
		}

		[TestMethod]
		public void GetItemNull()
		{
			var srv = CreateMock<IItemService>();

			var ctrl = new DynItemController();

			var all = ctrl.Get(10).ConfigureAwait(false).GetAwaiter().GetResult();

			Assert.IsNull(all);
		}
		[TestMethod]
        public void CreateObject()
        {
            var srv = CreateMock<IItemService>();

            Item itemEntity = CreateItem();

            srv.Get(1).Returns(itemEntity);

            var ctrl = new DynItemController();

            var item = ctrl.Create(1);
            Assert.IsNotNull(item);

            DynItemControllerTestClass item2 = (DynItemControllerTestClass)item;

            Assert.AreEqual("Hallo", item2.StringProperty);
            Assert.AreEqual(1, item2.IntProperty);
            Assert.AreEqual(1, item2.IntProperty);
            Assert.AreEqual(1.234, item2.DoubleProperty);
            Assert.AreEqual(1.234, item2.DoubleNullProperty);
            Assert.AreEqual(9.876m, item2.DecimalProperty);
            Assert.AreEqual(9.876m, item2.DecimalNullProperty);
        }

        private static Item CreateItem()
        {
            return new Item
            {
                ItemID = 1,
                Name = "Hallo",
                ClassName = typeof(DynItemControllerTestClass).AssemblyQualifiedName,
                ItemProperties = new[]
                            {
                                new ItemProperty{ ItemID = 1, Name = "StringProperty", Value = "Hallo" },
                                new ItemProperty{ ItemID = 1, Name = "IntProperty", Value = "1" },
                                new ItemProperty{ ItemID = 1, Name = "DoubleProperty",  Value = "1.234" },
                                new ItemProperty{ ItemID = 1, Name = "DecimalProperty", Value = "9.876" },
                                new ItemProperty{ ItemID = 1, Name = "IntNullProperty" },
                                new ItemProperty{ ItemID = 1, Name = "DoubleNullProperty",  Value = "1.234" },
                                new ItemProperty{ ItemID = 1, Name = "DecimalNullProperty", Value = "9.876" }
                            }
            };
        }

        [TestMethod]
        public void AddObject()
        {
            var srv = CreateMock<IItemService>();

            Item itemEntity = CreateItem();

            DynItemControllerTestClass obj = new DynItemControllerTestClass()
            {
                StringProperty = "Hallo",
                IntProperty = 1,
                DoubleProperty = 1.234,
                DoubleNullProperty = 1.234,
                DecimalProperty = 9.876m,
                DecimalNullProperty = 9.876m
            };

            var ctrl = new DynItemController();

            var id = ctrl.Add("Hallo", obj);

            srv.Received().Add(Arg.Is<Item>(x => x.Name == "Hallo"));
            srv.Received().Add(Arg.Is<Item>(x => x.ItemID == 0));
            srv.Received().Add(Arg.Is<Item>(x => x.ItemProperties.Count == 7));
            srv.Received().Add(Arg.Is<Item>(x => x.ItemProperties.Where(y => y.Name == "StringProperty").FirstOrDefault().Value == "Hallo" ));
            srv.Received().Add(Arg.Is<Item>(x => x.ItemProperties.Where(y => y.Name == "DoubleProperty").FirstOrDefault().Value == "1.234"));
            srv.Received().Add(Arg.Is<Item>(x => x.ItemProperties.Where(y => y.Name == "DecimalNullProperty").FirstOrDefault().Value == "9.876"));
        }

        [TestMethod]
        public void DeleteItem()
        {
            // arrange

            var srv = CreateMock<IItemService>();

            Item itemEntity = CreateItem();
            srv.Get(1).Returns(itemEntity);

            var ctrl = new DynItemController();

            //act

            ctrl.Delete(1).ConfigureAwait(false).GetAwaiter().GetResult();

            //assert
            srv.Received().Get(1);
            srv.Received().Delete(itemEntity);
        }

        [TestMethod]
        public void DeleteItemNone()
        {
            // arrange

            var srv = CreateMock<IItemService>();

            var ctrl = new DynItemController();

            //act

            ctrl.Delete(1).ConfigureAwait(false).GetAwaiter().GetResult();

            //assert
            srv.Received().Get(1);
            srv.DidNotReceiveWithAnyArgs().Delete(null);
        }


		[TestMethod]
		public void SaveItem()
		{
			// arrange

			var srv = CreateMock<IItemService>();
			var ctrl = new DynItemController();

			//act

			ctrl.Save(1,"Test",new DynItemControllerTestClass() {IntProperty=1 }).ConfigureAwait(false).GetAwaiter().GetResult();

			//assert
			srv.Received().Update(Arg.Is<Item>(x => x.ItemID == 1));
			srv.Received().Update(Arg.Is<Item>(x => x.ItemProperties.FirstOrDefault(y=>y.Name=="IntProperty").Value=="1"));
			srv.DidNotReceiveWithAnyArgs().Delete(null);
		}
	}
}
