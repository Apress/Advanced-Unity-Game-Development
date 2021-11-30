using System.Collections.Generic;

/// <summary>
/// A class that manages the lap time entries providing serialization and deserialization support for a set of lap times.
/// </summary>
public class LapTimeManager {
   //***** Static Class Fields *****
   /// <summary>
   /// The first index in recorded lap time that is used when cleaning out entries.
   /// </summary>
   public static int CLEANING_START_INDEX = 33;

   /// <summary>
   /// THe last index in recorded lap times that is not used when cleaning out entries.
   /// </summary>
   public static int LAST_NOT_CLEANED_INDEX = 32;

   //***** Class Fields *****
   /// <summary>
   /// A list of lap times for the current player.
   /// </summary>
   public List<LapTime> lapTimes = new List<LapTime>();

   /// <summary>
   /// The last entry in the list of lap times.
   /// </summary>
   public LapTime lastEntry = null;

   /// <summary>
   /// The fastest entry in the list of lap times.
   /// </summary>
   public LapTime bestEntry = null;

   //***** Internal Variables: FindBestLapTime/CleanTimes *****
   private LapTime retFbt = null;
   private int lFbt = 0;
   private int iFbt = 0;
   private LapTime ltFbt = null;
   private int lCt = 0;
   private int iCt = 0;

   //***** Internal Variables: Serialize/ToString *****
   private string retSer = "";
   private int lSer = 0;
   private int iSer = 0;
   private string retTs = "";
   private int lTs = 0;
   private int iTs = 0;

   //***** Internal Variables: Deserialize *****
   private char[] cDes = null;
   private string[] csDes = null;
   private int lDes = 0;
   private int iDes = 0;
   private string tmpDes = "";
   private LapTime ltDes = null;

   /// <summary>
   /// A method used to add a new LapTime to the list of lap times.
   /// </summary>
   /// <param name="lt">A new lap time to add to the list of lap times.</param>
   public void AddEntry(LapTime lt) {
      lapTimes.Add(lt);
      lastEntry = lt;
   }

   /// <summary>
   /// A method that finds the best lap time based on the performance of the last entry as a comparison point.
   /// </summary>
   /// <returns>The best LapTime in the set of lap times managed by this class.</returns>
   public LapTime FindBestLapTimeByLastEntry() {
      return FindBestLapTime(lastEntry.track, lastEntry.type, lastEntry.diff, lastEntry.timeNum);
   }

   /// <summary>
   /// A method that finds the best lap time based on the race settings specified as method arguments.
   /// </summary>
   /// <param name="track">The track the lap took place on.</param>
   /// <param name="type">The type of race the lap took place on.</param>
   /// <param name="diff">The difficulty of the race the lap took place on.</param>
   /// <param name="timeNum">The time number of the lap.</param>
   /// <returns>The best lap time that matches the given lap time criteria.</returns>
   public LapTime FindBestLapTime(int track, int type, int diff, int timeNum) {
      retFbt = null;
      ltFbt = null;
      if (lapTimes != null) {
         lFbt = lapTimes.Count;
         for (iFbt = 0; iFbt < lFbt; iFbt++) {
            ltFbt = (LapTime)lapTimes[iFbt];
            if (ltFbt != null) {
               if (ltFbt.track == track && ltFbt.type == type && ltFbt.diff == diff) {
                  if (ltFbt.timeNum < timeNum) {
                     retFbt = ltFbt;
                  }
               }
            }
         }
      }
      bestEntry = retFbt;
      return retFbt;
   }

   /// <summary>
   /// A method used to clean up the list of lap times. Any LapTimes > 32 entries is trimmed from the list of lap times.
   /// </summary>
   public void CleanTimes() {
      if (lapTimes != null && lapTimes.Count > 1) {
         if (lapTimes.Count > LAST_NOT_CLEANED_INDEX) {
            lCt = lapTimes.Count;
            for (iCt = CLEANING_START_INDEX; iCt < lCt; iCt++) {
               lapTimes.Remove(lapTimes[iCt]);
               lCt--;
            }
         }
      }
   }

   /// <summary>
   /// A method that is used to serialize the set of managed lap times.
   /// </summary>
   /// <returns>A string representation of the list of managed lap times.</returns>
   public string Serialize() {
      retSer = "";
      if (lapTimes != null) {
         lSer = lapTimes.Count;
         for (iSer = 0; iSer < lSer; iSer++) {
            if (lapTimes[iSer] != null) {
               retSer += ((LapTime)lapTimes[iSer]).Serialize();
               if (iSer < lSer - 1) {
                  retSer += "^";
               }
            }
         }
      }
      return retSer;
   }

   /// <summary>
   /// A method that creates a short string representation of this class.
   /// </summary>
   /// <returns>A short string representation of the class.</returns>
   public string ToStringShort() {
      return "Lap Times: " + lapTimes.Count;
   }

   /// <summary>
   /// A method that creates a string representation of this class.
   /// </summary>
   /// <returns>A long string representation of the class.</returns>
   public override string ToString() {
      retTs = "";
      if (lapTimes != null) {
         lTs = lapTimes.Count;
         for (iTs = 0; iTs < lTs; iTs++) {
            if (lapTimes[iTs] != null) {
               retTs += "Lap Time Entry: " + (iTs + 1) + "\n";
               retTs += ((LapTime)lapTimes[iTs]).ToString() + "\n";
            }
         }
      }
      return retTs;
   }

   /// <summary>
   /// A method for deserializing a serialized string representation of this class into individual lap times serializations that are then converted back into actual LapTime instances.
   /// </summary>
   /// <param name="s">A serialized string representation of this class.</param>
   /// <returns>A boolean flag indicating the success of this operation.</returns>
   public bool Deserialize(string s) {
      lapTimes = new List<LapTime>();
      if (s != null && s != "") {
         cDes = "^".ToCharArray();
         csDes = s.Split(cDes);
         if (csDes != null && csDes.Length > 0) {
            lDes = csDes.Length;
            for (iDes = 0; iDes < lDes; iDes++) {
               tmpDes = csDes[iDes];
               if (tmpDes != null) {
                  ltDes = new LapTime();
                  ltDes.Deserialize(tmpDes);
                  lapTimes.Add(ltDes);
               }
            }
            return true;
         } else {
            return false;
         }
      } else {
         return false;
      }
   }
}