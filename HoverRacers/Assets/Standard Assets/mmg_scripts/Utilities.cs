using UnityEngine;


/// <summary>
/// A class that provides access to centralized helper methods.
/// </summary>
public class Utilities {
   /// <summary>
   /// A static boolean the controls this class' logging output.
   /// </summary>
   public static bool LOGGING_ON = true;

   /// <summary>
   /// The name of the jump sound effect.
   /// </summary>
   public static string SOUND_FX_JUMP = "buzzy_jump_01";

   /// <summary>
   /// The name of the bounce sound effect.
   /// </summary>
   public static string SOUND_FX_BOUNCE = "cute_bounce_01";

   /// <summary>
   /// The name of the boost sound effect.
   /// </summary>
   public static string SOUND_FX_BOOST = "rocket_lift_off_rnd_06";

   /// <summary>
   /// The name of the power up sound effect.
   /// </summary>
   public static string SOUND_FX_POWER_UP = "powerup_01";

   /// <summary>
   /// The name of the track help slow tag.
   /// </summary>
   public static string TAG_TRACK_HELP_SLOW = "TrackHelpSlow";

   /// <summary>
   /// The name of the track help turn tag.
   /// </summary>
   public static string TAG_TRACK_HELP_TURN = "TrackHelpTurn";

   /// <summary>
   /// The name of the players tag.
   /// </summary>
   public static string TAG_PLAYERS = "Players";

   /// <summary>
   /// The name of the untagged tag.
   /// </summary>
   public static string TAG_UNTAGGED = "Untagged";

   /// <summary>
   /// The name of the hittable tag.
   /// </summary>
   public static string TAG_HITTABLE = "Hittable";

   /// <summary>
   /// The name of the hittable no y tag.
   /// </summary>
   public static string TAG_HITTABLE_NOY = "HittableNoY";

   /// <summary>
   /// The name of the boost marker tag.
   /// </summary>
   public static string TAG_BOOST_MARKER = "BoostMarker";

   /// <summary>
   /// The name of the small boost marker tag.
   /// </summary>
   public static string TAG_SMALL_BOOST_MARKER = "SmallBoostMarker";

   /// <summary>
   /// The name of the tiny boost marker tag.
   /// </summary>
   public static string TAG_TINY_BOOST_MARKER = "TinyBoostMarker";

   /// <summary>
   /// The name of the medium boost marker tag.
   /// </summary>
   public static string TAG_MEDIUM_BOOST_MARKER = "MediumBoostMarker";

   /// <summary>
   /// The name of the tiny boost 2 marker tag.
   /// </summary>
   public static string TAG_TINY_BOOST_2_MARKER = "TinyBoostMarker2";

   /// <summary>
   /// The name of the jump marker tag.
   /// </summary>
   public static string TAG_JUMP_MARKER = "JumpMarker";

   /// <summary>
   /// The name of the help marker tag.
   /// </summary>
   public static string TAG_HEALTH_MARKER = "HealthMarker";

   /// <summary>
   /// The name of the gun marker tag.
   /// </summary>
   public static string TAG_GUN_MARKER = "GunMarker";

   /// <summary>
   /// The name of the invincibility marker tag.
   /// </summary>
   public static string TAG_INVINC_MARKER = "InvincibilityMarker";

   /// <summary>
   /// The name of the armor marker tag.
   /// </summary>
   public static string TAG_ARMOR_MARKER = "ArmorMarker";

   /// <summary>
   /// The max ammount of ammo a player can have.
   /// </summary>
   public static int MAX_AMMO = 6;

   /// <summary>
   /// The amount of ammo given by an ammo marker.
   /// </summary>
   public static int AMMO_INC = 3;

   /// <summary>
   /// The name of the game state object.
   /// </summary>
   public static string NAME_GAME_STATE_OBJ = "GameState";

   /// <summary>
   /// The root string used to locate the hover car game objects.
   /// </summary>
   public static string NAME_PLAYER_ROOT = "HoverCar";

   /// <summary>
   /// The root string used to locate the start position game objects.
   /// </summary>
   public static string NAME_START_ROOT = "StartPosition";

   /// <summary>
   /// The max transform Y position.
   /// </summary>
   public static float MAX_XFORM_POS_Y = 50.0f;

   /// <summary>
   /// The min transform Y position.
   /// </summary>
   public static float MIN_XFORM_POS_Y = 12.0f;

   /// <summary>
   /// The minimum number of seconds a battle mode marker will refresh in.
   /// </summary>
   public static int MARKER_REFRESH_MIN = 60;

   /// <summary>
   /// The maximum number of seconds a battle mode marker will refresh in.
   /// </summary>
   public static int MARKER_REFRESH_MAX = 90;

   /// <summary>
   /// A static method that provides support for centralized logging.
   /// </summary>
   /// <param name="s"></param>
   public static void wr(string s) {
      if (LOGGING_ON) {
         Debug.Log(s);
      }
   }

   /// <summary>
   /// A static method that provides support for centralized logging. This method always tries to write the debug log.
   /// </summary>
   /// <param name="s"></param>
   public static void wrForce(string s) {
      Debug.Log(s);
   }

   /// <summary>
   /// A static method that provides support for more detailed centralized logging.
   /// </summary>
   /// <param name="s"></param>
   /// <param name="sClass"></param>
   /// <param name="sMethod"></param>
   /// <param name="sNote"></param>
   public static void wr(string s, string sClass, string sMethod, string sNote) {
      if (LOGGING_ON) {
         Debug.Log(sClass + "." + sMethod + ": " + sNote + ": " + s);
      }
   }

   /// <summary>
   /// A static method that provides support for centralized error logging.
   /// </summary>
   /// <param name="s"></param>
   public static void wrErr(string s) {
      if (LOGGING_ON) {
         Debug.LogError(s);
      }
   }

   /// <summary>
   /// A static method that provides support for more centralized error logging.
   /// </summary>
   /// <param name="s"></param>
   /// <param name="sClass"></param>
   /// <param name="sMethod"></param>
   /// <param name="sNote"></param>
   public static void wrErr(string s, string sClass, string sMethod, string sNote) {
      if (LOGGING_ON) {
         Debug.LogError(sClass + "." + sMethod + ": " + sNote + ": " + s);
      }
   }

   /// <summary>
   /// A utility method used to safely play an AudioSource object with detailed logging.
   /// </summary>
   /// <param name="audioS">The AudioSource object to play.</param>
   /// <param name="sClass">The name of the class this method was called from.</param>
   /// <param name="sMethod">The name of the method this method was called from.</param>
   /// <param name="sNote">A note about the sound being played.</param>
   /// <param name="name">The name of the sound being played.</param>
   public static void SafePlaySoundFx(AudioSource audioS, string sClass, string sMethod, string sNote, string name) {
      Utilities.wr("Playing sound " + name, sClass, sMethod, sNote);
      SafePlaySoundFx(audioS);
   }

   /// <summary>
   /// A utility method used to safely play an AudioSource object.
   /// </summary>
   /// <param name="audioS"></param>
   public static void SafePlaySoundFx(AudioSource audioS) {
      if (audioS != null) {
         if (audioS.isPlaying == false) {
            audioS.Play();
         }
      }
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="className"></param>
   /// <param name="gameStateObj"></param>
   /// <param name="gameState"></param>
   /// <returns></returns>
   public static object[] LoadStartingSet(string className, out GameState gameState) {
      GameObject gameStateObj = GameObject.Find(Utilities.NAME_GAME_STATE_OBJ);
      if (gameStateObj != null) {
         gameState = gameStateObj.GetComponent<GameState>();
         if (gameState != null) {
            gameState.PrepGame();
            return new object[] { gameStateObj, gameState, true };
         } else {
            Utilities.wrForce(className + ": gameState is null! Deactivating...");
            return new object[] { gameStateObj, gameState, false };
         }
      } else {
         Utilities.wrForce(className + ": gameStateObj is null! Deactivating...");
         gameState = null;
         return new object[] { gameStateObj, gameState, false };
      }
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="className"></param>
   /// <param name="gameStateObj"></param>
   /// <param name="gameState"></param>
   /// <param name="pi"></param>
   /// <param name="playerIndex"></param>
   /// <param name="p"></param>
   /// <param name="g"></param>
   /// <param name="inParent"></param>
   /// <returns></returns>
   public static object[] LoadStartingSetAndLocalPlayerInfo(string className, out GameState gameState, out PlayerInfo pi, out int playerIndex, out PlayerState p, GameObject g, bool inParent) {
      GameObject gameStateObj = GameObject.Find(Utilities.NAME_GAME_STATE_OBJ);
      if (gameStateObj != null) {
         gameState = gameStateObj.GetComponent<GameState>();
         if (gameState != null) {
            gameState.PrepGame();
         } else {
            Utilities.wrForce(className + ": gameState is null! Deactivating...");
            pi = null;
            playerIndex = -1;
            p = null;
            return new object[] { gameStateObj, gameState, false, pi, playerIndex, p };
         }
      } else {
         Utilities.wrForce(className + ": gameStateObj is null! Deactivating...");
         gameState = null;
         pi = null;
         playerIndex = -1;
         p = null;
         return new object[] { gameStateObj, gameState, false, pi, playerIndex, p };
      }

      if (g != null) {
         if (inParent) {
            pi = g.GetComponentInParent<PlayerInfo>();
         } else {
            pi = g.GetComponent<PlayerInfo>();
         }
         if (pi != null) {
            playerIndex = pi.playerIndex;
            p = gameState.GetPlayer(playerIndex);
            if (p != null) {
               return new object[] { gameStateObj, gameState, true, pi, playerIndex, p };
            } else {
               Utilities.wrForce(className + ": p is null! Deactivating...");
               p = null;
               return new object[] { gameStateObj, gameState, false, pi, playerIndex, p };
            }
         } else {
            Utilities.wrForce(className + ": pi is null! Deactivating...");
            pi = null;
            playerIndex = -1;
            p = null;
            return new object[] { gameStateObj, gameState, false, pi, playerIndex, p };
         }
      } else {
         Utilities.wrForce(className + ": g is null! Deactivating...");
         pi = null;
         playerIndex = -1;
         p = null;
         return new object[] { gameStateObj, gameState, false, pi, playerIndex, p };
      }
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="className"></param>
   /// <param name="pi"></param>
   /// <param name="playerIndex"></param>
   /// <param name="p"></param>
   /// <param name="g"></param>
   /// <param name="gameState"></param>
   /// <param name="inParent"></param>
   /// <returns></returns>
   public static object[] LoadPlayerInfo(string className, out PlayerInfo pi, out int playerIndex, out PlayerState p, GameObject g, GameState gameState, bool inParent, bool verbose = false) {
      if (g != null && gameState != null) {
         if (inParent) {
            pi = g.GetComponentInParent<PlayerInfo>();
         } else {
            pi = g.GetComponent<PlayerInfo>();
         }
         if (pi != null) {
            playerIndex = pi.playerIndex;

            p = gameState.GetPlayer(playerIndex);
            if (p != null) {
               return new object[] { pi, playerIndex, true, p };
            } else {
               if (verbose) {
                  Utilities.wrForce(className + ": p is null! Deactivating...");
               }
               p = null;
               return new object[] { pi, playerIndex, false, p };
            }
         } else {
            if (verbose) {
               Utilities.wrForce(className + ": pi is null! Deactivating...");
            }
            pi = null;
            playerIndex = -1;
            p = null;
            return new object[] { pi, playerIndex, false, p };
         }
      } else {
         if (verbose) {
            Utilities.wrForce(className + ": g is null! Deactivating...");
         }
         pi = null;
         playerIndex = -1;
         p = null;
         return new object[] { pi, playerIndex, false, p };
      }
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="audioSetSrc"></param>
   /// <param name="audioSetNames"></param>
   /// <returns></returns>
   public static AudioSource[] LoadAudioResources(AudioSource[] audioSetSrc, string[] audioSetNames) {
      AudioSource[] audioSetDst = null;
      int count = 0;
      if (audioSetSrc != null && audioSetNames != null) {
         audioSetDst = new AudioSource[audioSetNames.Length];
         for (int i = 0; i < audioSetSrc.Length; i++) {
            AudioSource aS = (AudioSource)audioSetSrc[i];
            for (int j = 0; j < audioSetNames.Length; j++) {
               if (aS != null && aS.clip.name == audioSetNames[j]) {
                  //Utilities.wr("Found audio clip: " + audioSetNames[j]);
                  audioSetDst[j] = aS;
                  count++;
                  break;
               }
            }

            if(count == audioSetNames.Length) {
               break;
            }
         }
      }
      return audioSetDst;
   }
}