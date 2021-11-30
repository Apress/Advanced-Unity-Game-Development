using UnityEngine;

/// <summary>
/// A MonoBehavior class designed to simply hold the player index.
/// Allows the script to be attached to game objects and used to look up player index specific information.
/// </summary>
public class PlayerInfo : MonoBehaviour {
   //***** Class Fields *****
   /// <summary>
   /// The index of the player in the array of available players.
   /// </summary>
   public int playerIndex = 0;
}
