using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "MissionRule", Namespace = "")]
[Serializable]
public class MissionRule {
    [XmlElement(ElementName = "Prerequisites")]
    public List<PrerequisiteItem> Prerequisites;

    [XmlElement(ElementName = "Criteria")]
    public MissionCriteria Criteria;
}
