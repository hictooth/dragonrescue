using System;
using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "MP", Namespace = "")]
[Serializable]
public class MissionPair {
    [XmlElement(ElementName = "MID")]
    public int? MissionID { get; set; }

    [XmlElement(ElementName = "VID")]
    public int? VersionID { get; set; }
}
