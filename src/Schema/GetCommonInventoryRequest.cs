using System;
using System.Xml.Serialization;

[XmlRoot(ElementName = "GCIR", Namespace = "")]
[Serializable]
public class GetCommonInventoryRequest {
    [XmlElement(ElementName = "CID", IsNullable = false)]
    public int ContainerId { get; set; }

    [XmlElement(ElementName = "LOC", IsNullable = true)]
    public string Locale { get; set; }

    [XmlElement(ElementName = "CIIDS", IsNullable = true)]
    public int?[] CommonInventoryIDS { get; set; }

    [XmlElement(ElementName = "LTS", IsNullable = true)]
    public bool? LoadItemStats { get; set; }
}
