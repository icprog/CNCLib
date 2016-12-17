////////////////////////////////////////////////////////
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
////////////////////////////////////////////////////////

#pragma once

////////////////////////////////////////////////////////

#define X_MAXSIZE 200000        // in mm1000_t
#define Y_MAXSIZE 200000 
#define Z_MAXSIZE 100000 
#define A_MAXSIZE 360000 

////////////////////////////////////////////////////////
// NoReference, ReferenceToMin, ReferenceToMax

#define X_USEREFERENCE	EReverenceType::ReferenceToMin
#define Y_USEREFERENCE	EReverenceType::ReferenceToMin
#define Z_USEREFERENCE	EReverenceType::ReferenceToMax
#define A_USEREFERENCE	EReverenceType::NoReference

//#define REFMOVE_1_AXIS  Z_AXIS
//#define REFMOVE_2_AXIS  Y_AXIS
//#define REFMOVE_3_AXIS  X_AXIS
#define REFMOVE_1_AXIS  255
#define REFMOVE_2_AXIS  255
#define REFMOVE_3_AXIS  255
#define REFMOVE_4_AXIS  255

////////////////////////////////////////////////////////

//#define STEPPERTYPE 1		// CStepperL298N
//#define STEPPERTYPE 2		// CStepperSMC800
//#define STEPPERTYPE 3		// CStepperTB6560
#define STEPPERTYPE 4		// CStepperCNCShield

////////////////////////////////////////////////////////

#if STEPPERTYPE==1
#include "Configuration_MiniCNC_L298N.h"
#elif STEPPERTYPE==2
#include "Configuration_MiniCNC_SMC800.h"
#elif STEPPERTYPE==3
#include "Configuration_MiniCNC_TB6560.h"
#elif STEPPERTYPE==4
#include "Configuration_MiniCNC_CNCShield.h"
#endif

////////////////////////////////////////////////////////

#define GO_DEFAULT_STEPRATE		((steprate_t) CConfigEeprom::GetSlotU32(EConfigSlot::MaxStepRate))	// steps/sec
#define G1_DEFAULT_STEPRATE		10000			// steps/sec
#define G1_DEFAULT_MAXSTEPRATE	((steprate_t) CConfigEeprom::GetSlotU32(EConfigSlot::MaxStepRate))	// steps/sec

#define STEPRATERATE_REFMOVE	CNC_MAXSPEED // GO_DEFAULT_STEPRATE

////////////////////////////////////////////////////////

extern float scaleToMm;
extern float scaleToMachine;

inline mm1000_t MiniCNCToMm1000(axis_t axis, sdist_t val)
{
	switch (axis)
	{
		default:
		case X_AXIS: return  (mm1000_t)(val * scaleToMm);
		case Y_AXIS: return  (mm1000_t)(val * scaleToMm);
		case Z_AXIS: return  (mm1000_t)(val * scaleToMm);
		case A_AXIS: return  (mm1000_t)(val * scaleToMm);
	}
}

inline sdist_t MiniCNCToMachine(axis_t axis, mm1000_t  val)
{
	switch (axis)
	{
		default:
		case X_AXIS: return  (sdist_t)(val * scaleToMachine);
		case Y_AXIS: return  (sdist_t)(val * scaleToMachine);
		case Z_AXIS: return  (sdist_t)(val * scaleToMachine);
		case A_AXIS: return  (sdist_t)(val * scaleToMachine);
	}
}

////////////////////////////////////////////////////////

#include <MessageCNCLib.h>

#define MESSAGE_MYCONTROL_Proxxon_Starting					F("MiniCNC-E:" __DATE__ )

