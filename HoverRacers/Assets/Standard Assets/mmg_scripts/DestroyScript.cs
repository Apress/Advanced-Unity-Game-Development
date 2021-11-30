using UnityEngine;

/// <summary>
/// A class designed to destroy any object it collides with.
/// </summary>
public class DestroyScript : WaterResetScript {
   /// <summary>
   /// A trigger event handler that fires when the collision object enters the collider.
   /// </summary>
   /// <param name="otherObj">The collider that registered the trigger event.</param>
   public override void OnTriggerEnter(Collider otherObj) {
      if (otherObj.gameObject.CompareTag(Utilities.TAG_PLAYERS)) {
         base.OnTriggerEnter(otherObj);
      } else {
         Destroy(otherObj.gameObject);
      }
   }
}