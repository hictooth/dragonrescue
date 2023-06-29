using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "BPDC", Namespace = "", IsNullable = true)]
[Serializable]
public class BluePrintDeductibleConfig
{
	[XmlElement(ElementName = "BPIID", IsNullable = false)]
	public int BluePrintItemID { get; set; }

	[XmlElement(ElementName = "DT", IsNullable = false)]
	public DeductibleType DeductibleType { get; set; }

	[XmlElement(ElementName = "IID", IsNullable = true)]
	public int? ItemID { get; set; }

	[XmlElement(ElementName = "QTY", IsNullable = false)]
	public int Quantity { get; set; }
}
