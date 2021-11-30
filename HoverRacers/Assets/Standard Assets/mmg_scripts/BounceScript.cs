using UnityEngine;

/// <summary>
/// A class that is used to bounce the player's car after a collision.
/// </summary>
public class BounceScript : BaseScript {
   //***** Class Fields *****
   /// <summary>
   /// A bounce dampener used to limit the initial bounce velocity of the player's car.
   /// </summary>
   public float bounceDampener = 0.75f;

   /// <summary>
   /// The minimum velocity a bounce in the horizontal direction should have.
   /// </summary>
   public float minBounceVelocityHor = 20.0f;

   /// <summary>
   /// The minimum velocity a bounce in the vertical direction should have.
   /// </summary>
   public float minBounceVelocityVer = 20.0f;

   /// <summary>
   /// A boolean flag indicating if calculated reflection should be used in the bounce calculations.
   /// </summary>
   private bool useReflect = false;

   //***** Internal Variables: BounceObjOff *****
   private Vector3 v3;
   private float x;
   private float y;
   private float z;

   //***** Internal Variables: OnTriggerEnter *****
   private CharacterMotor cm = null;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      base.Prep(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }
      
      audioS = GetComponent<AudioSource>();
      if (audioS == null) {
         Utilities.wrForce(scriptName + ": audioS is null!");
      }
   }

   /// <summary>
   /// Class method used to adjust the direction of the bounced game object by the calculated amount.
   /// </summary>
   /// <param name="go">The GameObject to bounce.</param>
   /// <param name="otherObj">The colliding object, or other object.</param>
   /// <param name="p">The player state associated with the bouncing player.</param>
   /// <param name="cm">The character motor associated with the bouncing player.</param>
   public void BounceObjOff(GameObject go, Collider otherObj, PlayerState p, CharacterMotor cm) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }
      
      v3 = Vector3.zero;
      x = 0;
      y = 0;
      z = 0;
      p.isBouncing = true;

      x = cm.movement.velocity.x;
      if (useReflect == true) {
         x = x * bounceDampener;
      } else {
         x = x * -1 * bounceDampener;
      }

      if (x < 0) {
         if (x > -minBounceVelocityHor) {
            x = -minBounceVelocityHor;
         }
      } else if (x >= 0) {
         if (x < minBounceVelocityHor) {
            x = minBounceVelocityHor;
         }
      }

      z = cm.movement.velocity.z;
      if (useReflect == true) {
         z = z * bounceDampener;
      } else {
         z = z * -1 * bounceDampener;
      }

      if (z < 0) {
         if (z > -minBounceVelocityHor) {
            z = -minBounceVelocityHor;
         }
      } else if (z >= 0) {
         if (z < minBounceVelocityHor) {
            z = minBounceVelocityHor;
         }
      }

      if (useReflect == true) {
         v3 = Vector3.Reflect(v3, otherObj.ClosestPointOnBounds(go.transform.position).normalized);
      } else {
         v3 = new Vector3(x, y, z);
      }

      cm.movement.velocity = v3;
      if (audioS != null) {
         if (audioS.isPlaying == false) {
            audioS.Play();
         }
      }
      p.isBouncing = false;
   }

   /// <summary>
   /// A trigger event handler that fires when the collision object enters the collider.
   /// </summary>
   /// <param name="otherObj">The collider that registered the trigger event.</param>
   public void OnTriggerEnter(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      cm = null;
      if (otherObj != null && otherObj.gameObject != null && otherObj.gameObject.CompareTag(Utilities.TAG_PLAYERS)) {
         Utilities.LoadPlayerInfo(GetType().Name, out PlayerInfo pi, out int playerIndex, out p, otherObj.gameObject, gameState, false);
         if (p != null) {
            cm = p.cm;
         }

         if (p != null && cm != null && p.isBouncing == false) {
            BounceObjOff(otherObj.gameObject, otherObj, p, cm);
         }
      }
   }
}