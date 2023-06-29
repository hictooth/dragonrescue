using System;
using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "RA", Namespace = "")]
[Serializable]
public class RankAttribute {
    [XmlElement(ElementName = "k")]
    public string Key;

    [XmlElement(ElementName = "v")]
    public string Value;
}
