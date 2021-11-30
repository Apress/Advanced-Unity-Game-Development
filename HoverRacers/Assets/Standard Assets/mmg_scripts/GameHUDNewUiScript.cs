using UnityEngine;

/// <summary>
/// A class used to process the main game HUD UI input.
/// </summary>
public class GameHUDNewUiScript : BaseScript {
   //***** Class Fields *****
   /// <summary>
   /// A boolean value that indicates if the exit button has been pressed.
   /// </summary>
   //private bool btnPressed = false;

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
   /// A method used to process exit button UI input and play a menu sound effect.
   /// </summary>
   public void PerformExitPromptUI() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (gameState != null) {
         if (gameState.IsExitMenuShowing() == false && gameState.IsHelpMenuShowing() == false && gameState.IsPauseMenuShowing() == false && gameState.IsEndMenuShowing() == false && gameState.IsStartMenuShowing() == false) {
            PerformExitPrompt();
         }
      }
   }

   /// <summary>
   /// A method used to process the exit button click after the sound effect is done playing.
   /// </summary>
   public void PerformExitPrompt() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.PlayMenuSound();
      gameState.ShowExitMenu();
   }
}