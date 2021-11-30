using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A class the manages the state of the game and the players in it.
/// </summary>
public class GameState : MonoBehaviour {
   //***** Enumerations *****
   /// <summary>
   /// An enumeration that represents the current game state.
   /// </summary>
   public enum GameStateIndex {
      FIRST,
      NONE,
      MAIN_MENU_SCREEN,
      GAME_OVER_SCREEN,
      GAME_PAUSE_SCREEN,
      GAME_PLAY_SCREEN,
      GAME_EXIT_PROMPT
   };

   /// <summary>
   /// An enumeration that represents the current track difficulty.
   /// </summary>
   public enum GameDifficulty {
      LOW,
      MED,
      HIGH
   };

   //***** Static Class Fields *****
   /// <summary>
   /// A static boolean value that indicates if this is the first run of the game.
   /// </summary>
   private static bool FIRST_RUN = true;

   /// <summary>
   /// A boolean value that indicates if the car details are shown when the GUI debug data is displayed.
   /// </summary>
   public static bool ON_GUI_SHOW_CAR_DETAILS = false;

   /// <summary>
   /// A boolean value that indicates if the exit button is shown when the GUI debug data is displayed.
   /// </summary>
   public static bool ON_GUI_SHOW_EXIT_BUTTON = false;

   /// <summary>
   /// A boolean value that indicates if the waypoint information should be written to the log during the game's loading process.
   /// </summary>
   public static bool SHOW_WAYPOINT_OUTPUT = false;

   //***** Constants *****
   /// <summary>
   /// The number of seconds to wait before the start of a race.
   /// </summary>
   public static readonly float START_GAME_SECONDS = 5.0f;

   /// <summary>
   /// The number of seconds to display track help messages.
   /// </summary>
   public static readonly float TRACK_HELP_SECONDS = 2.0f;

   /// <summary>
   /// The default value used to set the total laps for the current track.
   /// </summary>
   public static readonly int DEFAULT_TOTAL_LAPS = 10;

   //***** Class Fields *****
   /// <summary>
   /// An array of available players.
   /// </summary>
   public ArrayList players = null;

   /// <summary>
   /// The state for player 1.
   /// </summary>
   public PlayerState p0;

   /// <summary>
   /// The state for player 2.
   /// </summary>
   public PlayerState p1;

   /// <summary>
   /// The state for player 3.
   /// </summary>
   public PlayerState p2;

   /// <summary>
   /// The state for player 4.
   /// </summary>
   public PlayerState p3;

   /// <summary>
   /// The state for player 5.
   /// </summary>
   public PlayerState p4;

   /// <summary>
   /// The state for player 6.
   /// </summary>
   public PlayerState p5;

   /// <summary>
   /// The state for the current player.
   /// </summary>
   public PlayerState currentPlayer = null;

   //***** Internal Variables: Start *****
   public int[] positions = null;
   public bool sortingPositions = false;
   public int currentIndex = 0;
   public int player1Index = 0;
   public LapTimeManager lapTimeManager = null;
   public int totalLaps = DEFAULT_TOTAL_LAPS;
   public int gameSettingsSet = 0;           //track type
   public int gameSettingsSubSet = 0;        //track difficulty
   public bool debugOn = false;
   public bool forceGameStart = false;
   public bool scsMode = false;
   private GUIStyle style1 = null;
   private GUIStyle style2 = null;
   public WaypointCompare wpc = new WaypointCompare();
   public ArrayList waypointRoutes = null;

   public ArrayList waypointData = null;
   public AudioSource audioBgroundSound = null;
   private AudioSource audioS = null;
   private bool nightTime = false;
   private bool player1AiOn = true;            //Player1 AI Driven
   private bool prepped = false;               //SetPlayers Method Called
   private bool ready = false;                 //Ready to Begin Game
   private bool startGame = false;             //Starting the race
   private float startGameTime = 0.0f;         //Race start count down
   public bool gameWon = false;                //Game is won
   public bool gameRunning = false;            //Game is actually running
   public bool gamePaused = false;             //Game is running but is currently paused

   //***** Internal Variables: Track Help Variables *****
   public bool trackHelpAccelOn = false;
   public float trackHelpAccelTime = 0.0f;
   public bool trackHelpSlowOn = false;
   public float trackHelpSlowTime = 0.0f;
   public bool trackHelpTurnOn = false;
   public float trackHelpTurnTime = 0.0f;

   //***** Internal Variables: Track Settings *****
   public int raceTrack = 0;
   public bool easyOn = false;
   public int raceType = 0;
   public int waypointWidthOverride = 6;
   public int waypointZeroOverride = 1;
   public bool trackHelpOn = false;
   private TrackScript trackScript = null;
   public GameDifficulty difficulty = GameDifficulty.LOW;
   public GameStateIndex gameStateIndex = GameStateIndex.FIRST;

   //***** Internal Variables: Camera Variables *****
   private GameObject blimpCamera = null;
   public string sceneName = "";
   public Camera gameCamera = null;
   public Camera rearCamera = null;

   //***** Internal Variables: Menu System Variables *****
   private GameObject gamePauseMenu = null;
   private GameObject gameStartMenu = null;
   private GameObject gameOverMenu = null;
   private GameObject gameExitMenu = null;
   private GameObject gameHelpMenu = null;
   public GameHUDNewScript hudNewScript = null;
   public GameOverMenu gameOverMenuScript = null;

   //***** Internal Variables: Touch screen variables *****
   public bool accelOn = false;
   public bool newTouch = false;
   public bool touchScreen = false;

   //***** Internal Variables: Input Variables *****
   private bool handleKeyA = false;
   private bool handleKeyD = false;
   private bool handleKey1 = false;
   private bool handleKey2 = false;
   private bool handleKey3 = false;
   private bool handleKey4 = false;
   private bool handleKey5 = false;
   private bool handleKey6 = false;

   //***** Internal Variables *****
   private int gsiTmp = 0;
   private PlayerState p;
   private int l;
   private int i;
   private int iOnGui = -1;
   private int diffOnGui = 20;
   private int posXOnGui = 10;
   private int widthLgOnGui = 180;
   private int widthSmOnGui = 140;
   private int lPg = 0;
   private int iPg = 0;
   private PlayerState pPg = null;
   private int lUpg = 0;
   private int iUpg = 0;
   private PlayerState pUpg = null;

   /// <summary>
   /// A heler OnGUI method used to position text on the screen.
   /// </summary>
   /// <param name="idx"></param>
   /// <param name="rowHeight"></param>
   /// <returns></returns>
   private int GetOnGuiPosY(int idx, int rowHeight) {
      return (idx * rowHeight);
   }

   /// <summary>
   /// A method used to draw GUI elements to the screen, used for debugging purposes and should be commented out or disbaled for release.
   /// </summary>
   public void OnGUI() {
      //Demo scene detection
      if (forceGameStart) {
         if (GUI.Button(new Rect(10, Screen.height - 55, 100, 50), "Restart")) {
            StartDemoScene();
         }

         if (SceneManager.GetActiveScene().name == "DemoCollideTrack") {
            GUI.Label(new Rect(10, Screen.height - 78, 98, 50), "Off Track: " + currentPlayer.offTrack, style2);
            GUI.Label(new Rect(10, Screen.height - 80, 100, 50), "Off Track: " + currentPlayer.offTrack, style1);
            GUI.Label(new Rect(10, Screen.height - 103, 98, 50), "Off Track Time (ms): " + currentPlayer.offTrackTime, style2);
            GUI.Label(new Rect(10, Screen.height - 105, 100, 50), "Off Track Time (ms): " + currentPlayer.offTrackTime, style1);
         } else if (SceneManager.GetActiveScene().name == "DemoCollideWaypoint") {
            GUI.Label(new Rect(10, Screen.height - 78, 98, 50), "Wrong Direction: " + currentPlayer.wrongDirection, style2);
            GUI.Label(new Rect(10, Screen.height - 80, 100, 50), "Wrong Direction: " + currentPlayer.wrongDirection, style1);
         } else if (SceneManager.GetActiveScene().name == "DemoCollideTrackHelp") {
            GUI.Label(new Rect(10, Screen.height - 78, 98, 50), "Track Help Accel: " + trackHelpAccelOn, style2);
            GUI.Label(new Rect(10, Screen.height - 80, 100, 50), "Track Help Accel: " + trackHelpAccelOn, style1);
            GUI.Label(new Rect(10, Screen.height - 103, 98, 50), "Track Help Slow: " + trackHelpSlowOn, style2);
            GUI.Label(new Rect(10, Screen.height - 105, 100, 50), "Track Help Slow: " + trackHelpSlowOn, style1);
            GUI.Label(new Rect(10, Screen.height - 128, 98, 50), "Track Help Turn: " + trackHelpTurnOn, style2);
            GUI.Label(new Rect(10, Screen.height - 130, 100, 50), "Track Help Turn: " + trackHelpTurnOn, style1);
            GUI.Label(new Rect(10, Screen.height - 153, 98, 50), "Wrong Direction: " + currentPlayer.wrongDirection, style2);
            GUI.Label(new Rect(10, Screen.height - 155, 100, 50), "Wrong Direction: " + currentPlayer.wrongDirection, style1);
         } else if (SceneManager.GetActiveScene().name == "DemoCarSensorScriptAutoPass") {
            GUI.Label(new Rect(10, Screen.height - 78, 98, 50), "Is Drafting: " + currentPlayer.isDrafting, style2);
            GUI.Label(new Rect(10, Screen.height - 80, 100, 50), "Is Drafting: " + currentPlayer.isDrafting, style1);
            GUI.Label(new Rect(10, Screen.height - 103, 98, 50), "Passing Mode: " + currentPlayer.aiPassingMode, style2);
            GUI.Label(new Rect(10, Screen.height - 105, 100, 50), "Passing Mode: " + currentPlayer.aiPassingMode, style1);
            GUI.Label(new Rect(10, Screen.height - 128, 98, 50), "AI Is Passing: " + currentPlayer.aiIsPassing, style2);
            GUI.Label(new Rect(10, Screen.height - 130, 100, 50), "AI Is Passing: " + currentPlayer.aiIsPassing, style1);
         } else if (SceneManager.GetActiveScene().name == "DemoCarSensorScriptGunShot") {
            GUI.Label(new Rect(10, Screen.height - 78, 98, 50), "AI Has Target: " + currentPlayer.aiHasTarget, style2);
            GUI.Label(new Rect(10, Screen.height - 80, 100, 50), "AI Has Target: " + currentPlayer.aiHasTarget, style1);
            GUI.Label(new Rect(10, Screen.height - 103, 98, 50), "AI Can Fire: " + currentPlayer.aiCanFire, style2);
            GUI.Label(new Rect(10, Screen.height - 105, 100, 50), "AI Can Fire: " + currentPlayer.aiCanFire, style1);
            GUI.Label(new Rect(10, Screen.height - 128, 98, 50), "Ammo: " + currentPlayer.ammo, style2);
            GUI.Label(new Rect(10, Screen.height - 130, 100, 50), "Ammo: " + currentPlayer.ammo, style1);
         } else if (SceneManager.GetActiveScene().name == "DemoEngineWhineScript") {
            float p = currentPlayer.speedPrct;
            float pch = Mathf.Clamp(p * 4.1f, 0.5f, 4.1f);
            float vlm = Mathf.Clamp(p * 0.6f, 0.2f, 0.6f);
            GUI.Label(new Rect(10, Screen.height - 78, 98, 50), "Pitch: " + pch, style2);
            GUI.Label(new Rect(10, Screen.height - 80, 100, 50), "Pitch: " + pch, style1);
            GUI.Label(new Rect(10, Screen.height - 103, 98, 50), "Volume: " + vlm, style2);
            GUI.Label(new Rect(10, Screen.height - 105, 100, 50), "Volume: " + vlm, style1);
         }
      }

      if (ON_GUI_SHOW_CAR_DETAILS) {
         iOnGui = -1;
         diffOnGui = 20;
         posXOnGui = 10;
         widthLgOnGui = 180;
         widthSmOnGui = 140;

         if (debugOn == true) {
            if (gameRunning == false) {
               ++iOnGui;
               if (style1 != null) {
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Start In: " + (START_GAME_SECONDS - startGameTime), style1);
               } else {
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Start In: " + (START_GAME_SECONDS - startGameTime));
               }
            } else {
               if (currentPlayer != null && currentPlayer.player != null && style1 != null) {
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthLgOnGui, diffOnGui), "Player: " + currentIndex + ", Active: " + currentPlayer.active + ", Pos: " + currentPlayer.position, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Speed: " + Mathf.RoundToInt(currentPlayer.speed) + "/" + ", (" + currentPlayer.speedPrct + ")", style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthLgOnGui, diffOnGui), "Time: " + currentPlayer.time + ", " + currentPlayer.timeNum, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Life: " + currentPlayer.GetLife(), style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Points: " + currentPlayer.points, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Offtrack: " + currentPlayer.offTrack + ", " + currentPlayer.offTrackTime, style1);

                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Boost: " + currentPlayer.boostOn, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Invinc: " + currentPlayer.invincOn + ", " + currentPlayer.invincTime, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Ammo: " + currentPlayer.ammo, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Drafting: " + currentPlayer.isDrafting, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Bouncing: " + currentPlayer.isBouncing, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Jumping: " + currentPlayer.isJumping + ", " + currentPlayer.controller.isGrounded, style1);

                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Armor: " + currentPlayer.armorOn, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Drafting: " + currentPlayer.isDrafting, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiDist: " + Mathf.Abs(currentPlayer.waypointDistance - currentPlayer.aiWaypointDistance), style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Dist: " + currentPlayer.waypointDistance, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiStuck: " + currentPlayer.aiIsStuckMode, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiSlide: " + currentPlayer.aiSlide, style1);

                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiSpeed: " + currentPlayer.aiSpeedStrength, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiStrafe: " + currentPlayer.aiStrafeStrength, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiTurn: " + currentPlayer.aiTurnStrength, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiSlowOn: " + currentPlayer.aiSlowDownOn + ", " + currentPlayer.aiSlowDownTime, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthLgOnGui, diffOnGui), "AiWpIndex: " + currentPlayer.aiWaypointIndex + ", " + currentPlayer.aiWaypointTime, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiPassing: " + currentPlayer.aiIsPassing + ", " + currentPlayer.aiPassingMode + ", " + currentPlayer.aiPassingTime, style1);

                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiIsLargeTurn: " + currentPlayer.aiIsLargeTurn, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiAngle: " + currentPlayer.aiNext2LookAngle, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiATarget: " + currentPlayer.aiHasTarget + ", " + currentPlayer.aiHasTargetTime, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "LastLook: " + currentPlayer.aiLastLookAngle, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "MidLook: " + currentPlayer.aiMidLookAngle, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Mid2Look: " + currentPlayer.aiMid2LookAngle, style1);

                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "NextLook: " + currentPlayer.aiNextLookAngle, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Next2Look: " + currentPlayer.aiNext2LookAngle, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiJumpCnt: " + currentPlayer.aiWaypointJumpCount, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiPassCnt: " + currentPlayer.aiWaypointPassCount, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiDist: " + currentPlayer.waypointDistance, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Rot: " + currentPlayer.player.transform.rotation, style1);

                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "RotEul: " + currentPlayer.player.transform.rotation.eulerAngles, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Vel: " + currentPlayer.cm.movement.velocity + " (" + currentPlayer.cm.movement.velocity.magnitude + ")", style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "AiRel: " + currentPlayer.aiRelativePoint, style1);
                  GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Pos: " + currentPlayer.player.transform.position, style1);
               }
            }
         } else {
            if (gameRunning == true) {
               GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthLgOnGui, diffOnGui), "Position:  " + currentPlayer.position, style1);
               GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Speed:     " + Mathf.RoundToInt(currentPlayer.speed), style1);
               GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthLgOnGui, diffOnGui), "Lap Time:  " + currentPlayer.time, style1);
               GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Life:      " + currentPlayer.GetLifeHUD(), style1);
               GUI.Label(new Rect(posXOnGui, GetOnGuiPosY(++iOnGui, diffOnGui), widthSmOnGui, diffOnGui), "Laps Left: " + currentPlayer.GetLapsLeft(), style1);
            }
         }
      }

      if (player1AiOn == false && IsPauseMenuShowing() == false) {
         if (ON_GUI_SHOW_EXIT_BUTTON) {
            if (GUI.Button(new Rect(10, Screen.height - 55, 100, 50), "Exit")) {
               ToggleCurrentCarAi();
               ShowStartMenu();
            }
         }
      }
   }

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      if (style1 == null) {
         style1 = new GUIStyle();
         style1.normal.textColor = Color.red;
         style1.fontStyle = FontStyle.Bold;
         style1.fontSize = 16;
      }

      if (style2 == null) {
         style2 = new GUIStyle();
         style2.normal.textColor = Color.black;
         style2.fontStyle = FontStyle.Bold;
         style2.fontSize = 16;
      }

      if (forceGameStart == true) {
         if (SceneManager.GetActiveScene().name == "DemoCollideTrackHelp") {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
         } else if (SceneManager.GetActiveScene().name == "DemoCollideScript") {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("BattleOn", 1);
            PlayerPrefs.SetInt("HighOn", 1);
            PlayerPrefs.Save();
         } else if (SceneManager.GetActiveScene().name == "DemoCarSensorScriptAutoPass") {
            CarSensorScript.TRIGGER_SPEED_PASSING = 0.00f;
         } else if (SceneManager.GetActiveScene().name == "DemoCarSensorScriptGunShot") {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("BattleOn", 1);
            PlayerPrefs.SetInt("HighOn", 1);
            PlayerPrefs.Save();
         } else if (SceneManager.GetActiveScene().name == "DemoCameraFollowXz") {
            GameStartMenu.TRACK_NAME_1 = "DemoCameraFollowXz";
            GameStartMenu.TRACK_NAME_2 = "DemoCameraFollowXz";
         } else if (SceneManager.GetActiveScene().name == "Main13Demonstration") {
            GameState.ON_GUI_SHOW_CAR_DETAILS = true;
            debugOn = true;
            PlayerState.SHOW_AI_LOGIC = true;
         }
      }

      audioS = GetComponent<AudioSource>();
      if (audioS == null) {
         Utilities.wrForce("GameState: audioS is null!");
      }
   }

   /// <summary>
   /// A class method that returns a boolean value indicating if certain menus are visible.
   /// </summary>
   /// <returns>A boolean value indicating if the menu screen are visible or not.</returns>
   private bool AreMenusShowing() {
      if (IsStartMenuShowing() == true || IsEndMenuShowing() == true || IsHelpMenuShowing() == true || IsExitMenuShowing() == true) {
         return true;
      } else {
         return false;
      }
   }

   /// <summary>
   /// An event handler that fires when the application is paused.
   /// </summary>
   /// <param name="pauseStatus">A boolean value indicating the current pause status.</param>
   public void OnApplicationPause(bool pauseStatus) {
      if (AreMenusShowing()) {
         if (pauseStatus == true) {
            PauseGame();
         } else {
            UnPauseGame();
         }
      } else {
         if (pauseStatus == true) {
            if (gameStateIndex == GameStateIndex.GAME_PLAY_SCREEN) {
               ShowPauseMenu();
            } else {
               PauseGame();
            }
         } else {
            if (gameStateIndex == GameStateIndex.GAME_PLAY_SCREEN) {
               HidePauseMenu();
            } else {
               UnPauseGame();
            }
         }
      }
   }

   /// <summary>
   /// Returns a boolean value indicating if the help menu is showing.
   /// </summary>
   /// <returns></returns>
   public bool IsHelpMenuShowing() {
      if (gameHelpMenu != null) {
         return gameHelpMenu.activeSelf;
      } else {
         return false;
      }
   }

   /// <summary>
   /// Returns a boolean value indicating if the pause menu is showing.
   /// </summary>
   /// <returns></returns>
   public bool IsPauseMenuShowing() {
      if (gamePauseMenu != null) {
         return gamePauseMenu.activeSelf;
      } else {
         return false;
      }
   }

   /// <summary>
   /// Returns a boolean value indicating if the end menu is showing.
   /// </summary>
   /// <returns></returns>
   public bool IsEndMenuShowing() {
      if (gameOverMenu != null) {
         return gameOverMenu.activeSelf;
      } else {
         return false;
      }
   }

   /// <summary>
   /// Returns a boolean value indicating if the start menu is showing.
   /// </summary>
   /// <returns></returns>
   public bool IsStartMenuShowing() {
      if (gameStartMenu != null) {
         return gameStartMenu.activeSelf;
      } else {
         return false;
      }
   }

   /// <summary>
   /// Returns a boolean value indicating if the exit menu is showing.
   /// </summary>
   /// <returns></returns>
   public bool IsExitMenuShowing() {
      if (gameExitMenu != null) {
         return gameExitMenu.activeSelf;
      } else {
         return false;
      }
   }

   /// <summary>
   /// Returns a boolean value indicating if track help is turned on.
   /// </summary>
   /// <returns></returns>
   public bool IsTrackHelpOn() {
      return trackHelpOn;
   }

   /// <summary>
   /// A method used to pause the game.
   /// </summary>
   public void PauseGame() {
      gamePaused = true;
      Time.timeScale = 0;

      if (players != null) {
         iPg = 0;
         pPg = null;
         lPg = players.Count;
         for (iPg = 0; iPg < lPg; iPg++) {
            pPg = (PlayerState)players[iPg];
            if (pPg != null) {
               pPg.PauseSound();
            }
         }
      }

      if (audioBgroundSound != null) {
         audioBgroundSound.Stop();
      }
   }

   /// <summary>
   /// A method used to un-pause the game.
   /// </summary>
   public void UnPauseGame() {
      gamePaused = false;
      Time.timeScale = 1;

      if (players != null) {
         iUpg = 0;
         pUpg = null;
         lUpg = players.Count;
         for (iUpg = 0; iUpg < lUpg; iUpg++) {
            pUpg = (PlayerState)players[iUpg];
            if (pUpg != null) {
               pUpg.UnPauseSound();
            }
         }
      }

      if (audioBgroundSound != null) {
         audioBgroundSound.Play();
      }
   }

   /// <summary>
   /// A method used to hide the help menu.
   /// </summary>
   public void HideHelpMenu() {
      if (gameHelpMenu != null) {
         gameHelpMenu.SetActive(false);
         UnPauseGame();
      }
   }

   /// <summary>
   /// A method used to show the help menu.
   /// </summary>
   public void ShowHelpMenu() {
      if (gameHelpMenu != null) {
         gameHelpMenu.SetActive(true);
         PauseGame();
      }
   }

   /// <summary>
   /// A method used to hide the pause menu.
   /// </summary>
   public void HidePauseMenu() {
      if (gamePauseMenu != null) {
         gamePauseMenu.SetActive(false);
         UnPauseGame();
      }
   }

   /// <summary>
   /// A method used to show the pause menu.
   /// </summary>
   public void ShowPauseMenu() {
      if (gamePauseMenu != null) {
         gamePauseMenu.SetActive(true);
         PauseGame();
      }
   }

   /// <summary>
   /// A method used to hide the exit prompt.
   /// </summary>
   public void HideExitMenu() {
      if (gameExitMenu != null) {
         gameExitMenu.SetActive(false);
         UnPauseGame();
      }
   }

   /// <summary>
   /// A method used to show the menu prompt.
   /// </summary>
   public void ShowExitMenu() {
      if (gameExitMenu != null) {
         gameExitMenu.SetActive(true);
         PauseGame();
      }
   }

   /// <summary>
   /// A method used to hide the start menu.
   /// </summary>
   public void HideStartMenu() {
      if (gameStartMenu != null) {
         gameStartMenu.SetActive(false);
      }
   }

   /// <summary>
   /// A method used to show the start menu.
   /// </summary>
   public void ShowStartMenu() {
      if (gameStartMenu != null) {
         gameStartMenu.SetActive(true);
      }
   }

   /// <summary>
   /// A method used to hide the end menu.
   /// </summary>
   public void HideEndMenu() {
      if (gameOverMenu != null) {
         gameOverMenu.SetActive(false);
      }
   }

   /// <summary>
   /// A method used to show the end menu.
   /// </summary>
   public void ShowEndMenu() {
      if (gameOverMenu != null) {
         gameOverMenu.SetActive(true);
      }
   }

   /// <summary>
   /// A method used to play the menu sound effect.
   /// </summary>
   public void PlayMenuSound() {
      if (audioS != null) {
         audioS.Play();
      }
   }

   /// <summary>
   /// A method used to print the list of found waypoints.
   /// </summary>
   public void PrintWaypoints() {
      if (waypointData != null && waypointData.Count > 0) {
         ArrayList data = (ArrayList)waypointData[0];
         int l = data.Count;
         WaypointCheck wc = null;
         for (int j = 0; j < l; j++) {
            wc = (WaypointCheck)data[j];
            if (SHOW_WAYPOINT_OUTPUT) {
               Utilities.wr(j + " Found waypoint: " + wc.waypointIndex + ", Center: " + wc.transform.position);
            }
         }
      }
   }

   /// <summary>
   /// A method used to find a waypoint in the list of known waypoints.
   /// </summary>
   public void FindWaypoints() {
      GameObject[] list = GameObject.FindGameObjectsWithTag("Waypoint");
      ArrayList routes = new ArrayList();
      int l = list.Length;
      WaypointCheck wc = null;
      int i = 0;
      int j = 0;

      for (i = 0; i < l; i++) {
         if (list[i].activeSelf == true) {
            wc = (list[i].GetComponent<WaypointCheck>());
            if (wc != null) {
               if (routes.Contains(wc.waypointRoute + "") == false) {
                  routes.Add(wc.waypointRoute + "");
               }
            }
         }
      }

      ArrayList waypoints = new ArrayList();
      ArrayList row = new ArrayList();
      l = routes.Count;

      for (i = 0; i < l; i++) {
         row.Clear();
         int l2 = list.Length;
         for (j = 0; j < l2; j++) {
            if (list[j].activeSelf == true) {
               if (waypointWidthOverride != -1) {
                  if (list[j].transform.localScale.z < 10) {
                     list[j].transform.localScale.Set(list[j].transform.localScale.x, list[j].transform.localScale.y, waypointWidthOverride);
                  }
               }

               if (waypointZeroOverride != -1) {
                  if (list[j].transform.localPosition.y == 0) {
                     list[j].transform.localScale.Set(list[j].transform.localScale.x, 1, waypointWidthOverride);
                  }
               }

               wc = (list[j].GetComponent<WaypointCheck>());
               if (wc != null) {
                  if ((wc.waypointRoute + "") == (routes[i] + "")) {
                     row.Add(wc);
                  }
               }
            }
         }

         object[] ar = row.ToArray();
         System.Array.Sort(ar, wpc);
         row = new ArrayList(ar);
         l2 = row.Count;

         for (j = 0; j < l2; j++) {
            wc = (WaypointCheck)row[j];
            wc.waypointIndex = j;
         }

         waypoints.Add(row);
      }

      waypointRoutes = routes;
      waypointData = waypoints;
   }

   /// <summary>
   /// A method used to return an ArrayList of away points.
   /// </summary>
   /// <param name="index"></param>
   /// <returns></returns>
   public ArrayList GetWaypoints(int index) {
      if (waypointData == null) {
         return null;
      } else {
         if (index >= 0 && index < waypointData.Count) {
            return (ArrayList)waypointData[index];
         } else {
            return null;
         }
      }
   }

   /// <summary>
   /// A method used to turn debugging on.
   /// </summary>
   public void ToggleDebugOn() {
      if (debugOn == true) {
         debugOn = false;
      } else {
         debugOn = true;
      }
   }

   /// <summary>
   /// A method used to turn debugging off.
   /// </summary>
   public void ToggleCurrentCarAi() {
      PlayerState player;
      player = GetCurrentPlayer();

      if (player1AiOn == true) {
         player1AiOn = false;
      } else {
         player1AiOn = true;
      }

      if (player1AiOn == true) {
         player.aiOn = true;
         player.cm.aiOn = true;
         player.fpsInput.aiOn = true;
         player.mouseInput.aiOn = true;
         player.offTrackSeconds = 5.0f;
      } else {
         player.aiOn = false;
         player.cm.aiOn = false;
         player.fpsInput.aiOn = false;
         player.mouseInput.aiOn = false;
         player.offTrackSeconds = 10.0f;
      }

      if (forceGameStart) {
         player.offTrackSeconds = 10000.0f;
         player.wrongDirectionSeconds = 10000.0f;
      }
   }

   /// <summary>
   /// A method used to check that the player state index is valid.
   /// </summary>
   /// <param name="idx">The player state index to check.</param>
   /// <returns>A boolean value indicating if the player state index is valid.</returns>
   private bool PlayerStateIdxCheck(int idx) {
      if (players != null && idx >= 0 && idx < players.Count) {
         return true;
      } else {
         return false;
      }
   }

   /// <summary>
   /// A method used to set the current game type and associated class field vlaues.
   /// </summary>
   public void SetCarDetails() {
      PlayerState player = null;
      int i = 0;
      int l = players.Count;

      for(i = 0; i < l; i++) {
         if (PlayerStateIdxCheck(i)) {
            player = (PlayerState)players[i];
            if (player != null) {
               if (i == player1Index) {
                  gameCamera = player.camera;
                  player.camera.enabled = true;

                  rearCamera = player.rearCamera;
                  player.rearCamera.enabled = true;
                  player.audioListener.enabled = true;

                  if (player1AiOn == true) {
                     player.aiOn = true;
                     player.cm.aiOn = true;
                     player.fpsInput.aiOn = true;
                     player.mouseInput.aiOn = true;
                  } else {
                     player.aiOn = false;
                     player.cm.aiOn = false;
                     player.fpsInput.aiOn = false;
                     player.mouseInput.aiOn = false;
                  }
               } else {
                  player.camera.enabled = false;
                  player.rearCamera.enabled = false;
                  player.audioListener.enabled = false;
                  player.aiOn = true;
                  player.cm.aiOn = true;
                  player.fpsInput.aiOn = true;
                  player.mouseInput.aiOn = true;
               }
               player.waypoints = GetWaypoints(0);
            }
         }
      }
   }

   /// <summary>
   /// Gets the PlayerState of player 1.
   /// </summary>
   /// <returns></returns>
   public PlayerState GetPlayer1() {
      return (PlayerState)players[player1Index];
   }

   /// <summary>
   /// Gets the PlayerState of the current player.
   /// </summary>
   /// <returns></returns>
   public PlayerState GetCurrentPlayer() {
      return (PlayerState)players[currentIndex];
   }

   /// <summary>
   /// Gets the PlayerState of the specified player.
   /// </summary>
   /// <param name="i"></param>
   /// <returns></returns>
   public PlayerState GetPlayer(int i) {
      if (i >= 0 && i < players.Count) {
         return (PlayerState)players[i];
      } else {
         return null;
      }
   }

   /// <summary>
   /// A method used to reset the current game.
   /// </summary>
   public void ResetGame() {
      gamePaused = true;
      Time.timeScale = 0;

      prepped = false;
      ready = false;
      startGame = false;
      startGameTime = 0.0f;
      gameRunning = false;
      gameWon = false;

      Time.timeScale = 1;
      gamePaused = false;
   }

   /// <summary>
   /// A method used to set the car type for the specified player's PlayerState object.
   /// </summary>
   /// <param name="player"></param>
   public void SetCarDetailsByGameType(PlayerState player) {
      int idx = player.index;
      player.player = GameObject.Find(Utilities.NAME_PLAYER_ROOT + idx);
      player.player.transform.position = GameObject.Find(Utilities.NAME_START_ROOT + idx).transform.position;

      player.maxSpeed = PlayerState.DEFAULT_MAX_SPEED;
      player.gravity = PlayerState.DEFAULT_GRAVITY;

      player.maxForwardSpeedSlow = 50;
      player.maxSidewaysSpeedSlow = 12;
      player.maxBackwardsSpeedSlow = 5;
      player.maxGroundAccelerationSlow = 25;

      player.maxForwardSpeedNorm = 200;
      player.maxSidewaysSpeedNorm = 50;
      player.maxBackwardsSpeedNorm = 20;
      player.maxGroundAccelerationNorm = 100;

      player.maxForwardSpeedBoost = 250;
      player.maxSidewaysSpeedBoost = 60;
      player.maxBackwardsSpeedBoost = 30;
      player.maxGroundAccelerationBoost = 120;

      if (idx != player1Index) {
         if (difficulty == GameDifficulty.LOW) {
            player.maxSpeed = PlayerState.DEFAULT_MAX_SPEED;
            player.maxGroundAccelerationNorm += 5;
         } else if (difficulty == GameDifficulty.MED) {
            player.maxSpeed = PlayerState.DEFAULT_MAX_SPEED + 5;
            player.maxForwardSpeedNorm += 10;
            player.maxGroundAccelerationNorm += 10;
         } else if (difficulty == GameDifficulty.HIGH) {
            player.maxSpeed = PlayerState.DEFAULT_MAX_SPEED + 10;
            player.maxForwardSpeedNorm += 15;
            player.maxGroundAccelerationNorm += 40;
            player.maxForwardSpeedBoost += 15;
            player.maxGroundAccelerationBoost += 15;
         }
      } else if (idx == player1Index) {
         player.maxSpeed += Random.Range(0, 12);
         player.maxForwardSpeedNorm += Random.Range(0, 6);
         player.maxGroundAccelerationNorm += Random.Range(0, 6);
         player.maxForwardSpeedBoost += Random.Range(0, 6);
         player.maxGroundAccelerationBoost += Random.Range(0, 6);
      }
   }

   /// <summary>
   /// A method used to turn off the armor markers.
   /// </summary>
   private void TurnOffArmorMarkers() {
      AdjustTagActive(false, "ArmorMarker");
   }

   /// <summary>
   /// A method used to turn on the armor markers.
   /// </summary>
   private void TurnOnArmorMarkers() {
      AdjustTagActive(true, "ArmorMarker");
   }

   /// <summary>
   /// A method used to turn off the gun markers.
   /// </summary>
   private void TurnOffGunMarkers() {
      AdjustTagActive(false, "GunMarker");
   }

   /// <summary>
   /// A method used to turn on the gun markers.
   /// </summary>
   private void TurnOnGunMarkers() {
      AdjustTagActive(true, "GunMarker");
   }

   /// <summary>
   /// A method used to turn off the health markers.
   /// </summary>
   private void TurnOffHealthMarkers() {
      AdjustTagActive(false, "HealthMarker");
   }

   /// <summary>
   /// A method used to turn on the health markers.
   /// </summary>
   private void TurnOnHealthMarkers() {
      AdjustTagActive(true, "HealthMarker");
   }

   /// <summary>
   /// A method used to turn off the invincibility markers.
   /// </summary>
   private void TurnOffInvincMarkers() {
      AdjustTagActive(false, "InvincibilityMarker");
   }

   /// <summary>
   /// A method used to turn on the invincibility markers.
   /// </summary>
   private void TurnOnInvincMarkers() {
      AdjustTagActive(true, "InvincibilityMarker");
   }

   /// <summary>
   /// A method used to turn off hittable markers.
   /// </summary>
   private void TurnOffHittableMarkers() {
      AdjustTagActive(false, "Hittable");
   }

   /// <summary>
   /// A method used to turn on hittable markers.
   /// </summary>
   private void TurnOnHittableMarkers() {
      AdjustTagActive(true, "Hittable");
   }

   /// <summary>
   /// A method used to turn off oil drum stack markers.
   /// </summary>
   private void TurnOffOilDrumStackMarkers() {
      AdjustTagActive(false, "OilDrumStack");
   }

   /// <summary>
   /// A method used to turn on oil drum stack markers.
   /// </summary>
   private void TurnOnOilDrumStackMarkers() {
      AdjustTagActive(true, "OilDrumStack");
   }

   /// <summary>
   /// A method used to turn off fun box markers.
   /// </summary>
   private void TurnOffFunBoxMarkers() {
      AdjustTagActive(false, "FullFunBox");
   }

   /// <summary>
   /// A method used to turn on fun box markers.
   /// </summary>
   private void TurnOnFunBoxMarkers() {
      AdjustTagActive(true, "FullFunBox");
   }

   /// <summary>
   /// A helper method that adjusts the found objects with the given tag and mark them as active.
   /// </summary>
   /// <param name="active"></param>
   /// <param name="tag"></param>
   private void AdjustTagActive(bool active, string tag) {
      GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
      int l = gos.Length;
      for (int i = 0; i < l; i++) {
         gos[i].SetActive(active);
      }
   }

   /// <summary>
   /// A method used to log the current lap time.
   /// </summary>
   /// <param name="p"></param>
   public void LogLapTime(PlayerState p) {
      string time = p.time;
      int timeNum = p.timeNum;
      int lap = p.currentLap;
      int track = raceTrack;
      int type = gameSettingsSet;       //0 = easy, 1 = battle, 2 = classic
      int diff = gameSettingsSubSet;    //0 = low, 1 = med, 2 = high

      LapTime lt = new LapTime();
      lt.time = time;
      lt.timeNum = timeNum;
      lt.lap = lap;
      lt.type = type;
      lt.diff = diff;
      lt.track = track;

      lapTimeManager.AddEntry(lt);
      lapTimeManager.CleanTimes();
      lapTimeManager.FindBestLapTimeByLastEntry();
      PlayerPrefs.SetString("LapTimes", lapTimeManager.Serialize());
   }

   /// <summary>
   /// A method used to setup the current set of players.
   /// </summary>
   public void PrepGame() {
      if (prepped == true) {
         return;
      }
      prepped = true;

      //Prep waypoints and track script settings //line 7
      FindWaypoints();
      trackScript = GetComponent<TrackScript>();
      totalLaps = trackScript.laps;
      nightTime = trackScript.headLightsOn;
      sceneName = trackScript.sceneName;

      //Prep menu screens //line 14
      if (hudNewScript == null) {
         if (GameObject.Find("GameHUD") != null) {
            hudNewScript = GameObject.Find("GameHUD").GetComponent<GameHUDNewScript>();
         }
      }

      if (hudNewScript != null) {
         hudNewScript.HideAll();
      }

      if (gameOverMenuScript == null) {
         if (GameObject.Find("GameOverMenu") != null) {
            gameOverMenuScript = GameObject.Find("GameOverMenu").GetComponent<GameOverMenu>();
         }
      }

      if (gameOverMenuScript != null) {
         gameOverMenuScript.HideWinImage();
         gameOverMenuScript.ShowLoseImage();
      }

      if (audioBgroundSound == null) {
         if (GameObject.Find("BgMusic") != null) {
            audioBgroundSound = GameObject.Find("BgMusic").GetComponent<AudioSource>();
         }
      }

      if (gamePauseMenu == null) {
         gamePauseMenu = GameObject.Find("GamePauseMenu");
         if (gamePauseMenu != null) {
            gamePauseMenu.SetActive(false);
         }
      }

      if (gameStartMenu == null) {
         gameStartMenu = GameObject.Find("GameStartMenu");
         if (gameStartMenu != null) {
            gameStartMenu.SetActive(false);
         }
      }

      if (gameOverMenu == null) {
         gameOverMenu = GameObject.Find("GameOverMenu");
         if (gameOverMenu != null) {
            gameOverMenu.SetActive(false);
         }
      }

      if (gameExitMenu == null) {
         gameExitMenu = GameObject.Find("GameExitMenu");
         if (gameExitMenu != null) {
            gameExitMenu.SetActive(false);
         }
      }

      if (gameHelpMenu == null) {
         gameHelpMenu = GameObject.Find("GameHelpMenu");
         if (gameHelpMenu != null) {
            gameHelpMenu.SetActive(false);
         }
      }

      //Prep player prefs default values //line 77
      if (FIRST_RUN && gameStateIndex == GameStateIndex.FIRST) {
         PlayerPrefs.DeleteKey("GameStateIndex");
         if (PlayerPrefs.HasKey("EasyOn") == false && PlayerPrefs.HasKey("BattleOn") == false && PlayerPrefs.HasKey("ClassicOn") == false) {
            PlayerPrefs.SetInt("EasyOn", 1);
            PlayerPrefs.SetInt("BattleOn", 0);
            PlayerPrefs.SetInt("ClassicOn", 0);
         }

         if (PlayerPrefs.HasKey("LowOn") == false && PlayerPrefs.HasKey("MedOn") == false && PlayerPrefs.HasKey("HighOn") == false) {
            PlayerPrefs.SetInt("LowOn", 1);
            PlayerPrefs.SetInt("MedOn", 0);
            PlayerPrefs.SetInt("HighOn", 0);
         }
         PlayerPrefs.Save();
      }

      //Prep lap time manager //line 94
      string tmpStr = PlayerPrefs.GetString("LapTimes", "");
      lapTimeManager = new LapTimeManager();
      if (tmpStr != null && tmpStr != "") {
         Utilities.wr("Found lap times: " + tmpStr);
         lapTimeManager.Deserialize(tmpStr);
      }

      //Prep difficulty //line 102
      if (PlayerPrefs.HasKey("LowOn") == true && PlayerPrefs.GetInt("LowOn") == 1) {
         difficulty = GameDifficulty.LOW;
      } else if (PlayerPrefs.HasKey("MedOn") == true && PlayerPrefs.GetInt("MedOn") == 1) {
         difficulty = GameDifficulty.MED;
      } else if (PlayerPrefs.HasKey("HighOn") == true && PlayerPrefs.GetInt("HighOn") == 1) {
         difficulty = GameDifficulty.HIGH;
      }

      //Prep track configuration //line 111
      if (PlayerPrefs.HasKey("EasyOn") && PlayerPrefs.GetInt("EasyOn") == 1) {
         gameSettingsSet = 0;
         totalLaps = 2;
         TurnOffArmorMarkers();
         TurnOffGunMarkers();
         TurnOffInvincMarkers();
         TurnOffHealthMarkers();
         TurnOffHittableMarkers();
         if (difficulty == GameDifficulty.LOW) {
            gameSettingsSubSet = 0;
            TurnOffOilDrumStackMarkers();
            TurnOffFunBoxMarkers();
         } else if (difficulty == GameDifficulty.MED) {
            gameSettingsSubSet = 1;
            TurnOffOilDrumStackMarkers();
            TurnOnFunBoxMarkers();
         } else if (difficulty == GameDifficulty.HIGH) {
            gameSettingsSubSet = 2;
            TurnOnOilDrumStackMarkers();
            TurnOnFunBoxMarkers();
         }
      } else if (PlayerPrefs.HasKey("BattleOn") && PlayerPrefs.GetInt("BattleOn") == 1) {
         gameSettingsSet = 1;
         totalLaps = trackScript.laps;
         TurnOnArmorMarkers();
         TurnOnGunMarkers();
         TurnOnInvincMarkers();
         TurnOnHealthMarkers();
         TurnOnHittableMarkers();
         if (difficulty == GameDifficulty.LOW) {
            gameSettingsSubSet = 0;
            TurnOffOilDrumStackMarkers();
            TurnOffFunBoxMarkers();
         } else if (difficulty == GameDifficulty.MED) {
            gameSettingsSubSet = 1;
            TurnOffOilDrumStackMarkers();
            TurnOnFunBoxMarkers();
         } else if (difficulty == GameDifficulty.HIGH) {
            gameSettingsSubSet = 2;
            TurnOnOilDrumStackMarkers();
            TurnOnFunBoxMarkers();
         }
      } else if (PlayerPrefs.HasKey("ClassicOn") && PlayerPrefs.GetInt("ClassicOn") == 1) {
         gameSettingsSet = 2;
         totalLaps = 4;
         TurnOffArmorMarkers();
         TurnOffGunMarkers();
         TurnOffInvincMarkers();
         TurnOffHealthMarkers();
         TurnOnHittableMarkers();
         if (difficulty == GameDifficulty.LOW) {
            gameSettingsSubSet = 0;
            TurnOffOilDrumStackMarkers();
            TurnOffFunBoxMarkers();
         } else if (difficulty == GameDifficulty.MED) {
            gameSettingsSubSet = 1;
            TurnOffOilDrumStackMarkers();
            TurnOnFunBoxMarkers();
         } else if (difficulty == GameDifficulty.HIGH) {
            gameSettingsSubSet = 2;
            TurnOnOilDrumStackMarkers();
            TurnOnFunBoxMarkers();
         }
      }

      //Prep game state //line 177
      if (!FIRST_RUN && PlayerPrefs.HasKey("GameStateIndex") == true) {
         gsiTmp = PlayerPrefs.GetInt("GameStateIndex");
         if (gsiTmp == 0) {
            gameStateIndex = GameStateIndex.FIRST;
         } else if (gsiTmp == 1) {
            gameStateIndex = GameStateIndex.NONE;
         } else if (gsiTmp == 2) {
            gameStateIndex = GameStateIndex.MAIN_MENU_SCREEN;
         } else if (gsiTmp == 3) {
            gameStateIndex = GameStateIndex.GAME_OVER_SCREEN;
         } else if (gsiTmp == 4) {
            gameStateIndex = GameStateIndex.GAME_PAUSE_SCREEN;
         } else if (gsiTmp == 5) {
            gameStateIndex = GameStateIndex.GAME_PLAY_SCREEN;
         } else if (gsiTmp == 6) {
            gameStateIndex = GameStateIndex.MAIN_MENU_SCREEN;
         }
      }

      if (gameStateIndex == GameStateIndex.NONE || gameStateIndex == GameStateIndex.FIRST) {
         gameStateIndex = GameStateIndex.MAIN_MENU_SCREEN;
      }

      if (gameStateIndex == GameStateIndex.MAIN_MENU_SCREEN) {
         player1AiOn = true;
         ShowStartMenu();
         HidePauseMenu();
         HideEndMenu();
      } else if (gameStateIndex == GameStateIndex.GAME_OVER_SCREEN) {
         player1AiOn = true;
         ShowStartMenu();
         HidePauseMenu();
         HideEndMenu();
      } else if (gameStateIndex == GameStateIndex.GAME_PAUSE_SCREEN) {
         ShowPauseMenu();
      } else if (gameStateIndex == GameStateIndex.GAME_PLAY_SCREEN) {
         HidePauseMenu();
         HideEndMenu();
         HideStartMenu();
      }

      //Prep blimp camera //line 219
      if (blimpCamera == null) {
         blimpCamera = GameObject.Find("BlimpCamera");
      }

      //Prep track settings //line 224
      raceTrack = PlayerPrefs.GetInt("RaceTrack");
      int tmp = PlayerPrefs.GetInt("EasyOn");
      if (tmp == 0) {
         easyOn = false;
      } else {
         easyOn = true;
      }

      raceType = PlayerPrefs.GetInt("RaceType");
      Utilities.wr("RaceTrack: " + raceTrack);
      Utilities.wr("EasyOn: " + easyOn);
      Utilities.wr("RaceType: " + raceType);

      if (PlayerPrefs.GetInt("RaceTrackHelp" + raceTrack) != 1) {
         trackHelpOn = true;
      } else {
         trackHelpOn = false;
      }

      //Prep player positions //line 244
      positions = new int[6];
      positions[0] = 0;
      positions[1] = 1;
      positions[2] = 2;
      positions[3] = 3;
      positions[4] = 4;
      positions[5] = 5;
      players = new ArrayList();
      players.AddRange(GameObject.Find("GameState").GetComponents<PlayerState>());

      //Prep player states //line 255
      int l = players.Count;
      PlayerState player;
      Transform t;
      for (int i = 0; i < l; i++) {
         Utilities.wr("Setting up player " + i);
         player = (PlayerState)players[i];
         if (player != null) {
            player.index = i;
            player.carType = i;
            player.position = i;
            SetCarDetailsByGameType(player); //sets the model and speeds

            if (player.player != null) {
               player.active = true;
               player.controller = player.player.GetComponent<CharacterController>();
               player.cm = player.player.GetComponent<CharacterMotor>();
               player.camera = player.player.transform.Find("Main Camera").GetComponent<Camera>();
               player.rearCamera = player.player.transform.Find("Rear Camera").GetComponent<Camera>();
               player.audioListener = player.player.transform.Find("Main Camera").GetComponent<AudioListener>();
               player.mouseInput = player.player.GetComponent<MouseLookNew>();
               player.fpsInput = player.player.GetComponent<FPSInputController>();

               t = player.player.transform.Find("Car");
               if (t != null) {
                  player.gun = (GameObject)t.Find("Minigun_Head").gameObject;
                  player.gunBase = (GameObject)t.Find("Minigun_Base").gameObject;
               }

               player.lightHeadLight = (GameObject)player.player.transform.Find("HeadLight").gameObject;
               if (player.lightHeadLight != null && nightTime == false) {
                  player.lightHeadLight.SetActive(false);
               } else {
                  player.lightHeadLight.SetActive(true);
               }

               player.totalLaps = totalLaps;
               player.currentLap = 0;
               player.aiWaypointIndex = 0;
               player.aiWaypointRoute = 0;
               player.waypoints = GetWaypoints(player.aiWaypointRoute);
               player.flame = (GameObject)player.player.transform.Find("Flame").gameObject;
               player.gunExplosion = (GameObject)player.player.transform.Find("GunExplosion").gameObject;
               //TODO //player.gunExplosionParticleSystem = player.gunExplosion.GetComponent<ParticleEmitter>();
               player.gunHitSmoke = (GameObject)player.player.transform.Find("GunHitSmoke").gameObject;
               //TODO //player.gunHitSmokeParticleSystem = player.gunHitSmoke.GetComponent<ParticleEmitter>();

               if (player.gunOn == true) {
                  player.gun.SetActive(true);
                  player.gunBase.SetActive(true);
               } else {
                  player.gun.SetActive(false);
                  player.gunBase.SetActive(false);
               }

               player.flame.SetActive(false);
               player.gunExplosion.SetActive(false);
               //TODO //player.gunExplosionParticleSystem.emit = false;
               player.gunHitSmoke.SetActive(false);
               //TODO //player.gunHitSmokeParticleSystem.emit = false;
               player.LoadAudio();
            } else {
               Utilities.wr("Player model " + i + " is NULL. Deactivating...");
               player.active = false;
               player.prepped = false;
            }
         } else {
            Utilities.wr("Player " + i + " is NULL. Removing...");
            players.RemoveAt(i);
            l--;
         }

         player.prepped = true;
      }
      SetCarDetails();
      
      //Start game //line 324
      ready = true;
      FIRST_RUN = false;
   }

   /// <summary>
   /// A method used to restart the current demo scene.
   /// </summary>
   public void StartDemoScene() {
      PlayerPrefs.SetInt("GameStateIndex", 5);
      PlayerPrefs.Save();
      ResetGame();
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

   /// <summary>
   /// A method used to set the active car.
   /// </summary>
   /// <param name="j"></param>
   public void SetActiveCar(int j) {
      if (debugOn == false) {
         Utilities.wr("Method SetActiveCar says: debugOn is false, returning.");
         return;
      }

      PlayerState player = null;
      int l = players.Count;
      for (int i = 0; i < l; i++) {
         player = (PlayerState)players[i];
         if (player != null && player.player != null) {
            if (j == i) {
               currentIndex = i;
               currentPlayer = (PlayerState)players[currentIndex];
               player.camera.enabled = true;
               player.rearCamera.enabled = true;
               player.audioListener.enabled = true;
            } else {
               player.camera.enabled = false;
               player.rearCamera.enabled = false;
               player.audioListener.enabled = false;
            }
         }
      }
   }

   /// <summary>
   /// A method used to get the position of the car at the given index.
   /// </summary>
   /// <param name="idx">The index of the car to get the position for.</param>
   /// <param name="currentPosition">The current position of the car at the specified index.</param>
   /// <returns></returns>
   public int GetPosition(int idx, int currentPosition) {
      int i = 0;
      int l = 0;
      l = positions.Length;

      for (i = 0; i < l; i++) {
         if (positions[i] == idx) {
            return (i + 1);
         }
      }

      return 6;
   }

   /// <summary>
   /// A method used to set the positions of the players.
   /// </summary>
   public void SetPositions() {
      sortingPositions = true;
      System.Array.Sort(positions, PlayerStateCompare);
      sortingPositions = false;
   }

   /// <summary>
   /// A method used to cmopare the state of two players to determine who is in a better position.
   /// </summary>
   /// <param name="i1">The index of the first player to cmopare.</param>
   /// <param name="i2">The index of the second player to compare.</param>
   /// <returns></returns>
   public int PlayerStateCompare(int i1, int i2) {
      if (!PlayerStateIdxCheck(i1) || !PlayerStateIdxCheck(i2)) {
         return 0;
      }

      PlayerState obj1 = (PlayerState)players[i1];
      PlayerState obj2 = (PlayerState)players[i2];

      if (obj1.currentLap > obj2.currentLap) {
         return -1;
      } else if (obj1.currentLap < obj2.currentLap) {
         return 1;
      } else {
         if (obj1.aiWaypointIndex > obj2.aiWaypointIndex) {
            return -1;
         } else if (obj1.aiWaypointIndex < obj2.aiWaypointIndex) {
            return 1;
         } else {
            if (obj1.aiWaypointTime < obj2.aiWaypointTime) {
               return 1;
            } else if (obj1.aiWaypointTime > obj2.aiWaypointTime) {
               return -1;
            } else {
               return 0;
            }
         }
      }
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (gamePaused == true) {
         return;
      } else if (gameRunning == false) {
         if (ready == false) {
            return;
         } else if (ready == true && startGame == false) {
            startGame = true;
            startGameTime = 0f;

            if (hudNewScript != null) {
               hudNewScript.SetStartTimerPrep((int)START_GAME_SECONDS);
               if (IsHelpMenuShowing() == false && IsStartMenuShowing() == false && IsExitMenuShowing() == false && IsEndMenuShowing() == false && IsPauseMenuShowing() == false) {
                  hudNewScript.ShowStartTimer();
               }
            }

            if (audioBgroundSound != null) {
               audioBgroundSound.Stop();
            }
         } else if (ready == true && startGame == true) {
            startGameTime += Time.deltaTime;
            if (hudNewScript != null) {
               hudNewScript.SetStartTimer((int)(START_GAME_SECONDS - Mathf.FloorToInt(startGameTime)));
            }
         }

         if (startGameTime >= START_GAME_SECONDS) {
            if (audioBgroundSound != null) {
               audioBgroundSound.Play();
            }

            if (hudNewScript != null) {
               hudNewScript.HideAll();
            }

            if (IsStartMenuShowing() == true) {
               if (hudNewScript != null) {
                  hudNewScript.HideExitBtn();
               }
            } else if (IsExitMenuShowing() == true) {
               if (hudNewScript != null) {
                  hudNewScript.HideExitBtn();
               }
            } else if (IsEndMenuShowing() == true) {
               if (hudNewScript != null) {
                  hudNewScript.HideExitBtn();
               }
            } else if (IsPauseMenuShowing() == true) {
               if (hudNewScript != null) {
                  hudNewScript.HideExitBtn();
               }
            } else if (IsHelpMenuShowing() == true) {
               if (hudNewScript != null) {
                  hudNewScript.HideExitBtn();
               }
            } else {
               if (hudNewScript != null) {
                  hudNewScript.ShowExitBtn();
               }
            }

            if (hudNewScript != null) {
               hudNewScript.SetSpeedPrep();
               hudNewScript.SetPositionPrep((currentPlayer.index + 1));
               hudNewScript.SetLapsPrep(totalLaps);
               hudNewScript.SetTimePrep();
               hudNewScript.SetLifePrep(currentPlayer.GetLife());
               hudNewScript.ShowSpeed();
               hudNewScript.ShowMph();
               hudNewScript.ShowPosition();
               hudNewScript.ShowLaps();
               hudNewScript.ShowTime();
               hudNewScript.DimTarget();
            }

            if (gameSettingsSet == 1) {
               if (hudNewScript != null) {
                  hudNewScript.ShowLife();
               }
            } else {
               if (hudNewScript != null) {
                  hudNewScript.DimLife();
               }
            }

            if (hudNewScript != null) {
               hudNewScript.DimBoost();
               hudNewScript.DimDraft();
               hudNewScript.HideWrongWay();
               hudNewScript.HideOffTrack();
               hudNewScript.DimAmmo();
               hudNewScript.DimInvinc();
               hudNewScript.DimArmor();
               hudNewScript.DimTarget();
               hudNewScript.DimTargetMiss();
               hudNewScript.DimTargetHit();
               hudNewScript.DimDraft();
               hudNewScript.DimPassing();
               hudNewScript.DimArmor();
            }

            gameRunning = true;
            if ((gamePauseMenu == null || gamePauseMenu.activeSelf == false)
               && (gameStartMenu == null || gameStartMenu.activeSelf == false)
               && (gameOverMenu == null || gameOverMenu.activeSelf == false)
               && (gameHelpMenu == null || gameHelpMenu.activeSelf == false)) {
               if (scsMode == true) {
                  if (player1AiOn == false) {
                     ToggleCurrentCarAi();
                  }

                  if (trackHelpOn == true && player1AiOn == false) {
                     if (hudNewScript != null) {
                        hudNewScript.ShowHelpAccel();
                     }
                     trackHelpAccelOn = true;
                     trackHelpAccelTime = 0f;
                  }
               } else {
                  if (player1AiOn == true) {
                     ToggleCurrentCarAi();
                  }

                  if (trackHelpOn == true && player1AiOn == false) {
                     if (hudNewScript != null) {
                        hudNewScript.ShowHelpAccel();
                     }
                     trackHelpAccelOn = true;
                     trackHelpAccelTime = 0f;
                  }
               }
            }
         }
      } else {
         if (gameWon == false) {
            SetPositions();
         }

         l = players.Count;
         for (i = 0; i < l; i++) {
            p = (PlayerState)players[i];
            if (p != null) {
               p.Update();
            }
         }

         currentPlayer = (PlayerState)players[currentIndex];

         //Track Help Accel
         if (trackHelpAccelOn == true) {
            trackHelpAccelTime += Time.deltaTime;
         } else {
            trackHelpAccelTime = 0f;
         }

         if (trackHelpAccelOn == true && trackHelpAccelTime >= TRACK_HELP_SECONDS) {
            if (hudNewScript != null) {
               hudNewScript.HideHelpAccel();
            }
            trackHelpAccelOn = false;
            trackHelpAccelTime = 0f;
         }

         //Track Help Slow
         if (trackHelpSlowOn == true) {
            trackHelpSlowTime += Time.deltaTime;
         } else {
            trackHelpSlowTime = 0f;
         }

         if (trackHelpSlowOn == true && trackHelpSlowTime >= TRACK_HELP_SECONDS) {
            if (hudNewScript != null) {
               hudNewScript.HideHelpSlow();
            }
            trackHelpSlowOn = false;
            trackHelpSlowTime = 0f;
         }

         //Track Help Turn
         if (trackHelpTurnOn == true) {
            trackHelpTurnTime += Time.deltaTime;
         } else {
            trackHelpTurnTime = 0f;
         }

         if (trackHelpTurnOn == true && trackHelpTurnTime >= TRACK_HELP_SECONDS) {
            if (hudNewScript != null) {
               hudNewScript.HideHelpTurn();
            }
            trackHelpTurnOn = false;
            trackHelpTurnTime = 0f;
            PlayerPrefs.SetInt("RaceTrackHelp" + raceTrack, 1);
         }

         //Other
         if (IsStartMenuShowing() == true) {
            if (hudNewScript != null) {
               hudNewScript.HideExitBtn();
            }
         } else if (IsExitMenuShowing() == true) {
            if (hudNewScript != null) {
               hudNewScript.HideExitBtn();
            }
         } else if (IsEndMenuShowing() == true) {
            if (hudNewScript != null) {
               hudNewScript.HideExitBtn();
            }
         } else if (IsPauseMenuShowing() == true) {
            if (hudNewScript != null) {
               hudNewScript.HideExitBtn();
            }
         } else if (IsHelpMenuShowing() == true) {
            if (hudNewScript != null) {
               hudNewScript.HideExitBtn();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.ShowExitBtn();
            }
         }

         if (currentPlayer.boostOn == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimBoost();
               hudNewScript.SetSpeedStr("---");
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimBoost();
               hudNewScript.SetSpeed((int)currentPlayer.speed);
            }
         }

         if (hudNewScript != null) {
            hudNewScript.SetPosition(currentPlayer.position);
            hudNewScript.SetLaps((currentPlayer.currentLap + 1));
            hudNewScript.SetTimeStr(currentPlayer.time);
         }

         if (currentPlayer.gunOn == true) {
            if (hudNewScript != null) {
               hudNewScript.SetAmmo(currentPlayer.ammo);
               hudNewScript.UndimAmmo();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimAmmo();
            }
         }

         if (currentPlayer.invincOn == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimInvinc();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimInvinc();
            }
         }

         if (currentPlayer.armorOn == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimArmor();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimArmor();
            }
         }

         if (currentPlayer.aiIsPassing == true) {
            if (hudNewScript != null) {
               hudNewScript.DimDraft();
               hudNewScript.UndimPassing();
            }
         } else if (currentPlayer.isDrafting == true) {
            if (hudNewScript != null) {
               hudNewScript.DimPassing();
               hudNewScript.UndimDraft();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimPassing();
               hudNewScript.DimDraft();
            }
         }

         if (currentPlayer.wrongDirection == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimWrongWay();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimWrongWay();
            }
         }

         if (currentPlayer.offTrack == true && currentPlayer.offTrackTime > 0.3F) {
            if (hudNewScript != null) {
               hudNewScript.UndimOffTrack();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimOffTrack();
            }
         }

         if (currentPlayer.aiHasTarget == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimTarget();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimTarget();
            }
         }

         if (currentPlayer.isHit == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimTargetHit();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimTargetHit();
            }
         }

         if (currentPlayer.isMiss == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimTargetMiss();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimTargetMiss();
            }
         }

         if (currentPlayer.isShot == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimYourHit();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimYourHit();
            }
         }

         if (currentPlayer.aiHasGainedLife == true) {
            if (hudNewScript != null) {
               hudNewScript.UndimNewLife();
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimNewLife();
            }
         }

         if (gameSettingsSet == 1) {
            if (hudNewScript != null) {
               hudNewScript.SetLife(currentPlayer.GetLife());
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimLife();
            }
         }

         if (gameWon == true) {
            if (hudNewScript != null) {
               if (hudNewScript.IsGameOverShowing() == false) {
                  if (currentPlayer.position == 1) {
                     hudNewScript.HideYouWonTxt();
                     hudNewScript.HideYouLostTxt();
                     gameOverMenuScript.HideLoseImage();
                     gameOverMenuScript.ShowWinImage();
                  } else {
                     hudNewScript.HideYouWonTxt();
                     hudNewScript.HideYouLostTxt();
                     gameOverMenuScript.HideWinImage();
                     gameOverMenuScript.ShowLoseImage();
                  }
               }
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.DimGameOver();
               hudNewScript.HideYouLostTxt();
               hudNewScript.HideYouWonTxt();
            }
         }

         if (currentPlayer.lapComplete == true) {
            if (hudNewScript != null) {
               hudNewScript.ShowCurrLap(lapTimeManager.lastEntry.time);
               if (lapTimeManager.bestEntry != null) {
                  hudNewScript.ShowBestLapEver(lapTimeManager.bestEntry.time);
               }
            }
         } else {
            if (hudNewScript != null) {
               hudNewScript.HideCurrLap();
               hudNewScript.HideBestLapEver();
            }
         }
      }

      if (Input.touchCount == 1) {
         touchScreen = true;

         if (Input.GetTouch(0).phase == TouchPhase.Began) {
            newTouch = true;
            accelOn = true;
         } else if (Input.GetTouch(0).phase == TouchPhase.Moved) {
            newTouch = false;
         } else if (Input.GetTouch(0).phase == TouchPhase.Stationary) {
            newTouch = false;
         } else if ((Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)) {
            newTouch = false;
            accelOn = false;
         }
      } else {
         newTouch = false;
         accelOn = false;
      }

      if (Input.GetKeyDown("1") == true) {
         handleKey1 = true;
      } else if (Input.GetKeyDown("2") == true) {
         handleKey2 = true;
      } else if (Input.GetKeyDown("3") == true) {
         handleKey3 = true;
      } else if (Input.GetKeyDown("4") == true) {
         handleKey4 = true;
      } else if (Input.GetKeyDown("5") == true) {
         handleKey5 = true;
      } else if (Input.GetKeyDown("6") == true) {
         handleKey6 = true;
      } else
        if (Input.GetKeyDown(KeyCode.P) == true) {
         handleKeyA = true;
      } else if (Input.GetKeyDown(KeyCode.D) == true) {
         handleKeyD = true;
      }

      if (handleKey1 == true) {
         handleKey1 = false;
         SetActiveCar(0);
      } else if (handleKey2 == true) {
         handleKey2 = false;
         SetActiveCar(1);
      } else if (handleKey3 == true) {
         handleKey3 = false;
         SetActiveCar(2);
      } else if (handleKey4 == true) {
         handleKey4 = false;
         SetActiveCar(3);
      } else if (handleKey5 == true) {
         handleKey5 = false;
         SetActiveCar(4);
      } else if (handleKey6 == true) {
         handleKey6 = false;
         SetActiveCar(5);
      } else
        if (handleKeyA == true) {
         handleKeyA = false;
         ToggleCurrentCarAi();
      } else if (handleKeyD == true) {
         handleKeyD = false;
         ToggleDebugOn();
      }
   }
}