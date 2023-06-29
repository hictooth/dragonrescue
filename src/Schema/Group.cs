using System.Xml.Serialization;
using dragonrescue.Schema;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "GP", IsNullable = true, Namespace = "")]
[Serializable]
public class Group {

    [XmlElement(ElementName = "G")]
    public string GroupID;

    [XmlElement(ElementName = "N")]
    public string Name;

    [XmlElement(ElementName = "D")]
    public string Description;

    [XmlElement(ElementName = "T")]
    public GroupType Type;

    [XmlElement(ElementName = "O", IsNullable = true)]
    public string OwnerID;

    [XmlElement(ElementName = "L", IsNullable = true)]
    public string Logo;

    [XmlElement(ElementName = "C")]
    public string Color;

    [XmlElement(ElementName = "M", IsNullable = true)]
    public int? MemberLimit;

    [XmlElement(ElementName = "TC", IsNullable = true)]
    public int? TotalMemberCount;

    [XmlElement(ElementName = "A")]
    public bool Active;

    [XmlElement(ElementName = "P", IsNullable = true)]
    public string ParentGroupID;

    [XmlElement(ElementName = "PS", IsNullable = true)]
    public int? Points;

    [XmlElement(ElementName = "RK", IsNullable = true)]
    public int? Rank;

    [XmlElement(ElementName = "RT", IsNullable = true)]
    public int? RankTrend;
}
