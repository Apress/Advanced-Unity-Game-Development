using UnityEngine;

/// <summary>
/// A class that makes the blimp camera follow the main player's car across the track.
/// </summary>
public class CameraFollowXz : BaseScript {
   //***** Class Constants *****
   /// <summary>
   /// A readonly value that indicates the height the blimp camera uses to follow the player.
   /// </summary>
   public static readonly float BLIMP_FLY_HEIGHT = 110f;

   /// <summary>
   /// A readonly value that indicates a minimum value that is added to the blimp camera's calculated height.
   /// </summary>
   public static readonly float BLIMP_FLY_MIN = 30f;

   //***** Internal variables: Updtate *****
   private float x = 0.0f;
   private float y = 0.0f;
   private float z = 0.0f;

   /// <summary>
   /// The start method is called once per instance before the Update method is called.
   /// </summary>
   void Start() {
      base.Prep(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (gameState != null) {
         p = gameState.GetCurrentPlayer();
         if (p != null) {
            if (p.player != null && p.player.transform != null) {
               x = p.player.transform.position.x;
               y = (p.player.transform.position.y + ((BLIMP_FLY_HEIGHT * p.speedPrct) + BLIMP_FLY_MIN));
               z = p.player.transform.position.z;
               transform.position = new Vector3(x, y, z);
            }
         }
      }
   }
}