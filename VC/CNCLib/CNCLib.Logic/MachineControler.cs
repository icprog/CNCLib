﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Tools;
using Framework.EF;
using CNCLib.Repository;
using CNCLib.Repository.Context;
using CNCLib.Repository.RepositoryInterface;

namespace CNCLib.Logic
{
    public class MachineControler : ControlerBase
	{
		public DTO.Machine[] GetMachines()
		{
			using (var rep = RepositoryFactory.Create<IMachineRepository>())
			{
				var machines = rep.GetMachines();
				List<DTO.Machine> l = new List<DTO.Machine>();
				l.AddCloneProperties(machines);
				return l.ToArray();
			}
		}

        public DTO.Machine GetMachine(int id)
        {
			using (var rep = RepositoryFactory.Create<IMachineRepository>())
			{
				var machine = rep.GetMachine(id);
				return ObjectConverter.NewCloneProperties<DTO.Machine, CNCLib.Repository.Entities.Machine>(machine);
			}
        }

        public void Delete(DTO.Machine m)
        {
			using (var rep = RepositoryFactory.Create<IMachineRepository>())
			{
				rep.Delete(m.NewCloneProperties<CNCLib.Repository.Entities.Machine, DTO.Machine>());
			}
        }

		public DTO.MachineCommand[] GetMachineCommands(int machineID)
		{
			using (var rep = RepositoryFactory.Create<IMachineRepository>())
			{
				var machines = rep.GetMachineCommands(machineID);
				List<DTO.MachineCommand> l = new List<DTO.MachineCommand>();
				l.AddCloneProperties(machines);
				return l.ToArray();
			}
		}
		public DTO.MachineInitCommand[] GetMachineInitCommands(int machineID)
		{
			using (var rep = RepositoryFactory.Create<IMachineRepository>())
			{
				var machines = rep.GetMachineInitCommands(machineID).OrderBy((c) => c.SeqNo);
				List<DTO.MachineInitCommand> l = new List<DTO.MachineInitCommand>();
				l.AddCloneProperties(machines);
				return l.ToArray();
			}
		}

		public int StoreMachine(DTO.Machine m)
		{
			using (var rep = RepositoryFactory.Create<IMachineRepository>())
			{
				var me = m.NewCloneProperties<CNCLib.Repository.Entities.Machine, DTO.Machine>();
				me.MachineCommands = m.MachineCommands.ToArray().CloneProperties<CNCLib.Repository.Entities.MachineCommand, CNCLib.Logic.DTO.MachineCommand>();
				me.MachineInitCommands = m.MachineInitCommands.ToArray().CloneProperties<CNCLib.Repository.Entities.MachineInitCommand, CNCLib.Logic.DTO.MachineInitCommand>();

				return rep.StoreMachine(me);
			}
		}

		#region Default machine

		public int GetDetaultMachine()
		{
			using (var rep = RepositoryFactory.Create<IConfigurationRepository>())
			{
				var config = rep.Get("Environment", "DefaultMachineID");

				if (config == default(CNCLib.Repository.Entities.Configuration))
					return -1;

				return int.Parse(config.Value);
			}
		}
		public void SetDetaultMachine(int defaultMachineID)
		{
			using (var rep = RepositoryFactory.Create<IConfigurationRepository>())
			{
				rep.Save(new Repository.Entities.Configuration() { Group = "Environment", Name = "DefaultMachineID", Type = "Int32", Value = defaultMachineID.ToString() });
			}
		}


		#endregion
	}
}
