using UnityEngine;

/// <summary>
/// A class that is used to process collision events for a specific player's car.
/// </summary>
public class CollideScript : BaseScript {
   //***** Static Class Fields *****
   /// <summary>
   /// The duration in milliseconds that it should take to perform a bounce.
   /// </summary>
   public readonly float BOUNCE_DURATION = 80.0f;

   /// <summary>
   /// The duration in milliseconds that it should take to perfrom a boost.
   /// </summary>
   public readonly float BOOST_DURATION = 200.0f;

   /// <summary>
   /// The minimum value that a jump force can have.
   /// </summary>
   public readonly float MIN_JUMP_FORCE = 18.0f;

   /// <summary>
   /// The maximum value that a jump force can have.
   /// </summary>
   public readonly float MAX_JUMP_FORCE = 22.0f;

   //***** Class Fields *****
   /// <summary>
   /// The car's maximum speed without boost.
   /// </summary>
   private float maxSpeed = 200.0f;

   /// <summary>
   /// A GameObject that represents the current player.
   /// </summary>
   private GameObject player = null;

   /// <summary>
   /// A CharacterContoller object that is used to control the current player's car.
   /// </summary>
   private CharacterController controller = null;

   /// <summary>
   /// A CharacterMotor object that is used to move the current player's car.
   /// </summary>
   private CharacterMotor cm = null;

   /// <summary>
   /// A float value used to increase the force of a jump.
   /// </summary>
   public float forceMultiplier = 2.5f;

   /// <summary>
   /// A float value used to create a minimum force value for hittable object collisions.
   /// </summary>
   public float minForce = 20.0f;

   /// <summary>
   /// A float value used to create a maximum force value for hittable object collisions.
   /// </summary>
   public float maxForce = 80.0f;

   /// <summary>
   /// A boolean value used to indicate that the Y axis is locked during the object collision handling.
   /// </summary>
   public bool lockAxisY = false;

   /// <summary>
   /// A float value used to lessen the forces involved during a bounce.
   /// </summary>
   public float bounceDampener = 1.0f;

   /// <summary>
   /// A float value used to indicate the minimum horizontal velocity to use during a bounce.
   /// </summary>
   public float minBounceVelocityHor = 25.0f;

   /// <summary>
   /// A float value used to indicate the minimum vertical velocity to use during a bounce.
   /// </summary>
   public float minBounceVelocityVer = 25.0f;

   /// <summary>
   /// A float value used to set the baseline vertical force for a jump.
   /// </summary>
   public float jump = 9.0f;

   //***** Internal Variables: Mod Markers *****
   private GameObject lastHealthMarker = null;
   private GameObject lastGunMarker = null;
   private GameObject lastInvcMarker = null;
   private GameObject lastArmorMarker = null;

   //***** Internal Variables: Start *****
   private AudioSource audioJump = null;
   private AudioSource audioBounce = null;
   private AudioSource audioBoost = null;
   private AudioSource audioPowerUp = null;

   //***** Internal Variables: PerformHit *****
   private float collideStrength;
   private float jumpHit = 15.0f;
   private Rigidbody body = null;
   private Vector3 moveDirection = Vector3.zero;
   private Vector3 rotateDirection = Vector3.zero;
   private AudioSource phAudioS = null;

   //***** Internal Variables: PerformBoost *****
   private float pbAbsX = 0.0f;
   private float pbAbsZ = 0.0f;
   private bool boostOn = false;
   private bool boostHandOff = false;
   private Vector3 boostV3 = Vector3.zero;
   private float boostTime = 0.0f;

   //***** Internal Variables: PerformBounce *****
   private PlayerInfo lpi = null;
   private int lpIdx = 0;
   private PlayerState lp = null;
   private CollideScript lc = null;
   private Vector3 v3;
   private float x;
   private float y;
   private float z;
   private bool isBouncing = false;
   private bool useReflect = false;
   private bool useInverse = false;
   private bool bounceHandOff = false;
   private Vector3 bounceV3 = Vector3.zero;
   private float bounceTime = 0.0f;

   //***** Internal Variables: PerformJump *****
   private bool isJumping = false;
   private bool jumpHandOff = false;
   private Vector3 jumpV3 = Vector3.zero;
   private float jumpStrength;
   private float gravity = 10.0f;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      base.PrepPlayerInfo(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      } else {
         player = p.player;
         maxSpeed = p.maxSpeed;
         controller = p.controller;
         cm = p.cm;

         if (controller == null) {
            Utilities.wrForce("CollideScript: controller is null! Deactivating...");
            MarkScriptActive(false);
            return;
         }

         if (player == null) {
            Utilities.wrForce("CollideScript: player is null! Deactivating...");
            MarkScriptActive(false);
            return;
         }

         if (cm == null) {
            Utilities.wrForce("CollideScript: cm is null! Deactivating...");
            MarkScriptActive(false);
            return;
         }

         AudioSource[] audioSetDst = Utilities.LoadAudioResources(GetComponentsInParent<AudioSource>(), new string[] { Utilities.SOUND_FX_JUMP, Utilities.SOUND_FX_BOUNCE, Utilities.SOUND_FX_BOOST, Utilities.SOUND_FX_POWER_UP });
         if(audioSetDst != null) {
            audioJump = audioSetDst[0];
            audioBounce = audioSetDst[1];
            audioBoost = audioSetDst[2];
            audioPowerUp = audioSetDst[3];
         }
      }
   }

   /// <summary>
   /// A method to calculate the collision strength based on the player speed.
   /// </summary>
   private void CalcCollideStrength() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (p == null) {
         collideStrength = 0;
      } else {
         collideStrength = (p.speed * forceMultiplier) / maxSpeed;
      }
   }

   /// <summary>
   /// A method used to calculate a minimum force value based on a given value.
   /// </summary>
   /// <param name="v">A given component input value.</param>
   /// <returns>A minimum force.</returns>
   private float GetMinForce(float v) {
      if (Mathf.Abs(v) < minForce) {
         if (v < 0) {
            return -minForce;
         } else {
            return minForce;
         }
      }
      return v;
   }

   /// <summary>
   /// A method used to calculate a maximum force value based on a given value.
   /// </summary>
   /// <param name="v">A given component input value.</param>
   /// <returns>A maximum force.</returns>
   private float GetMaxForce(float v) {
      if (Mathf.Abs(v) > maxForce) {
         if (v < 0) {
            return -maxForce;
         } else {
            return maxForce;
         }
      }
      return v;
   }

   /// <summary>
   /// A method to perform a gun shot hit on the current player's car.
   /// </summary>
   /// <param name="go">The GameObject that was hit by the car.</param>
   /// <param name="hit">The hit collision object from the car.</param>
   public void PerformHit(GameObject go, ControllerColliderHit hit) {
      if (BaseScript.IsActive(scriptName) == false || go == null || hit == null) {
         return;
      }

      body = hit.collider.attachedRigidbody;
      if (body == null || body.isKinematic) {
         return;
      }

      moveDirection = Vector3.zero;
      CalcCollideStrength();
      if (lockAxisY == false) {
         moveDirection.y = (jumpHit * collideStrength);
      } else {
         moveDirection.y = 0;
      }
      moveDirection.x = (cm.movement.velocity.x * collideStrength);
      moveDirection.z = (cm.movement.velocity.z * collideStrength);

      if (minForce > 0) {
         moveDirection.x = GetMinForce(moveDirection.x);
         moveDirection.z = GetMinForce(moveDirection.z);

         if (lockAxisY == false) {
            moveDirection.y = GetMinForce(moveDirection.y);
         }
      }

      if (maxForce > 0) {
         moveDirection.x = GetMaxForce(moveDirection.x);
         moveDirection.z = GetMaxForce(moveDirection.z);

         if (lockAxisY == false) {
            moveDirection.y = GetMaxForce(moveDirection.y);
         }
      }

      rotateDirection = (moveDirection * 1);
      body.rotation = Quaternion.Euler(rotateDirection);
      body.velocity = moveDirection;

      phAudioS = go.GetComponent<AudioSource>();
      if (phAudioS != null) {
         if (phAudioS.isPlaying == false) {
            phAudioS.Play();
         }
      }
   }

   /// <summary>
   /// Gets a bounce velocity on a horizontal axis given an input velocity.
   /// </summary>
   /// <param name="v">An input bounce velocity for a horizontal axis.</param>
   /// <returns>An adjusted horizontal bounce velocity.</returns>
   private float GetBounceVelHor(float v) {
      if (useReflect == true) {
         v = v * bounceDampener;
      } else {
         if (useInverse == true) {
            v = v * -1 * bounceDampener;
         } else {
            v = v * bounceDampener;
         }
      }

      if (v < 0) {
         if (v > -minBounceVelocityHor) {
            v = -minBounceVelocityHor;
         }
      } else if (v >= 0) {
         if (v < minBounceVelocityHor) {
            v = minBounceVelocityHor;
         }
      }
      return v;
   }

   /// <summary>
   /// A method to perform a bounce on the current player's car.
   /// </summary>
   /// <param name="go">The GameObject that was hit by the gun shot.</param>
   /// <param name="hit">The hit collision from the gun shot.</param>
   public void PerformBounce(GameObject go, ControllerColliderHit hit) {
      if (BaseScript.IsActive(scriptName) == false || go == null || hit == null) {
         return;
      }

      x = GetBounceVelHor(cm.movement.velocity.x);
      y = cm.movement.velocity.y;
      z = GetBounceVelHor(cm.movement.velocity.z);

      if (useReflect == true) {
         v3 = Vector3.Reflect(v3, hit.collider.ClosestPointOnBounds(player.transform.position).normalized);
      } else {
         v3 = new Vector3(x, y, z);
      }

      Utilities.LoadPlayerInfo(GetType().Name, out lpi, out lpIdx, out lp, hit.gameObject, gameState, false);
      if (lp != null) {
         lc = lp.player.GetComponent<CollideScript>();
         lc.bounceHandOff = true;
         lc.bounceV3 = v3;
      }
   }

   /// <summary>
   /// A method that gets the horizontal boost velocity for the given mode and movement velocity. 
   /// </summary>
   /// <param name="mode">A mode value to determine the boost velocity to assign.</param>
   /// <param name="movVel">A movement velocity used to determine direction.</param>
   /// <returns>An adjusted horizontal boost value.</returns>
   private float GetBoostVelHor(int mode, float movVel) {
      float v3 = 0.0f;
      if (mode == 0) {
         v3 = 200;
      } else if (mode == 1) {
         v3 = 50;
      } else if (mode == 2) {
         v3 = 25;
      } else if (mode == 3) {
         v3 = 100;
      } else if (mode == 4) {
         v3 = 15;
      }

      if (movVel < 0) {
         v3 *= -1;
      }
      return v3;
   }

   /// <summary>
   /// A method to perform a boost on the current player's car, //0 = normal, 1 = small, 2 = tiny.
   /// </summary>
   /// <param name="go">The GameObject that was hit by the gun shot.</param>
   /// <param name="hit">The hit collision from the gun shot.</param>
   /// <param name="mode">The boost mode to apply to the car.</param>
   public void PerformBoost(GameObject go, ControllerColliderHit hit, int mode) {
      if (BaseScript.IsActive(scriptName) == false || go == null || hit == null) {
         return;
      }

      pbAbsX = Mathf.Abs(p.cm.movement.velocity.x);
      pbAbsZ = Mathf.Abs(p.cm.movement.velocity.z);
      boostV3 = Vector3.zero;

      if (pbAbsX > pbAbsZ) {
         boostV3.x = GetBoostVelHor(mode, p.cm.movement.velocity.x);
      } else {
         boostV3.z = GetBoostVelHor(mode, p.cm.movement.velocity.z);
      }

      boostHandOff = true;
      if (audioBoost != null) {
         if (audioBoost.isPlaying == false) {
            audioBoost.Play();
         }
      }

      if (p != null) {
         p.flame.SetActive(true);
      }
   }

   /// <summary>
   /// A method to perform a jump on the current player's car.
   /// </summary>
   /// <param name="go">The GameObject that was hit by the gun shot.</param>
   /// <param name="hit">The hit collision from the gun shot.</param>
   public void PerformJump(GameObject go, ControllerColliderHit hit) {
      if (BaseScript.IsActive(scriptName) == false || go == null || hit == null) {
         return;
      }

      jumpStrength = ((p.speed) * forceMultiplier) / maxSpeed;
      jumpV3 = Vector3.zero;
      jumpV3.y = (jump * jumpStrength);

      if (jumpV3.y < MIN_JUMP_FORCE) {
         jumpV3.y = MIN_JUMP_FORCE;
      }

      if (jumpV3.y >= MAX_JUMP_FORCE) {
         jumpV3.y = MAX_JUMP_FORCE;
      }

      jumpHandOff = true;
      if (audioJump != null) {
         if (audioJump.isPlaying == false) {
            audioJump.Play();
         }
      }
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (gameState != null) {
         if (gameState.gamePaused == true) {
            return;
         } else if (gameState.gameRunning == false) {
            return;
         }
      }

      //bounce code
      if (bounceHandOff == true) {
         bounceTime = 0f;
         isBouncing = true;
         p.isBouncing = true;

         if (audioBounce != null) {
            if (audioBounce.isPlaying == false) {
               audioBounce.Play();
            }
         }
      }

      if (isBouncing == true) {
         bounceTime += (Time.deltaTime * 100);
         bounceHandOff = false;
         controller.Move(bounceV3 * Time.deltaTime);
      }

      if (isBouncing == true && bounceTime >= BOUNCE_DURATION) {
         isBouncing = false;
         p.isBouncing = false;
      }

      //boost code
      if (boostHandOff == true) {
         boostTime = 0f;
         boostOn = true;
         p.offTrack = false;
         p.boostOn = true;
         p.SetBoost();
      }

      if (boostOn == true) {
         boostTime += (Time.deltaTime * 100);
         boostHandOff = false;
         controller.Move(boostV3 * Time.deltaTime);
      }

      if (boostOn == true && boostTime >= BOOST_DURATION) {
         boostOn = false;
         p.boostOn = false;
         p.SetNorm();
         p.flame.SetActive(false);
      }

      //jump code
      if (controller.isGrounded == true) {
         cm.jumping.jumping = false;
         p.isJumping = false;
         isJumping = false;
      }

      if (jumpHandOff == true) {
         p.offTrack = false;
         cm.jumping.jumping = true;
         p.isJumping = true;
         isJumping = true;
      }

      if (isJumping == true) {
         jumpHandOff = false;
         controller.Move(jumpV3 * Time.deltaTime);
      }

      //gravity code
      if ((controller.isGrounded == false || cm.movement.velocity.y > 0) && isJumping == true) {
         jumpV3.y -= gravity * Time.deltaTime;
      }

      if (player != null && player.transform.position.y >= Utilities.MAX_XFORM_POS_Y && cm.movement.velocity.y > Utilities.MIN_XFORM_POS_Y) {
         cm.movement.velocity.y -= gravity * Time.deltaTime;
      } else if (controller.isGrounded == false || cm.movement.velocity.y > 0 || p.player.transform.position.y > 0) {
         cm.movement.velocity.y -= gravity * Time.deltaTime;
      }

      if (controller.isGrounded == false) {
         cm.movement.velocity.y -= gravity * Time.deltaTime;
      }
   }

   /// <summary>
   /// A collision event handler that processes when the current player's car controller hit.
   /// </summary>
   /// <param name="hit">Provides information about the collision event.</param>
   public void OnControllerColliderHit(ControllerColliderHit hit) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (hit.gameObject.CompareTag(Utilities.TAG_UNTAGGED)) {
         return;
      } else if (hit.gameObject.CompareTag(Utilities.TAG_HITTABLE)) {
         lockAxisY = false;
         PerformHit(hit.gameObject, hit);
      } else if (hit.gameObject.CompareTag(Utilities.TAG_HITTABLE_NOY)) {
         lockAxisY = true;
         PerformHit(hit.gameObject, hit);
         lockAxisY = false;
      } else if (hit.gameObject.CompareTag(Utilities.TAG_PLAYERS)) {
         if (hit != null && hit.gameObject != null) {
            PerformBounce(hit.gameObject, hit);
         }
      } else if (hit.gameObject.CompareTag(Utilities.TAG_BOOST_MARKER)) {
         if (p.boostOn == true || p.aiIsPassing == false) {
            PerformBoost(hit.gameObject, hit, 0);
         }
      } else if (hit.gameObject.CompareTag(Utilities.TAG_SMALL_BOOST_MARKER)) {
         if (p.boostOn == true || p.aiIsPassing == false) {
            PerformBoost(hit.gameObject, hit, 1);
         }
      } else if (hit.gameObject.CompareTag(Utilities.TAG_TINY_BOOST_MARKER)) {
         if (p.boostOn == true || p.aiIsPassing == false) {
            PerformBoost(hit.gameObject, hit, 2);
         }
      } else if (hit.gameObject.CompareTag(Utilities.TAG_MEDIUM_BOOST_MARKER)) {
         if (p.boostOn == true || p.aiIsPassing == false) {
            PerformBoost(hit.gameObject, hit, 3);
         }
      } else if (hit.gameObject.CompareTag(Utilities.TAG_TINY_BOOST_2_MARKER)) {
         if (p.boostOn == true || p.aiIsPassing == false) {
            PerformBoost(hit.gameObject, hit, 4);
         }
      } else if (hit.gameObject.CompareTag(Utilities.TAG_JUMP_MARKER)) {
         if (p.isJumping == false) {
            PerformJump(hit.gameObject, hit);
         }
      } else if (hit.gameObject.CompareTag(Utilities.TAG_HEALTH_MARKER)) {
         if (audioPowerUp != null) {
            if (audioPowerUp.isPlaying == false) {
               audioPowerUp.Play();
            }
         }

         if (p.damage - 1 >= 0) {
            p.damage -= 1;
         }

         p.aiHasGainedLife = true;
         p.aiHasGainedLifeTime = 0;
         hit.gameObject.SetActive(false);
         lastHealthMarker = hit.gameObject;
         Invoke(nameof(RecreateHealthMarker), Random.Range(Utilities.MARKER_REFRESH_MIN, Utilities.MARKER_REFRESH_MAX));
      } else if (hit.gameObject.CompareTag(Utilities.TAG_GUN_MARKER)) {
         if (audioPowerUp != null) {
            if (audioPowerUp.isPlaying == false) {
               audioPowerUp.Play();
            }
         }

         if (p.ammo <= Utilities.MAX_AMMO) {
            p.ammo += Utilities.AMMO_INC;
         }

         p.gunOn = true;
         p.ShowGun();
         hit.gameObject.SetActive(false);
         lastGunMarker = hit.gameObject;
         Invoke(nameof(RecreateGunMarker), Random.Range(Utilities.MARKER_REFRESH_MIN, Utilities.MARKER_REFRESH_MAX));
      } else if (hit.gameObject.CompareTag(Utilities.TAG_INVINC_MARKER)) {
         if (audioPowerUp != null) {
            if (audioPowerUp.isPlaying == false) {
               audioPowerUp.Play();
            }
         }

         p.invincOn = true;
         p.invincTime = 0;
         p.ShowInvinc();
         hit.gameObject.SetActive(false);
         lastInvcMarker = hit.gameObject;
         Invoke(nameof(RecreateInvcMarker), Random.Range(Utilities.MARKER_REFRESH_MIN, Utilities.MARKER_REFRESH_MAX));
      } else if (hit.gameObject.CompareTag(Utilities.TAG_ARMOR_MARKER)) {
         if (audioPowerUp != null) {
            if (audioPowerUp.isPlaying == false) {
               audioPowerUp.Play();
            }
         }

         p.armorOn = true;
         hit.gameObject.SetActive(false);
         lastArmorMarker = hit.gameObject;
         Invoke(nameof(RecreateArmorMarker), Random.Range(Utilities.MARKER_REFRESH_MIN, Utilities.MARKER_REFRESH_MAX));
      }
   }

   /// <summary>
   /// A method used to recreate the health marker after it has been collided with and set to inactive.
   /// </summary>
   public void RecreateHealthMarker() {
      lastHealthMarker.SetActive(true);
   }

   /// <summary>
   /// A method used to recreate the gun marker after it has been collided with and set to inactive.
   /// </summary>
   public void RecreateGunMarker() {
      lastGunMarker.SetActive(true);
   }

   /// <summary>
   /// A method used to recreate the invincibility marker after it has been collided with and set to inactive.
   /// </summary>
   public void RecreateInvcMarker() {
      lastInvcMarker.SetActive(true);
   }

   /// <summary>
   /// A method used to recreate the armor marker after it has been collided with and set to inactive.
   /// </summary>
   public void RecreateArmorMarker() {
      lastArmorMarker.SetActive(true);
   }
}