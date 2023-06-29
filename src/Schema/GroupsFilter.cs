using System;
using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "GroupsFilter", IsNullable = true)]
[Serializable]
public class GroupsFilter {
    [XmlElement(ElementName = "Count")]
    public int? Count { get; set; }

    [XmlElement(ElementName = "FromDate", IsNullable = true)]
    public DateTime? FromDate { get; set; }

    [XmlElement(ElementName = "ToDate", IsNullable = true)]
    public DateTime? ToDate { get; set; }

    [XmlElement(ElementName = "Refresh", IsNullable = true)]
    public bool? Refresh { get; set; }

    [XmlElement(ElementName = "GroupType")]
    public GroupType? GroupType;

    [XmlElement(ElementName = "Locale")]
    public string Locale;

    [XmlElement(ElementName = "PointTypeID")]
    public int? PointTypeID;
}
