using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "Pairs", Namespace = "", IsNullable = true)]
[Serializable]
public class PairData {
    [XmlElement("Pair", IsNullable = true)]
    public Pair[] Pairs { get; set; }
}