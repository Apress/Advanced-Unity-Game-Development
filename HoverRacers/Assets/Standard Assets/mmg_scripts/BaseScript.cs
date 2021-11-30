using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour {
   //***** Static Class Fields *****
   /// <summary>
   /// A central dictionary that is used to track the active status of each class.
   /// </summary>
   public static Dictionary<string, bool> SCRIPT_ACTIVE_LIST = new Dictionary<string, bool>();

   //***** Class Fields *****
   /// <summary>
   /// A reference to the current player's PlayerState class.
   /// </summary>
   public PlayerState p = null;

   /// <summary>
   /// A reference to the gameState object for the current game.
   /// </summary>
   public GameState gameState = null;

   /// <summary>
   /// A boolean indicating if the current script, the child not this base, is active or not.
   /// </summary>
   public bool scriptActive = true;

   /// <summary>
   /// The name of the child class the inherits this class.
   /// </summary>
   public string scriptName = "";

   /// <summary>
   /// A reference to an AudioSource component.
   /// </summary>
   [System.NonSerialized]
   public AudioSource audioS = null;

   /// <summary>
   /// A method that is used to prep a game class by loading the standard references.
   /// </summary>
   /// <param name="sName">The name of the child class.</param>
   /// <returns>A boolean value indicating if the initialization was a success.</returns>
   public bool Prep(string sName) {
      scriptName = sName;
      scriptActive = (bool)Utilities.LoadStartingSet(scriptName, out gameState)[2];
      MarkScriptActive(scriptName, scriptActive);
      return scriptActive;
   }

   /// <summary>
   /// A method that is used to indicate if a class is active or not.
   /// </summary>
   /// <param name="sName">The name of the child class.</param>
   /// <returns>A boolean value indicating if the class is active or not.</returns>
   public static bool IsActive(string sName) {
      if (SCRIPT_ACTIVE_LIST.ContainsKey(sName)) {
         return SCRIPT_ACTIVE_LIST[sName];
      } else {
         return false;
      }
   }

   /// <summary>
   /// A method used to register this class and mark it as active or not.
   /// </summary>
   /// <param name="val">A boolean value indicating if this class is active.</param>
   public void MarkScriptActive(bool val) {
      scriptActive = val;
      MarkScriptActive(scriptName, scriptActive);
   }

   /// <summary>
   /// A method used to register this class and mark it as active or not.
   /// </summary>
   /// <param name="sName">The name of the child class.</param>
   /// <param name="val">A boolean value indicating if the class is active or not.</param>
   public static void MarkScriptActive(string sName, bool val) {
      if (SCRIPT_ACTIVE_LIST.ContainsKey(sName)) {
         SCRIPT_ACTIVE_LIST.Remove(sName);
      }
      SCRIPT_ACTIVE_LIST.Add(sName, val);
   }

   /// <summary>
   /// A method used to load the standard game state reference and the local player state references.
   /// </summary>
   /// <param name="sName">The name of the child class.</param>
   /// <returns>A boolean value indicating if the initialization was a success.</returns>
   public bool PrepPlayerInfo(string sName) {
      scriptName = sName;
      scriptActive = (bool)Utilities.LoadStartingSetAndLocalPlayerInfo(scriptName, out gameState, out PlayerInfo pi, out int playerIndex, out p, gameObject, true)[2];
      MarkScriptActive(scriptName, scriptActive);
      return scriptActive;
   }
}