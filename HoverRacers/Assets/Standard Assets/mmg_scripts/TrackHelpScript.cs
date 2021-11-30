using UnityEngine;

/// <summary>
/// A class that is responsible for triggering certain help dialogs that show the player tips on how to play the game.
/// </summary>
public class TrackHelpScript : BaseScript {
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
   /// A method that processes a track help indicator.
   /// </summary>
   /// <param name="otherObj"></param>
   public void ProcessTrackHelp(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (otherObj != null && otherObj.gameObject.CompareTag(Utilities.TAG_PLAYERS)) {
         Utilities.LoadPlayerInfo(GetType().Name, out PlayerInfo pi, out int playerIndex, out p, otherObj.gameObject, gameState, false);
         if (p != null) {
            if (gameObject != null && p == gameState.GetCurrentPlayer() && p.aiOn == false) {
               if (gameObject.CompareTag("TrackHelpSlow")) {
                  if (gameState.hudNewScript != null) {
                     gameState.hudNewScript.HideHelpAccel();
                     gameState.hudNewScript.HideHelpTurn();
                     gameState.hudNewScript.ShowHelpSlow();
                  }
                  gameState.trackHelpSlowOn = true;
                  gameState.trackHelpSlowTime = 0f;
               } else if (gameObject.CompareTag("TrackHelpTurn")) {
                  if (gameState.hudNewScript != null) {
                     gameState.hudNewScript.HideHelpAccel();
                     gameState.hudNewScript.ShowHelpTurn();
                     gameState.hudNewScript.HideHelpSlow();
                  }
                  gameState.trackHelpTurnOn = true;
                  gameState.trackHelpTurnTime = 0f;
                  gameState.trackHelpOn = false;
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
         if (gameState.trackHelpOn == true) {
            ProcessTrackHelp(otherObj);
         }
      }
   }
}