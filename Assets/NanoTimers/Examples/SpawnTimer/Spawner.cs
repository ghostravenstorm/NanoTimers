// Program: Nano Timers
// Author:  GhostRavenstorm
// Version: 0.1.3
//
// Summary: Timer library that includes countdown timers and stopwatches.

using UnityEngine;
using UnityEngine.UI;
using NanoTimers;

public class Spawner : MonoBehaviour{

   // Spawn rate in milliseconds.
   [SerializeField]
   private int spawnFreq = 1500;

   // CountdownTimer object ref
   private CountdownTimer m_spawntimer;

   // Counter and array for storing the objects this spawner creates.
   private int spawnCount = 0;
   private GameObject[] spawnedThings = new GameObject[5];

   private void Start(){

      // Create timer object for spawner object.
      m_spawntimer = gameObject.AddComponent<CountdownTimer>();

      // Initialize timer.
      // Passing null as the param for the GUI because this timer doesn't need to be displayed.
      m_spawntimer.Initialize(new NanoTimers.Time(0, 0, spawnFreq), null, m_spawn, false);

      m_spawntimer.StartTimer();

   }

   private void m_spawn(){
      // Create object with sphere mesh of random size.
      GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      float randSize = Random.Range(0.3f, 1.0f);
      obj.transform.localScale = new Vector3(randSize, randSize, randSize);

      // Add collider and rigidbody for physics simulation.
      obj.AddComponent<SphereCollider>();
      obj.AddComponent<Rigidbody>();

      // Create random color.
      obj.GetComponent<Renderer>().material.color = Random.ColorHSV();

      // Set original position to the spawner's position.
      obj.transform.position = gameObject.transform.position;

      // Add spawned obj to array and delete the oldest obj once 5 have been created.
      if(spawnedThings[spawnCount] != null) Destroy(spawnedThings[spawnCount]);
      spawnedThings[spawnCount] = obj;
      spawnCount++;
      if(spawnCount > 4) spawnCount = 0;

      // Restart the timer.
      m_spawntimer.StartTimer();
   }
}
