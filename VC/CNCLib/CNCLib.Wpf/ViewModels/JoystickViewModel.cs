﻿////////////////////////////////////////////////////////
/*
  This file is part of CNCLib - A library for stepper motors.

  Copyright (c) 2013-2014 Herbert Aitenbichler

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
using System.Windows.Input;
using Framework.Wpf.ViewModels;
using Framework.Wpf.Helpers;
using CNCLib.Wpf.Helpers;
using System.Threading.Tasks;

namespace CNCLib.Wpf.ViewModels
{
	public class JoystickViewModel : BaseViewModel, IDisposable
	{

		#region crt

		public JoystickViewModel()
		{
		}

		public override async Task Loaded()
		{
			await base.Loaded();
			await LoadJoystick();
		}

		#endregion

		#region dispose

		public void Dispose()
		{
		}

		#endregion

		#region Properties

		Models.Joystick _currentJoystick = new Models.Joystick();
		int _id = -1;

        public string ComPort
        {
			get { return _currentJoystick.ComPort; }
            set { SetProperty(() => _currentJoystick.ComPort == value, () => _currentJoystick.ComPort = value); }
        }

		public int BaudRate
		{
			get { return _currentJoystick.BaudRate; }
            set { SetProperty(() => _currentJoystick.BaudRate == value, () => _currentJoystick.BaudRate = value); }
		}
		public string InitCommands
		{
			get { return _currentJoystick.InitCommands; }
			set { SetProperty(() => _currentJoystick.InitCommands == value, () => _currentJoystick.InitCommands = value); }
		}

		#endregion

		#region Operations
		public async Task LoadJoystick()
		{
			var joystick = await JoystickHelper.Load();
			_currentJoystick = joystick.Item1;
			_id = joystick.Item2;

			OnPropertyChanged(() => ComPort);
			OnPropertyChanged(() => BaudRate);
			OnPropertyChanged(() => InitCommands);
		}

		public async void SaveJoystick()
		{
			_id = await JoystickHelper.Save(_currentJoystick, _id);
			CloseAction();
        }

		public bool CanSaveJoystick()
		{
			return true;
		}

		#endregion

		#region Commands

		public ICommand SaveJoystickCommand => new DelegateCommand(SaveJoystick, CanSaveJoystick); 

        #endregion
    }
}
