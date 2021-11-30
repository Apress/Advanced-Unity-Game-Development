using UnityEngine.UI;

/// <summary>
/// A class that is used to keep track of the popup messages that are being displayed on the screen.
/// </summary>
public class PopupMsgTracker {
   //***** Class Fields *****
   /// <summary>
   /// The index of this popup message in the list of visible popup messages.
   /// </summary>
   public int index = 0;

   /// <summary>
   /// The image used to display the popup message.
   /// </summary>
   public Image image = null;

   /// <summary>
   /// An integer value that represents the popup message's position index with regard to the other visible popup messages.
   /// </summary>
   public int posIdx = 0;

   /// <summary>
   /// A boolean flag indicating that this popup message is moving up a position.
   /// </summary>
   public bool movingUp = false;

   /// <summary>
   /// An integer value that represents the type of popup message being displayed.
   /// </summary>
   public int type = 0;
}