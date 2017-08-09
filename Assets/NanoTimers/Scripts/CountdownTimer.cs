// Program: Nano Timers
// Author:  GhostRavenstorm
// Date:    2017-08-09
// Version: 0.1.2beta
//
// Summary: Timer library that includes countdown timers and stopwatches.

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace NanoTimers{

// Summary:
// Timer class that counts from a determined time to 0 and makes an event call to
// a function upon expiring.
public class CountdownTimer : Timer{

	// Summary:
	// Reference to a method that will be invoked when this timer expires.
	private Action m_callback;

	// Summary:
	// Initializes the timer with default vaules and references.
	//
	// Remarks:
	// null can be passed for timerText if no GUI is desired.
	public void Initialize(NanoTimers.Time time, Text timerText, Action callback){
		m_minutes                 = time.minutes;
		m_seconds                 = time.seconds;
		m_millis                  = time.millis;
		m_state                   = ETimerState.New;
		m_originTimeState.minutes = time.minutes;
		m_originTimeState.seconds = time.seconds;
		m_originTimeState.millis  = time.millis;
		m_timerText               = timerText;
		m_callback                = callback;
		m_isInitialized           = true;
		UpdateUI();
	}

	void FixedUpdate(){
		switch(m_state){
			case ETimerState.Active:{

				m_millis -= (int)(UnityEngine.Time.fixedDeltaTime * 1000);

				if(m_millis < 0){
					m_millis = 999;
					m_seconds -= 1;
				}

				if(m_seconds < 0){
					m_seconds = 59;
					m_minutes -= 1;
				}

				if(m_minutes < 0){
					m_minutes = 0;
					m_seconds = 0;
					m_millis = 0;
					m_state = ETimerState.Expired;
					m_callback();
				}

				UpdateUI();

				break;
			}
		}
	}

} // End of class.

} // End of namespace.
