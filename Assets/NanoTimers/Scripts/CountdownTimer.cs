// Program: Nano Timers
// Author:  GhostRavenstorm
// Date:    2017-05-22
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
//
public class CountdownTimer : MonoBehaviour{


	// Summary:
	// The number of minutes currently on this timer.
	private int minutes_;

	// Summary:
	// The number of seconds currently on this timer.
	private int seconds_;

	// Summary:
	// The number of milliseconds currently on this timer in thousandths of a second.
	private int millis_;

	// Summary:
	// The current state of this timer.
	private ETimerState state_;

	// Summary:
	// The initial amount of minutes and seconds that were given to this timer
	// upon its creation.
	private NanoTimers.Time originTimeState_;

	// Summary:
	// Reference to a method that will be invoked when this timer expires.
	private Action callback_;

	// Summary:
	// Reference to the coroutine that can be stopped and started.
	private Coroutine coroutine_;

	// Summary:
	// Reference to the UI Text object where the timer's minutes and seconds are
	// displayed.
	private Text timerText_;

	// Summary:
	// Determines if this timer has been properly initialized and prevent it from
	// being started with null values.
	private bool isInitialized_;

	// Summary:
	// Reference to a CountdownTimer that determines how long this timer is
	// paused for.
	private CountdownTimer pausedTimer_;

	// Summary:
	// Will this timer also dispaly Milliseconds?
	private bool showMillis_ = false;

	// Summary:
	// Initializes the timer with default vaules and references.
	//
	// Remarks:
	// Timer will not function unless Initialize is called with the correct
	// parameters before StartTimer is called.
	//
	public void Initialize(
		int minutes,
		int seconds,
		int millis,
		Text timerText,
		Action callback
	){
		this.minutes_ = minutes;
		this.seconds_ = seconds;
		this.millis_ = millis;
		this.state_ = ETimerState.New;
		this.originTimeState_.minutes = minutes;
		this.originTimeState_.seconds = seconds;
		this.timerText_ = timerText;
		this.callback_ = callback;
		this.isInitialized_ = true;
		this.UpdateUI_();
	}

	// Summary:
	// Initializes the timer with default vaules and references.
	//
	// Remarks:
	// Overloaded method that takes a NanoTimers.Time as a parameter.
	//
	public void Initialize(
		NanoTimers.Time time,
		Text timerText,
		Action callback
	){
		this.minutes_ = time.minutes;
		this.seconds_ = time.seconds;
		this.millis_ = time.millis;
		this.state_ = ETimerState.New;
		this.originTimeState_.minutes = time.minutes;
		this.originTimeState_.seconds = time.seconds;
		this.timerText_ = timerText;
		this.callback_ = callback;
		this.isInitialized_ = true;
		this.UpdateUI_();
	}

	// Summary:
	// Starts the timer's countdown sequence when called.
	//
	public void StartTimer(){
		if(!this.isInitialized_){
			Debug.LogError(this + " is not initialized and cannot be started!");
			return;
		}

		switch(this.state_){
         case ETimerState.New:{
            this.coroutine_ = StartCoroutine(this.Tick_());
            this.state_ = ETimerState.Active;
            break;
         }
			case ETimerState.Active:{
				break;
			}
			case ETimerState.Paused:{
				this.coroutine_ = StartCoroutine(this.Tick_());
				this.state_ = ETimerState.Active;
				break;
			}
         case ETimerState.Expired:{
				this.Reset();
				this.StartTimer();
				break;
			}
      }

	}

	// Summary:
	// Pauses the timer indefinitely until StartTimer is called again.
	//
	public void Pause(){
		if(this.coroutine_ == null)
         return;

      StopCoroutine(this.coroutine_);
      this.state_ = ETimerState.Paused;
	}

	// Summary:
	// Pauses the timer for a determied amount of seconds.
	//
	// Remarks:
	// Creates another CountdownTimer on this game object that has no UI.
	// Gets recycled when possible.
	//
	public void PauseForSeconds(int seconds, int millis){
		if(this.coroutine_ == null)
         return;

		StopCoroutine(this.coroutine_);
		this.state_ = ETimerState.Paused;

		if(this.pausedTimer_ == null){
			this.pausedTimer_ = this.gameObject.AddComponent<CountdownTimer>();
			this.pausedTimer_.Initialize(0, seconds, millis, null, this.StartTimer);
			this.pausedTimer_.StartTimer();
		}
		else if(this.pausedTimer_.GetTimerState() == ETimerState.Expired){
			this.pausedTimer_.Reset();
			this.pausedTimer_.SetTime(0, seconds, millis);
			this.pausedTimer_.StartTimer();
		}
	}

	// Summary:
	// Adds time to the timer.
	//
	// Remarks:
	// If seconds are greater than 60, the timer gets rounded into minutes.
	// Does not add milliseconds.
	//
	public void AddTime(int minutes, int seconds){
		minutes += this.minutes_;
		seconds += this.seconds_;

		while(seconds >= 60){
			seconds -= 60;
			minutes += 1;
		}

		this.minutes_ = minutes;
		this.seconds_ = seconds;

		this.UpdateUI_();
	}

	// Summary:
	// Subtracts time from the timer.
	//
	// Remarks:
	// Timer get arounded into minutes when seconds become less than 0.
	// Does not subtract milliseconds.
	//
	public void SubTime(int minutes, int seconds){
		minutes = this.minutes_ - minutes;
		seconds = this.seconds_ - seconds;

		while(seconds < 0){
			seconds += 60;
			minutes -= 1;
		}

		if(minutes < 0){
			this.minutes_ = 0;
			this.seconds_ = 0;
		}
		else{
			this.minutes_ = minutes;
			this.seconds_ = seconds;
		}

		this.UpdateUI_();
	}

	// Summary:
	// Sets the timer's time to this specific amount.
	//
	// Remarks:
	// Milliseconds roll over into seconds and seconds roll over into minutes.
	//
	public void SetTime(int minutes, int seconds, int millis){

		while(millis >= 1000){
			millis -= 1000;
			seconds += 1;
		}

		while(seconds >= 60){
			seconds -= 60;
			minutes += 1;
		}

		this.minutes_ = minutes;
		this.seconds_ = seconds;

		this.UpdateUI_();
	}

	// Summary:
	// Returns the timer's current time as a NanoTimers.Time structure.
	//
	public NanoTimers.Time GetTime(){
		NanoTimers.Time time;
		time.minutes = this.minutes_;
		time.seconds = this.seconds_;
		time.millis = this.millis_;

		return time;
	}

	// Summary:
	// Returns the timer's current state.
	//
	public ETimerState GetTimerState(){
		return this.state_;
	}

	// Summary:
	// Resets the timer to the original minutes and seconds it was created with
	// and updates the ui.
	//
	// Remarks:
	// Does not re-start the timer. StartTimer must be called afterwards.
	//
	public void Reset(){
		this.minutes_ = originTimeState_.minutes;
		this.seconds_ = originTimeState_.seconds;
		this.millis_ = originTimeState_.millis;
		this.state_ = ETimerState.New;
		StopCoroutine(this.coroutine_);
		this.UpdateUI_();
	}

	public void SetShowMillis(bool isShown){
		this.showMillis_ = isShown;
		this.UpdateUI_();
	}

	// Summary:
	// Makes an exact clone of this timer for this game object and returns a
	// reference to the newly cloned timer.
	//
	// Remarks:
	// Does not deep clone the original start time of this timer.
	//
	public CountdownTimer Clone(Text timerText){
		CountdownTimer timer = this.gameObject.AddComponent<CountdownTimer>();

		timer.Initialize(
			this.minutes_,
			this.seconds_,
			this.millis_,
			timerText,
			this.callback_
		);

		if(this.state_ == ETimerState.Active){
			timer.StartTimer();
		}

		return timer;
	}

	// Summary:
	// Overloaded method that takes a gameobject reference
	//
	public CountdownTimer Clone(Text timerText, GameObject gameobject){
		CountdownTimer timer = gameobject.AddComponent<CountdownTimer>();

		timer.Initialize(
			this.minutes_,
			this.seconds_,
			this.millis_,
			timerText,
			this.callback_
		);

		if(this.state_ == ETimerState.Active){
			timer.StartTimer();
		}

		return timer;
	}

	// Summary:
	// Updates visual text with the timer's current time.
	//
   // Remarks:
   // Will return instantly if no Text object is given in Initialize.
   //
	private void UpdateUI_(){
		if(this.timerText_ == null)
			return;

		string minutes;
		string seconds;
		string millis;

		// Format string for minutes.
		if(this.minutes_ < 10)
			minutes = "0" + this.minutes_.ToString();
		else
			minutes = this.minutes_.ToString();

		// Format string for seconds.
		if(this.seconds_ < 10)
			seconds = "0" + this.seconds_.ToString();
		else
			seconds = this.seconds_.ToString();

		// Format string for milliseconds if its being shown.
		if(this.showMillis_){
			if(this.millis_ < 10)
				millis = "0" + this.millis_.ToString();
			else
				millis = this.millis_.ToString();

			// Update display with milliseconds.
			this.timerText_.text = minutes + " : " + seconds + " . " + millis;
		}
		else{
			// Update display without milliseconds.
			this.timerText_.text = minutes + " : " + seconds;
		}
	}

	// Summary:
	// Primary update method that uses an infinate for-loop in a coroutine to
	// subtract 1 from Seconds every second.
	//
	private IEnumerator Tick_(){
		for(;;){

			if(this.state_ == ETimerState.Active){
				this.millis_ -= 1;

				if(this.millis_ < 0){
					this.millis_ = 99;
					this.seconds_ -= 1;

					if(this.seconds_ < 0){
						this.seconds_ = 59;
						this.minutes_ -= 1;

						if(this.minutes_ < 0){
							this.state_ = ETimerState.Expired;
							StopCoroutine(this.coroutine_);
							this.UpdateUI_();
							this.callback_();
							yield return null;
						}
					}
				}
			}

			this.UpdateUI_();

			yield return new WaitForSeconds(0.001f);
		}
	}

} // End of class.

} // End of namespace.
