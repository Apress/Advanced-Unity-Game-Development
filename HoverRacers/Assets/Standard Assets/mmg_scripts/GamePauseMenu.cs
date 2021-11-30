using UnityEngine;
using UnityEngine.SceneManagement;
using static GameState;

/// <summary>
/// A class used to manage the pause menu.
/// </summary>
public class GamePauseMenu : BasePromptScript {
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
   /// A method used to perform the resume game UI event.
   /// </summary>
   public void PerformResumeGameUI() {
      PerformResumeGame();
   }

   /// <summary>
   /// A method used to resume the currently paused game.
   /// </summary>
   public void PerformResumeGame() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.PlayMenuSound();
      gameState.HidePauseMenu();
   }

   /// <summary>
   /// A method used to perform the end game UI event.
   /// </summary>
   public void PerformEndGameUI() {
      PerformEndGame();
   }

   /// <summary>
   /// A method used to end the currently paused game.
   /// </summary>
   public void PerformEndGame() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      PlayerPrefs.SetInt("GameStateIndex", (int)GameStateIndex.NONE);
      PlayerPrefs.Save();
      gameState.PlayMenuSound();
      gameState.ResetGame();
      SceneManager.LoadScene(gameState.sceneName);
   }
}