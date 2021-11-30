using UnityEngine;

//Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/Character Motor")]

/// <summary>
/// A class used to manage the character's motor.
/// </summary>
public class CharacterMotor : BaseScript {
   //***** Sub-Classes *****
   /// <summary>
   /// A class used to control the character motor movement.
   /// </summary>
   [System.Serializable]
   public class CharacterMotorMovement {
      /// <summary>
      /// The maximum horizontal speed when moving.
      /// </summary>
      public float maxForwardSpeed = 3.0f;

      /// <summary>
      /// The maximum sideways, strafe, speed when moving.
      /// </summary>
      public float maxSidewaysSpeed = 10.0f;

      /// <summary>
      /// The maximum backwards speed.
      /// </summary>
      public float maxBackwardsSpeed = 10.0f;

      /// <summary>
      /// Curve for multiplying speed based on slope (negative = downwards).
      /// </summary>
      public AnimationCurve slopeSpeedMultiplier = new AnimationCurve(new Keyframe(-90, 1), new Keyframe(0, 1), new Keyframe(90, 0.70f));

      /// <summary>
      /// How fast does the character change speeds?  Higher is faster. 
      /// </summary>
      public float maxGroundAcceleration = 30.0f;

      /// <summary>
      /// How fast can the character move while in the air.
      /// </summary>
      public float maxAirAcceleration = 20.0f;

      /// <summary>
      /// The gravity for the character. 
      /// </summary>
      public float gravity = 10.0f;

      /// <summary>
      /// The maximum fall speed.
      /// </summary>
      public float maxFallSpeed = 22.0f;

      // For the next variables, [System.NonSerialized] tells Unity to not serialize the variable or show it in the inspector view.
      // Very handy for organization!

      /// <summary>
      /// The last collision flags returned from controller.Move
      /// </summary>
      [System.NonSerialized]
      public CollisionFlags collisionFlags;

      /// <summary>
      /// We will keep track of the character's current velocity.
      /// </summary>
      [System.NonSerialized]
      public Vector3 velocity;

      /// <summary>
      /// This keeps track of our current velocity for the current frame.
      /// </summary>
      [System.NonSerialized]
      public Vector3 frameVelocity = Vector3.zero;

      /// <summary>
      /// The collision hit point.
      /// </summary>
      [System.NonSerialized]
      public Vector3 hitPoint = Vector3.zero;

      /// <summary>
      /// The previous collision hit point.
      /// </summary>
      [System.NonSerialized]
      public Vector3 lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);
   }

   /// <summary>
   /// A class used to control the character motor jumping.
   /// We will contain all the jumping related variables in one helper class for clarity.
   /// </summary>
   [System.Serializable]
   public class CharacterMotorJumping {
      /// <summary>
      /// Can the character jump?
      /// </summary>
      public bool enabled = true;

      /// <summary>
      /// How high do we jump when pressing jump and letting go immediately.
      /// </summary>
      public float baseHeight = 1.0f;

      /// <summary>
      /// We add extraHeight units(meters) on top when holding the button down longer while jumping.
      /// </summary>
      public float extraHeight = 4.1f;

      /// <summary>
      /// How much does the character jump out perpendicular to the surface on walkable surfaces?
      /// 0 means a fully vertical jump and 1 means fully perpendicular.
      /// </summary>
      public float perpAmount = 0.0f;

      /// <summary>
      /// How much does the character jump out perpendicular to the surface on too steep surfaces?
      /// 0 means a fully vertical jump and 1 means fully perpendicular. 
      /// </summary>
      public float steepPerpAmount = 0.5f;

      // For the next variables, [System.NonSerialized] tells Unity to not serialize the variable or show it in the inspector view.
      // Very handy for organization!

      /// <summary>
      /// Are we jumping?(Initiated with jump button and not grounded yet)
      /// To see ifwe are just in the air(initiated by jumping OR falling) see the grounded variable.
      /// </summary>
      [System.NonSerialized]
      public bool jumping = false;

      /// <summary>
      /// Is the jump button heing held down.
      /// </summary>
      [System.NonSerialized]
      public bool holdingJumpButton = false;

      /// <summary>
      /// The time we jumped at (Used to determine for how long to apply extra jump power after jumping.)
      /// </summary>
      [System.NonSerialized]
      public float lastStartTime = 0.0f;

      /// <summary>
      /// The down time of the last button press.
      /// </summary>
      [System.NonSerialized]
      public float lastButtonDownTime = -100.0f;

      /// <summary>
      /// The Vector3 that describes the jump direction.
      /// </summary>
      [System.NonSerialized]
      public Vector3 jumpDir = Vector3.up;
   }

   /// <summary>
   /// A class used to control the character motor sliding.
   /// </summary>
   [System.Serializable]
   public class CharacterMotorSliding {
      /// <summary>
      /// Does the character slide on too steep surfaces?
      /// </summary>
      public bool enabled = true;

      /// <summary>
      /// How fast does the character slide on steep surfaces?
      /// </summary>
      public float slidingSpeed = 15.0f;

      /// <summary>
      /// How much can the player control the sliding direction? if the value is 0.5 the player can slide sideways with half the speed of the downwards sliding speed.
      /// </summary>
      public float sidewaysControl = 1.0f;
 
      /// <summary>
      /// How much can the player influence the sliding speed? if the value is 0.5 the player can speed the sliding up to 150% or slow it down to 50%.
      /// </summary>
      public float speedControl = 0.4f;
   }

   //***** Class Fields *****
   /// <summary>
   /// A boolean flag indicating that the AI control is on.
   /// </summary>
   public bool aiOn = false;

   /// <summary>
   /// Does this script currently respond to input?
   /// </summary>
   bool canControl = true;

   /// <summary>
   /// A boolean flag indicating that the fixed update call is used instead of the normal Update callback.
   /// </summary>
   bool useFixedUpdate = true;

   // For the next variables, [System.NonSerialized] tells Unity to not serialize the variable or show it in the inspector view.
   // Very handy for organization!

   /// <summary>
   /// The current direction we want the character to move in.
   /// </summary>
   [System.NonSerialized]
   public Vector3 inputMoveDirection = Vector3.zero;

   /// <summary>
   /// Is the jump button held down? We use this interface instead of checking
   /// for the jump button directly so this script can also be used by AIs.
   /// </summary>
   [System.NonSerialized]
   public bool inputJump = false;

   /// <summary>
   /// A class that describes the character motor horizontal movement.
   /// </summary>
   public CharacterMotorMovement movement = new CharacterMotorMovement();

   /// <summary>
   /// A class that describes the character motor jumping movement.
   /// </summary>
   public CharacterMotorJumping jumping = new CharacterMotorJumping();

   /// <summary>
   /// A class that describes the character motor sliding movement.
   /// </summary>
   public CharacterMotorSliding sliding = new CharacterMotorSliding();

   /// <summary>
   /// A boolean that indicates if the character is grounded.
   /// </summary>
   [System.NonSerialized]
   public bool grounded = true;

   /// <summary>
   /// A Vector3 that indicates a normal vector in the direction of the ground.
   /// </summary>
   [System.NonSerialized]
   public Vector3 groundNormal = Vector3.zero;

   /// <summary>
   /// The previous Vector3 that indicates a normal vector in the direction of the ground.
   /// </summary>
   private Vector3 lastGroundNormal = Vector3.zero;

   /// <summary>
   /// A reference to a GameObject transform.
   /// </summary>
   private Transform tr;

   /// <summary>
   /// A reference to a Unity CharacterController.
   /// </summary>
   private CharacterController controller;

   /// <summary>
   /// A MonoBehaviour life cycle callback. Called when the class initializes as part of the game engine.
   /// </summary>
   void Awake() {
      controller = GetComponent<CharacterController>();
      tr = transform;
      base.PrepPlayerInfo(this.GetType().Name);
      if (BaseScript.IsActive(scriptName) == false) {
         Utilities.wrForce(scriptName + ": Is Deactivating...");
         return;
      } else {
         aiOn = p.aiOn;
      }
   }

   /// <summary>
   /// The class' update method which calculate the movement of the attached GameObject. 
   /// </summary>
   private void UpdateFunction() {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      // We copy the actual velocity into a temporary variable that we can manipulate.
      Vector3 velocity = movement.velocity;

      // Update velocity based on input
      velocity = ApplyInputVelocityChange(velocity);

      // Apply gravity and jumping force
      velocity = ApplyGravityAndJumping(velocity);

      // Save lastPosition for velocity calculation.
      Vector3 lastPosition = tr.position;

      // We always want the movement to be framerate independent.  Multiplying by Time.deltaTime does this.
      Vector3 currentMovementOffset = velocity * Time.deltaTime;

      // Find out how much we need to push towards the ground to avoid loosing grouning
      // when walking down a step or over a sharp change in slope.
      float pushDownOffset = Mathf.Max(controller.stepOffset, new Vector3(currentMovementOffset.x, 0, currentMovementOffset.z).magnitude);
      if (grounded) {
         currentMovementOffset -= pushDownOffset * Vector3.up;
      }

      // Reset variables that will be set by collision function
      //movingPlatform.hitPlatform = null;
      groundNormal = Vector3.zero;

      // Move our character!
      movement.collisionFlags = controller.Move(currentMovementOffset);

      movement.lastHitPoint = movement.hitPoint;
      lastGroundNormal = groundNormal;

      // Calculate the velocity based on the current and previous position.  
      // This means our velocity will only be the amount the character actually moved as a result of collisions.
      Vector3 oldHVelocity = new Vector3(velocity.x, 0, velocity.z);
      movement.velocity = (tr.position - lastPosition) / Time.deltaTime;
      Vector3 newHVelocity = new Vector3(movement.velocity.x, 0, movement.velocity.z);

      // The CharacterController can be moved in unwanted directions when colliding with things.
      // We want to prevent this from influencing the recorded velocity.
      if (oldHVelocity == Vector3.zero) {
         movement.velocity = new Vector3(0, movement.velocity.y, 0);
      } else {
         float projectedNewVelocity = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
         movement.velocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + movement.velocity.y * Vector3.up;
      }

      if (movement.velocity.y < velocity.y - 0.001) {
         if (movement.velocity.y < 0) {
            // Something is forcing the CharacterController down faster than it should.
            // Ignore this
            movement.velocity.y = velocity.y;
         } else {
            // The upwards movement of the CharacterController has been blocked.
            // This is treated like a ceiling collision - stop further jumping here.
            jumping.holdingJumpButton = false;
         }
      }

      // We were grounded but just lost grounding
      if (grounded && !IsGroundedTest()) {
         grounded = false;

         // We pushed the character down to ensure it would stay on the ground ifthere was any.
         // But there wasn't so now we cancel the downwards offset to make the fall smoother.
         tr.position += pushDownOffset * Vector3.up;
      }
      // We were not grounded but just landed on something
      else if (!grounded && IsGroundedTest()) {
         grounded = true;
      }
   }

   /// <summary>
   /// Update is called once per frame at a fixeds interval.
   /// </summary>
   void FixedUpdate() {
      if (useFixedUpdate) {
         UpdateFunction();
      }
   }

   /// <summary>
   /// Update is called once per frame.
   /// </summary>
   void Update() {
      if (!useFixedUpdate) {
         UpdateFunction();
      }
   }

   /// <summary>
   /// A method that applies the Vector3 argument as an input velocity to the GameObject.
   /// </summary>
   /// <param name="velocity"></param>
   /// <returns></returns>
   private Vector3 ApplyInputVelocityChange(Vector3 velocity) {
      if (!canControl) {
         inputMoveDirection = Vector3.zero;
      }

      // Find desired velocity
      Vector3 desiredVelocity;
      if (grounded && TooSteep()) {
         // The direction we're sliding in
         desiredVelocity = new Vector3(groundNormal.x, 0, groundNormal.z).normalized;
         // Find the input movement direction projected onto the sliding direction

         Vector3 projectedMoveDir = Vector3.Project(inputMoveDirection, desiredVelocity);

         // Add the sliding direction, the speed control, and the sideways control vectors
         desiredVelocity = desiredVelocity + projectedMoveDir * sliding.speedControl + (inputMoveDirection - projectedMoveDir) * sliding.sidewaysControl;
         // Multiply with the sliding speed
         desiredVelocity *= sliding.slidingSpeed;
      } else {
         desiredVelocity = GetDesiredHorizontalVelocity();
      }

      if (grounded) {
         desiredVelocity = AdjustGroundVelocityToNormal(desiredVelocity, groundNormal);
      } else {
         velocity.y = 0;
      }

      // Enforce max velocity change
      float maxVelocityChange = GetMaxAcceleration(grounded) * Time.deltaTime;
      Vector3 velocityChangeVector = (desiredVelocity - velocity);
      if (velocityChangeVector.sqrMagnitude > maxVelocityChange * maxVelocityChange) {
         velocityChangeVector = velocityChangeVector.normalized * maxVelocityChange;
      }

      // If we're in the air and don't have control, don't apply any velocity change at all.
      // If we're on the ground and don't have control we do apply it - it will correspond to friction.
      if (grounded || canControl) {
         velocity += velocityChangeVector;
      }

      if (grounded) {
         // When going uphill, the CharacterController will automatically move up by the needed amount.
         // Not moving it upwards manually prevent risk of lifting off from the ground.
         // When going downhill, DO move down manually, as gravity is not enough on steep hills.
         velocity.y = Mathf.Min(velocity.y, 0);
      }
      return velocity;
   }

   /// <summary>
   /// A method that applies the Vector3 argument as an input velocity in the gravity/jumping movement axis.
   /// </summary>
   /// <param name="velocity"></param>
   /// <returns></returns>
   private Vector3 ApplyGravityAndJumping(Vector3 velocity) {
      if (!inputJump || !canControl) {
         jumping.holdingJumpButton = false;
         jumping.lastButtonDownTime = -100;
      }

      if (inputJump && jumping.lastButtonDownTime < 0 && canControl) {
         jumping.lastButtonDownTime = Time.time;
      }

      if (grounded) {
         velocity.y = Mathf.Min(0, velocity.y) - movement.gravity * Time.deltaTime;
      } else {
         velocity.y = movement.velocity.y - movement.gravity * Time.deltaTime;

         // When jumping up we don't apply gravity for some time when the user is holding the jump button.
         // This gives more control over jump height by pressing the button longer.
         if (jumping.jumping && jumping.holdingJumpButton) {
            // Calculate the duration that the extra jump force should have effect.
            // ifwe're still less than that duration after the jumping time, apply the force.
            if (Time.time < jumping.lastStartTime + jumping.extraHeight / CalculateJumpVerticalSpeed(jumping.baseHeight)) {
               // Negate the gravity we just applied, except we push in jumpDir rather than jump upwards.
               velocity += jumping.jumpDir * movement.gravity * Time.deltaTime;
            }
         }

         // Make sure we don't fall any faster than maxFallSpeed. This gives our character a terminal velocity.
         velocity.y = Mathf.Max(velocity.y, -movement.maxFallSpeed);
      }

      if (grounded) {
         // Jump only if the jump button was pressed down in the last 0.2 seconds.
         // We use this check instead of checking ifit's pressed down right now
         // because players will often try to jump in the exact moment when hitting the ground after a jump
         // and ifthey hit the button a fraction of a second too soon and no new jump happens as a consequence,
         // it's confusing and it feels like the game is buggy.
         if (jumping.enabled && canControl && (Time.time - jumping.lastButtonDownTime < 0.2)) {
            grounded = false;
            jumping.jumping = true;
            jumping.lastStartTime = Time.time;
            jumping.lastButtonDownTime = -100;
            jumping.holdingJumpButton = true;

            // Calculate the jumping direction
            if (TooSteep()) {
               jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.steepPerpAmount);
            } else {
               jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.perpAmount);
            }

            // Apply the jumping force to the velocity. Cancel any vertical velocity first.
            velocity.y = 0;
            velocity += jumping.jumpDir * CalculateJumpVerticalSpeed(jumping.baseHeight);
         } else {
            jumping.holdingJumpButton = false;
         }
      }
      return velocity;
   }

   /// <summary>
   /// A collision event handler that processes when the current character controller's hit.
   /// </summary>
   /// <param name="hit">Provides information about the collision event.</param>
   void OnControllerColliderHit(ControllerColliderHit hit) {
      if (BaseScript.IsActive(scriptName) == false) {
         return;
      }

      if (hit.normal.y > 0 && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0) {
         if ((hit.point - movement.lastHitPoint).sqrMagnitude > 0.001 || lastGroundNormal == Vector3.zero) {
            groundNormal = hit.normal;
         } else {
            groundNormal = lastGroundNormal;
         }

         movement.hitPoint = hit.point;
         movement.frameVelocity = Vector3.zero;
      }
   }

   /// <summary>
   /// A method that returns the calculates, desirec horizontal velocity.
   /// </summary>
   /// <returns></returns>
   private Vector3 GetDesiredHorizontalVelocity() {
      // Find desired velocity
      Vector3 desiredLocalDirection = tr.InverseTransformDirection(inputMoveDirection);
      float maxSpeed = MaxSpeedInDirection(desiredLocalDirection);
      if (grounded) {
         // Modify max speed on slopes based on slope speed multiplier curve
         var movementSlopeAngle = Mathf.Asin(movement.velocity.normalized.y) * Mathf.Rad2Deg;
         maxSpeed *= movement.slopeSpeedMultiplier.Evaluate(movementSlopeAngle);
      }
      return tr.TransformDirection(desiredLocalDirection * maxSpeed);
   }

   /// <summary>
   /// A method that adjust the ground velocity to a normalized velocity vector.
   /// </summary>
   /// <param name="hVelocity">The horizontal velocity vector.</param>
   /// <param name="groundNormal">A vector representing the gound, normal vector.</param>
   /// <returns></returns>
   private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal) {
      Vector3 sideways = Vector3.Cross(Vector3.up, hVelocity);
      return Vector3.Cross(sideways, groundNormal).normalized * hVelocity.magnitude;
   }

   /// <summary>
   /// A method that returns a boolean value indicating if the groundNormal vector is on the ground.
   /// </summary>
   /// <returns></returns>
   private bool IsGroundedTest() {
      return (groundNormal.y > 0.01);
   }

   /// <summary>
   /// A method that returns the maximum acceleration based on the grounded state of the GameObject.
   /// </summary>
   /// <param name="grounded"></param>
   /// <returns></returns>
   float GetMaxAcceleration(bool grounded) {
      // Maximum acceleration on ground and in air
      if (grounded) {
         return movement.maxGroundAcceleration;
      } else {
         return movement.maxAirAcceleration;
      }
   }

   /// <summary>
   /// A method used to calculare the vertical jump speed based on the provided jump height.
   /// </summary>
   /// <param name="targetJumpHeight">The target jump height to attain.</param>
   /// <returns></returns>
   float CalculateJumpVerticalSpeed(float targetJumpHeight) {
      // From the jump height and gravity we deduce the upwards speed 
      // for the character to reach at the apex.
      return Mathf.Sqrt(2 * targetJumpHeight * movement.gravity);
   }

   /// <summary>
   /// A method that returns a boolean value indicating if the current incline is too steep.
   /// </summary>
   /// <returns></returns>
   bool TooSteep() {
      return (groundNormal.y <= Mathf.Cos(controller.slopeLimit * Mathf.Deg2Rad));
   }

   /// <summary>
   /// Project a direction onto elliptical quater segments based on forward, sideways, and backwards speed.
   /// The function returns the length of the resulting vector.
   /// </summary>
   /// <param name="desiredMovementDirection"></param>
   /// <returns></returns>
   float MaxSpeedInDirection(Vector3 desiredMovementDirection) {
      if (desiredMovementDirection == Vector3.zero) {
         return 0;
      } else {
         float zAxisEllipseMultiplier = (desiredMovementDirection.z > 0 ? movement.maxForwardSpeed : movement.maxBackwardsSpeed) / movement.maxSidewaysSpeed;
         Vector3 temp = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
         float length = new Vector3(temp.x, 0, temp.z * zAxisEllipseMultiplier).magnitude * movement.maxSidewaysSpeed;
         return length;
      }
   }
}