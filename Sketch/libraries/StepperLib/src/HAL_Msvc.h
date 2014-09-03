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
// MSC
////////////////////////////////////////////////////////

#if defined(_MSC_VER)

#include <arduino.h>
#include <avr/interrupt.h>
#include <avr/io.h>

#define TIMER0FREQUENCE		62500L
#define TIMER1FREQUENCE		2000000L
#define TIMER2FREQUENCE		62500L
#define TIMER3FREQUENCE		62500L
#define TIMER4FREQUENCE		62500L
#define TIMER5FREQUENCE		62500L

#define TIMEROVERHEAD		(0)				// decrease Timervalue for ISR overhead before set new timer

inline void CHAL::DisableInterrupts()	{	cli(); }
inline void CHAL::EnableInterrupts()	{	sei(); }

inline irqflags_t CHAL::GetSREG()				{ return SREG; }
inline void CHAL::SetSREG(irqflags_t a)			{ SREG=a; }

#define __asm__(a)

inline void CHAL::InitTimer0(HALEvent evt){ _TimerEvent0 = evt; }
inline void CHAL::RemoveTimer0()			{}
inline void CHAL::StartTimer0(timer_t)		{}
inline void CHAL::StopTimer0()				{}

inline void CHAL::InitTimer1(HALEvent evt){ _TimerEvent1 = evt; }
inline void CHAL::RemoveTimer1()			{}
inline void CHAL::StartTimer1(timer_t)		{}
inline void CHAL::StopTimer1()				{}

inline void CHAL::InitTimer2(HALEvent evt){ _TimerEvent2 = evt; }
inline void CHAL::RemoveTimer2()			{}
inline void CHAL::StartTimer2(timer_t)		{}
inline void CHAL::StopTimer2()				{}

inline void CHAL::InitTimer3(HALEvent evt){ _TimerEvent3 = evt; }
inline void CHAL::RemoveTimer3()			{}
inline void CHAL::StartTimer3(timer_t)		{}
inline void CHAL::StopTimer3()				{}

inline void CHAL::InitTimer4(HALEvent evt){ _TimerEvent4 = evt; }
inline void CHAL::RemoveTimer4()			{}
inline void CHAL::StartTimer4(timer_t)		{}
inline void CHAL::StopTimer4()				{}

inline void CHAL::InitTimer5(HALEvent evt){ _TimerEvent5 = evt; }
inline void CHAL::RemoveTimer5()			{}
inline void CHAL::StartTimer5(timer_t)		{}
inline void CHAL::StopTimer5()				{}

#define HALFastdigitalRead(a) CHAL::digitalRead(a)
#define HALFastdigitalWrite(a,b) CHAL::digitalWrite(a,b)
#define HALFastdigitalWriteNC(a,b) CHAL::digitalWrite(a,b)

inline void CHAL::digitalWrite(uint8_t pin, uint8_t lowOrHigh)
{
	::digitalWrite(pin,lowOrHigh);
}

inline unsigned char CHAL::digitalRead(uint8_t pin)
{
	return ::digitalRead(pin);
}

inline void CHAL::pinMode(unsigned char pin, unsigned char mode)			
{ 
	::pinMode(pin,mode); 
}

////////////////////////////////////////////////////////

#endif 
