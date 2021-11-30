using System.Collections;
using UnityEngine;

/// <summary>
/// A class that is used to detect objects that are in front of the car.
/// </summary>
public class CarSensorScript : BaseScript {
   //***** Static Class Fields *****
   /// <summary>
   /// The base boost speed.
   /// </summary>
   public static float BASE_BOOST = 200.0f;

   /// <summary>
   /// The base non boost speed.
   /// </summary>
   public static float BASE_NON_BOOST = 25.0f;

   /// <summary>
   /// The name of the audio source file to use for the gun shot sound.
   /// </summary>
   public static string AUDIO_SOURCE_NAME_GUN_SHOT = "explosion_dirty_rnd_01";

   /// <summary>
   /// The name of the audio source file to use for the gun shot sound.
   /// </summary>
   public static string AUDIO_SOURCE_NAME_TARGETTING = "alien_crickets_lp_01";

   //***** Constants *****
   /// <summary>
   /// The amount of time drafting to trigger passing the car in front.
   /// </summary>
   public static readonly float TRIGGER_TIME_DRAFTING = 2.5f;

   /// <summary>
   /// The amount of time the passing event takes to occur.
   /// </summary>
   public static readonly float TRIGGER_TIME_PASSING = 2.5f;

   /// <summary>
   /// The percentage of the minimum speed needed to trigger the car sensor passing mechanic
   /// </summary>
   public static float TRIGGER_SPEED_PASSING = 0.90f;

   /// <summary>
   /// The maximum distance to maintain to trigger the car sensor passing mechanic. If you are too far or too slow the sensor won't tirgger.
   /// </summary>
   public static readonly float SAFE_FOLLOW_DIST = 80.0f;

   /// <summary>
   /// The maximum distance to still be able to fire the gun at the car in front.
   /// </summary>
   public static readonly float GUN_SHOT_DIST = 160.0f;

   /// <summary>
   /// The amount of time needed to reload the gun.
   /// </summary>
   public static readonly float GUN_RELOAD_TIME = 500.0f;

   /// <summary>
   /// The minimum amount of time the target must be in the sensor for the gun to fire.
   /// </summary>
   public static readonly float MIN_TARGET_TO_FIRE_TIME = 100.0f;

   /// <summary>
   /// The maximum amount of time the gun explosion event has to complete.
   /// </summary>
   public static readonly float MAX_EXPLOSION_TIME = 120.0f;

   //***** Class Fields *****
   /// <summary>
   /// An AudioSource to play when the car's gun is fired.
   /// </summary>
   private AudioSource audioGunShot = null;

   /// <summary>
   /// An AudioSource to play when the car's targetting action is on.
   /// </summary>
   private AudioSource audioTargetting = null;

   /// <summary>
   /// An ArrayList of the current set of cars as Collider objects.
   /// </summary>
   private ArrayList cars = null;

   /// <summary>
   /// A reference to the GameObject that is used to represent the current player's car.
   /// </summary>
   private GameObject player = null;

   //***** Internal Variables: SetBoostVectors *****
   private float absX = 0;
   private float absZ = 0;
   private Vector3 passLeftV3 = Vector3.zero;
   private Vector3 passGoV3 = Vector3.zero;
   private Vector3 passV3 = Vector3.zero;

   //***** Internal Variables: PerformShot *****
   private PlayerState p2 = null;
   private int r2 = 0;

   //***** Internal Variables: Update *****
   private Collider obj = null;
   private int i2 = 0;
   private int l2 = 0;
   private bool tb = false;
   private Vector3 t1 = Vector3.zero;
   private Vector3 t2 = Vector3.zero;
   private float dist = 0.0f;
   private float moveTime = 0.0f;
   private bool explosionOn = false;
   private float explosionTime = 0.0f;
   private Collider target = null;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      cars = new ArrayList();
      base.PrepPlayerInfo(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      } else {
         player = p.player;
         AudioSource[] audioSetDst = Utilities.LoadAudioResources(GetComponentsInParent<AudioSource>(), new string[] { AUDIO_SOURCE_NAME_GUN_SHOT, AUDIO_SOURCE_NAME_TARGETTING });
         if (audioSetDst != null) {
            audioGunShot = audioSetDst[0];
            audioTargetting = audioSetDst[1];
         }
      }
   }

   /// <summary>
   /// Sets the boost vectors for the player's car helping the player pass the car in front of it.
   /// </summary>
   public void SetBoostVectors() {
      if (p == null) {
         return;
      } else if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      absX = Mathf.Abs(p.cm.movement.velocity.x);
      absZ = Mathf.Abs(p.cm.movement.velocity.z);
      passLeftV3 = Vector3.zero;
      passGoV3 = Vector3.zero;
      passV3 = Vector3.zero;

      if (absX > absZ) {
         passGoV3.x = BASE_BOOST;
         if (p.cm.movement.velocity.x < 0) {
            passGoV3.x *= -1;
         }

         passLeftV3.z = BASE_NON_BOOST;
         if (p.cm.movement.velocity.z < 0) {
            passLeftV3.z *= -1;
         }

         passV3.z = passLeftV3.z;
         passV3.x = passGoV3.x;
      } else {
         passGoV3.z = BASE_BOOST;
         if (p.cm.movement.velocity.z < 0) {
            passGoV3.z *= -1;
         }

         passLeftV3.x = BASE_NON_BOOST;
         if (p.cm.movement.velocity.x < 0) {
            passLeftV3.x *= -1;
         }

         passV3.x = passLeftV3.x;
         passV3.z = passGoV3.z;
      }
   }

   /// <summary>
   /// Attempts to shoot the car in front of this car if the current player has a gun mod and ammo and keeps the car on target lock
   /// for the specified amount of time.
   /// </summary>
   /// <param name="otherObj"></param>
   public void PerformGunShotAttempt(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (otherObj.gameObject.CompareTag(Utilities.TAG_PLAYERS)) {
         Utilities.LoadPlayerInfo(GetType().Name, out PlayerInfo pi2, out int playerIndex2, out p2, otherObj.gameObject, gameState, false);
         if(p2 != null) {
            r2 = Random.Range(1, 6);

            explosionOn = true;
            explosionTime = 0f;
            if (p2 != null && p2.gunExplosion != null) {
               p2.gunExplosion.SetActive(true);
            }

            if (audioGunShot != null) {
               if (audioGunShot.isPlaying == false) {
                  audioGunShot.Play();
               }
            }

            if (r2 == 1 || r2 == 2 || r2 == 4 || r2 == 5) {
               p2.isHit = true;
               p2.isMiss = false;
               p2.isMissTime = 0f;
               p2.PerformGunShotHit();
            } else {
               p2.isHit = false;
               p2.isMiss = true;
               p2.isHitTime = 0f;
            }

            CancelTarget();
         }
      }
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (p == null) {
         return;
      } else if (BaseScript.IsActive(scriptName) == false) {
         return;
      } else {
         if (gameState != null) {
            if (gameState.gamePaused == true) {
               return;
            } else if (gameState.gameRunning == false) {
               return;
            }
         }
      }

      //Process Car Sensor Targets
      if (p.aiPassingMode == 0 && p.aiIsPassing == false) {
         l2 = cars.Count;
         tb = false;
         for (i2 = 0; i2 < l2; i2++) {
            obj = (Collider)cars[i2];
            t1 = obj.gameObject.transform.position;
            t2 = player.transform.position;
            dist = Vector3.Distance(t1, t2);

            //Auto Passing Check
            if (dist <= SAFE_FOLLOW_DIST && p.speed >= (TRIGGER_SPEED_PASSING * p.maxSpeed)) {
               tb = true;
               break;
            }

            if (gameState.gameSettingsSet == 2) {
               //No Gun Play
               continue;
            } 

            //Targetting Check
            if (dist <= GUN_SHOT_DIST && p.gunOn == true && p.ammo > 0 && p.aiHasTarget == false && p.aiIsReloading == false) {
               target = obj;
               p.aiHasTarget = true;
               p.aiHasTargetTime = 0f;

               if (audioTargetting != null) {
                  if (audioTargetting.isPlaying == false) {
                     audioTargetting.Play();
                  }
               }
            } else if (dist <= GUN_SHOT_DIST && p.gunOn == true && p.ammo > 0 && p.aiHasTarget == true && p.aiCanFire == true && p.aiIsReloading == false) {
               p.aiHasTarget = false;
               p.aiHasTargetTime = 0f;
               p.aiCanFire = false;
               p.ammo--;

               if (p.ammo <= 0) {
                  p.ammo = 0;
                  p.HideGun();
               }

               if (audioTargetting != null) {
                  audioTargetting.Stop();
               }

               PerformGunShotAttempt(obj);
               p.aiIsReloading = true;
               p.aiIsReloadingTime = 0f;
            } else if (dist > GUN_SHOT_DIST) {
               if (audioTargetting != null) {
                  audioTargetting.Stop();
               }
               target = null;
               p.aiHasTarget = false;
               p.aiHasTargetTime = 0f;
               p.aiCanFire = false;
            }
         }  //end for loop

         //Auto Passing Start
         if (tb == true) {
            p.aiPassingTime += Time.deltaTime;
            if (p.aiPassingTime > TRIGGER_TIME_PASSING && p.aiPassingMode == 0) {
               p.aiPassingMode = 1;
               p.aiIsPassing = true;
               p.aiPassingTime = 0f;
               moveTime = 0f;
               p.SetBoost();
               p.SetCurrentSpeed();
               SetBoostVectors();
            }
         } else {
            p.aiPassingMode = 0;
            p.aiIsPassing = false;
            p.aiPassingTime = 0f;
            moveTime = 0f;
            p.SetNorm();
            p.SetCurrentSpeed();
         }
      } //main if statement

      //Auto Passing Applied
      if (p.aiIsPassing == true) {
         moveTime += Time.deltaTime * 100;
         if (p.aiPassingMode == 1) {
            p.controller.Move(passLeftV3 * Time.deltaTime);
            if (moveTime >= 50) {
               p.aiPassingMode = 2;
               moveTime = 0;
            }
         } else if (p.aiPassingMode == 2) {
            p.controller.Move(passGoV3 * Time.deltaTime);
            if (moveTime >= 100) {
               p.aiPassingMode = 0;
               p.aiIsPassing = false;
               p.aiPassingTime = 0f;
               p.SetNorm();
               moveTime = 0f;
            }
         }
      }

      //Auto Passing End
      if (p.isJumping == true) {
         p.aiPassingMode = 0;
         p.aiIsPassing = false;
         p.aiPassingTime = 0f;
         p.SetNorm();
         moveTime = 0f;
      }

      //Targetting to Fire
      if (p.aiHasTarget == true && p.aiIsReloading == false) {
         p.aiHasTargetTime += Time.deltaTime * 100;
         if (p.aiHasTargetTime >= MIN_TARGET_TO_FIRE_TIME) {
            p.aiHasTargetTime = 0f;
            p.aiCanFire = true;
         }
      } else if (p.aiIsReloading == true) {
         p.aiIsReloadingTime += Time.deltaTime * 100;
         if (p.aiIsReloadingTime >= GUN_RELOAD_TIME) {
            p.aiIsReloading = false;
            p.aiIsReloadingTime = 0f;
         }
      }

      //Targetting Gun Explosion Effect
      if (explosionOn == true) {
         explosionTime += Time.deltaTime * 100;
      }

      if (explosionOn == true && explosionTime >= MAX_EXPLOSION_TIME) {
         explosionOn = false;
         explosionTime = 0f;
         p.isHit = false;
         p.isMiss = false;
         if (p != null && p.gunExplosion != null) { // && p.gunExplosionParticleSystem != null)
            p.gunExplosion.SetActive(false);
            //p.gunExplosionParticleSystem.emit = false;
         }
      }
   }

   /// <summary>
   /// A trigger event handler that fires when the collision object enters the collider.
   /// </summary>
   /// <param name="otherObj">The collider that registered the trigger event.</param>
   void OnTriggerEnter(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (p != null && otherObj.CompareTag(Utilities.TAG_PLAYERS)) {
         if (cars.Contains(otherObj) == false) {
            cars.Add(otherObj);
         }

         if (cars.Count > 0) {
            p.SetDraftingBonusOn();
         }
      }
   }

   /// <summary>
   /// Candels the current car taget and stops the targetting system.
   /// </summary>
   public void CancelTarget() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (audioTargetting != null) {
         audioTargetting.Stop();
      }

      target = null;
      if (p != null) {
         p.aiHasTarget = false;
         p.aiHasTargetTime = 0f;
         p.aiCanFire = false;
      }
   }

   /// <summary>
   /// A trigger event handler that fires when the collision object exits the collider.
   /// </summary>
   /// <param name="otherObj">The collider that registered the trigger event.</param>
   void OnTriggerExit(Collider otherObj) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (p != null && otherObj.CompareTag(Utilities.TAG_PLAYERS)) {
         if (cars.Contains(otherObj) == true) {
            cars.Remove(otherObj);
         }

         if (cars.Count == 0) {
            p.SetDraftingBonusOff();
         }

         if (target == otherObj) {
            CancelTarget();
         }
      }
   }
}