using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(Namespace = "http://api.jumpstart.com/", ElementName = "ArrayOfBuddy", IsNullable = true)]
[Serializable]
public class BuddyList {
    [XmlElement(ElementName = "Buddy")]
    public Buddy[] Buddy;

    public static ArrayOfMessage mStatusMessages = null;

    public static List<BuddyStatusMessage> pStatusMessages = new List<BuddyStatusMessage>();

    public static bool pIsStatusReady = false;
}
