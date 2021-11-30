using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class used to manage the exit game prompt.
/// </summary>
public class GameExitMenu : BasePromptScript {
   //***** Static Class Fields *****
   /// <summary>
   /// A class field used to track the count of game exit clicks used to support the screen shot mode.
   /// This mode helps take screen shots by letting the game go into AI mode one iteration before showing the normal UI.
   /// </summary>
   private static int cnt = 1;

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
   /// A method used to perform the exit game UI event.
   /// </summary>
   public void PerformExitGameUI() {
      PerformExitGame();
   }

   /// <summary>
   /// A method that is used to exit the current game.
   /// </summary>
   public void PerformExitGame() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.ToggleCurrentCarAi();
      if (gameState.scsMode == true) {
         if (cnt % 2 == 0) {
            gameState.ShowStartMenu();
            cnt++;
         } else {
            cnt++;
         }
      }
      gameState.PlayMenuSound();
      gameState.ShowStartMenu();
      gameState.HideExitMenu();
   }

   /// <summary>
   /// A method used to perform the cancel game UI event.
   /// </summary>
   public void PerformCancelUI() {
      PerformCancel();
   }

   /// <summary>
   /// A method that is used to cancel the current game.
   /// </summary>
   public void PerformCancel() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.PlayMenuSound();
      gameState.HideExitMenu();
   }
}