using UnityEngine;

/// <summary>
/// A class that holds information necessary to describe a waypoint check on the race track.
/// </summary>
public class WaypointCheck : BaseScript {
   //***** Class Fields *****
   /// <summary>
   /// An integer value that indicates what route this waypoint is associated with.
   /// </summary>
   public int waypointRoute = 0;

   /// <summary>
   /// An integer value that indicates the index of the waypoint in a list of waypoints.
   /// </summary>
   public int waypointIndex = 0;

   /// <summary>
   /// A float value that indicates what the Y value should be of a car that is placed at this waypoint.
   /// </summary>
   public float waypointStartY = 4;

   /// <summary>
   /// A float value that is used to slow the car down by the specified percentage normalized amount.
   /// </summary>
   public float waypointSlowDown = 1.0f;

   /// <summary>
   /// A boolean flag that indicates if this is a slow down waypoint.
   /// </summary>
   public bool isSlowDown = false;

   /// <summary>
   /// A float value that indicates the duration of the slow down.
   /// </summary>
   public float slowDownDuration = 100.0f;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      base.Prep(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }
   }

   /// <summary>
   /// A method for processing the current waypoint with regard to the associated player's car.
   /// </summary>
   /// <param name="otherObj">The Collider object associated with this collision.</param>
   public void ProcessWaypoint(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (otherObj != null && otherObj.gameObject.CompareTag(Utilities.TAG_PLAYERS)) {
         Utilities.LoadPlayerInfo(GetType().Name, out PlayerInfo pi, out int playerIndex, out p, otherObj.gameObject, gameState, false);
         if (p != null) {
            if ((waypointIndex + 1) < p.aiWaypointIndex && ((waypointIndex + 1) - p.aiWaypointIndex) <= 3) {
               p.wrongDirection = true;
            } else {
               p.wrongDirection = false;
            }

            if ((waypointIndex + 1) > p.aiWaypointIndex && ((waypointIndex + 1) - p.aiWaypointIndex) <= 5) {
               if(p.aiWaypointLastIndex != p.aiWaypointIndex) {
                  p.aiWaypointPassCount++;
               }

               p.aiWaypointLastIndex = p.aiWaypointIndex;
               p.StampWaypointTime();

               if (p.IsValidWaypointIndex(waypointIndex + 1) == true) {
                  p.aiWaypointIndex = (waypointIndex + 1);
               } else {
                  if (p == gameState.GetCurrentPlayer() && gameState.gameWon == false) {
                     gameState.LogLapTime(p);
                     p.lapComplete = true;
                  }

                  p.aiWaypointJumpCount = 0;
                  p.aiWaypointPassCount = 0;
                  p.aiWaypointIndex = 0;
                  if (p.currentLap + 1 <= p.totalLaps) {
                     p.currentLap++;
                  }
                  p.ResetTime();
               }

               if (p.aiWaypointIndex == 1 && p.currentLap == gameState.totalLaps && playerIndex == gameState.currentIndex) {
                  //game over
                  if (gameState.IsStartMenuShowing() == false) {
                     gameState.gameWon = true;
                     gameState.SetPositions();
                     gameState.ShowEndMenu();
                  }
               }
            } else {
               p.skippedWaypoint = true;
            }

            if (p.aiOn == true) {
               if (isSlowDown == true) {
                  p.aiSlowDownTime = 0f;
                  p.aiSlowDownDuration = slowDownDuration;
                  p.aiSlowDownOn = true;
                  p.aiSlowDown = waypointSlowDown;
               }
            }
         }
      }
   }

   /// <summary>
   /// A trigger event handler that fires when the collision object enters the collider.
   /// </summary>
   /// <param name="otherObj">The collider that registered the trigger event.</param>
   public void OnTriggerEnter(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (otherObj != null) {
         ProcessWaypoint(otherObj);
      }
   }
}