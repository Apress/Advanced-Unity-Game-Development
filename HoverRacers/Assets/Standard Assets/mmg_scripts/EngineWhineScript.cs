using UnityEngine;

/// <summary>
/// A class that manages the whine of the car's engine based on the car's current speed.
/// </summary>
public class EngineWhineScript : BaseScript {
   //***** Static Class Fields *****
   /// <summary>
   /// The name of the audio source file to use for the engine whine sound.
   /// </summary>
   public static string AUDIO_SOURCE_NAME_WHINE = "car_idle_lp_01";

   //***** Internal Variables: Update *****
   private float pTmp = 0.0f;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      base.PrepPlayerInfo(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }

      audioS = Utilities.LoadAudioResources(GetComponents<AudioSource>(), new string[] { AUDIO_SOURCE_NAME_WHINE })[0];
      if (audioS != null) {
         audioS.volume = 0.2f;
         audioS.pitch = 0.2f;
      }
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
 
      if (p != null) {
         pTmp = p.speedPrct;
         if (audioS != null) {
            audioS.pitch = Mathf.Clamp(pTmp * 4.1f, 0.5f, 4.1f); //p is clamped to sane values
            audioS.volume = Mathf.Clamp(pTmp * 0.6f, 0.2f, 0.6f);
         }
      }
   }
}