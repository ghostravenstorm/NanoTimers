using UnityEngine;
using UnityEngine.UI;

using NanoTimers;

public class GameManagerTimersDemo : MonoBehaviour{

	public Text countdownTimerText;
	public Text stopwatchText;
	public Toggle toggle;

	private CountdownTimer countdownTimer;
	private Stopwatch stopwatch;

	private void Start(){
		this.countdownTimer = this.gameObject.AddComponent<CountdownTimer>();
		this.countdownTimer.Initialize(3, 0, 0, countdownTimerText, this.OnTimerExpired);
		//this.countdownTimer.StartTimer();

		this.stopwatch = this.gameObject.AddComponent<Stopwatch>();
		this.stopwatch.Initialize(stopwatchText);
		//this.stopwatch.StartTimer();
	}

	private void OnTimerExpired(){
		Debug.Log("Timer expired.");
	}

	public void StartCountdownTimer(){
		this.countdownTimer.StartTimer();
	}

	public void PauseCountdownTimer(){
		this.countdownTimer.Pause();
	}

	public void ResetCountdownTimer(){
		this.countdownTimer.Reset();
	}

	public void PauseCountdownTimer3Seconds(){
		this.countdownTimer.PauseForSeconds(3, 0);
	}

	public void Add30Seconds(){
		this.countdownTimer.AddTime(0, 30);
	}

	public void Sub30Seconds(){
		this.countdownTimer.SubTime(0, 30);
	}

	public void OnShowMillis(bool isOn){
		Debug.Log(isOn);
		this.countdownTimer.SetShowMillis(this.toggle.isOn);
	}

	public void StartStopwatch(){
		this.stopwatch.StartTimer();
	}

	public void PauseStopwatch(){
		this.stopwatch.Pause();
	}

	public void ResetStopwatch(){
		this.stopwatch.Reset();
	}

	public void PauseStopwatch3Seconds(){
		this.stopwatch.PauseForSeconds(3, 0);
	}
}
