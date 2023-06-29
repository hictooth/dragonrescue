using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "RAD", Namespace = "")]
[Serializable]
public class RankAttributeData {
    [XmlElement(ElementName = "r")]
    public int RankID;

    [XmlElement(ElementName = "a", IsNullable = true)]
    public RankAttribute[] Attributes;
}
