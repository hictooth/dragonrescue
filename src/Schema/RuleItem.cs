using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "RuleItem", Namespace = "")]
[Serializable]
public class RuleItem {
    [XmlElement(ElementName = "Type")]
    public RuleItemType Type;

    [XmlElement(ElementName = "MissionID")]
    public int MissionID;

    [XmlElement(ElementName = "ID")]
    public int ID;

    [XmlElement(ElementName = "Complete")]
    public int Complete;
}
