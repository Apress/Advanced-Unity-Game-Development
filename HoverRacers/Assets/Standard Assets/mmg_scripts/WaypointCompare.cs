
using System.Collections;

/// <summary>
/// A class used to compare two different waypoint checks for equality.
/// </summary>
public class WaypointCompare : IComparer {
   //***** Internal Variables: Compare *****
   private WaypointCheck obj1 = null;
   private WaypointCheck obj2 = null;

   /// <summary>
   /// A method to compare two WaypointCheck objects for equality.
   /// </summary>
   /// <param name="o1">The first object to compare.</param>
   /// <param name="o2">The second object to compare.</param>
   /// <returns></returns>
   public int Compare(object o1, object o2) {
      obj1 = (WaypointCheck)o1;
      obj2 = (WaypointCheck)o2;
      if (obj1.waypointIndex > obj2.waypointIndex) {
         return 1;
      } else if (obj1.waypointIndex < obj2.waypointIndex) {
         return -1;
      } else {
         return 0;
      }
   }
}