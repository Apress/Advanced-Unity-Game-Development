using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameState;

/// <summary>
/// A class used to manage the game over menu.
/// </summary>
public class GameOverMenu : BasePromptScript {
   //***** Enumerations *****
   /// <summary>
   /// An enumeraion the holds values representing the different race tracks,
   /// </summary>
   public enum RaceTracks {
      Track1,
      Track2,
      Track3,
      Track4
   };

   //***** Class Fields: Read Only *****
   /// <summary>
   /// The scene name for track 1.
   /// </summary>
   public readonly string TRACK_NAME_1 = "Main13";

   /// <summary>
   /// The scene name for track 2.
   /// </summary>
   public readonly string TRACK_NAME_2 = "Main14";

   /// <summary>
   /// The scene name for track 3.
   /// </summary>
   public readonly string TRACK_NAME_3 = "MyTrack15Demonstration";

   /// <summary>
   /// The scene name for track 4.
   /// </summary>
   public readonly string TRACK_NAME_4 = "MyTrack15Demonstration";

   //***** Class Fields *****
   /// <summary>
   /// An Image object used to display the win image.
   /// </summary>
   public Image win = null;

   /// <summary>
   /// An Image object used to display the lose image.
   /// </summary>
   public Image lose = null;

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
   /// A method used to show the win image.
   /// </summary>
   public void ShowWinImage() {
      win.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the win image.
   /// </summary>
   public void HideWinImage() {
      win.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the lose image.
   /// </summary>
   public void ShowLoseImage() {
      lose.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the show image.
   /// </summary>
   public void HideLoseImage() {
      lose.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to process the end game button click event.
   /// </summary>
   public void PerformEndGameUI() {
      PerformEndGame();
   }

   /// <summary>
   /// A method used to end the current game.
   /// </summary>
   public void PerformEndGame() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      PlayerPrefs.SetInt("GameStateIndex", (int)GameStateIndex.NONE);
      PlayerPrefs.Save();
      gameState.PlayMenuSound();
      gameState.PauseGame();
      gameState.ResetGame();
      SceneManager.LoadScene(gameState.sceneName);
   }

   /// <summary>
   /// A method used to process the replay game button click event.
   /// </summary>
   public void PerformReplayGameUI() {
      PerformReplayGame();
   }

   /// <summary>
   /// A method used to restart the current race.
   /// </summary>
   public void PerformReplayGame() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.PlayMenuSound();
      gameState.PauseGame();
      StartLevel();
   }

   /// <summary>
   /// A method used to start the level specified by the player preferences race track value.
   /// </summary>
   public void StartLevel() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      int raceTrack = PlayerPrefs.GetInt("RaceTrack");
      PlayerPrefs.SetInt("GameStateIndex", (int)GameStateIndex.GAME_PLAY_SCREEN);
      PlayerPrefs.Save();

      gameState.ResetGame();
      if (raceTrack == (int)RaceTracks.Track1) {
         SceneManager.LoadScene(TRACK_NAME_1);
      } else if (raceTrack == (int)RaceTracks.Track2) {
         SceneManager.LoadScene(TRACK_NAME_2);
      } else if (raceTrack == (int)RaceTracks.Track3) {
         SceneManager.LoadScene(TRACK_NAME_3);
      } else if (raceTrack == (int)RaceTracks.Track4) {
         SceneManager.LoadScene(TRACK_NAME_4);
      }
   }
}