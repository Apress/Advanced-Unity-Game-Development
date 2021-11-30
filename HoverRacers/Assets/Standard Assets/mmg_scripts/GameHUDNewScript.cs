using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class used to manage the HUD display elements.
/// </summary>
public class GameHUDNewScript : BaseScript {
   //***** Class Fields: Text *****
   /// <summary>
   /// A Text object that represents the current car's speed.
   /// </summary>
   public Text txtSpeed = null;

   /// <summary>
   /// A Text object that represents the MPH scale for the car's speed.
   /// </summary>
   public Text txtMph = null;

   /// <summary>
   /// A Text object that represents the current car's position in the race.
   /// </summary>
   public Text txtPosition = null;

   /// <summary>
   /// A Text object that represents the current car's lap in the race.
   /// </summary>
   public Text txtLap = null;

   /// <summary>
   /// A Text object that represents the current car's lap time.
   /// </summary>
   public Text txtTime = null;

   /// <summary>
   /// A Text object that represents the current car's ammo count.
   /// </summary>
   public Text txtAmmo = null;

   /// <summary>
   /// A Text object that represents the current player's life total.
   /// </summary>
   public Text txtLife = null;

   /// <summary>
   /// A Text object that represents the current player's starting time.
   /// </summary>
   public Text txtStartingTime = null;

   /// <summary>
   /// A Text object that represents the current player's win text message.
   /// </summary>
   public Text txtYouWon = null;

   /// <summary>
   /// A Text object that represents the current player's lose text message.
   /// </summary>
   public Text txtYouLost = null;

   /// <summary>
   /// A Text object that represents the current player's best lap time of the race text message.
   /// </summary>
   public Text txtBestLapNow = null;

   /// <summary>
   /// A Text object that represents the current player's best lap time ever text mesaage.
   /// </summary>
   public Text txtBestLapEver = null;

   /// <summary>
   /// A Text object that represents the current player's current lap time as text.
   /// </summary>
   public Text txtCurrLap = null;

   //***** Class Fields: Images *****
   /// <summary>
   /// An Image object that displays the car boost on modifier image.
   /// </summary>
   public Image imgBoostOn = null;

   /// <summary>
   /// An Image object that displays the car boost off modifier image.
   /// </summary>
   public Image imgBoostOff = null;

   /// <summary>
   /// An Image object that displays the car armor on modifier image.
   /// </summary>
   public Image imgAmmoOn = null;

   /// <summary>
   /// An Image object that displays the car armor off modifier image.
   /// </summary>
   public Image imgAmmoOff = null;

   /// <summary>
   /// An Image object that displays the car's ammo indicator.
   /// </summary>
   public Image imgAmmoBubble = null;

   /// <summary>
   /// An Image object that displays the car's invincibility on modifier image.
   /// </summary>
   public Image imgInvincOn = null;

   /// <summary>
   /// An Image object that displays the car's invincibility off modifier image.
   /// </summary>
   public Image imgInvincOff = null;

   /// <summary>
   /// An Image object that displays the car's armor on modifier image.
   /// </summary>
   public Image imgArmorOn = null;

   /// <summary>
   /// An Image object that displays the car's armor off modifier image.
   /// </summary>
   public Image imgArmorOff = null;

   /// <summary>
   /// An Image object that displays the car's draft on modifier image.
   /// </summary>
   public Image imgDraftOn = null;

   /// <summary>
   /// An Image object that displays the car's draft off modifier image.
   /// </summary>
   public Image imgDraftOff = null;

   /// <summary>
   /// An Image object that displays the car's passing on modifier image.
   /// </summary>
   public Image imgPassingOn = null;

   /// <summary>
   /// An Image object that displays the car's passing off modifier image.
   /// </summary>
   public Image imgPassingOff = null;

   /// <summary>
   /// An Image object that displays the car's target on modifier image.
   /// </summary>
   public Image imgTargetOn = null;

   /// <summary>
   /// An Image object that displays the car's target off modifier image.
   /// </summary>
   public Image imgTargetOff = null;

   /// <summary>
   /// An Image object that displays the car's gun shot hit on image.
   /// </summary>
   public Image imgTargetHitOn = null;

   /// <summary>
   /// An Image object that displays the car's gun shot hit off image.
   /// </summary>
   public Image imgTargetHitOff = null;

   /// <summary>
   /// An Image object that displays the car's gun shot miss on image.
   /// </summary>
   public Image imgTargetMissOn = null;

   /// <summary>
   /// An Image object that displays the car's gun shot miss off image.
   /// </summary>
   public Image imgTargetMissOff = null;

   /// <summary>
   /// An Image object used to display the border image around the MPH text.
   /// </summary>
   public Image imgBorderMph = null;

   /// <summary>
   /// An Image object used to display the border image around the top part of the HUD.
   /// </summary>
   public Image imgTopHud = null;

   /// <summary>
   /// An Image object used to display the car's +1 health on indicator.
   /// </summary>
   public Image imgLife1On = null;

   /// <summary>
   /// An Image object used to display the car's +1 health off indicator.
   /// </summary>
   public Image imgLife1Off = null;

   /// <summary>
   /// An Image object used to display the car's +2 health on indicator.
   /// </summary>
   public Image imgLife2On = null;

   /// <summary>
   /// An Image object used to display the car's +2 health off indicator.
   /// </summary>
   public Image imgLife2Off = null;

   /// <summary>
   /// An Image object used to display the car's +3 health on indicator.
   /// </summary>
   public Image imgLife3On = null;

   /// <summary>
   /// An Image object used to display the car's +3 health off indicator.
   /// </summary>
   public Image imgLife3Off = null;

   //***** Class Fields: Images Banner *****
   /// <summary>
   /// An Image object used to display a banner that indicates you've been hit by another car's gun shot.
   /// </summary>
   public Image imgYourHit = null;

   /// <summary>
   /// An Image object used to display a banner that indicates you've lost life from another car's gun shot hit.
   /// </summary>
   public Image imgLostLife = null;

   /// <summary>
   /// An Image object used to display a banner that indicates that you've gained a new life.
   /// </summary>
   public Image imgNewLife = null;

   /// <summary>
   /// An Image object used to display a banner that indicates that you've lost the game.
   /// </summary>
   public Image imgGameOver = null;

   /// <summary>
   /// An Image object used to display a banner that indicates that you're headed in the wrong direction.
   /// </summary>
   public Image imgWrongWay = null;

   /// <summary>
   /// An Image object used to display a banner that indicates that you're currently off the track.
   /// </summary>
   public Image imgOffTrack = null;

   //***** Class Fields: Images Help *****
   /// <summary>
   /// An Image object used to display help information on how to accelerate the car.
   /// </summary>
   public Image imgHelpAccel = null;

   /// <summary>
   /// An Image object used to display help information on how to slow down the car.
   /// </summary>
   public Image imgHelpSlow = null;

   /// <summary>
   /// An Image object used to display help information on how to turn the car.
   /// </summary>
   public Image imgHelpTurn = null;

   /// <summary>
   /// A Button object used to display an exit button on the main game screen's HUD.
   /// </summary>
   public Button btnExit = null;

   /// <summary>
   /// An AudioSource used to play the count down sound effect on the race start screen.
   /// </summary>
   public AudioSource audioStartTick1 = null;

   /// <summary>
   /// An AudioSource used to play the final count down sound effect on the race start screen.
   /// </summary>
   public AudioSource audioStartTick2 = null;

   //***** Internal Variables *****
   private int prevStartTimer = 6;             //(should be GameState.START_GAME_SECONDS + 1)
   private int maxLaps = 6;
   private string lapStr = null;
   private string speedStr = null;
   private int life = 2;
   private List<Vector3> posPopupMsg = new List<Vector3>();

   public readonly int POS_POPUP_MSG_SIZE = 4;
   private int posPopupMsgIdx = 0;
   private List<PopupMsgTracker> posPopupMsgVis = new List<PopupMsgTracker>();
   private Vector3 haT;

   //***** Internal Variables: RemovePopupMsgTracker *****
   int lRpmt = 0;
   int iRpmt = 0;
   PopupMsgTracker tmpRpmt = null;

   //***** Internal Variables: AddPopupMsgTracker *****
   PopupMsgTracker pt = null;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      base.Prep(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }
      HideAll();
   }

   /// <summary>
   /// A method used to determine if the value passed in is greater than the current player's life total.
   /// </summary>
   /// <param name="l">An integer value to compare against the current player's life total.</param>
   /// <returns>A boolean value indicating if the passed in value is greater than the current player's life total.</returns>
   public bool GainedLife(int l) {
      if (l > life) {
         return true;
      }
      return false;
   }

   /// <summary>
   /// A method used to hide all UI objects on the main game HUD.
   /// </summary>
   public void HideAll() {
      txtSpeed.gameObject.SetActive(false);
      txtMph.gameObject.SetActive(false);
      txtPosition.gameObject.SetActive(false);
      txtLife.gameObject.SetActive(false);
      txtLap.gameObject.SetActive(false);
      txtTime.gameObject.SetActive(false);
      txtAmmo.gameObject.SetActive(false);
      txtStartingTime.gameObject.SetActive(false);

      txtYouWon.gameObject.SetActive(false);
      txtYouLost.gameObject.SetActive(false);
      txtBestLapNow.gameObject.SetActive(false);
      txtBestLapEver.gameObject.SetActive(false);
      txtCurrLap.gameObject.SetActive(false);

      imgBoostOn.gameObject.SetActive(false);
      imgBoostOff.gameObject.SetActive(false);
      imgInvincOn.gameObject.SetActive(false);
      imgInvincOff.gameObject.SetActive(false);
      imgAmmoOn.gameObject.SetActive(false);
      imgAmmoOff.gameObject.SetActive(false);
      imgAmmoBubble.gameObject.SetActive(false);
      imgArmorOn.gameObject.SetActive(false);
      imgArmorOff.gameObject.SetActive(false);
      imgDraftOn.gameObject.SetActive(false);
      imgDraftOff.gameObject.SetActive(false);
      imgPassingOn.gameObject.SetActive(false);
      imgPassingOff.gameObject.SetActive(false);
      imgTargetOn.gameObject.SetActive(false);
      imgTargetOff.gameObject.SetActive(false);
      imgTargetHitOn.gameObject.SetActive(false);
      imgTargetHitOff.gameObject.SetActive(false);
      imgTargetMissOn.gameObject.SetActive(false);
      imgTargetMissOff.gameObject.SetActive(false);
      btnExit.gameObject.SetActive(false);

      imgYourHit.gameObject.SetActive(false);
      imgLostLife.gameObject.SetActive(false);
      imgNewLife.gameObject.SetActive(false);
      imgGameOver.gameObject.SetActive(false);
      imgWrongWay.gameObject.SetActive(false);
      imgOffTrack.gameObject.SetActive(false);

      imgHelpAccel.gameObject.SetActive(false);
      imgHelpSlow.gameObject.SetActive(false);
      imgHelpTurn.gameObject.SetActive(false);

      posPopupMsg = new List<Vector3>();
      haT = Vector3.zero;

      haT = imgGameOver.gameObject.transform.position;
      posPopupMsg.Add(new Vector3(haT.x, haT.y, haT.z));

      haT = imgNewLife.gameObject.transform.position;
      posPopupMsg.Add(new Vector3(haT.x, haT.y, haT.z));

      haT = imgLostLife.gameObject.transform.position;
      posPopupMsg.Add(new Vector3(haT.x, haT.y, haT.z));

      haT = imgWrongWay.gameObject.transform.position;
      posPopupMsg.Add(new Vector3(haT.x, haT.y, haT.z));

      posPopupMsgIdx = 0;
      posPopupMsgVis = new List<PopupMsgTracker>();
   }

   /// <summary>
   /// A method used to show the car's acceleration help banner.
   /// </summary>
   public void ShowHelpAccel() {
      imgHelpAccel.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the car's acceleration help banner.
   /// </summary>
   public void HideHelpAccel() {
      imgHelpAccel.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the car's slow help banner.
   /// </summary>
   public void ShowHelpSlow() {
      imgHelpSlow.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the car's slow help banner.
   /// </summary>
   public void HideHelpSlow() {
      imgHelpSlow.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the car's turn help banner.
   /// </summary>
   public void ShowHelpTurn() {
      imgHelpTurn.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the car's turn help banner.
   /// </summary>
   public void HideHelpTurn() {
      imgHelpTurn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the game exit button on the game's HUD.
   /// </summary>
   public void ShowExitBtn() {
      btnExit.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the game exit button on the game's HUD.
   /// </summary>
   public void HideExitBtn() {
      btnExit.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the track's count down timer.
   /// </summary>
   public void ShowStartTimer() {
      prevStartTimer = 6;
      txtStartingTime.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the track's count down timer.
   /// </summary>
   public void HideStartTimer() {
      txtStartingTime.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to prepare the track's start timer for display.
   /// </summary>
   /// <param name="t">An integer value of the current count down time.</param>
   public void SetStartTimerPrep(int t) {
      prevStartTimer = (t + 1);
      txtStartingTime.text = ("Get Ready: " + t);
   }

   /// <summary>
   /// A method used to update the track's start timer and play a sound effect during the 
   /// count down with a special sound effect on the last second before the start of the race.
   /// </summary>
   /// <param name="t">An integer value indicating the new count down timer value.</param>
   public void SetStartTimer(int t) {
      if (t < prevStartTimer) {
         prevStartTimer = t;

         if (t >= 1) {
            if (audioStartTick1 != null) {
               audioStartTick1.Play();
            }
         } else {
            if (audioStartTick2 != null) {
               audioStartTick2.Play();
            }
         }
      }
      txtStartingTime.text = ("Get Ready: " + t);
   }

   /// <summary>
   /// A method used to hide/deactivate the life bonus HUD indicators.
   /// </summary>
   public void DimLife() {
      imgLife1On.gameObject.SetActive(false);
      imgLife1Off.gameObject.SetActive(true);

      imgLife2On.gameObject.SetActive(false);
      imgLife2Off.gameObject.SetActive(true);

      imgLife3On.gameObject.SetActive(false);
      imgLife3Off.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show/activate the life bonus HUD indicators.
   /// </summary>
   public void UndimLife() {
      SetLifePrep(life);
   }

   /// <summary>
   /// A method used to show/activate the life bonus HUD indicators.
   /// </summary>
   public void ShowLife() {
      SetLifePrep(life);
   }

   /// <summary>
   /// A method used to hide/deactivate all life bonus HUD indicators.
   /// </summary>
   public void HideLife() {
      imgLife1On.gameObject.SetActive(false);
      imgLife1Off.gameObject.SetActive(false);

      imgLife2On.gameObject.SetActive(false);
      imgLife2Off.gameObject.SetActive(false);

      imgLife3On.gameObject.SetActive(false);
      imgLife3Off.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to prepare the life gain indicators for a new race.
   /// </summary>
   /// <param name="l">An integer value of the player's current life value.</param>
   public void SetLifePrep(int l) {
      life = l;
      if (l == 0) {
         imgLife1On.gameObject.SetActive(false);
         imgLife1Off.gameObject.SetActive(true);

         imgLife2On.gameObject.SetActive(false);
         imgLife2Off.gameObject.SetActive(true);

         imgLife3On.gameObject.SetActive(false);
         imgLife3Off.gameObject.SetActive(true);
      } else if (l == 1) {
         imgLife1On.gameObject.SetActive(true);
         imgLife1Off.gameObject.SetActive(false);

         imgLife2On.gameObject.SetActive(false);
         imgLife2Off.gameObject.SetActive(true);

         imgLife3On.gameObject.SetActive(false);
         imgLife3Off.gameObject.SetActive(true);
      } else if (l == 2) {
         imgLife1On.gameObject.SetActive(true);
         imgLife1Off.gameObject.SetActive(false);

         imgLife2On.gameObject.SetActive(true);
         imgLife2Off.gameObject.SetActive(false);

         imgLife3On.gameObject.SetActive(false);
         imgLife3Off.gameObject.SetActive(true);
      } else if (l == 3) {
         imgLife1On.gameObject.SetActive(true);
         imgLife1Off.gameObject.SetActive(false);

         imgLife2On.gameObject.SetActive(true);
         imgLife2Off.gameObject.SetActive(false);

         imgLife3On.gameObject.SetActive(true);
         imgLife3Off.gameObject.SetActive(false);
      }
   }

   /// <summary>
   /// A method used to set the current life value and prep the HUD display.
   /// </summary>
   /// <param name="l">An integer value of the player's current life value.</param>
   public void SetLife(int l) {
      SetLifePrep(l);
   }

   /// <summary>
   /// A method used to show the MPH speed label.
   /// </summary>
   public void ShowMph() {
      txtMph.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the MPH speed label.
   /// </summary>
   public void HideMph() {
      txtMph.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the game win text.
   /// </summary>
   public void ShowYouWonTxt() {
      txtYouWon.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the game win text.
   /// </summary>
   public void HideYouWonTxt() {
      txtYouWon.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to determine if the game win text is visible.
   /// </summary>
   /// <returns></returns>
   public bool IsYouWonShowing() {
      return txtYouWon.gameObject.activeSelf;
   }

   /// <summary>
   /// A method used to show the game loss text.
   /// </summary>
   public void ShowYouLostTxt() {
      txtYouLost.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the game loss text.
   /// </summary>
   public void HideYouLostTxt() {
      txtYouLost.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to determine if the game loss text is visible.
   /// </summary>
   /// <returns>A boolean value the indicates if the you lost text is visible.</returns>
   public bool IsYouLostShowing() {
      return txtYouLost.gameObject.activeSelf;
   }

   /// <summary>
   /// A method used to show the best lap now text.
   /// </summary>
   /// <param name="s">A string representation of the best lap time of the race.</param>
   public void ShowBestLapNow(string s) {
      //TODO Revisit
      //txtBestLapNow.text = "BEST LAP NOW  " + s;
      //txtBestLapNow.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the best lap now text.
   /// </summary>
   public void HideBestLapNow() {
      txtBestLapNow.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the best lap ever text.
   /// </summary>
   /// <param name="s">A string representation of the best lap time ever.</param>
   public void ShowBestLapEver(string s) {
      //TODO Revisit
      //txtBestLapEver.text = "BEST LAP EVER  " + s;
      //txtBestLapEver.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the best lap ever text.
   /// </summary>
   public void HideBestLapEver() {
      txtBestLapEver.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the current lap time.
   /// </summary>
   /// <param name="s">A string representation of the current lap time.</param>
   public void ShowCurrLap(string s) {
      txtCurrLap.text = "LAP TIME  " + s;
      txtCurrLap.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the current lap time.
   /// </summary>
   public void HideCurrLap() {
      txtCurrLap.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the car's current speed.
   /// </summary>
   public void ShowSpeed() {
      txtSpeed.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the car's current speed.
   /// </summary>
   public void HideSpeed() {
      txtSpeed.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to prepare the display of the car's speed.
   /// </summary>
   public void SetSpeedPrep() {
      txtSpeed.text = ("000");
   }

   /// <summary>
   /// A method used to set the car's current display speed.
   /// </summary>
   /// <param name="s">An integer representation of the car's current speed.</param>
   public void SetSpeed(int s) {
      speedStr = "";
      if (s < 100) {
         speedStr += "0";
      }

      if (s < 10) {
         speedStr += "0";
      }

      speedStr += "" + s;
      txtSpeed.text = (speedStr);
   }

   /// <summary>
   /// A method used to set the car's speed as a string.
   /// </summary>
   /// <param name="s">A string representation of the car's current speed.</param>
   public void SetSpeedStr(string s) {
      txtSpeed.text = ("" + s);
   }

   /// <summary>
   /// A method used to show the car's current position display text.
   /// </summary>
   public void ShowPosition() {
      if (gameState != null) {
         if (gameState.raceTrack == 0 || gameState.raceTrack == 2) {
            txtPosition.color = Color.black;
         } else {
            txtPosition.color = Color.white;
         }
      }
      txtPosition.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the car's current position display text.
   /// </summary>
   public void HidePosition() {
      txtPosition.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to prepare the display of the car's current position.
   /// </summary>
   /// <param name="p">The position to set for the current player's car.</param>
   public void SetPositionPrep(int p) {
      txtPosition.text = ("" + p + "/6");
   }

   /// <summary>
   /// A method used to set the cars current position display text.
   /// </summary>
   /// <param name="p">The position to set for the current player's car.</param>
   public void SetPosition(int p) {
      txtPosition.text = ("" + p + "/6");
   }

   /// <summary>
   /// A method used to show the lap information display text.
   /// </summary>
   public void ShowLaps() {
      if (gameState != null) {
         if (gameState.raceTrack == 0 || gameState.raceTrack == 2) {
            txtLap.color = Color.black;
         } else {
            txtLap.color = Color.white;
         }
      }
      txtLap.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the display text for the lap information display text.
   /// </summary>
   public void HideLaps() {
      txtLap.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to prepare the display text for the lap information.
   /// </summary>
   /// <param name="m">The maximum number of laps for this race.</param>
   public void SetLapsPrep(int m) {
      maxLaps = m;
      lapStr = "/" + maxLaps;
      txtLap.text = ("1" + lapStr);
   }

   /// <summary>
   /// A method used to set the lap number.
   /// </summary>
   /// <param name="c"></param>
   public void SetLaps(int c) {
      txtLap.text = ("" + c + lapStr);
   }

   /// <summary>
   /// A method used to show the current lap time display text.
   /// </summary>
   public void ShowTime() {
      if (gameState != null) {
         if (gameState.raceTrack == 0 || gameState.raceTrack == 2) {
            txtTime.color = Color.black;
         } else {
            txtTime.color = Color.white;
         }
      }
      txtTime.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the current lap time display text.
   /// </summary>
   public void HideTime() {
      txtTime.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to prepare the current lap time display text.
   /// </summary>
   public void SetTimePrep() {
      txtTime.text = "00:00:000"; //"00:00:00:000";
   }

   /// <summary>
   /// A method used to set the current lap time.
   /// </summary>
   /// <param name="i">An integer representation of the current lap time.</param>
   public void SetTime(int i) {
      txtTime.text = ("" + i);
   }

   /// <summary>
   /// A method used to set the current lap time.
   /// </summary>
   /// <param name="s">A string representation of the current lap time.</param>
   public void SetTimeStr(string s) {
      txtTime.text = s;
   }

   /// <summary>
   /// A method used to check and fix the current popup message index if it's outside of the range.
   /// </summary>
   public void CheckPopupMsgIdx() {
      if (posPopupMsgIdx >= POS_POPUP_MSG_SIZE || posPopupMsgIdx < 0) {
         posPopupMsgIdx = 0;
      }
   }

   /// <summary>
   /// A method used to remove a popup message from the list of visible popup messages.
   /// </summary>
   /// <param name="tp">The index of the popup message to remove.</param>
   public void RemovePopupMsgTracker(int tp) {
      if (posPopupMsgVis != null) {
         lRpmt = posPopupMsgVis.Count;
         for (iRpmt = 0; iRpmt < lRpmt; iRpmt++) {
            tmpRpmt = posPopupMsgVis[iRpmt];
            if (tmpRpmt != null && tmpRpmt.type == tp) {
               posPopupMsgVis.RemoveAt(iRpmt);
               lRpmt--;
               posPopupMsgIdx--;
               CheckPopupMsgIdx();
            }
         }
      }
   }

   /// <summary>
   /// A method used to add a poup message into the list of visible popup messages.
   /// </summary>
   /// <param name="i">The image object to display for this popup message index.</param>
   /// <param name="idx">The index of the popup message to add to the list of visible messages.</param>
   /// <param name="tp">The type of popup message to add to the list of visbile popup messages.</param>
   public void AddPopupMsgTracker(Image i, int idx, int tp) {
      pt = new PopupMsgTracker();
      pt.index = posPopupMsgVis.Count;
      pt.posIdx = idx;
      pt.image = i;
      pt.type = tp;
      posPopupMsgVis.Add(pt);
      posPopupMsgIdx++;
   }

   /// <summary>
   /// A method used to show the dimmed you're hit indicator.
   /// </summary>
   public void DimYourHit() {
      if (imgYourHit.gameObject.activeSelf == true) {
         imgYourHit.gameObject.SetActive(false);
         RemovePopupMsgTracker(0);
      }
   }

   /// <summary>
   /// A method used to show the undimmed you're hit indicator.
   /// </summary>
   public void UndimYourHit() {
      if (imgYourHit.gameObject.activeSelf == false) {
         imgYourHit.gameObject.SetActive(true);
         imgYourHit.gameObject.transform.position = posPopupMsg[posPopupMsgIdx];
         AddPopupMsgTracker(imgYourHit, posPopupMsgIdx, 0);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to show the you're hit indicator.
   /// </summary>
   public void ShowYourHit() {
      if (imgYourHit.gameObject.activeSelf == false) {
         imgYourHit.gameObject.SetActive(true);
         imgYourHit.gameObject.transform.position = posPopupMsg[posPopupMsgIdx];
         AddPopupMsgTracker(imgYourHit, posPopupMsgIdx, 0);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to hide the you're hit indicator.
   /// </summary>
   public void HideYourHit() {
      if (imgYourHit.gameObject.activeSelf == true) {
         imgYourHit.gameObject.SetActive(false);
         RemovePopupMsgTracker(0);
      }
   }

   /// <summary>
   /// A method used to show the dimmed new life indicator.
   /// </summary>
   public void DimNewLife() {
      if (imgNewLife.gameObject.activeSelf == true) {
         imgNewLife.gameObject.SetActive(false);
         RemovePopupMsgTracker(1);
      }
   }

   /// <summary>
   /// A method used to show the undimmed new life indicator.
   /// </summary>
   public void UndimNewLife() {
      if (imgNewLife.gameObject.activeSelf == false) {
         imgNewLife.gameObject.SetActive(true);
         imgNewLife.gameObject.transform.position = posPopupMsg[posPopupMsgIdx++];
         AddPopupMsgTracker(imgYourHit, posPopupMsgIdx, 1);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to show the new life indicator.
   /// </summary>
   public void ShowNewLife() {
      if (imgNewLife.gameObject.activeSelf == false) {
         imgNewLife.gameObject.SetActive(true);
         imgNewLife.gameObject.transform.position = posPopupMsg[posPopupMsgIdx++];
         AddPopupMsgTracker(imgYourHit, posPopupMsgIdx, 1);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to hide the new life indicator.
   /// </summary>
   public void HideNewLife() {
      if (imgNewLife.gameObject.activeSelf == true) {
         imgNewLife.gameObject.SetActive(false);
         RemovePopupMsgTracker(1);
      }
   }

   /// <summary>
   /// A method used to show the dimmed wrong way indicator.
   /// </summary>
   public void DimWrongWay() {
      if (imgWrongWay.gameObject.activeSelf == true) {
         imgWrongWay.gameObject.SetActive(false);
         RemovePopupMsgTracker(2);
      }
   }

   /// <summary>
   /// A method used to show the undimmed wrong way indicator.
   /// </summary>
   public void UndimWrongWay() {
      if (imgWrongWay.gameObject.activeSelf == false) {
         imgWrongWay.gameObject.SetActive(true);
         imgWrongWay.gameObject.transform.position = posPopupMsg[posPopupMsgIdx++];
         AddPopupMsgTracker(imgWrongWay, posPopupMsgIdx, 2);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to show the wrong way indicator.
   /// </summary>
   public void ShowWrongWay() {
      if (imgWrongWay.gameObject.activeSelf == false) {
         imgWrongWay.gameObject.SetActive(true);
         imgWrongWay.gameObject.transform.position = posPopupMsg[posPopupMsgIdx++];
         AddPopupMsgTracker(imgWrongWay, posPopupMsgIdx, 2);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to hide the wrong way indicator.
   /// </summary>
   public void HideWrongWay() {
      if (imgWrongWay.gameObject.activeSelf == true) {
         imgWrongWay.gameObject.SetActive(false);
         RemovePopupMsgTracker(2);
      }
   }

   /// <summary>
   /// A method used to show the dimmed off track indicator.
   /// </summary>
   public void DimOffTrack() {
      if (imgOffTrack.gameObject.activeSelf == true) {
         imgOffTrack.gameObject.SetActive(false);
         RemovePopupMsgTracker(3);
      }
   }

   /// <summary>
   /// A method used to show the undimmed off track indicator.
   /// </summary>
   public void UndimOffTrack() {
      if (imgOffTrack.gameObject.activeSelf == false) {
         imgOffTrack.gameObject.SetActive(true);
         imgOffTrack.gameObject.transform.position = posPopupMsg[posPopupMsgIdx++];
         AddPopupMsgTracker(imgOffTrack, posPopupMsgIdx, 3);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to show the off track indicator.
   /// </summary>
   public void ShowOffTrack() {
      if (imgOffTrack.gameObject.activeSelf == false) {
         imgOffTrack.gameObject.SetActive(true);
         imgOffTrack.gameObject.transform.position = posPopupMsg[posPopupMsgIdx++];
         AddPopupMsgTracker(imgOffTrack, posPopupMsgIdx, 3);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to hide the off track indicator.
   /// </summary>
   public void HideOffTrack() {
      if (imgOffTrack.gameObject.activeSelf == true) {
         imgOffTrack.gameObject.SetActive(false);
         RemovePopupMsgTracker(3);
      }
   }

   /// <summary>
   /// A method used to show the dimmed game over indicator.
   /// </summary>
   public void DimGameOver() {
      if (imgGameOver.gameObject.activeSelf == true) {
         imgGameOver.gameObject.SetActive(false);
         RemovePopupMsgTracker(4);
      }
   }

   /// <summary>
   /// A method the indicates if the game over indicator is showing.
   /// </summary>
   /// <returns></returns>
   public bool IsGameOverShowing() {
      return imgGameOver.gameObject.activeSelf;
   }

   /// <summary>
   /// A method used to show the undimmed game over indicator.
   /// </summary>
   public void UndimGameOver() {
      if (imgGameOver.gameObject.activeSelf == false) {
         imgGameOver.gameObject.SetActive(true);
         imgGameOver.gameObject.transform.position = posPopupMsg[posPopupMsgIdx++];
         AddPopupMsgTracker(imgGameOver, posPopupMsgIdx, 4);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to show the game over indicator.
   /// </summary>
   public void ShowGameOver() {
      if (imgGameOver.gameObject.activeSelf == false) {
         imgGameOver.gameObject.SetActive(true);
         imgGameOver.gameObject.transform.position = posPopupMsg[posPopupMsgIdx++];
         AddPopupMsgTracker(imgGameOver, posPopupMsgIdx, 4);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to hide the game over indicator.
   /// </summary>
   public void HideGameOver() {
      if (imgGameOver.gameObject.activeSelf == true) {
         imgGameOver.gameObject.SetActive(false);
         RemovePopupMsgTracker(4);
      }
   }

   /// <summary>
   /// A method used to show the dimmed lost life indicator.
   /// </summary>
   public void DimLostLife() {
      if (imgLostLife.gameObject.activeSelf == true) {
         imgLostLife.gameObject.SetActive(false);
         RemovePopupMsgTracker(5);
      }
   }

   /// <summary>
   /// A method used to show the undimmed lost life indicator.
   /// </summary>
   public void UndimLostLife() {
      if (imgLostLife.gameObject.activeSelf == false) {
         imgLostLife.gameObject.SetActive(true);
         imgLostLife.gameObject.transform.position = posPopupMsg[posPopupMsgIdx];
         AddPopupMsgTracker(imgLostLife, posPopupMsgIdx, 5);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to show the lost life indicator.
   /// </summary>
   public void ShowLostLife() {
      if (imgLostLife.gameObject.activeSelf == false) {
         imgLostLife.gameObject.SetActive(true);
         imgLostLife.gameObject.transform.position = posPopupMsg[posPopupMsgIdx];
         AddPopupMsgTracker(imgGameOver, posPopupMsgIdx, 5);
         CheckPopupMsgIdx();
      }
   }

   /// <summary>
   /// A method used to hide the lost life indicator.
   /// </summary>
   public void HideLostLife() {
      if (imgLostLife.gameObject.activeSelf == true) {
         imgLostLife.gameObject.SetActive(false);
         RemovePopupMsgTracker(5);
      }
   }

   /// <summary>
   /// A method used to show the dimmed boost indicator.
   /// </summary>
   public void DimBoost() {
      imgBoostOn.gameObject.SetActive(false);
      imgBoostOff.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the undimmed boost indicator.
   /// </summary>
   public void UndimBoost() {
      imgBoostOn.gameObject.SetActive(true);
      imgBoostOff.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the boost indicator.
   /// </summary>
   public void ShowBoost() {
      imgBoostOn.gameObject.SetActive(false);
      imgBoostOff.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the boost indicator.
   /// </summary>
   public void HideBoost() {
      imgBoostOn.gameObject.SetActive(false);
      imgBoostOff.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the dimmed ammo indicator.
   /// </summary>
   public void DimAmmo() {
      txtAmmo.gameObject.SetActive(false);
      imgAmmoOff.gameObject.SetActive(true);
      imgAmmoOn.gameObject.SetActive(false);
      imgAmmoBubble.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the undimmed ammo indicator.
   /// </summary>
   public void UndimAmmo() {
      txtAmmo.gameObject.SetActive(true);
      imgAmmoOff.gameObject.SetActive(false);
      imgAmmoOn.gameObject.SetActive(true);
      imgAmmoBubble.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the ammo indicator.
   /// </summary>
   public void ShowAmmo() {
      txtAmmo.gameObject.SetActive(false);
      imgAmmoOff.gameObject.SetActive(true);
      imgAmmoOn.gameObject.SetActive(false);
      imgAmmoBubble.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the ammo indicator.
   /// </summary>
   public void HideAmmo() {
      txtAmmo.gameObject.SetActive(false);
      imgAmmoOff.gameObject.SetActive(false);
      imgAmmoOn.gameObject.SetActive(false);
      imgAmmoBubble.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to set the value of the current player's ammo.
   /// </summary>
   /// <param name="i">An integer value that represents the amount of ammo.</param>
   public void SetAmmo(int i) {
      txtAmmo.text = ("" + i);
   }

   /// <summary>
   /// A method used to show the dimmed invincibility indicator.
   /// </summary>
   public void DimInvinc() {
      imgInvincOff.gameObject.SetActive(true);
      imgInvincOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the undimmed invincibility indicator.
   /// </summary>
   public void UndimInvinc() {
      imgInvincOff.gameObject.SetActive(false);
      imgInvincOn.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the invincibility indicator.
   /// </summary>
   public void ShowInvinc() {
      imgInvincOff.gameObject.SetActive(true);
      imgInvincOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to hide the invincibility indicator.
   /// </summary>
   public void HideInvinc() {
      imgInvincOff.gameObject.SetActive(false);
      imgInvincOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the dimmed armor indicator.
   /// </summary>
   public void DimArmor() {
      imgArmorOff.gameObject.SetActive(true);
      imgArmorOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the undimmed armor indicator.
   /// </summary>
   public void UndimArmor() {
      imgArmorOff.gameObject.SetActive(false);
      imgArmorOn.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the armor indicator.
   /// </summary>
   public void ShowArmor() {
      imgArmorOff.gameObject.SetActive(true);
      imgArmorOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to hide the armor indicator.
   /// </summary>
   public void HideArmor() {
      imgArmorOff.gameObject.SetActive(false);
      imgArmorOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the dimmed draft indicator.
   /// </summary>
   public void DimDraft() {
      imgDraftOff.gameObject.SetActive(true);
      imgDraftOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the undimmed draft indicator.
   /// </summary>
   public void UndimDraft() {
      imgDraftOff.gameObject.SetActive(false);
      imgDraftOn.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the draft indicator.
   /// </summary>
   public void ShowDraft() {
      imgDraftOff.gameObject.SetActive(true);
      imgDraftOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to hide the draft indicator.
   /// </summary>
   public void HideDraft() {
      imgDraftOff.gameObject.SetActive(false);
      imgDraftOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the dimmed passing indicator.
   /// </summary>
   public void DimPassing() {
      imgPassingOn.gameObject.SetActive(false);
      imgPassingOff.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the undimmed passing indicator.
   /// </summary>
   public void UndimPassing() {
      imgPassingOn.gameObject.SetActive(true);
      imgPassingOff.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the passing indicator.
   /// </summary>
   public void ShowPassing() {
      imgPassingOn.gameObject.SetActive(false);
      imgPassingOff.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to hide the passing indicator.
   /// </summary>
   public void HidePassing() {
      imgPassingOn.gameObject.SetActive(false);
      imgPassingOff.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the dimmed targetting indicator.
   /// </summary>
   public void DimTarget() {
      imgTargetOff.gameObject.SetActive(true);
      imgTargetOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the undimmed targetting indicator.
   /// </summary>
   public void UndimTarget() {
      imgTargetOff.gameObject.SetActive(false);
      imgTargetOn.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the targetting indicator.
   /// </summary>
   public void ShowTarget() {
      imgTargetOff.gameObject.SetActive(true);
      imgTargetOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to hide the targetting indicator.
   /// </summary>
   public void HideTarget() {
      imgTargetOff.gameObject.SetActive(false);
      imgTargetOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the dimmed target hit indicator.
   /// </summary>
   public void DimTargetHit() {
      imgTargetHitOff.gameObject.SetActive(true);
      imgTargetHitOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the undimmed target hit indicator.
   /// </summary>
   public void UndimTargetHit() {
      imgTargetHitOff.gameObject.SetActive(false);
      imgTargetHitOn.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the target hit indicator.
   /// </summary>
   public void ShowTargetHit() {
      imgTargetHitOff.gameObject.SetActive(true);
      imgTargetHitOn.gameObject.SetActive(false);
   }

   /// <summary>
   ///  A method used to hide the target hit indicator.
   /// </summary>
   public void HideTargetHit() {
      imgTargetHitOff.gameObject.SetActive(false);
      imgTargetHitOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the dimmed target miss indicator.
   /// </summary>
   public void DimTargetMiss() {
      imgTargetMissOff.gameObject.SetActive(true);
      imgTargetMissOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to show the undimmed target miss indicator.
   /// </summary>
   public void UndimTargetMiss() {
      imgTargetMissOff.gameObject.SetActive(false);
      imgTargetMissOn.gameObject.SetActive(true);
   }

   /// <summary>
   /// A method used to show the target miss indicator.
   /// </summary>
   public void ShowTargetMiss() {
      imgTargetMissOff.gameObject.SetActive(true);
      imgTargetMissOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// A method used to hide the target miss indicator.
   /// </summary>
   public void HideTargetMiss() {
      imgTargetMissOff.gameObject.SetActive(false);
      imgTargetMissOn.gameObject.SetActive(false);
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (Input.GetButtonUp("Cancel")) {
         if (gameState.gameRunning == true && gameState.gamePaused == false) {
            btnExit.onClick.Invoke();
         }
      }
   }
}