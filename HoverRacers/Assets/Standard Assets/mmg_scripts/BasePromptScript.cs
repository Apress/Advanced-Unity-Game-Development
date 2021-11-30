using UnityEngine;
using UnityEngine.UI;

public class BasePromptScript : BaseScript {
   //***** Class Fields *****
   /// <summary>
   /// A boolean flag representing if the keyboard is the active source of input.
   /// </summary>
   public bool keyBrdInput = false;

   /// <summary>
   /// The index of the current selected input option.
   /// </summary>
   public int keyBrdInputIdx = 0;

   /// <summary>
   /// The index of the previous selected input option.
   /// </summary>
   public int keyBrdInputPrevIdx = -1;

   /// <summary>
   /// The maximum input index of the current form.
   /// </summary>
   public int keyBrdInputIdxMax = 2;

   /// <summary>
   /// The minimum input index of the current form.
   /// </summary>
   public int keyBrdInputIdxMin = 0;

   /// <summary>
   /// A Text value used to represent information about the current form.
   /// </summary>
   public Text txt = null;

   /// <summary>
   /// A boolean value indicating that a button has been pressed.
   /// </summary>
   public bool btnPressed = false;

   /// <summary>
   /// The first button on the menu.
   /// </summary>
   public Button btnOne;

   /// <summary>
   /// The second button on the menu.
   /// </summary>
   public Button btnTwo;

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   public void Update() {
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
            keyBrdInputIdx = (keyBrdInputIdxMax - 1);
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
               keyBrdInputPrevIdx = keyBrdInputIdx;
               keyBrdInputIdx = (keyBrdInputIdxMax - 1);
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
   public void InvokeClick(int current) {
      if (current == 0) {
         btnOne.onClick.Invoke();
      } else if (current == 1) {
         btnTwo.onClick.Invoke();
      }
   }

   /// <summary>
   /// A method used to set the text color of a form button.
   /// </summary>
   /// <param name="prev">The previous form button to adjust.</param>
   /// <param name="current">The current form button to adjust.</param>
   public void SetBtnTextColor(int prev, int current) {
      if (prev == 0) {
         txt = btnOne.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.white;
      } else if (prev == 1) {
         txt = btnTwo.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.white;
      }

      if (current == 0) {
         txt = btnOne.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.red;
      } else if (current == 1) {
         txt = btnTwo.transform.GetChild(0).GetComponent<Text>();
         txt.color = Color.red;
      }
   }
}
