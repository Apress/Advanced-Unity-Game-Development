using System.Collections;
using UnityEngine;

/// <summary>
/// A class that is used to track the current state of a given player.
/// </summary>
public class PlayerState : BaseScript {
   //***** Static Fields *****
   /// <summary>
   /// A boolean flag that is responsible for showing AI internal logic om the scene while the game runs.
   /// </summary>
   public static bool SHOW_AI_LOGIC = true;

   //***** Constants *****
   /// <summary>
   /// Indicates a speed type of slow.
   /// </summary>
   public static readonly int TYPE_SPEED_SLOW = 0;

   /// <summary>
   /// Indicates a speed type of normal.
   /// </summary>
   public static readonly int TYPE_SPEED_NORM = 1;

   /// <summary>
   /// Indicates a speed type of boost.
   /// </summary>
   public static readonly int TYPE_SPEED_BOOST = 2;

   /// <summary>
   /// Indicates a player type of human.
   /// </summary>
   public static readonly int TYPE_PLAYER_HUMAN = 0;

   /// <summary>
   /// Indicates a player type of computer.
   /// </summary>
   public static readonly int TYPE_PLAYER_COMPUTER = 1;

   /// <summary>
   /// Indicates a car of type green.
   /// </summary>
   public static readonly int TYPE_CAR_HOVER_GREEN = 0;

   /// <summary>
   /// Indicates a car of type black.
   /// </summary>
   public static readonly int TYPE_CAR_HOVER_BLACK = 3;

   /// <summary>
   /// Indicates a car of type red.
   /// </summary>
   public static readonly int TYPE_CAR_HOVER_RED = 4;

   /// <summary>
   /// Indicates a car of type purple.
   /// </summary>
   public static readonly int TYPE_CAR_HOVER_PURPLE = 5;

   /// <summary>
   /// The default value of the car's position.
   /// </summary>
   public static readonly int DEFAULT_POSITION = 0;

   /// <summary>
   /// The default value of the car's max speed.
   /// </summary>
   public static readonly float DEFAULT_MAX_SPEED = 200.0f;

   /// <summary>
   /// The default gravity of the car.
   /// </summary>
   public static readonly float DEFAULT_GRAVITY = 11.0f;

   /// <summary>
   /// The maximum speed limit of the car.
   /// </summary>
   public static float LIMIT_MAX_SPEED = 300.0f;

   /// <summary>
   /// The maximum miss notification display time in seconds.
   /// </summary>
   public readonly int MAX_IS_MISS_TIME = 2;

   /// <summary>
   /// The maximum notification display shot time in seconds.
   /// </summary>
   public readonly int MAX_IS_SHOT_TIME = 2;

   /// <summary>
   /// The maximum notification display hit time in seconds.
   /// </summary>
   public readonly int MAX_IS_HIT_TIME = 2;

   /// <summary>
   /// The maximum notification display lap complete time in seconds.
   /// </summary>
   public readonly int MAX_IS_LAP_COMPLETE_TIME = 6;

   /// <summary>
   /// The default amount of life a player has.
   /// </summary>
   public static readonly int DEFAULT_LIFE_AMOUNT = 3;

   /// <summary>
   /// The maximum invincibility time in seconds.
   /// </summary>
   public readonly int INVINC_SECONDS = 10;

   /// <summary>
   /// The default amount of damage a player has.
   /// </summary>
   public static readonly int DEFAULT_DAMAGE_AMOUNT = 0;

   /// <summary>
   /// A distance marker used in determining if the current player's car is stuck.
   /// </summary>
   public readonly float MIN_STUCK_DISTANCE = 30.0f;

   /// <summary>
   /// The minimum amount of time that has to pass before a player's car is marked as stuck.
   /// </summary>
   public readonly float MIN_STUCK_TIME = 3.0f;

   /// <summary>
   /// The minimum distance necessary to switch to a waypoint.
   /// </summary>
   public readonly float MIN_WAYPOINT_DISTANCE = 30.0f;

   /// <summary>
   /// The maximum speed bonus received from drafting another car.
   /// </summary>
   public readonly int MAX_SPEED_BONUS_DRAFTING = 4;

   /// <summary>
   /// The maximum speed bonus received from passing another car.
   /// </summary>
   public readonly int MAX_SPEED_BONUS_PASSING = 20;

   /// <summary>
   /// The maximum amount of time in milliseconds that the car's gun will smoke.
   /// </summary>
   public readonly float MAX_SMOKE_TIME = 1000.0f;

   /// <summary>
   /// The name of the gun shot audio clip.
   /// </summary>
   public static readonly string AUDIO_CLIP_GUN_SHOT = "explosion_short_blip_rnd_01";

   /// <summary>
   /// The name of the car sound audio clip.
   /// </summary>
   public static readonly string AUDIO_CLIP_CAR_SOUND1 = "CarAirNonGapless";

   /// <summary>
   /// The name of the car's idle engine sound audio clip.
   /// </summary>
   public static readonly string AUDIO_CLIP_CAR_SOUND2 = "car_idle_lp_01";

   /// <summary>
   /// A timing field, used to determine that the maximum gained life time has passed, used in notification display timing.
   /// </summary>
   public readonly int MAX_GAINED_LIFE_TIME = 2;

   //***** Class Fields *****
   /// <summary>
   /// The index of this player in the array of available players.
   /// </summary>
   public int index = 0;

   /// <summary>
   /// The GameoBject that represents this player.
   /// </summary>
   public GameObject player;

   /// <summary>
   /// A boolean flag indicating if the current player is active or not.
   /// </summary>
   public bool active = true;

   /// <summary>
   /// The minimum amount of time that a player's car has to be off track before the off track indicator is shown.
   /// </summary>
   public float offTrackSeconds = 6.0f;

   /// <summary>
   /// The minimum amount of time that a player's car has to facing in the wrong direction before the wrong direction indicator is shown.
   /// </summary>
   public float wrongDirectionSeconds = 6.0f;

   //***** Input Class Fields *****
   /// <summary>
   /// A character controller for the current player.
   /// </summary>
   public CharacterController controller;

   /// <summary>
   /// A character motor for the current player.
   /// </summary>
   public CharacterMotor cm;

   /// <summary>
   /// Mouse input handler for the current player.
   /// </summary>
   public MouseLookNew mouseInput;

   /// <summary>
   /// FPS input handler for the current player. Handles gamepad input.
   /// </summary>
   public FPSInputController fpsInput;

   /// <summary>
   /// The transform that indicates the home for the current player.
   /// </summary>
   public Transform home;

   /// <summary>
   /// A boolean indicating if the home Transform has been set.
   /// </summary>
   private bool hasSetHome = false;

   /// <summary>
   /// A boolean indicating if the current player has been paused.
   /// </summary>
   public bool pause = false;

   //***** Car Descriptor Class Fields *****
   /// <summary>
   /// The current speed type of the current player's car.
   /// </summary>
   public int speedType = TYPE_SPEED_SLOW;             //0 = slow, 1 = norm, 2 = boost

   /// <summary>
   /// The current player type of the current player's car.
   /// </summary>
   public int playerType = TYPE_PLAYER_HUMAN;          //0 = human, 1 = computer

   /// <summary>
   /// The current car type of the current player's car.
   /// </summary>
   public int carType = TYPE_CAR_HOVER_GREEN;          //0 = green hover, 1 = red hover, 2 = black hover, 3 = black hover, 4 = red hover, 5 = purple hover

   //***** Speed Class Fields *****
   /// <summary>
   /// The speed of the current player's car.
   /// </summary>
   public float speed = 0.0f;

   /// <summary>
   /// The percentage of the maximum speed based on the car's current speed.
   /// </summary>
   public float speedPrct = 0.0f;

   /// <summary>
   /// The limit of the current speed as a percentage of the maximum speed.
   /// </summary>
   public float speedPrctLimit = 0.0f;

   /// <summary>
   /// The maximum speed the car can attain naturally.
   /// </summary>
   public float maxSpeed = DEFAULT_MAX_SPEED;

   /// <summary>
   /// The current position of the car in the race.
   /// </summary>
   public int position = DEFAULT_POSITION;

   /// <summary>
   /// The current gravity of the car.
   /// </summary>
   public float gravity = DEFAULT_GRAVITY;

   //***** Time Class Fields *****
   /// <summary>
   /// The current track time in a string representation.
   /// </summary>
   public string time;

   /// <summary>
   /// An large integer representation of the lap time.
   /// </summary>
   public int timeNum;

   /// <summary>
   /// The total lap time in milliseconds.
   /// </summary>
   public float totalTime = 0;

   /// <summary>
   /// The number of hours in the current track time.
   /// </summary>
   public float hour = 0;

   /// <summary>
   /// The number of minutes in the current track time.
   /// </summary>
   public float min = 0;

   /// <summary>
   /// The number of seconds in the current track time.
   /// </summary>
   public float s = 0;

   /// <summary>
   /// The number of milliseconds in the current track time.
   /// </summary>
   public float ms = 0;

   //***** Off Track Class Fields *****
   /// <summary>
   /// A boolean flag that indicates if the current player's car is off the track.
   /// </summary>
   public bool offTrack = false;

   /// <summary>
   /// The number of milliseconds that the current player's car is off track.
   /// </summary>
   public float offTrackTime = 0.0f;

   //***** Wrong Direction Class Fields *****
   /// <summary>
   /// A boolean flag that indicates if the current player's car is going in the wrong direction.
   /// </summary>
   public bool wrongDirection = false;

   /// <summary>
   /// The number of milliseconds that the current player's car has been going in the wrong direction.
   /// </summary>
   public float wrongDirectionTime = 0.0f;

   //***** Skipped Waypoint Class Fields *****
   /// <summary>
   /// A boolean flag indicating if the current player has skipped a waypoint.
   /// </summary>
   public bool skippedWaypoint = false;

   /// <summary>
   /// The number of milliseconds that the current player has skipped a waypoint.
   /// </summary>
   public float skippedWaypointTime = 0.0f;

   //***** Cameras and Objects Class Fields *****
   /// <summary>
   /// A GameObject that represents the car's gun.
   /// </summary>
   public GameObject gun;

   /// <summary>
   /// A GameObject that represents the gun's base.
   /// </summary>
   public GameObject gunBase;

   /// <summary>
   /// The main game camera that is positioned in the cockpit of the player's car.
   /// </summary>
   public new Camera camera;

   /// <summary>
   /// The read view camera of the current player's car.
   /// </summary>
   public Camera rearCamera;

   /// <summary>
   /// The current player's car.
   /// </summary>
   public GameObject car;

   /// <summary>
   /// The current car's sensor object.
   /// </summary>
   public GameObject carSensor;

   //***** Car Status Class Fields *****
   /// <summary>
   /// An integer value that tracks how much ammo the current player has.
   /// </summary>
   public int ammo = 0;

   /// <summary>
   /// A boolean indicating if the current player has their gun activated.
   /// </summary>
   public bool gunOn = false;

   /// <summary>
   /// A boolean flag that indicates that the current player's car is bouncing.
   /// </summary>
   public bool isBouncing = false;

   /// <summary>
   /// A boolean flag that indicates that the current player's car is jumping.
   /// </summary>
   public bool isJumping = false;

   /// <summary>
   /// A boolean flag that indicates that the current player's car is drafting.
   /// </summary>
   public bool isDrafting = false;

   /// <summary>
   /// A boolean flag that indicates that you have just been shot.
   /// </summary>
   public bool isShot = false;                     //true when you've been shot, resets with hit sound

   /// <summary>
   /// A float value that tracks how much time the is shot notification is visible.
   /// </summary>
   public float isShotTime = 0.0f;

   /// <summary>
   /// A boolean flag that indicates that your shot has just hit.
   /// </summary>
   public bool isHit = false;                      //true when your shot has hit, resets with reload

   /// <summary>
   /// A float value that tracks how much time the is hit notification is visible.
   /// </summary>
   public float isHitTime = 0.0f;

   /// <summary>
   /// A boolean flag that indicates your shot has missed its target.
   /// </summary>
   public bool isMiss = false;                     //true when your shot has missed, resets with reload

   /// <summary>
   /// A float value that tracks how much time the is miss notification is visible.
   /// </summary>
   public float isMissTime = 0.0f;

   /// <summary>
   /// A boolean flag that indicates that a lap has completed.
   /// </summary>
   public bool lapComplete = false;

   /// <summary>
   /// A float vlaue that tracks how much time it took to complete the lap.
   /// </summary>
   public float lapCompleteTime = 0.0f;

   /// <summary>
   /// A boolean flag that indicates if the current player's car has the armor mod.
   /// </summary>
   public bool armorOn = false;

   /// <summary>
   /// A boolean flag that indicates if the current player's car has the boost mod.
   /// </summary>
   public bool boostOn = false;

   /// <summary>
   /// A boolean flag that indicates if the current player's car has the invincibility mod.
   /// </summary>
   public bool invincOn = false;

   /// <summary>
   /// A float value that tracks how much time the invincibility mod has been on.
   /// </summary>
   public float invincTime = 0.0f;

   /// <summary>
   /// The current player's life total.
   /// </summary>
   public int lifeTotal = DEFAULT_LIFE_AMOUNT;

   /// <summary>
   /// The current amount of damage the current player has been dealt.
   /// </summary>
   public int damage = DEFAULT_DAMAGE_AMOUNT;

   /// <summary>
   /// The number of points the player has accrued.
   /// </summary>
   public int points = 0;

   /// <summary>
   /// A boolean flag indicating if the current player is active or not.
   /// </summary>
   public bool alive = true;

   //***** Speed Class Fields *****
   /// <summary>
   /// The maximum foward speed of the current player's car if the slow mod is on.
   /// </summary>
   public int maxForwardSpeedSlow = 50;

   /// <summary>
   /// The maximum sideways speed of the current player's car if the slow mod is on.
   /// </summary>
   public int maxSidewaysSpeedSlow = 12;

   /// <summary>
   /// The maximum backwards speed of the current player's car if the slow mod is on.
   /// </summary>
   public int maxBackwardsSpeedSlow = 5;

   /// <summary>
   /// The maximum ground acceleration of the current player's car if the slow mod is on.
   /// </summary>
   public int maxGroundAccelerationSlow = 25;

   /// <summary>
   /// The maximum forward speed if no modifiers are on.
   /// </summary>
   public int maxForwardSpeedNorm = 200;

   /// <summary>
   /// The maximum sideways speed if no modifiers are on.
   /// </summary>
   public int maxSidewaysSpeedNorm = 50;

   /// <summary>
   /// The maximum backwards speed if no modifiers are on.
   /// </summary>
   public int maxBackwardsSpeedNorm = 20;

   /// <summary>
   /// The maximum ground acceleration when no modifiers are on.
   /// </summary>
   public int maxGroundAccelerationNorm = 100;

   /// <summary>
   /// The maximum forward speed when the boost modifier is on.
   /// </summary>
   public int maxForwardSpeedBoost = 250;

   /// <summary>
   /// The maximum sideways speed when the boost modifier is on.
   /// </summary>
   public int maxSidewaysSpeedBoost = 60;

   /// <summary>
   /// The maximum backwards speed when the boost modifier is on.
   /// </summary>
   public int maxBackwardsSpeedBoost = 30;

   /// <summary>
   /// The maximum ground acceleration when the boost modifier is on.
   /// </summary>
   public int maxGroundAccelerationBoost = 120;

   //***** Waypoint Class Fields *****
   /// <summary>
   /// An array of waypoints used by the enemy car AI.
   /// </summary>
   public ArrayList waypoints = null;

   /// <summary>
   /// A float value that indicates how far away the next waypoint is.
   /// </summary>
   public float waypointDistance = 0.0f;

   /// <summary>
   /// A float value that indicates how far away the previous waypoint is.
   /// </summary>
   public float waypointDistancePrev = 0.0f;

   //***** Internal variables *****
   private WaypointCheck wc = null;
   private Vector3 wcV = Vector3.zero;
   private WaypointCheck wc1 = null;
   private Vector3 wcV1 = Vector3.zero;
   private Quaternion tr;
   private WaypointCheck wc2 = null;

   private Vector3 wcV2 = Vector3.zero;
   private WaypointCheck wc5 = null;
   private Vector3 wcV5 = Vector3.zero;
   private WaypointCheck fpsWc;
   private float fpsTmp1;
   private float fpsTmp2;

   private float fpsTmp;
   private Vector3 fpsV = Vector3.zero;
   private AudioSource[] audioSetLa = null;
   private int lLa = 0;
   private int iLa = 0;
   private AudioSource aSLa = null;

   private Vector3 wcVpla;
   private int sidesUfp = 0;
   private int aboveUfp = 0;
   private bool collSidesUfp = false;
   private bool collAboveUfp = false;

   private float umlA = 0.0f;
   private Vector3 umlForward = Vector3.zero;
   private int umlTmpIdx;
   private float umlAngle;

   //***** AI Class Fields *****
   public bool aiOn = false;
   public int aiWaypointTime = 0;
   public int aiWaypointLastIndex = -1;
   public int aiWaypointIndex = 0;             //0 = first node
   public int aiWaypointRoute = 0;
   public float aiTurnStrength = 1.0f;         //1.2f;

   public float aiSpeedStrength = 1.0f;
   public float aiStrafeStrength = 0.0f;
   public float aiSlide = 0.0f;
   public int aiIsStuckMode = 0;               //0 = looking, 1 = testing, 2 = acting
   public bool aiIsStuck = false;
   public float aiWaypointDistance = 0f;

   public Vector3 aiRelativePoint = Vector3.zero;
   public float aiTime1 = 0.0f;
   public float aiTime2 = 0.0f;
   public float aiSlowDownTime = 0.0f;
   public float aiSlowDown = 0.0f;
   public bool aiSlowDownOn = false;

   public float aiSlowDownDuration = 100.0f;
   public bool aiIsPassing = false;
   public float aiPassingTime = 0.0f;
   public int aiPassingMode = 0;
   public bool aiHasTarget = false;
   public float aiHasTargetTime = 0.0f;

   public bool aiIsReloading = false;
   public float aiIsReloadingTime = 0.0f;
   public bool aiHasGainedLife = false;
   public float aiHasGainedLifeTime = 0.0f;
   public bool aiIsLargeTurn = false;
   public float aiIsLargeTurnSpeed = 0.0f;

   public float aiLastLookAngle = 0.0f;
   public float aiNextLookAngle = 0.0f;
   public float aiNext2LookAngle = 0.0f;
   public float aiMidLookAngle = 0.0f;
   public float aiMid2LookAngle = 0.0f;
   public bool aiCanFire = false;

   public float aiBoostTime = 0.0f;
   public int aiBoostMode = 0;
   public int aiWaypointJumpCount = 0;
   public int aiWaypointPassCount = 0;

   //***** Other Class Fields *****
   public GameObject gunExplosion = null;
   public GameObject gunHitSmoke = null;
   public bool gunSmokeOn = false;
   public float gunSmokeTime = 0.0f;
   //public ParticleEmitter gunExplosionParticleSystem = null;
   //public ParticleEmitter gunHitSmokeParticleSystem = null;

   public GameObject flame = null;
   public int totalLaps = 3;
   public int currentLap = 0;
   public bool prepped = false;
   public GameObject lightHeadLight = null;

   public AudioListener audioListener = null;
   public AudioSource audioGunHit = null;
   public AudioSource audioCarSound1 = null;
   public AudioSource audioCarSound2 = null;

   /// <summary>
   /// Use this for initialization.
   /// </summary>
   void Start() {
      base.Prep(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      }
   }

   /// <summary>
   /// A method that loads the sound effects used by the car.
   /// </summary>
   public void LoadAudio() {
      audioSetLa = player.GetComponents<AudioSource>();
      if (audioSetLa != null) {
         lLa = audioSetLa.Length;
         for (iLa = 0; iLa < lLa; iLa++) {
            aSLa = (AudioSource)audioSetLa[iLa];
            if (aSLa != null) {
               if (aSLa.clip.name == AUDIO_CLIP_GUN_SHOT) {
                  audioGunHit = aSLa;
               } else if (aSLa.clip.name == AUDIO_CLIP_CAR_SOUND1) {
                  audioCarSound1 = aSLa;
               } else if (aSLa.clip.name == AUDIO_CLIP_CAR_SOUND2) {
                  audioCarSound2 = aSLa;
               }
            }
         }
      }
   }

   /// <summary>
   /// Pause the car's sound effects.
   /// </summary>
   public void PauseSound() {
      if (audioCarSound1 != null) {
         audioCarSound1.Stop();
      }

      if (audioCarSound2 != null) {
         audioCarSound2.Stop();
      }
   }

   /// <summary>
   /// Unpause the car's sound effects.
   /// </summary>
   public void UnPauseSound() {
      if (audioCarSound1 != null) {
         audioCarSound1.Play();
      }

      if (audioCarSound2 != null) {
         audioCarSound2.Play();
      }
   }

   /// <summary>
   /// Checks to see if the specified index is a valid waypoint index in the current set of waypoints.
   /// </summary>
   /// <param name="index">The index value to check.</param>
   /// <returns>A boolean flag indicating if the waypoint index if valid.</returns>
   public bool IsValidWaypointIndex(int index) {
      if (waypoints == null) {
         waypoints = gameState.GetWaypoints(aiWaypointRoute);
      }

      if (waypoints != null && index >= 0 && index <= (waypoints.Count - 1)) {
         return true;
      } else {
         return false;
      }
   }

   /// <summary>
   /// Stamps the current waypoint
   /// </summary>
   public void StampWaypointTime() {
      aiWaypointTime = timeNum;
   }

   /// <summary>
   /// Performs a hit on the current player's car.
   /// </summary>
   /// <returns>A boolean flag indicating if the shot was a hit.</returns>
   public bool PerformGunShotHit() {
      if (armorOn == true) {
         armorOn = false;
         isShot = true;
         gunSmokeOn = true;
         gunSmokeTime = 0.0f;
         gunHitSmoke.SetActive(true);
         //gunHitSmokeParticleSystem.Emit();
         return true;
      } else {
         if (invincOn == true) {
            return false;
         } else {
            damage++;
            isShot = true;
            gunSmokeOn = true;
            gunSmokeTime = 0.0f;
            gunHitSmoke.SetActive(true);
            //gunHitSmokeParticleSystem.Emit();
            PlayGunShotHitAudio();

            if (GetLife() <= 0) {
               aiWaypointIndex = GetPastWaypointIndex(aiWaypointIndex);
               damage = 0;
               if (aiWaypointIndex >= 0 && aiWaypointIndex < waypoints.Count) {
                  MoveToCurrentWaypoint();
               }
            }
            return true;
         }
      }
   }

   /// <summary>
   /// Plays the gun hit sound effect.
   /// </summary>
   public void PlayGunShotHitAudio() {
      if (audioGunHit != null) {
         if (audioGunHit.isPlaying == false) {
            audioGunHit.Play();
         }
      }
   }

   /// <summary>
   /// A method that drives the AI FPS control of the car.
   /// </summary>
   /// <returns></returns>
   public Vector3 UpdateAiFpsController() {
      if (player == null || prepped == false || cm == null) {
         return Vector3.zero;
      }

      if (waypoints == null) {
         waypoints = gameState.GetWaypoints(aiWaypointRoute);
      }

      if (waypoints == null) {
         return Vector3.zero;
      }

      //calculate strafe strength
      aiStrafeStrength = 0.0f;
      aiSpeedStrength = 1.0f;

      if (waypoints != null) {
         fpsWc = (WaypointCheck)waypoints[aiWaypointIndex];
         aiRelativePoint = player.transform.InverseTransformPoint(fpsWc.transform.position);

         if (aiRelativePoint.x <= -30.0f) {
            aiStrafeStrength = -0.30f;
         } else if (aiRelativePoint.x >= 30.0f) {
            aiStrafeStrength = 0.30f;
         } else if (aiRelativePoint.x <= -20.0f) {
            aiStrafeStrength = -0.20f;
         } else if (aiRelativePoint.x >= 20.0f) {
            aiStrafeStrength = 0.20f;
         } else if (aiRelativePoint.x <= -15.0f) {
            aiStrafeStrength = -0.15f;
         } else if (aiRelativePoint.x >= 15.0f) {
            aiStrafeStrength = 0.15f;
         } else if (aiRelativePoint.x <= -10.0f) {
            aiStrafeStrength = -0.10f;
         } else if (aiRelativePoint.x >= 10.0f) {
            aiStrafeStrength = 0.10f;
         } else if (aiRelativePoint.x <= -5.0f) {
            aiStrafeStrength = -0.05f;
         } else if (aiRelativePoint.x >= 5.0f) {
            aiStrafeStrength = 0.05f;
         } else if (aiRelativePoint.x <= -1.0f) {
            aiStrafeStrength = -0.01f;
         } else if (aiRelativePoint.x >= 1.0f) {
            aiStrafeStrength = 0.01f;
         }
      }

      //calculate side, above, collisions
      sidesUfp = (int)(cm.movement.collisionFlags & CollisionFlags.Sides);
      aboveUfp = (int)(cm.movement.collisionFlags & CollisionFlags.Above);

      if (sidesUfp == 0) {
         collSidesUfp = false;
      } else {
         collSidesUfp = true;
      }

      if (aboveUfp == 0) {
         collAboveUfp = false;
      } else {
         collAboveUfp = true;
      }

      //calculate is stuck data
      if (aiTime2 > 1 && cm.movement.collisionFlags == CollisionFlags.None) {
         aiTime2 = 0;
         aiIsStuckMode = 0;
         aiTime1 = 0;
         aiIsStuck = false;
      } else if (aiTime2 > 1 && Mathf.Abs(waypointDistance - aiWaypointDistance) > MIN_STUCK_DISTANCE && !(collAboveUfp || collSidesUfp)) {
         aiTime2 = 0;
         aiIsStuckMode = 0;
         aiTime1 = 0;
         aiIsStuck = false;
      } else if (collAboveUfp || collSidesUfp) {
         aiTime2 = 0;
         aiIsStuckMode = 1;
         aiWaypointDistance = waypointDistance;
         aiIsStuck = true;
      }

      //test and apply is stuck data
      if (aiIsStuckMode == 1 && aiTime1 >= MIN_STUCK_TIME && cm.movement.velocity.magnitude <= 30 && Mathf.Abs(waypointDistance - aiWaypointDistance) <= MIN_STUCK_DISTANCE) {
         aiIsStuckMode = 2;
         aiTime2 = 0f;
         aiTime1 = 0f;
         aiIsStuck = true;
      } else if (aiIsStuckMode == 1 && aiTime1 > MIN_STUCK_TIME) {
         aiIsStuckMode = 0;
         aiTime2 = 0f;
         aiTime1 = 0f;
         aiIsStuck = false;
      }

      //process aiIsStuckMode
      if (aiIsStuckMode == 1) {
         aiTime1 += Time.deltaTime;
      } else if (aiIsStuckMode == 2) {
         if (waypoints != null && waypoints.Count > 0) {
            //move car to waypoint center
            aiWaypointIndex = GetPastWaypointIndex(aiWaypointIndex);
            if (!(aiWaypointIndex >= 0 && aiWaypointIndex < waypoints.Count)) {
               fpsV = new Vector3(0, 0, 0);
               return fpsV;
            }
            MoveToCurrentWaypoint();
            aiIsStuckMode = 0;
            aiIsStuck = false;
            aiTime2 = 0f;
            aiTime1 = 0f;
            aiStrafeStrength = 0f;
         }

         fpsV = new Vector3(0, 0, 0);
         return fpsV;
      }

      if (aiIsStuckMode != 0) {
         aiTime2 += Time.deltaTime;
      }

      //apply waypoint slow down
      if ((aiSlowDownOn == true && aiSlowDown < 1.0f && speedPrct > 0.3f) || (aiSlowDown >= 1.0f)) {
         aiSlowDownTime += (Time.deltaTime * 100);
         aiSpeedStrength = aiSlowDown;
         if (aiSlowDownTime > aiSlowDownDuration) {
            aiSlowDownOn = false;
            aiSlowDownTime = 0.0f;
         }
      }

      //handle large turn
      if (aiIsLargeTurn == true) {
         if (aiSpeedStrength > aiIsLargeTurnSpeed) {
            aiSpeedStrength = aiIsLargeTurnSpeed;
         }
      }

      fpsV = new Vector3(aiStrafeStrength, 0, aiSpeedStrength);
      return fpsV;
   }

   /// <summary>
   /// Gets the current waypoint check.
   /// </summary>
   /// <returns></returns>
   public WaypointCheck GetCurrentWaypointCheck() {
      if (waypoints != null) {
         return (WaypointCheck)waypoints[aiWaypointIndex];
      } else {
         return null;
      }
   }

   /// <summary>
   /// Performs a look at the specified waypoint check object.
   /// </summary>
   /// <param name="wc"></param>
   public void PerformLookAt(WaypointCheck wc) {
      wcVpla = wc.transform.position;
      wcVpla.y = player.transform.position.y;
      player.transform.LookAt(wcVpla);
   }

   /// <summary>
   /// Moves the current player's car to the current waypoint.
   /// </summary>
   public void MoveToCurrentWaypoint() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      pause = true;
      WaypointCheck wc = (WaypointCheck)waypoints[aiWaypointIndex];
      Vector3 wcV = wc.transform.position;
      wcV.y = wc.waypointStartY;

      cm.movement.velocity = Vector3.zero;
      player.transform.position = wcV;
      isDrafting = false;
      isJumping = false;
      isBouncing = false;
      SetNorm();
      ShowInvinc();

      if (aiWaypointIndex + 1 >= 0 && aiWaypointIndex + 1 < waypoints.Count) {
         wc = (WaypointCheck)waypoints[aiWaypointIndex + 1];
      }
      aiWaypointJumpCount++;
      PerformLookAt(wc);
      pause = false;
   }

   /// <summary>
   /// Moves the current player's car to the specified waypoint.
   /// </summary>
   /// <param name="index">The index of the waypoint to move the current player's car to.</param>
   public void MoveToWaypoint(int index) {
      aiWaypointIndex = index;
      MoveToCurrentWaypoint();
   }

   /// <summary>
   /// A method that drives the AI mouse look control of the car.
   /// </summary>
   public void UpdateAiMouseLook() {
      if (BaseScript.IsActive(scriptName) == false) { 
         return; 
      }

      if (waypoints == null) { 
         waypoints = gameState.GetWaypoints(aiWaypointRoute); 
      }

      if (waypoints == null || player == null || prepped == false || !(aiWaypointIndex >= 0 && aiWaypointIndex < waypoints.Count)) { 
         return; 
      }

      wc1 = (WaypointCheck)waypoints[aiWaypointIndex];
      if (SHOW_AI_LOGIC) { 
         Debug.DrawRay(player.transform.position, (wc1.transform.position - player.transform.position), Color.green); 
      }
      
      umlA = 0.0f;
      umlForward = (player.transform.TransformDirection(Vector3.forward) * 20);
      
      if (SHOW_AI_LOGIC) { 
         Debug.DrawRay(player.transform.position, umlForward, Color.magenta); 
      }

      if (waypointDistance >= MIN_WAYPOINT_DISTANCE) {
         wcV1 = wc1.transform.position;
         wcV1.y = player.transform.position.y;
         umlA = Vector3.Angle(umlForward, (wcV1 - player.transform.position));
         aiLastLookAngle = umlA;

         umlTmpIdx = 0;
         if (aiWaypointIndex + 1 >= 0 && aiWaypointIndex + 1 < waypoints.Count) {
            umlTmpIdx = (aiWaypointIndex + 1);
         } else {
            umlTmpIdx = 0;
         }

         wc2 = (WaypointCheck)waypoints[umlTmpIdx];
         wcV2 = wc2.transform.position;
         wcV2.y = player.transform.position.y;
         umlA = Vector3.Angle(umlForward, (wcV2 - player.transform.position));
         aiNextLookAngle = umlA;

         if (SHOW_AI_LOGIC) { 
            Debug.DrawRay(player.transform.position, (wc2.transform.position - player.transform.position), Color.green); 
         }

         umlTmpIdx = 0;
         if (aiWaypointIndex + 2 >= 0 && aiWaypointIndex + 2 < waypoints.Count) {
            umlTmpIdx = (aiWaypointIndex + 2);
         } else {
            umlTmpIdx = 0;
         }

         wc5 = (WaypointCheck)waypoints[umlTmpIdx];
         wcV5 = wc5.transform.position;
         wcV5.y = player.transform.position.y;
         umlA = Vector3.Angle(umlForward, (wcV5 - player.transform.position));
         aiNext2LookAngle = umlA;

         if (SHOW_AI_LOGIC) { 
            Debug.DrawRay(player.transform.position, (wc5.transform.position - player.transform.position), Color.green); 
         }

         if (speedPrct > 0.2f) {
            umlAngle = Mathf.Abs(aiNextLookAngle);
         
            if (umlAngle > 80) {
               aiIsLargeTurn = true;
               aiIsLargeTurnSpeed = 0.65f;
            
            } else if (umlAngle >= 65 && umlAngle <= 80) {
               aiIsLargeTurn = true;
            
               if (speedPrct >= 0.95f) {
                  aiIsLargeTurnSpeed = 0.05f;
               } else if (speedPrct >= 0.85f) {
                  aiIsLargeTurnSpeed = 0.10f;
               } else {
                  aiIsLargeTurnSpeed = 0.15f;
               }

            } else if (umlAngle >= 60) {
               aiIsLargeTurn = true;
               
               if (speedPrct >= 0.95f) {
                  aiIsLargeTurnSpeed = 0.10f;
               } else if (speedPrct >= 0.85f) {
                  aiIsLargeTurnSpeed = 0.15f;
               } else {
                  aiIsLargeTurnSpeed = 0.25f;
               }

            } else if (umlAngle >= 45) {
               aiIsLargeTurn = true;
               
               if (speedPrct >= 0.95f) {
                  aiIsLargeTurnSpeed = 0.20f;
               } else if (speedPrct >= 0.85f) {
                  aiIsLargeTurnSpeed = 0.25f;
               } else {
                  aiIsLargeTurnSpeed = 0.35f;
               }

            } else if (umlAngle >= 30) {
               aiIsLargeTurn = true;
               
               if (speedPrct >= 0.95f) {
                  aiIsLargeTurnSpeed = 0.40f;
               } else if (speedPrct >= 0.85f) {
                  aiIsLargeTurnSpeed = 0.45f;
               } else {
                  aiIsLargeTurnSpeed = 0.55f;
               }

            } else if (umlAngle >= 15) {
               aiIsLargeTurn = true;
               
               if (speedPrct >= 0.95f) {
                  aiIsLargeTurnSpeed = 0.60f;
               } else if (speedPrct >= 0.85f) {
                  aiIsLargeTurnSpeed = 0.65f;
               } else {
                  aiIsLargeTurnSpeed = 0.75f;
               }

            } else {
               aiIsLargeTurn = false;
            }
         } else {
            aiIsLargeTurn = false;
         }

         tr = Quaternion.LookRotation(wcV1 - player.transform.position);
         player.transform.rotation = Quaternion.Slerp(player.transform.rotation, tr, Time.deltaTime * 5.0f);
      } else {
         aiLastLookAngle = 0.0f;
         aiNextLookAngle = 0.0f;
         aiMidLookAngle = 0.0f;
      }
   }

   /// <summary>
   /// Resets the current player's total lap time.
   /// </summary>
   public void ResetTime() {
      totalTime = 0f;
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   public void Update() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (prepped == false || cm == null) {
         return;
      } else if (hasSetHome == false && player != null) {
         home = player.transform;
         hasSetHome = true;
      }

      //speed calculations
      speed = cm.movement.velocity.magnitude;
      if (boostOn == true || aiIsPassing == true) {
         speed = LIMIT_MAX_SPEED;
      }
      speedPrct = (speed / maxSpeed);
      speedPrctLimit = (speed / LIMIT_MAX_SPEED);

      position = gameState.GetPosition(index, position);

      //timing values
      totalTime += Time.deltaTime;
      ms = Mathf.RoundToInt((totalTime % 1) * 1000);
      s = Mathf.RoundToInt(Mathf.Floor(totalTime));
      min = Mathf.RoundToInt(Mathf.Floor((s * 1f) / 60f));
      s -= (min * 60f);
      hour = Mathf.RoundToInt(Mathf.Floor((min * 1f) / 60f));
      min -= (hour * 60f);
      time = string.Format("{0:00}:{1:00}:{2:000}", min, s, ms);
      timeNum = int.Parse(string.Format("{0:00}{1:00}{2:00}{3:000}", hour, min, s, ms));

      //waypoint distance calculations
      if (waypoints != null && waypoints.Count > 0) {
         wc = (WaypointCheck)waypoints[aiWaypointIndex];
         if (wc != null) {
            wcV = wc.transform.position;
            wcV.y = player.transform.position.y;
            waypointDistancePrev = waypointDistance;
            waypointDistance = Vector3.Distance(wcV, player.transform.position);
         }
      }

      //invincibility modifier
      if (invincOn == true) {
         invincTime += Time.deltaTime;
      } else {
         invincTime = 0f;
      }

      if (invincOn == true && invincTime >= INVINC_SECONDS) {
         invincOn = false;
      }

      //has gained life
      if (aiHasGainedLife == true) {
         aiHasGainedLifeTime += Time.deltaTime;
      } else {
         aiHasGainedLifeTime = 0f;
      }

      if (aiHasGainedLife == true && aiHasGainedLifeTime >= MAX_GAINED_LIFE_TIME) {
         aiHasGainedLife = false;
      }

      //gun smoke effect
      if (gunSmokeOn == true) {
         gunSmokeTime += Time.deltaTime * 100f;
      } else {
         gunSmokeTime = 0f;
      }

      if (gunSmokeOn == true && gunSmokeTime >= MAX_SMOKE_TIME) {
         gunSmokeOn = false;
         gunSmokeTime = 0f;
         gunHitSmoke.SetActive(false);
         //gunHitSmokeParticleSystem.emit = false;
      }

      //is shot time
      if (isShot == true) {
         isShotTime += Time.deltaTime;
      } else {
         isShotTime = 0f;
      }

      if (isShot == true && isShotTime >= MAX_IS_SHOT_TIME) {
         isShot = false;
      }

      //is hit time
      if (isHit == true) {
         isHitTime += Time.deltaTime;
      } else {
         isHitTime = 0f;
      }

      if (isHit == true && isHitTime >= MAX_IS_SHOT_TIME) {
         isHit = false;
      }

      //is miss time
      if (isMiss == true) {
         isMissTime += Time.deltaTime;
      } else {
         isMissTime = 0f;
      }

      if (isMiss == true && isMissTime >= MAX_IS_SHOT_TIME) {
         isMiss = false;
      }

      //lap complete time
      if (lapComplete == true) {
         lapCompleteTime += Time.deltaTime;
      } else {
         lapCompleteTime = 0f;
      }

      if (lapComplete == true && lapCompleteTime >= MAX_IS_LAP_COMPLETE_TIME) {
         lapComplete = false;
      }

      //off track checks
      if (offTrack == true) {
         offTrackTime += Time.deltaTime;
      } else {
         offTrackTime = 0f;
      }

      if (offTrack == true && offTrackTime >= offTrackSeconds) {
         if (waypoints != null && waypoints.Count > 0) {
            //move car to waypoint center
            aiWaypointIndex = GetPastWaypointIndex(aiWaypointIndex);
            if (aiWaypointIndex >= 0 && aiWaypointIndex < waypoints.Count) {
               MoveToCurrentWaypoint();
            }
            offTrack = false;
            offTrackTime = 0f;
         }
      }

      //wrong direction checks
      if (wrongDirection == true) {
         wrongDirectionTime += Time.deltaTime;
      } else {
         wrongDirectionTime = 0f;
      }

      if (wrongDirection == true && wrongDirectionTime >= wrongDirectionSeconds) {
         if (waypoints != null && waypoints.Count > 0) {
            //move car to waypoint center
            aiWaypointIndex = GetPastWaypointIndex(aiWaypointIndex);
            if (aiWaypointIndex >= 0 && aiWaypointIndex < waypoints.Count) {
               MoveToCurrentWaypoint();
            }
            wrongDirection = false;
            wrongDirectionTime = 0;
         }
      }
   }

   /// <summary>
   /// A method that is used to find a past waypoint index that is as close to five waypoints as possbile.
   /// </summary>
   /// <param name="wpIdx">The waypoint index to use as the basis of this methods calculation.</param>
   /// <returns>The determined waypoint index.</returns>
   private int GetPastWaypointIndex(int wpIdx) {
      if (wpIdx - 5 >= 0) {
         wpIdx -= 5;
      } else if (wpIdx - 4 >= 0) {
         wpIdx -= 4;
      } else if (wpIdx - 3 >= 0) {
         wpIdx -= 3;
      } else if (wpIdx - 2 >= 0) {
         wpIdx -= 2;
      } else if (wpIdx - 1 >= 0) {
         wpIdx -= 1;
      } else {
         wpIdx = 0;
      }
      return wpIdx;
   }

   /// <summary>
   /// Gets the current player's life total.
   /// </summary>
   /// <returns>The current player's life total.</returns>
   public int GetLife() {
      return (lifeTotal - damage);
   }

   /// <summary>
   /// Gets the current player's life total.
   /// </summary>
   /// <returns>The current player's life total.</returns>
   public int GetLifeHUD() {
      return (lifeTotal - damage);
   }

   /// <summary>
   /// Gets the number of laps remaining in the race.
   /// </summary>
   /// <returns>The number of laps remaining in the race.</returns>
   public int GetLapsLeft() {
      return (totalLaps - currentLap);
   }

   /// <summary>
   /// Resets the current player's state.
   /// </summary>
   public void Reset() {
      totalTime = 0f;
      min = 0f;
      s = 0f;
      ms = 0f;
      hour = 0f;
      ammo = 0;
      damage = 0;
      points = 0;

      boostOn = false;
      invincOn = false;
      invincTime = 0.0f;
      gunOn = false;
      armorOn = false;
      offTrack = true;
      gunSmokeOn = false;
      gunSmokeTime = 0f;

      prepped = false;
      offTrack = false;
      offTrackTime = 0.0f;
      wrongDirection = false;
      wrongDirectionTime = 0.0f;
      skippedWaypoint = false;
      skippedWaypointTime = 0.0f;
      position = 6;
      currentLap = 0;
      waypointDistance = 0.0f;
      waypointDistancePrev = 0.0f;
      alive = true;

      isBouncing = false;
      isJumping = false;
      isDrafting = false;
      isShot = false;
      isShotTime = 0.0f;
      isHit = false;
      isHitTime = 0.0f;
      isMiss = false;
      isMissTime = 0.0f;

      aiIsStuck = false;
      aiIsPassing = false;
      aiPassingTime = 0.0f;
      aiPassingMode = 0;
      aiHasTarget = false;
      aiHasTargetTime = 0.0f;
      aiIsReloading = false;
      aiIsReloadingTime = 0.0f;

      aiIsLargeTurn = false;
      aiIsLargeTurnSpeed = 0.0f;
      aiCanFire = false;
      aiBoostTime = 0.0f;
      aiBoostMode = 0;
      aiWaypointTime = 0;
      aiWaypointLastIndex = -1;
      aiWaypointIndex = 0;
      aiWaypointJumpCount = 0;
      aiWaypointPassCount = 0;
   }

   /// <summary>
   /// Turns the drafting mod on.
   /// </summary>
   public void SetDraftingBonusOn() {
      isDrafting = true;
      SetCurrentSpeed();
   }

   /// <summary>
   /// Turns the drafting mod off.
   /// </summary>
   public void SetDraftingBonusOff() {
      isDrafting = false;
      SetCurrentSpeed();
   }

   /// <summary>
   /// Turns the boost mod on.
   /// </summary>
   public void SetBoostOn() {
      boostOn = true;
      SetBoost();
   }

   /// <summary>
   /// Turns the boost mod off.
   /// </summary>
   public void SetBoostOff() {
      boostOn = false;
      SetNorm();
   }

   /// <summary>
   /// Puts the car into the speed mode associated with its current speed type.
   /// </summary>
   public void SetCurrentSpeed() {
      if (speedType == 0) {
         SetSlow();
      } else if (speedType == 1) {
         SetNorm();
      } else if (speedType == 2) {
         SetBoost();
      }
   }

   /// <summary>
   /// Turns on the car's invinicibility mood.
   /// </summary>
   public void ShowInvinc() {
      invincOn = true;
      invincTime = 0f;
   }

   /// <summary>
   /// Turns off the car's invincibility mod.
   /// </summary>
   public void HideInvinc() {
      invincOn = false;
      invincTime = 0f;
   }

   /// <summary>
   /// Shows the car's gun turret.
   /// </summary>
   public void ShowGun() {
      gunOn = true;
      if (gun != null) {
         gun.SetActive(true);
      }

      if (gunBase != null) {
         gunBase.SetActive(true);
      }
   }

   /// <summary>
   /// Hides the car's gun turret.
   /// </summary>
   public void HideGun() {
      gunOn = false;
      if (gun != null) {
         gun.SetActive(false);
      }

      if (gunBase != null) {
         gunBase.SetActive(false);
      }
   }

   /// <summary>
   /// Puts the current player's car into slow mode.
   /// </summary>
   public void SetSlow() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (cm == null) {
         return;
      }

      speedType = 0;
      cm.movement.maxForwardSpeed = maxForwardSpeedSlow;
      if (isDrafting == true) {
         cm.movement.maxForwardSpeed += MAX_SPEED_BONUS_DRAFTING;
      }

      if (aiIsPassing == true) {
         cm.movement.maxForwardSpeed += MAX_SPEED_BONUS_PASSING;
      }
      cm.movement.maxSidewaysSpeed = maxSidewaysSpeedSlow;
      cm.movement.maxBackwardsSpeed = maxBackwardsSpeedSlow;
      cm.movement.maxGroundAcceleration = maxGroundAccelerationSlow;
   }

   /// <summary>
   /// Puts the current player's car into normal mode.
   /// </summary>
   public void SetNorm() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (cm == null) {
         return;
      }

      speedType = 1;
      cm.movement.maxForwardSpeed = maxForwardSpeedNorm;
      if (isDrafting == true) {
         cm.movement.maxForwardSpeed += MAX_SPEED_BONUS_DRAFTING;
      }

      if (aiIsPassing == true) {
         cm.movement.maxForwardSpeed += MAX_SPEED_BONUS_PASSING;
      }
      cm.movement.maxSidewaysSpeed = maxSidewaysSpeedNorm;
      cm.movement.maxBackwardsSpeed = maxBackwardsSpeedNorm;
      cm.movement.maxGroundAcceleration = maxGroundAccelerationNorm;
   }

   /// <summary>
   /// Puts the current player's car into boost mode.
   /// </summary>
   public void SetBoost() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (cm == null) {
         return;
      }

      speedType = 2;
      cm.movement.maxForwardSpeed = maxForwardSpeedBoost;
      if (isDrafting == true) {
         cm.movement.maxForwardSpeed += MAX_SPEED_BONUS_DRAFTING;
      }

      if (aiIsPassing == true) {
         cm.movement.maxForwardSpeed += MAX_SPEED_BONUS_PASSING;
      }
      cm.movement.maxSidewaysSpeed = maxSidewaysSpeedBoost;
      cm.movement.maxBackwardsSpeed = maxBackwardsSpeedBoost;
      cm.movement.maxGroundAcceleration = maxGroundAccelerationBoost;
   }
}