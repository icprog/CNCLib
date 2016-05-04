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
using Framework.Wpf.ViewModels;

namespace CNCLib.Wpf.ViewModels.ManualControl
{
	public class DetailViewModel : BaseViewModel
	{
		private IManualControlViewModel Vm { get; set; }
		public DetailViewModel(IManualControlViewModel vm)
		{
			Vm = vm;
		}
		public Framework.Arduino.ArduinoSerialCommunication Com
		{
			get { return Framework.Tools.Pattern.Singleton<Framework.Arduino.ArduinoSerialCommunication>.Instance; }
		}
		public bool Connected
		{
			//get { return true; }
			get { return Com.IsConnected; }
		}
		protected void AsyncRunCommand(Action todo)
		{
			Vm.AsyncRunCommand(todo);
		}
		protected void SetPositions(string[] positions, int positionIdx)
		{
			Vm.SetPositions(positions, positionIdx);
		}

		#region Command/CanCommand

		public bool CanSend()
		{
			return Connected;
		}

		#endregion
	}
}
