using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "CIRI", Namespace = "")]
[Serializable]
public class CommonInventoryResponseItem
{
	[XmlElement(ElementName = "iid")]
	public int ItemID;

	[XmlElement(ElementName = "cid")]
	public int CommonInventoryID;

	[XmlElement(ElementName = "qty")]
	public int Quantity;
}
