﻿////////////////////////////////////////////////////////
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

using CNCLib.Wpf.ViewModels.ManualControl;
using System;
using System.Threading.Tasks;
using Framework.Arduino.SerialCommunication;
using Framework.Contracts.Logging;
using SerialCom = Framework.Arduino.SerialCommunication.Serial;

namespace CNCLib.Wpf.Helpers
{
    class JoystickArduinoSerialCommunication : SerialCom
    {
        public JoystickArduinoSerialCommunication(ILogger<SerialCom> logger) : base(logger)
        {
            OkTag = ""; // every new line is "end of command"
        }

        public void RunCommandInNewTask(Action todo)
        {
            Task.Run(() =>
            {
                todo();
                Global.Instance.Com.Current.WriteCommandHistory(CommandHistoryViewModel.CommandHistoryFile);
            });
        }

        protected override void OnReplyReceived(SerialEventArgs info)
        {
            base.OnReplyReceived(info);

            if (info.Info.StartsWith(";CNCJoystick"))
            {
                if (Global.Instance.Joystick?.InitCommands != null)
                {
                    RunCommandInNewTask(async () => { await new JoystickHelper().SendInitCommands(Global.Instance.Joystick?.InitCommands); });
                }
            }
            else
            {
                RunCommandInNewTask(() => { new JoystickHelper().JoystickReplyReceived(info.Info.Trim()); });
            }
        }
    }
}