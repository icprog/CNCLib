////////////////////////////////////////////////////////
/*
  This file is part of CNCLib - A library for stepper motors.

  Copyright (c) 2013-2018 Herbert Aitenbichler

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

using System.Linq;
using CNCLib.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace CNCLib.Repository.SqlServer
{
    public class MigrationCNCLibContext : CNCLibContext
    {
        public static string ConnectString { get; set; } = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = CNCLib; Integrated Security = True";

        static MigrationCNCLibContext()
        {
            OnConfigure = (optionsBuilder) => { optionsBuilder.UseSqlServer(ConnectString); };
        }

        public static void InitializeDatabase(string connectstring, bool dropdatabase, bool isTest)
        {
            if (!string.IsNullOrEmpty(connectstring))
            {
                ConnectString = connectstring;
            }

            using (var ctx = new MigrationCNCLibContext())
            {
                if (dropdatabase)
                {
                    ctx.Database.EnsureDeleted();
                }

                ctx.Database.Migrate();
                if (!ctx.Machines.Any())
                {
                    new CNCLibDefaultData().CNCSeed(ctx, isTest);
                    ctx.SaveChanges();
                }
            }
        }
    }
}