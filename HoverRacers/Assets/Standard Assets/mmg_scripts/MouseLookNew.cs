using UnityEngine;

/// <summary>
/// A class used to process mouse input used to control a player's car.
/// </summary>
public class MouseLookNew : BaseScript {
   //***** Enumerations *****
   /// <summary>
   /// An enumeration that indicates which axis are being rotated.
   /// </summary>
   public enum RotationAxes {
      MouseXAndY = 0,
      MouseX = 1,
      MouseY = 2
   }

   //***** Class Fields *****
   /// <summary>
   /// The active axis of rotation
   /// </summary>
   public RotationAxes axes = RotationAxes.MouseXAndY;

   /// <summary>
   /// The mouse input sensitivity on the X axis.
   /// </summary>
   public float sensitivityX = 12.0f;

   /// <summary>
   /// The mouse input sensitivity on the Y axis.
   /// </summary>
   public float sensitivityY = 12.0f;

   /// <summary>
   /// The minimum rotation that can be made on the X axis.
   /// </summary>
   public float minimumX = -360.0f;

   /// <summary>
   /// The maximum rotation that can be made on the X axis.
   /// </summary>
   public float maximumX = 360.0f;

   /// <summary>
   /// The minimum rotation that can be made on the Y axis.
   /// </summary>
   public float minimumY = -60.0f;

   /// <summary>
   /// The maximum rotation that can be made on the Y axis.
   /// </summary>
   public float maximumY = 60.0f;

   /// <summary>
   /// The mouse X axis input.
   /// </summary>
   public float mouseX = 0f;

   /// <summary>
   /// The mouse Y axis input.
   /// </summary>
   public float mouseY = 0f;

   /// <summary>
   /// A boolean indicating if AI mode is turned on for this car.
   /// </summary>
   public bool aiOn = false;

   [AddComponentMenu("Camera-Control/Mouse Look")]

   //***** Internal Variables *****
   private Rigidbody rigidBodyTmp = null;

   /// <summary>
   /// Use this for initialization.
   /// </summary>		
   void Start() {
      base.PrepPlayerInfo(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      } else {
         aiOn = p.aiOn;
      }

      // Make the rigid body not change rotation
      rigidBodyTmp = GetComponent<Rigidbody>();
      if (rigidBodyTmp != null) {
         rigidBodyTmp.freezeRotation = true;
      }

      if (Input.touchSupported == true) {
         sensitivityX = 5.0f;
         sensitivityY = 5.0f;
      }
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (gameState.gamePaused == true) {
         return;
      } else if (gameState.gameRunning == false) {
         return;
      }

      if (aiOn == true && p != null) {
         if (p.pause == true) {
            return;
         }
         p.UpdateAiMouseLook();
      } else {
         if (gameState.newTouch == false) {
            mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0, mouseX * sensitivityX, 0);
         }
      }
   }
}