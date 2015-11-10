﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNCLib.Repository.Context;
using CNCLib.Repository;
using CNCLib.Repository.Entities;
using System.Threading.Tasks;
using Framework.EF;
using System.Collections.Generic;
using System.Linq;
using Framework.Tools;
using CNCLib.Repository.RepositoryInterface;

namespace CNCLib.Tests.Repository
{
	[TestClass]
	public class RepositoryTests
	{
		public TestContext TestContext { get; set; }
		static bool _init = false;
		static protected Framework.Tools.IFactory RepositoryFactory { get; set; }

		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{

			var factory = new Framework.Tools.Factory();

			factory.Register(typeof(CNCLib.Repository.RepositoryInterface.IConfigurationRepository), typeof(CNCLib.Repository.ConfigurationRepository));
			factory.Register(typeof(CNCLib.Repository.RepositoryInterface.IMachineRepository), typeof(CNCLib.Repository.MachineRepository));

			RepositoryFactory = factory;

			if (_init == false)
			{
				//drop and recreate the test Db everytime the tests are run. 
				AppDomain.CurrentDomain.SetData("DataDirectory", testContext.TestDeploymentDir);

				using (var uow = UnitOfWorkFactory.Create())
				{
					System.Data.Entity.Database.SetInitializer<CNCLibContext>(new CNCLibInitializer());
					uow.InitializeDatabase();
				}
				_init = true;
			}
		}

		[TestInitialize]
		public void Init()
		{
		}
	}
}
