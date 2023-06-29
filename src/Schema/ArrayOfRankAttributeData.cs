using System;
using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "ArrayOfRankAttributeData", Namespace = "http://tempuri.org/")]
[Serializable]
public class ArrayOfRankAttributeData {
    [XmlElement(ElementName = "RankAttributeData")]
    public RankAttributeData[] RankAttributeData;
}
