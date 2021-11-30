using UnityEngine;

/// <summary>
/// A class that is used to track information about a track.
/// </summary>
public class TrackScript : MonoBehaviour {
   //***** Class Fields *****
   /// <summary>
   /// The index of the current track in a list of available tracks.
   /// </summary>
   public int index = 0;

   /// <summary>
   /// A boolean flag indicating that the car's head light should be on.
   /// </summary>
   public bool headLightsOn = false;

   /// <summary>
   /// An integer value that represents the number of laps that this track has.
   /// </summary>
   public int laps = 3;

   /// <summary>
   /// The name of the scene thatis associated with the track.
   /// </summary>
   public string sceneName = "";
}
