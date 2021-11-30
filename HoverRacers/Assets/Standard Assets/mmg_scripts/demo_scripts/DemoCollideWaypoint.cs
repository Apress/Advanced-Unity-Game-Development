using UnityEngine;

/// <summary>
/// A class used by the DemoCollideWaypoint script to change the color of six game object 
/// based on the direction of the player's car in reference to a series of way points.
/// </summary>
public class DemoCollideWaypoint : MonoBehaviour {
   public Renderer cb1 = null;
   public Renderer cb2 = null;
   public Renderer cb3 = null;
   public Renderer cb4 = null;
   public Renderer cb5 = null;
   public Renderer cb6 = null;
   public PlayerState p = null;
   public Material mOn = null;
   public Material mOff = null;

   /// <summary>
   /// Update is called once per frame. 
   /// </summary>
   void Update() {
      if (p != null && cb1 != null && cb2 != null && cb3 != null && cb4 != null && cb5 != null && cb6 != null) {
         if (p.wrongDirection == true) {
            cb1.material = mOff;
            cb2.material = mOff;
            cb3.material = mOff;
            cb4.material = mOff;
            cb5.material = mOff;
            cb6.material = mOff;
         } else {
            cb1.material = mOn;
            cb2.material = mOn;
            cb3.material = mOn;
            cb4.material = mOn;
            cb5.material = mOn;
            cb6.material = mOn;
         }
      }
   }
}
