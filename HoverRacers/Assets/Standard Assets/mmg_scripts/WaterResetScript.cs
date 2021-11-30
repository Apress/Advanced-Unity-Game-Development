using UnityEngine;

/// <summary>
/// A class that is used to process cars that fall into the water and reset them.
/// </summary>
public class WaterResetScript : BaseScript {
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
   /// A method to process resetting a car that falls into the water on some tracks.
   /// </summary>
   /// <param name="otherObj"></param>
   public void ProcessWaterReset(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (otherObj.gameObject.CompareTag(Utilities.TAG_PLAYERS)) {
         Utilities.LoadPlayerInfo(GetType().Name, out PlayerInfo pi, out int playerIndex, out p, otherObj.gameObject, gameState, false);
         if (p != null) {
            if (p.waypoints != null && p.waypoints.Count > 0) {
               //move car to waypoint center
               if (p.aiWaypointIndex - 5 >= 0) {
                  p.aiWaypointIndex -= 5;
               } else if (p.aiWaypointIndex - 4 >= 0) {
                  p.aiWaypointIndex -= 4;
               } else if (p.aiWaypointIndex - 3 >= 0) {
                  p.aiWaypointIndex -= 3;
               } else if (p.aiWaypointIndex - 2 >= 0) {
                  p.aiWaypointIndex -= 2;
               } else if (p.aiWaypointIndex - 1 >= 0) {
                  p.aiWaypointIndex -= 1;
               } else {
                  p.aiWaypointIndex = 0;
               }

               if (p.aiWaypointIndex >= 0 && p.aiWaypointIndex < p.waypoints.Count) {
                  p.MoveToCurrentWaypoint();
               }

               p.offTrack = false;
               p.offTrackTime = 0;
            }
         }
      }
   }

   /// <summary>
   /// A trigger event handler that fires when the collision object enters the collider.
   /// </summary>
   /// <param name="otherObj">The collider that registered the trigger event.</param>
   public virtual void OnTriggerEnter(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      ProcessWaterReset(otherObj);
   }
}