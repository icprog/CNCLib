////////////////////////////////////////////////////////
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
////////////////////////////////////////////////////////

#pragma once

////////////////////////////////////////////////////////

#include <arduino.h>
#include "ConfigurationStepperLib.h"

////////////////////////////////////////////////////////

#define TIMER0VALUE(freq)	((timer_t)((unsigned long)TIMER0FREQUENCE/(unsigned long)freq))
#define TIMER1VALUE(freq)	((timer_t)((unsigned long)TIMER1FREQUENCE/(unsigned long)freq))
#define TIMER2VALUE(freq)	((timer_t)((unsigned long)TIMER2FREQUENCE/(unsigned long)freq))
#define TIMER3VALUE(freq)	((timer_t)((unsigned long)TIMER3FREQUENCE/(unsigned long)freq))
#define TIMER4VALUE(freq)	((timer_t)((unsigned long)TIMER4FREQUENCE/(unsigned long)freq))
#define TIMER5VALUE(freq)	((timer_t)((unsigned long)TIMER5FREQUENCE/(unsigned long)freq))

////////////////////////////////////////////////////////

#if defined(__SAM3X8E__)

typedef uint32_t pin_t;

#else

#define irqflags_t unsigned char
typedef uint8_t pin_t;

#endif


////////////////////////////////////////////////////////

class CHAL
{
public:

	typedef void(*HALEvent)();

	// min 8 bit 
	static void InitTimer0(HALEvent evt);
	static void RemoveTimer0();
	static void StartTimer0(timer_t timer);
	static void StopTimer0();

	// min 16 bit
	static void InitTimer1(HALEvent evt);
	static void RemoveTimer1();
	static void StartTimer1(timer_t timer);
	static void StopTimer1();

	// 8 bit
	static void InitTimer2(HALEvent evt);
	static void RemoveTimer2();
	static void StartTimer2(timer_t timer);
	static void StopTimer2();

	static HALEvent _TimerEvent0;
	static HALEvent _TimerEvent1;
	static HALEvent _TimerEvent2;

#if !defined( __AVR_ATmega328P__)

	// min 16 bit
	static void InitTimer3(HALEvent evt);
	static void RemoveTimer3();
	static void StartTimer3(timer_t timer);
	static void StopTimer3();

	// min 16 bit
	static void InitTimer4(HALEvent evt);
	static void RemoveTimer4();
	static void StartTimer4(timer_t timer);
	static void StopTimer4();

	// min 16 bit
	static void InitTimer5(HALEvent evt);
	static void RemoveTimer5();
	static void StartTimer5(timer_t timer);
	static void StopTimer5();

	static HALEvent _TimerEvent3;
	static HALEvent _TimerEvent4;
	static HALEvent _TimerEvent5;

#endif

#if defined(__SAM3X8E__)

	// use CAN as backgroundworker thread

	static void BackgroundRequest()				{ NVIC_SetPendingIRQ(CAN0_IRQn); }
	static void InitBackground(HALEvent evt)	{ NVIC_EnableIRQ(CAN0_IRQn);  NVIC_SetPriority(CAN0_IRQn, NVIC_EncodePriority(4, 7, 0)); _CAM0Event = evt; }

	static HALEvent _CAM0Event;

#endif

	static inline void DisableInterrupts();
	static inline void EnableInterrupts();

	static inline irqflags_t GetSREG();
	static inline void SetSREG(irqflags_t);

	static inline void pinMode(unsigned char pin, unsigned char mode);

	static void digitalWrite(unsigned char pin, unsigned char lowOrHigh);
	static unsigned char digitalRead(unsigned char pin);

};

//////////////////////////////////////////

class CCriticalRegion
{
private:
	irqflags_t _sreg;

public:

	inline CCriticalRegion() :_sreg(CHAL::GetSREG()) {  CHAL::DisableInterrupts(); };
	inline ~CCriticalRegion()	{ CHAL::SetSREG(_sreg); }
};

//////////////////////////////////////////

#include "HAL_AVR.h"
#include "HAL_Sam3x8e.h"
#include "HAL_Msvc.h"

//////////////////////////////////////////