using UnityEngine;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

/// <summary>
/// A class used to process gamepad input.
/// </summary>
public class FPSInputController : BaseScript {
   //***** Class Fields *****
   /// <summary>
   /// A CharacterMotor object used to control the movement of the character, CharacterController.
   /// </summary>
   private CharacterMotor motor;

   /// <summary>
   /// A boolean value that indicates this car is controlled by AI.
   /// </summary>
   public bool aiOn = false;

   /// <summary>
   /// A Vector3 object that indicates the direction of movement.
   /// </summary>
   public Vector3 directionVector = Vector3.zero;

   /// <summary>
   /// A float value indicating that the magnitude of the movement vector.
   /// </summary>
   public float directionLength = 0.0f;

   /// <summary>
   /// A Vector3 object that indicates the input move direction.
   /// </summary>
   public Vector3 inputMoveDirection = Vector3.zero;

   /// <summary>
   /// A float value that indicates the touch input based acceleration.
   /// </summary>
   public float touchSpeed = 0.0f;

   /// <summary>
   /// A float value that indicates the die value, applied to filter the touch input based acceleration.
   /// </summary>
   public float touchSpeedDie = 0.065f;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Awake() {
      motor = GetComponent<CharacterMotor>();
      base.PrepPlayerInfo(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      } else {
         aiOn = p.aiOn;
      }
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      // Get the input vector from keyboard or analog stick
      directionVector = Vector3.zero;
      if (gameState.gamePaused == true) {
         return;
      } else if (gameState.gameRunning == false) {
         return;
      }

      if (aiOn == true && p != null) {
         if (p.pause == true) {
            return;
         }
         directionVector = p.UpdateAiFpsController();
      } else {
         if (Input.touchSupported == true && gameState.accelOn == true) {
            if (Input.touchSupported == true) {
               if (gameState.accelOn == true) {
                  touchSpeed = 1.0f;
                  directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, touchSpeed);
               } else {
                  touchSpeed -= (touchSpeed * touchSpeedDie);
                  if (touchSpeed < 0.0) {
                     touchSpeed = 0.0f;
                  }
                  directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, touchSpeed);
               }
            }
         } else {
            if (Input.GetAxis("Turn") < 0.0f) {
               if (Input.GetAxis("Horizontal") < 0.0f) {
                  transform.Rotate(0, -1.75f, 0);
               } else {
                  transform.Rotate(0, -1.25f, 0);
               }
            }

            if (Input.GetAxis("Turn") > 0.0f) {
               if (Input.GetAxis("Horizontal") > 0.0f) {
                  transform.Rotate(0, 1.75f, 0);
               } else {
                  transform.Rotate(0, 1.25f, 0);
               }
            }

            if (Input.GetAxis("Vertical") > 0.0f) {
               touchSpeed = 1.0f;
               directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, touchSpeed);
            } else if (Input.GetAxis("Vertical") < 0.0f) {
               touchSpeed = -0.65f;
               directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, touchSpeed);
            } else {
               touchSpeed -= (touchSpeed * touchSpeedDie);
               if (touchSpeed < 0.0f) {
                  touchSpeed = 0.0f;
               }
               directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, touchSpeed);
            }
         }
      }

      if (directionVector != Vector3.zero) {
         // Get the length of the directon vector and then normalize it
         // Dividing by the length is cheaper than normalizing when we already have the length anyway
         directionLength = directionVector.magnitude;
         directionVector = directionVector / directionLength;

         // Make sure the length is no bigger than 1
         directionLength = Mathf.Min(1.0f, directionLength);

         // Make the input vector more sensitive towards the extremes and less sensitive in the middle
         // This makes it easier to control slow speeds when using analog sticks
         directionLength = directionLength * directionLength;

         // Multiply the normalized direction vector by the modified length
         directionVector = directionVector * directionLength;
      }

      // Apply the direction to the CharacterMotor
      inputMoveDirection = transform.rotation * directionVector;
      motor.inputMoveDirection = inputMoveDirection;
   }
}