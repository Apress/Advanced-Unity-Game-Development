using UnityEngine;

/// <summary>
/// A class that monitors a strip of track for cars on the track.
/// </summary>
public class RoadScript : BaseScript {
   //***** Class Fields *****
   /// <summary>
   /// The delay in seconds to use when invoking the slow down modifier on the current player's car.
   /// </summary>
   private float delay = 5.0f;

   /// <summary>
   /// A PlayerState object that represents the current player's state.
   /// </summary>
   private PlayerState sdp;

   //***** Internal Variables: OnTrigger Methods *****
   private PlayerState pEntr = null;
   private PlayerState pStay = null;
   private PlayerState pExit = null;

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
   /// A method that sets the car's speed mode to normal.
   /// </summary>
   /// <param name="p"></param>
   public void SpeedUp(PlayerState p) {
      if (p != null && p.offTrack == true) {
         p.offTrack = false;
         p.SetNorm();
      }
   }

   /// <summary>
   /// A method that sets the car's speed mode to slow.
   /// </summary>
   /// <param name="p"></param>
   public void SlowDown(PlayerState p) {
      if (p != null && p.offTrack == false && p.controller.isGrounded == true) {
         p.offTrack = true;
         p.SetSlow();
      }
   }

   /// <summary>
   /// A method to set the car's speed mode to slow.
   /// </summary>
   public void RunSlowDown() {
      SlowDown(sdp);
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
         Utilities.LoadPlayerInfo(GetType().Name, out PlayerInfo pi, out int playerIndex, out pEntr, otherObj.gameObject, gameState, false);
         if (pEntr != null) {
            SpeedUp(pEntr);
         }
      }
   }

   /// <summary>
   /// A trigger event handler that fires when the collision object stays in the collider.
   /// </summary>
   /// <param name="otherObj">The collider that registered the trigger event.</param>
   public void OnTriggerStay(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (otherObj != null) {
         Utilities.LoadPlayerInfo(scriptName, out PlayerInfo pi, out int playerIndex, out pStay, otherObj.gameObject, gameState, false);
         if (pStay != null) {
            SpeedUp(pStay);
         }
      }
   }

   /// <summary>
   /// A trigger event handler that fires when the collision object exits the collider.
   /// </summary>
   /// <param name="otherObj">The collider that registered the trigger event.</param>
   public void OnTriggerExit(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (otherObj != null) {
         Utilities.LoadPlayerInfo(GetType().Name, out PlayerInfo pi, out int playerIndex, out pExit, otherObj.gameObject, gameState, false);
         if (pExit != null && pExit.isJumping == false && pExit.boostOn == false) {
            sdp = pExit;
            Invoke(nameof(RunSlowDown), delay);
         }
      }
   }
}