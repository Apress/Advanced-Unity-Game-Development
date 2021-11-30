using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameState;

/// <summary>
/// A class used to manage the start menu.
/// </summary>
public class GameStartMenu : BasePromptScript {
   //***** Static Class Fields *****
   /// <summary>
   /// A string representation of the track 1 scene.
   /// </summary>
   public static string TRACK_NAME_1 = "Main13";

   /// <summary>
   /// A string representation of the track 2 scene.
   /// </summary>
   public static string TRACK_NAME_2 = "Main14";

   /// <summary>
   /// A string representation of the track 3 scene.
   /// </summary>
   public static string TRACK_NAME_3 = "MyTrack15Demonstration";

   /// <summary>
   /// A string representation of the track 4 scene.
   /// </summary>
   public static string TRACK_NAME_4 = "MyTrack15Demonstration";

   /// <summary>
   /// A boolean flag used to clear the player preferences before a race.
   /// </summary>
   public static bool CLEAR_PLAYER_PREFS = true;

   /// <summary>
   /// An integer value representing the maximum number of races before showing an ad.
   /// </summary>
   public readonly int MAX_RACES = 1;

   //***** Class Fields: Static *****
   /// <summary>
   /// A RaceTracks objects that represents one of the available tracks.
   /// </summary>
   public static RaceTracks raceTrack = RaceTracks.Track1;

   /// <summary>
   /// A boolean value that indicates the race type is set to easy. 
   /// </summary>
   public static bool easyOn = false;

   /// <summary>
   /// A boolean value that indicates the race type is set to battle.
   /// </summary>
   public static bool battleOn = false;

   /// <summary>
   /// A boolean value that indicates the race difficulty is set to classic.
   /// </summary>
   public static bool classicOn = false;

   /// <summary>
   /// A boolean value that indicates the race difficulty is set to low.
   /// </summary>
   public static bool lowOn = false;

   /// <summary>
   /// A boolean value that indicates the race difficulty is set to medium.
   /// </summary>
   public static bool medOn = false;

   /// <summary>
   /// A boolean value that indicates the race difficulty is set to high.
   /// </summary>
   public static bool highOn = false;

   /// <summary>
   /// The number of times a race has been started. Used to determine if an ad should be shown.
   /// </summary>
   private static int startCount = 0;

   /// <summary>
   /// A boolean value indicating if an ad support is enabled.
   /// </summary>
   public static bool ADS_ON = false;

   /// <summary>
   /// A boolean value indicating if unity ad support is enabled.
   /// </summary>
   public static bool UNITY_ADS_ON = false;

   //***** Enumerations *****
   /// <summary>
   /// An enumeration that represents the available tracks to race on.
   /// </summary>
   public enum RaceTracks {
      Track1,
      Track2,
      Track3,
      Track4
   };

   //***** Class Fields *****
   /// <summary>
   /// A Toggle UI element used to turn on the easy race type.
   /// </summary>
   public Toggle togEasyOn = null;

   /// <summary>
   /// A Toggle UI element used to turn on the battle race type.
   /// </summary>
   public Toggle togBattleOn = null;

   /// <summary>
   /// A Toggle UI element used to turn on the classic race type.
   /// </summary>
   public Toggle togClassicOn = null;

   /// <summary>
   /// A Toggle UI elements used to set the race difficulty to low.
   /// </summary>
   public Toggle togLowOn = null;

   /// <summary>
   /// A Toggle UI elements used to set the race difficulty to medium.
   /// </summary>
   public Toggle togMedOn = null;

   /// <summary>
   /// A Toggle UI elements used to set the race difficulty to high.
   /// </summary>
   public Toggle togHighOn = null;

   /// <summary>
   /// A Text UI element used to display text at the footer of the game start menu.
   /// </summary>
   public Text txtFooter = null;

   /// <summary>
   /// A Button UI element used to start a race on track 1.
   /// </summary>
   public Button trackOneButton;

   /// <summary>
   /// A Button UI element used to start a race on track 2.
   /// </summary>
   public Button trackTwoButton;

   /// <summary>
   /// A Button UI element used to start a race on track 3.
   /// </summary>
   public Button trackThreeButton;

   /// <summary>
   /// A Button UI element used to start a race on track 4.
   /// </summary>
   public Button trackFourButton;

   /// <summary>
   /// A Button UI element used to open the help menu.
   /// </summary>
   public Button trackHelpButton;

   /// <summary>
   /// Start is called before the first frame update. 
   /// </summary>
   void Start() {
      keyBrdInputIdxMax = 11;
      base.Prep(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }

      if(CLEAR_PLAYER_PREFS) {
         PlayerPrefs.DeleteAll();
      }

      if (ADS_ON == false) {
         txtFooter.gameObject.SetActive(false);
      }

      if (PlayerPrefs.HasKey("EasyOn") == true && PlayerPrefs.GetInt("EasyOn") == 1) {
         SetEasyOn(true, true);
      } else if (PlayerPrefs.HasKey("BattleOn") == true && PlayerPrefs.GetInt("BattleOn") == 1) {
         SetBattleOn(true, true);
      } else if (PlayerPrefs.HasKey("ClassicOn") == true && PlayerPrefs.GetInt("ClassicOn") == 1) {
         SetClassicOn(true, true);
      } else {
         SetEasyOn(true, true);
      }

      if (PlayerPrefs.HasKey("LowOn") == true && PlayerPrefs.GetInt("LowOn") == 1) {
         SetLowOn(true, true);
      } else if (PlayerPrefs.HasKey("MedOn") == true && PlayerPrefs.GetInt("MedOn") == 1) {
         SetMedOn(true, true);
      } else if (PlayerPrefs.HasKey("HighOn") == true && PlayerPrefs.GetInt("HighOn") == 1) {
         SetHighOn(true, true);
      } else {
         SetLowOn(true, true);
      }

      SetTxtFooter();
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   new void Update() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (keyBrdInput == false) {
         if (Input.GetButtonUp("MenuSelectUp")) {
            keyBrdInput = true;
            keyBrdInputIdx = 0;
            keyBrdInputPrevIdx = -1;
            SetBtnTextColor(keyBrdInputPrevIdx, keyBrdInputIdx);
         } else if (Input.GetButtonDown("MenuSelectDown")) {
            keyBrdInput = true;
            keyBrdInputIdx = 10;
            keyBrdInputPrevIdx = -1;
            SetBtnTextColor(keyBrdInputPrevIdx, keyBrdInputIdx);
         }
      } else {
         if (Input.GetButtonUp("MenuSelectUp")) {
            if (keyBrdInputIdx + 1 < keyBrdInputIdxMax) {
               keyBrdInputPrevIdx = keyBrdInputIdx;
               keyBrdInputIdx++;
            } else {
               keyBrdInputPrevIdx = (keyBrdInputIdxMax - 1);
               keyBrdInputIdx = 0;
            }
            SetBtnTextColor(keyBrdInputPrevIdx, keyBrdInputIdx);
         } else if (Input.GetButtonDown("MenuSelectDown")) {
            if (keyBrdInputIdx - 1 >= keyBrdInputIdxMin) {
               keyBrdInputPrevIdx = keyBrdInputIdx;
               keyBrdInputIdx--;
            } else {
               keyBrdInputPrevIdx = 0;
               keyBrdInputIdx = (keyBrdInputIdxMax - 1);
            }
            SetBtnTextColor(keyBrdInputPrevIdx, keyBrdInputIdx);
         } else if (Input.GetButtonUp("Submit")) {
            InvokeClick(keyBrdInputIdx);
         }
      }
   }

   /// <summary>
   /// A method used to invoke a button click on this form.
   /// </summary>
   /// <param name="current">An integer value that represents which button click to invoke.</param>
   public new void InvokeClick(int current) {
      if (current == 0) {
         trackOneButton.onClick.Invoke();
      } else if (current == 1) {
         trackTwoButton.onClick.Invoke();
      } else if (current == 2) {
         trackThreeButton.onClick.Invoke();
      } else if (current == 3) {
         trackFourButton.onClick.Invoke();
      } else if (current == 4) {
         trackHelpButton.onClick.Invoke();
      }

        //Toggle Set One
        else if (current == 5) {
         togEasyOn.isOn = true;
      } else if (current == 6) {
         togBattleOn.isOn = true;
      } else if (current == 7) {
         togClassicOn.isOn = true;
      }

        //Toggle Set Two
        else if (current == 8) {
         togLowOn.isOn = true;
      } else if (current == 9) {
         togMedOn.isOn = true;
      } else if (current == 10) {
         togHighOn.isOn = true;
      }
   }

   /// <summary>
   /// A method used to adjust the text color of UI elements.
   /// </summary>
   /// <param name="prev"></param>
   /// <param name="current"></param>
   public new void SetBtnTextColor(int prev, int current) {
      ColorBlock cb;

      if (prev == 0) {
         txt = trackOneButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.black;
      } else if (prev == 1) {
         txt = trackTwoButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.black;
      } else if (prev == 2) {
         txt = trackThreeButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.black;
      } else if (prev == 3) {
         txt = trackFourButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.black;
      } else if (prev == 4) {
         txt = trackHelpButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.black;
      }

        //Toggle Set One
        else if (prev == 5) {
         cb = togEasyOn.colors;
         cb.normalColor = Color.white;
         togEasyOn.colors = cb;
      } else if (prev == 6) {
         cb = togBattleOn.colors;
         cb.normalColor = Color.white;
         togBattleOn.colors = cb;
      } else if (prev == 7) {
         cb = togClassicOn.colors;
         cb.normalColor = Color.white;
         togClassicOn.colors = cb;
      }

        //Toggle Set Two
        else if (prev == 8) {
         cb = togLowOn.colors;
         cb.normalColor = Color.white;
         togLowOn.colors = cb;
      } else if (prev == 9) {
         cb = togMedOn.colors;
         cb.normalColor = Color.white;
         togMedOn.colors = cb;
      } else if (prev == 10) {
         cb = togHighOn.colors;
         cb.normalColor = Color.white;
         togHighOn.colors = cb;
      }

      if (current == 0) {
         txt = trackOneButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.red;
      } else if (current == 1) {
         txt = trackTwoButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.red;
      } else if (current == 2) {
         txt = trackThreeButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.red;
      } else if (current == 3) {
         txt = trackFourButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.red;
      } else if (current == 4) {
         txt = trackHelpButton.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.red;
      }

        //Toggle Set One
        else if (current == 5) {
         cb = togEasyOn.colors;
         cb.normalColor = Color.red;
         togEasyOn.colors = cb;
      } else if (current == 6) {
         cb = togBattleOn.colors;
         cb.normalColor = Color.red;
         togBattleOn.colors = cb;
      } else if (current == 7) {
         cb = togClassicOn.colors;
         cb.normalColor = Color.red;
         togClassicOn.colors = cb;
      }

        //Toggle Set Two
        else if (current == 8) {
         cb = togLowOn.colors;
         cb.normalColor = Color.red;
         togLowOn.colors = cb;
      } else if (current == 9) {
         cb = togMedOn.colors;
         cb.normalColor = Color.red;
         togMedOn.colors = cb;
      } else if (current == 10) {
         cb = togHighOn.colors;
         cb.normalColor = Color.red;
         togHighOn.colors = cb;
      }
   }

   /// <summary>
   /// A method used to set the footer text value. This has been implemented to show how many races can be run before the next ad is shown.
   /// </summary>
   private void SetTxtFooter() {
      if (txtFooter != null) {
         if ((MAX_RACES - startCount) == 1) {
            txtFooter.text = "- Ad will display in 1 race. -";
         } else if ((MAX_RACES - startCount) == 0) {
            txtFooter.text = "- Ad will display on the next race. -";
         } else {
            txtFooter.text = "- Ad will display in " + (MAX_RACES - startCount) + " races. -";
         }
      }
   }

   /// <summary>
   /// A method used to start the race track.
   /// </summary>
   public void StartLevel() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      gameState.PlayMenuSound();
      gameState.PauseGame();

      if (ADS_ON == false) {
         LoadLevel();
      }
   }

   /// <summary>
   /// A callback method used to handle a result from a displayed full screen ad.
   /// </summary>
   /// <param name="val">An integer representing that result from showing a full screen ad.</param>
   private void AdResult(int val) {
      LoadLevel();
   }

   /// <summary>
   /// A method used to convert the GameStateIndex enumeration to a standard integer value.
   /// </summary>
   /// <param name="gsi">The GameStateIndex to convert to a standard integer value.</param>
   /// <returns>An integer representation of the provided GameStateIndex.</returns>
   private int Gsi2Int(GameStateIndex gsi) {
      if (GameStateIndex.FIRST == gsi) {
         return 0;
      } else if (GameStateIndex.NONE == gsi) {
         return 1;
      } else if (GameStateIndex.MAIN_MENU_SCREEN == gsi) {
         return 2;
      } else if (GameStateIndex.GAME_OVER_SCREEN == gsi) {
         return 3;
      } else if (GameStateIndex.GAME_PAUSE_SCREEN == gsi) {
         return 4;
      } else if (GameStateIndex.GAME_PLAY_SCREEN == gsi) {
         return 5;
      } else if (GameStateIndex.GAME_EXIT_PROMPT == gsi) {
         return 1;
      } else {
         return -1;
      }
   }

   /// <summary>
   /// A method used to load the next race track to start the next race.
   /// </summary>
   private void LoadLevel() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      Utilities.wr("LoadLevel: GameStateIndex: " + Gsi2Int(GameStateIndex.GAME_PLAY_SCREEN));
      PlayerPrefs.SetInt("GameStateIndex", Gsi2Int(GameStateIndex.GAME_PLAY_SCREEN));
      PlayerPrefs.Save();
      gameState.ResetGame();

      if (SceneManager.GetActiveScene().name == "Main13Demonstration") {
         GameState.ON_GUI_SHOW_CAR_DETAILS = true;
         gameState.forceGameStart = true;
         gameState.debugOn = true;
         PlayerState.SHOW_AI_LOGIC = true;
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      } else  if (raceTrack == RaceTracks.Track1) {
         SceneManager.LoadScene(TRACK_NAME_1);
      } else if (raceTrack == RaceTracks.Track2) {
         SceneManager.LoadScene(TRACK_NAME_2);
      } else if (raceTrack == RaceTracks.Track3) {
         SceneManager.LoadScene(TRACK_NAME_3);
      } else if (raceTrack == RaceTracks.Track4) {
         SceneManager.LoadScene(TRACK_NAME_4);
      }
   }

   /// <summary>
   /// A method used to play a sound effect and then invoke the show help button click.
   /// </summary>
   public void ShowHelpUI() {
      ShowHelp();
   }

   /// <summary>
   /// A method used to show the help menu. 
   /// </summary>
   public void ShowHelp() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.PlayMenuSound();
      gameState.HideStartMenu();
      gameState.PauseGame();
      gameState.ShowHelpMenu();
   }

   /// <summary>
   /// A method used to play a sound effect and then invoke the set race track button click.
   /// </summary>
   /// <param name="i"></param>
   public void SetRaceTrackUI(int i) {
      SetRaceTrack(i);
      StartLevel();
   }

   /// <summary>
   /// A method used to set the current race track.
   /// </summary>
   /// <param name="i"></param>
   public void SetRaceTrack(int i) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      
      gameState.PlayMenuSound();
      
      if (i == 0) {
         raceTrack = RaceTracks.Track1;
      } else if (i == 1) {
         raceTrack = RaceTracks.Track2;
      } else if (i == 2) {
         raceTrack = RaceTracks.Track3;
      } else if (i == 3) {
         raceTrack = RaceTracks.Track4;
      }

      PlayerPrefs.SetInt("RaceTrack", (int)raceTrack);
      PlayerPrefs.Save();
   }

   /// <summary>
   /// A method used to clear all selections and reset the game start menu.
   /// </summary>
   /// <param name="ignoreIdx"></param>
   public void ClearAll(int ignoreIdx) {
      if (ignoreIdx != 0) {
         easyOn = false;
         PlayerPrefs.SetInt("EasyOn", 0);
      }

      if (ignoreIdx != 1) {
         battleOn = false;
         PlayerPrefs.SetInt("BattleOn", 0);
      }

      if (ignoreIdx != 2) {
         classicOn = false;
         PlayerPrefs.SetInt("ClassicOn", 0);
      }
      PlayerPrefs.Save();
   }

   /// <summary>
   /// A method used to clear all different menu selections and reset the game start menu.
   /// </summary>
   /// <param name="ignoreIdx"></param>
   public void ClearAllDif(int ignoreIdx) {
      if (ignoreIdx != 0) {
         lowOn = false;
         PlayerPrefs.SetInt("LowOn", 0);
      }

      if (ignoreIdx != 1) {
         medOn = false;
         PlayerPrefs.SetInt("MedOn", 0);
      }

      if (ignoreIdx != 2) {
         highOn = false;
         PlayerPrefs.SetInt("HighOn", 0);
      }
      PlayerPrefs.Save();
   }

   /// <summary>
   /// A method used to play a sound effect and then invoke the set race type to easy click.
   /// </summary>
   /// <param name="b"></param>
   public void SetEasyOnUI(bool b) {
      SetEasyOn(b, false);
   }

   /// <summary>
   /// A method used to set the race type to easy.
   /// </summary>
   /// <param name="b">A boolean value indicating what to set the Toggle UI element's value to.</param>
   /// <param name="updateUi">A boolean value indicating that the UI needs to be updated.</param>
   public void SetEasyOn(bool b, bool updateUi) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      gameState.PlayMenuSound();
      ClearAll(-1);
      easyOn = b;
      
      if (easyOn == true) {
         PlayerPrefs.SetInt("EasyOn", 1);
      } else {
         PlayerPrefs.SetInt("EasyOn", 0);
      }

      if (updateUi == true && togEasyOn != null) {
         togEasyOn.isOn = true;
      }
      PlayerPrefs.Save();
   }

   /// <summary>
   /// A method used to play a sound effect and then invoke the set race difficulty to low click.
   /// </summary>
   /// <param name="b"></param>
   public void SetLowOnUI(bool b) {
      SetLowOn(b, false);
   }

   /// <summary>
   /// A method used to set the race difficulty to low.
   /// </summary>
   /// <param name="b">A boolean value indicating what to set the Toggle UI element's value to.</param>
   /// <param name="updateUi">A boolean value indicating that the UI needs to be updated.</param>
   public void SetLowOn(bool b, bool updateUi) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      gameState.PlayMenuSound();
      ClearAllDif(-1);
      lowOn = b;

      if (lowOn == true) {
         PlayerPrefs.SetInt("LowOn", 1);
      } else {
         PlayerPrefs.SetInt("LowOn", 0);
      }

      if (updateUi == true && togLowOn != null) {
         togLowOn.isOn = true;
      }
      PlayerPrefs.Save();
   }

   /// <summary>
   /// A method used to play a sound effect and then invoke the set race type to battle click.
   /// </summary>
   /// <param name="b"></param>
   public void SetBattleOnUI(bool b) {
      SetBattleOn(b, false);
   }

   /// <summary>
   /// A method used to set the race type to battle.
   /// </summary>
   /// <param name="b">A boolean value indicating what to set the Toggle UI element's value to.</param>
   /// <param name="updateUi">A boolean value indicating that the UI needs to be updated.</param>
   public void SetBattleOn(bool b, bool updateUi) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      gameState.PlayMenuSound();
      ClearAll(-1);
      battleOn = b;

      if (battleOn == true) {
         PlayerPrefs.SetInt("BattleOn", 1);
      } else {
         PlayerPrefs.SetInt("BattleOn", 0);
      }

      if (updateUi == true && togBattleOn != null) {
         togBattleOn.isOn = true;
      }
      PlayerPrefs.Save();
   }

   /// <summary>
   /// A method used to play a sound effect and then invoke the set race difficulty to medium click.
   /// </summary>
   /// <param name="b">A boolean value indicating how to set the Toggle UI element.</param>
   public void SetMedOnUI(bool b) {
      SetMedOn(b, false);
   }

   /// <summary>
   /// A method used to set the race difficulty to medium.
   /// </summary>
   /// <param name="b">A boolean value indicating what to set the Toggle UI element's value to.</param>
   /// <param name="updateUi">A boolean value indicating that the UI needs to be updated.</param>
   public void SetMedOn(bool b, bool updateUi) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      gameState.PlayMenuSound();
      ClearAllDif(-1);
      medOn = b;

      if (medOn == true) {
         PlayerPrefs.SetInt("MedOn", 1);
      } else {
         PlayerPrefs.SetInt("MedOn", 0);
      }

      if (updateUi == true && togMedOn != null) {
         togMedOn.isOn = true;
      }
      PlayerPrefs.Save();
   }

   /// <summary>
   /// A method used to play a sound effect and then invoke the set race type to classic click.
   /// </summary>
   /// <param name="b">A boolean value indicating how to set the Toggle UI element.</param>
   public void SetClassicOnUI(bool b) {
      SetClassicOn(b, false);
   }

   /// <summary>
   /// A method used to set the race type to classic.
   /// </summary>
   /// <param name="b">A boolean value indicating what to set the Toggle UI element's value to.</param>
   /// <param name="updateUi">A boolean value indicating that the UI needs to be updated.</param>
   public void SetClassicOn(bool b, bool updateUi) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      gameState.PlayMenuSound();
      ClearAll(-1);
      classicOn = b;

      if (classicOn == true) {
         PlayerPrefs.SetInt("ClassicOn", 1);
      } else {
         PlayerPrefs.SetInt("ClassicOn", 0);
      }

      if (updateUi == true && togClassicOn != null) {
         togClassicOn.isOn = true;
      }
      PlayerPrefs.Save();
   }

   /// <summary>
   /// A method used to play a sound effect and then invoke the set race difficulty to high click.
   /// </summary>
   /// <param name="b">A boolean value indicating how to set the Toggle UI element.</param>
   public void SetHighOnUI(bool b) {
      SetHighOn(b, false);
   }

   /// <summary>
   /// A method used to set the race difficulty to high.
   /// </summary>
   /// <param name="b">A boolean value indicating what to set the Toggle UI element's value to.</param>
   /// <param name="updateUi">A boolean value indicating that the UI needs to be updated.</param>
   public void SetHighOn(bool b, bool updateUi) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      gameState.PlayMenuSound();
      ClearAllDif(-1);
      highOn = b;

      if (highOn == true) {
         PlayerPrefs.SetInt("HighOn", 1);
      } else {
         PlayerPrefs.SetInt("HighOn", 0);
      }

      if (updateUi == true && togHighOn != null) {
         togHighOn.isOn = true;
      }
      PlayerPrefs.Save();
   }
}