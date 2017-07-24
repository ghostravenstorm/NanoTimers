// Program: Nano Timers
// Author:  GhostRavenstorm
// Date:    2017-05-14
//
// Summary: Timer library that includes countdown timers and stopwatches.

namespace NanoTimers{

// Summary:
// Structure that contains minutes, seconds, and milliseconds.
//
// Remarks:
// 'Time' is a structure that already exists in UnityEngine namespace.
//
public struct Time{
	public int minutes;
	public int seconds;
	public int millis;

	public Time(int minutes, int seconds, int millis){
		this.minutes = minutes;
		this.seconds = seconds;
		this.millis = millis;
	}

	// Summary:
	// Custom operator for addition.
	//
	// public static TimeStamp operator +(TimeStamp ts1, TimeStamp ts2){
	// 	int minutes = ts1.minutes + ts2.minutes;
	// 	int seconds = ts1.seconds + ts2.seconds;
	//
	// 	while(seconds >= 60){
	// 		seconds -= 60;
	// 		minutes += 1;
	// 	}
	//
	// 	return new TimeStamp(minutes, seconds);
	// }

	// Summary:
	// Cusotm pperator for subtraction.
	//
	// public static TimeStamp operator -(TimeStamp ts1, TimeStamp ts2){
	// 	int minutes = ts1.minutes - ts2.minutes;
	// 	int seconds = ts1.seconds - ts2.seconds;
	//
	// 	while(seconds < 0){
	// 		seconds += 60;
	// 		minutes -= 1;
	// 	}
	//
	// 	if(minutes < 0)
	// 		return new TimeStamp(0, 0);
	// 	else
	// 		return new TimeStamp(minutes, seconds);
	//
	// }

} // End of struct.

} // End of namespace.
