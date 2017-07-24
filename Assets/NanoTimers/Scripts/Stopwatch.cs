// Program: Nano Timers
// Author:  GhostRavenstorm
// Date:    2017-05-14
//
// Summary: Timer library that includes countdown timers and stopwatches.


using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace NanoTimers{

// Summary:
// Timer class that counts from 0 to infinity in minutes and seconds.
//
// Remarks:
// Does not make event calls.
//
public class Stopwatch : MonoBehaviour{

   // Summary:
	// The number of minutes currently on this timer.
   private int minutes_;

   // Summary:
	// The number of seconds currently on this timer.
   private int seconds_;

   // Summary:
	// The current state of this timer.
   private ETimerState state_;

   // Summary:
	// Reference to the coroutine that can be stopped and started.
   private Coroutine coroutine_;

   // Summary:
	// Reference to the UI Text object where the timer's minutes and seconds are
	// displayed.
   private Text timerText_;

   // Summary:
	// Reference to a CountdownTimer that determines how long this timer is
	// paused for.
   private CountdownTimer pausedTimer_;

   // Summary:
	// Initializes the timer with default vaules and references.
	//
	// Remarks:
	// Not required to be called for the timer to function.
   // Is required to be called if live UI is desired.
	//
   public void Initialize(Text timerText){
      this.timerText_ = timerText;
      this.state_ = ETimerState.New;
   }

   // Summary:
   // Starts the timer's stopwatch sequence.
   //
   public void StartTimer(){

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
         this.pausedTimer_.StartTimer();
      }
   }

   // Summary:
	// Returns the timer's current time as a NanoTimers.Time structure.
	//
   // Remarks:
   // Does not use milliseconds.
   //
   public NanoTimers.Time GetTime(){
      NanoTimers.Time time;
      time.minutes = this.minutes_;
      time.seconds = this.seconds_;
      time.millis = 0;
      return time;
   }

   // Summary:
	// Returns the timer's current state.
	//
   public ETimerState GetTimerState(){
      return this.state_;
   }

   // Summary:
	// Stops the timer and resets the timer to 0.
	//
	// Remarks:
	// Does not re-start the timer. StartTimer must be called afterwards.
	//
   public void Reset(){
      this.minutes_ = 0;
      this.seconds_ = 0;
      this.state_ = ETimerState.New;
      StopCoroutine(this.coroutine_);
      this.UpdateUI_();
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

      string minutes, seconds;

      if(this.minutes_ < 10)
         minutes = "0" + this.minutes_.ToString();
      else
         minutes = this.minutes_.ToString();

      if(this.seconds_ < 10)
         seconds = "0" + this.seconds_.ToString();
      else
         seconds = this.seconds_.ToString();

      this.timerText_.text = minutes + " : " + seconds;
   }

   // Summary:
	// Primary update method that uses an infinate for-loop in a coroutine to
	// add 1 to Seconds every second.
	//
   private IEnumerator Tick_(){
      for(;;){
         yield return new WaitForSeconds(1.0f);

         if(this.state_ == ETimerState.Active){
				this.seconds_ += 1;

				if(this.seconds_ > 59){
					this.seconds_ = 0;
					this.minutes_ += 1;
				}
			}

         this.UpdateUI_();
      }
   }

} // End of class.

} // End of namespace.
