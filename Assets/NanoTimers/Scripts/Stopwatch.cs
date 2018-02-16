// Program: Nano Timers
// Author:  GhostRavenstorm
// Version: 0.1.3
//
// Summary: Timer library that includes countdown timers and stopwatches.


using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace NanoTimers{

// Summary:
// Timer class that counts from 0 to infinity.
//
// Remarks:
// Does not make event calls.
//
public class Stopwatch : Timer{

   // Summary:
	// Initializes the timer with default vaules and references.
   public void Initialize(Text timerText){
      m_timerText = timerText;
      m_state = ETimerState.New;
      m_isInitialized = true;
   }

   void FixedUpdate(){
		switch(m_state){
			case ETimerState.Active:{

				m_millis += (int)(UnityEngine.Time.fixedDeltaTime * 1000);

				if(m_millis > 999){
					m_millis = 0;
					m_seconds += 1;
				}

				if(m_seconds > 59){
					m_seconds = 0;
					m_minutes += 1;
				}

				UpdateUI();

				break;
			}
		}
	}

} // End of class.

} // End of namespace.
