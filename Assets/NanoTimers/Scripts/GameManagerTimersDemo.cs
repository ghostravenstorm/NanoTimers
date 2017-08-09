// Program: Nano Timers
// Author:  GhostRavenstorm
// Date:    2017-08-09
// Version: 0.1.2beta
//
// Summary: Timer library that includes countdown timers and stopwatches.

using UnityEngine;
using UnityEngine.UI;

using NanoTimers;

// Demo script that show how to use timers.

public class GameManagerTimersDemo : MonoBehaviour{

	public Text m_countdownTimerText;
	public Text m_stopwatchText;
	public Toggle m_toggle;

	private CountdownTimer m_countdownTimer;
	private Stopwatch m_stopwatch;

	private void Start(){
		// Create the timer object using AddComponent
		m_countdownTimer = gameObject.AddComponent<CountdownTimer>();

		// Call the initializer that sets the time, gives a reference to a text element,
		// and passes in a method to call when timer has expired.
		m_countdownTimer.Initialize(new NanoTimers.Time(3, 0, 0), m_countdownTimerText, OnTimerExpired);

		m_stopwatch = gameObject.AddComponent<Stopwatch>();
		m_stopwatch.Initialize(m_stopwatchText);
	}

	// Methods that executes when the timer's expired event is called.
	private void OnTimerExpired(){
		Debug.Log("Timer expired.");
	}

	// ** Demo Button Functions **

	public void StartCountdownTimer(){
		m_countdownTimer.StartTimer();
	}

	public void PauseCountdownTimer(){
		m_countdownTimer.Pause();
	}

	public void ResetCountdownTimer(){
		m_countdownTimer.Reset();
	}

	public void PauseCountdownTimer3Seconds(){

		// Calls the timed pause method and sets it to 3 seconds and 0 milliseconds.
		m_countdownTimer.PauseForSeconds(3, 0);
	}

	public void Add30Seconds(){

		// Adds 30 seconds using a struct initialized to 0 minutes, 30 seconds, and 0 milliseconds.
		m_countdownTimer.AddTime(new NanoTimers.Time(0, 30, 0));
	}

	public void Sub30Seconds(){

		// Substracts 30 seconds using a struct initialized to 0 minutes, 30 seconds, and 0 milliseconds.
		m_countdownTimer.SubTime(new NanoTimers.Time(0, 30, 0));
	}

	public void OnShowMillis(bool isOn){
		m_countdownTimer.SetShowMillis(m_toggle.isOn);
	}

	public void StartStopwatch(){
		m_stopwatch.StartTimer();
	}

	public void PauseStopwatch(){
		m_stopwatch.Pause();
	}

	public void ResetStopwatch(){
		m_stopwatch.Reset();
	}

	public void PauseStopwatch3Seconds(){
		m_stopwatch.PauseForSeconds(3, 0);
	}
}
