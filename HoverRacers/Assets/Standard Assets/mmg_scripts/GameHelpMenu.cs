using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class used to manage the help menu.
/// </summary>
public class GameHelpMenu : BasePromptScript {
   //***** Class Fields *****
   /// <summary>
   /// An image shown as page 1 of the help screens.
   /// </summary>
   public Image help1 = null;

   /// <summary>
   /// An image shown as page 2 of the help screens.
   /// </summary>
   public Image help2 = null;

   /// <summary>
   /// An image shown as page 3 of the help screens.
   /// </summary>
   public Image help3 = null;

   /// <summary>
   /// An image shown as page 4 of the help screens.
   /// </summary>
   public Image help4 = null;

   /// <summary>
   /// An image shown as page 5 of the help screens.
   /// </summary>
   public Image help5 = null;

   /// <summary>
   /// An image shown as page 6 of the help screens.
   /// </summary>
   public Image help6 = null;

   /// <summary>
   /// An image shown as page 7 of the help screens.
   /// </summary>
   public Image help7 = null;

   /// <summary>
   /// An image shown as page 8 of the help screens.
   /// </summary>
   public Image help8 = null;

   /// <summary>
   /// The index of the currently active help screen in the set of screens available on this form.
   /// </summary>
   private int idx = 0;

   /// <summary>
   /// An integer value that represents the maximum help screen index on this form.
   /// </summary>
   private int MAX_INDEX = 8;

   /// <summary>
   /// A Button object that enables navigating back through the help screens.
   /// </summary>
   public Button btnPrev = null;

   /// <summary>
   /// A Button object that enables navigating next through the help screens.
   /// </summary>
   public Button btnNext = null;

   /// <summary>
   /// A Button object that represents the third button on the form.
   /// </summary>
   public Button btnThree = null;

   //***** Internal Variables *****
   private Image img = null;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      keyBrdInputIdxMax = 3;
      base.Prep(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }
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
            if (idx == 0) {
               keyBrdInputIdx = 1;
               keyBrdInputPrevIdx = -1;
            } else {
               keyBrdInputIdx = 0;
               keyBrdInputPrevIdx = -1;
            }
            SetBtnTextColor(keyBrdInputPrevIdx, keyBrdInputIdx);
         } else if (Input.GetButtonDown("MenuSelectDown")) {
            keyBrdInput = true;
            if (idx == MAX_INDEX - 1) {
               keyBrdInputIdx = (keyBrdInputIdxMax - 1);
               keyBrdInputPrevIdx = -1;
            } else {
               keyBrdInputIdx = 1;
               keyBrdInputPrevIdx = -1;
            }
            SetBtnTextColor(keyBrdInputPrevIdx, keyBrdInputIdx);
         }
      } else {
         if (Input.GetButtonDown("MenuSelectUp")) {
            if (keyBrdInputIdx + 1 < keyBrdInputIdxMax) {
               keyBrdInputPrevIdx = keyBrdInputIdx;
               keyBrdInputIdx++;
            } else {
               keyBrdInputPrevIdx = (keyBrdInputIdxMax - 1);
               keyBrdInputIdx = 0;
            }

            if (idx == 0 && keyBrdInputIdx == 0) {
               keyBrdInputIdx++;
            } else if (idx == (MAX_INDEX - 1) && keyBrdInputIdx == (keyBrdInputIdxMax - 1)) {
               keyBrdInputIdx = 0;
            }

            SetBtnTextColor(keyBrdInputPrevIdx, keyBrdInputIdx);
         } else if (Input.GetButtonDown("MenuSelectDown")) {
            if (keyBrdInputIdx - 1 >= keyBrdInputIdxMin) {
               keyBrdInputPrevIdx = keyBrdInputIdx;
               keyBrdInputIdx--;
            } else {
               keyBrdInputPrevIdx = keyBrdInputIdx;
               keyBrdInputIdx = (keyBrdInputIdxMax - 1);
            }

            if (idx == 0 && keyBrdInputIdx == 0) {
               keyBrdInputIdx = (keyBrdInputIdxMax - 1);
            } else if (idx == (MAX_INDEX - 1) && keyBrdInputIdx == (keyBrdInputIdxMax - 1)) {
               keyBrdInputIdx--;
            }

            SetBtnTextColor(keyBrdInputPrevIdx, keyBrdInputIdx);
         } else if (Input.GetButtonDown("Submit")) {
            InvokeClick(keyBrdInputIdx);
         }
      }
   }

   /// <summary>
   /// A method used to invoke a form button click.
   /// </summary>
   /// <param name="current">The button to invoke a button click on.</param>
   public new void InvokeClick(int current) {
      if (current == 0) {
         if (idx > 0) {
            btnOne.onClick.Invoke();
         }
      } else if (current == 1) {
         btnTwo.onClick.Invoke();
      } else if (current == 2) {
         if (idx < MAX_INDEX - 1) {
            btnThree.onClick.Invoke();
         }
      }
   }

   /// <summary>
   /// A method used to set the text color of a form button.
   /// </summary>
   /// <param name="prev">The previous form button to adjust.</param>
   /// <param name="current">The current form button to adjust.</param>
   public new void SetBtnTextColor(int prev, int current) {
      if (prev == 0) {
         img = btnOne.GetComponent<Image>();
         img.color = Color.white;
      } else if (prev == 1) {
         txt = btnTwo.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.black;
      } else if (prev == 2) {
         img = btnThree.GetComponent<Image>();
         img.color = Color.white;
      }

      if (current == 0) {
         img = btnOne.GetComponent<Image>();
         img.color = Color.red;
      } else if (current == 1) {
         txt = btnTwo.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.red;
      } else if (current == 2) {
         img = btnThree.GetComponent<Image>();
         img.color = Color.red;
      }
   }

   /// <summary>
   /// A method used to enable the previous button.
   /// </summary>
   public void EnablePrev() {
      if (btnPrev != null) {
         btnPrev.interactable = true;
      }
   }

   /// <summary>
   /// A method used to disabled the previous button.
   /// </summary>
   public void DisablePrev() {
      if (btnPrev != null) {
         btnPrev.interactable = false;
      }
   }

   /// <summary>
   /// A method used to enable the next button.
   /// </summary>
   public void EnableNext() {
      if (btnNext != null) {
         btnNext.interactable = true;
      }
   }

   /// <summary>
   /// A method used to disabled the next button.
   /// </summary>
   public void DisableNext() {
      if (btnNext != null) {
         btnNext.interactable = false;
      }
   }

   /// <summary>
   /// A method used to perform the main menu UI event.
   /// </summary>
   public void PerformMainMenuUI() {
      PerformMainMenu();
   }

   /// <summary>
   /// A method that is used to navigate to the main menu.
   /// </summary>
   public void PerformMainMenu() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.PlayMenuSound();
      gameState.ShowStartMenu();
      gameState.HideHelpMenu();
   }

   /// <summary>
   /// A method used to perform the next UI event.
   /// </summary>
   public void PerformNextUI() {
      PerformNext();
   }

   /// <summary>
   /// A method that is used to navigate to the next help screen.
   /// </summary>
   public void PerformNext() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.PlayMenuSound();
      if ((idx + 1) < MAX_INDEX) {
         idx++;
      }
      ShowHelpScreen(idx);
   }

   /// <summary>
   /// A method used to perform the previous UI event.
   /// </summary>
   public void PerformPrevUI() {
      PerformPrev();
   }

   /// <summary>
   /// A method that is used to navigate to the previous help screen.
   /// </summary>
   public void PerformPrev() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      gameState.PlayMenuSound();
      if ((idx - 1) >= 0) {
         idx--;
      }
      ShowHelpScreen(idx);
   }

   /// <summary>
   /// A method used to show the indicated help screen and hide the other screens.
   /// </summary>
   /// <param name="i"></param>
   private void ShowHelpScreen(int i) {
      if (help1 != null) {
         help1.gameObject.SetActive(false);
      }

      if (help2 != null) {
         help2.gameObject.SetActive(false);
      }

      if (help3 != null) {
         help3.gameObject.SetActive(false);
      }

      if (help4 != null) {
         help4.gameObject.SetActive(false);
      }

      if (help5 != null) {
         help5.gameObject.SetActive(false);
      }

      if (help6 != null) {
         help6.gameObject.SetActive(false);
      }

      if (help7 != null) {
         help7.gameObject.SetActive(false);
      }

      if (help8 != null) {
         help8.gameObject.SetActive(false);
      }

      if (i == 0) {
         help1.gameObject.SetActive(true);
         DisablePrev();
         EnableNext();
      } else if (i == 1) {
         help2.gameObject.SetActive(true);
         EnablePrev();
         EnableNext();
      } else if (i == 2) {
         help3.gameObject.SetActive(true);
         EnablePrev();
         EnableNext();
      } else if (i == 3) {
         help4.gameObject.SetActive(true);
         EnablePrev();
         EnableNext();
      } else if (i == 4) {
         help5.gameObject.SetActive(true);
         EnablePrev();
         EnableNext();
      } else if (i == 5) {
         help6.gameObject.SetActive(true);
         EnablePrev();
         EnableNext();
      } else if (i == 6) {
         help7.gameObject.SetActive(true);
         EnablePrev();
         EnableNext();
      } else if (i == 7) {
         help8.gameObject.SetActive(true);
         EnablePrev();
         DisableNext();
      }
   }
}