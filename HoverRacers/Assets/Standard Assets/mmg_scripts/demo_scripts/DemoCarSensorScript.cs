using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCarSensorScript : BaseScript {
   //***** Static Class Fields *****
   /// <summary>
   /// A boolean value that indicates if the script is active.
   /// </summary>
   //public static bool SCRIPT_ACTIVE = true;

   //***** Internal variables: Start *****
   private PlayerState pInf = null;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      base.Prep(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }

      /*
      SCRIPT_ACTIVE = base.Prep(this.GetType().Name);
      if (SCRIPT_ACTIVE == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }
      */

      if (gameState != null && gameState.players != null && gameState.players.Count >= 2) {
         pInf = (PlayerState)gameState.players[1];
      }
   }

   // Update is called once per frame
   void Update() {
      if(pInf != null) {
         pInf.aiIsStuck = false;
         pInf.speed = 0.0f;
         pInf.prepped = false;
         pInf.aiOn = false;
      }
   }
}
