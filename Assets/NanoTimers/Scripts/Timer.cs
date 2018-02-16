// Program: Nano Timers
// Author:  GhostRavenstorm
// Version: 0.1.3
//
// Summary: Timer library that includes countdown timers and stopwatches.

using UnityEngine;
using UnityEngine.UI;

namespace NanoTimers{

// Summary:
// Framework for a basic timer.

public abstract class Timer : MonoBehaviour {

   // Summary:
	// The number of minutes currently on this timer.
   protected int m_minutes;

   // Summary:
	// The number of seconds currently on this timer.
   protected int m_seconds;

   // Summary:
	// The number of milliseconds currently on this timer in thousandths of a second.
   protected int m_millis;

   // Summary:
	// Will this timer also dispaly Milliseconds?
   protected bool m_showMillis;

   // Summary:
	// The current state of this timer.
   protected ETimerState m_state;

   // Summary:
	// Reference to the UI Text object where the timer's minutes and seconds are
	// displayed.
   protected Text m_timerText;

   // Summary:
	// Reference to a CountdownTimer that determines how long this timer is
	// paused for.
	protected CountdownTimer m_pausedTimer;

   // Summary:
	// The initial amount of minutes and seconds that were given to this timer
	// upon its creation.
	protected NanoTimers.Time m_originTimeState;

   // Summary:
	// Determines if this timer has been properly initialized and prevent it from
	// being started with null values.
	protected bool m_isInitialized;

   // Prints warnings and errors to console.
   [SerializeField]
   protected bool m_debug;

   // SUMMARY:
   // Returns the timer's current state.
   public ETimerState State{
      get{
         return m_state;
      }
   }

   // Summary:
	// Returns the timer's current time.
   public NanoTimers.Time Time{
      get{
         return new NanoTimers.Time(
            m_minutes,
            m_seconds,
            m_millis
         );
      }
   }

   // Summary:
	// Enables the timer's active state if initialized.
	public void StartTimer(){
		if(!m_isInitialized){
			Debug.LogError(this + " is not initialized and cannot be started!");
			return;
		}

		switch(m_state){
         case ETimerState.New:{
            m_state = ETimerState.Active;
            break;
         }
			case ETimerState.Active:{
				break;
			}
			case ETimerState.Paused:{
				m_state = ETimerState.Active;
				break;
			}
         case ETimerState.Expired:{
				Reset();
				StartTimer();
				break;
			}
      }
	}

   // Summary:
	// Pauses the timer indefinitely.
	public void Pause(){
      m_state = ETimerState.Paused;
	}

   // Summary:
	// Pauses the timer for a determied amount of seconds.
	//
	// Remarks:
	// Creates another CountdownTimer on this game object that has no UI.
	// Gets recycled when possible.
	public void PauseForSeconds(int seconds, int millis){

		m_state = ETimerState.Paused;

		if(m_pausedTimer == null){
			m_pausedTimer = gameObject.AddComponent<CountdownTimer>();
			m_pausedTimer.Initialize(new NanoTimers.Time(0, seconds, millis), null, StartTimer, false);
			m_pausedTimer.StartTimer();
		}
		else{
			switch(m_pausedTimer.State){
				case ETimerState.Expired:{
					m_pausedTimer.Reset();
					m_pausedTimer.SetTime(new NanoTimers.Time(0, seconds, millis));
					m_pausedTimer.StartTimer();
					break;
				}
			}
		}
	}

   // Summary:
	// Adds time to the timer.
	//
	// Remarks:
	// If seconds are greater than 60, the timer gets rounded into minutes.
	// Does not add milliseconds.
	public void AddTime(NanoTimers.Time time){

      if(m_state == ETimerState.Expired) return;

		int minutes = time.minutes + m_minutes;
		int seconds = time.seconds + m_seconds;
      int millis  = time.millis  + m_millis;

      while(millis >= 1000){
         millis  -= 1000;
         seconds += 1;
      }

		while(seconds >= 60){
			seconds -= 60;
			minutes += 1;
		}

      m_minutes = minutes;
      m_seconds = seconds;
      m_millis  = millis;

		UpdateUI();
	}

   // Summary:
	// Subtracts time from the timer.
	//
	// Remarks:
	// Timer get arounded into minutes when seconds become less than 0.
	// Does not subtract milliseconds.
	public void SubTime(NanoTimers.Time time){

      if(m_state == ETimerState.Expired) return;

      int minutes = m_minutes - time.minutes;
      int seconds = m_seconds - time.seconds;
      int millis  = m_millis  - time.millis;

      while(millis < 0){
         millis  += 1000;
         seconds -= 1;
      }

		while(seconds < 0){
			seconds += 60;
			minutes -= 1;
		}

      m_minutes = minutes;
      m_seconds = seconds;
      m_millis  = millis;

		UpdateUI();
	}

   // Summary:
   // Sets the timer's time to this specific amount.
   //
   // Remarks:
   // Milliseconds roll over into seconds and seconds roll over into minutes.
   public void SetTime(NanoTimers.Time time){

      int minutes = time.minutes;
      int seconds = time.seconds;
      int millis  = time.millis;

      while(millis >= 1000){
         millis -= 1000;
         seconds += 1;
      }

      while(seconds >= 60){
         seconds -= 60;
         minutes += 1;
      }

      m_minutes = minutes;
      m_seconds = seconds;
      m_millis = millis;

      UpdateUI();
   }

   // Summary:
	// Resets the timer to the original minutes and seconds it was created with
	// and updates the ui.
	//
	// Remarks:
	// Does not restart the timer. StartTimer must be called afterwards.
	public void Reset(){
		m_minutes = m_originTimeState.minutes;
		m_seconds = m_originTimeState.seconds;
		m_millis  = m_originTimeState.millis;
		m_state   = ETimerState.New;
		UpdateUI();
	}

   public void SetShowMillis(bool isShown){
		m_showMillis = isShown;
		UpdateUI();
	}

   // Summary:
	// Updates visual text with the timer's current time.
	//
   // Remarks:
   // Will return instantly if no Text object is given in Initialize.
   protected void UpdateUI(){
		if(!m_timerText) return;

		string minutes;
		string seconds;
		string millis;

		// Format string for minutes.
		if(m_minutes < 10)
			minutes = "0" + m_minutes.ToString();
		else
			minutes = m_minutes.ToString();

		// Format string for seconds.
		if(m_seconds < 10)
			seconds = "0" + m_seconds.ToString();
		else
			seconds = m_seconds.ToString();

		// Format string for milliseconds if its being shown.
		if(m_showMillis){
			if(m_millis < 10)
				millis = "00" + m_millis.ToString();
			else if(m_millis < 100)
				millis = "0" + m_millis.ToString();
			else
				millis = m_millis.ToString();

			m_timerText.text = minutes + " : " + seconds + " . " + millis;
		}
		else{
			m_timerText.text = minutes + " : " + seconds;
		}
	}

   static int ConvertToMillis(NanoTimers.Time time){
      return (((time.minutes * 60) + time.seconds) * 1000) + time.millis;
   }
}

}
