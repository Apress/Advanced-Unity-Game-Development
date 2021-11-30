
/// <summary>
/// A class that wraps the track time information and provides methods for serializing and deserializing that data to and from string form.
/// </summary>
public class LapTime {
   //***** Class Fields *****
   /// <summary>
   /// A string representation of the current lap time.
   /// </summary>
   public string time = "";

   /// <summary>
   /// A numeric representation of the duration of the lap time.
   /// </summary>
   public int timeNum = 0;

   /// <summary>
   /// An integer that indicates which track this lap time is associated with.
   /// </summary>
   public int track = 0;

   /// <summary>
   /// An integer that indicates the type of track this lap time is associated with.
   /// </summary>
   public int type = 0;            //0 = easy, 1 = battle, 2 = classic

   /// <summary>
   /// An integer that indicates the difficulty of the race this lap time is assocaited with.
   /// </summary>
   public int diff = 0;            //0 = low, 1 = med, 2 = high

   /// <summary>
   /// An integer that indicates which lap this lap time is associated with.
   /// </summary>
   public int lap = 0;

   /// <summary>
   /// A method used to serialize the class data into one string.
   /// </summary>
   /// <returns>A string representation of this class.</returns>
   public string Serialize() {
      string t = time;
      string tN = timeNum + "";
      string tr = track + "";
      string ty = type + "";
      string df = diff + "";
      string l = lap + "";
      string col = "~";
      return (t + col + tN + col + tr + col + ty + col + df + col + l);
   }

   /// <summary>
   /// A method used to represent the class data in a format suitable for output logs.
   /// </summary>
   /// <returns>An output log representation of this class.</returns>
   public override string ToString() {
      string ret = "";
      ret += "Time: " + time + "\n";
      ret += "Time Number: " + timeNum + "\n";
      ret += "Track: " + track + "\n";
      ret += "Type: " + type + "\n";
      ret += "Difficulty: " + diff + "\n";
      ret += "Lap: " + lap + "\n";
      return ret;
   }

   /// <summary>
   /// A method used to serialize the class data into one string.
   /// </summary>
   /// <param name="s">A string that is a serialized representation of this class.</param>
   /// <returns>A boolean indicating the success of the operation.</returns>
   public bool Deserialize(string s) {
      if (s != null && s != "") {
         char[] c = "~".ToCharArray();
         string[] cs = s.Split(c);
         if (cs.Length == 6) {
            time = cs[0] + "";
            timeNum = int.Parse(cs[1]);
            track = int.Parse(cs[2]);
            type = int.Parse(cs[3]);
            diff = int.Parse(cs[4]);
            lap = int.Parse(cs[5]);
            return true;
         } else {
            return false;
         }
      } else {
         return false;
      }
   }
}