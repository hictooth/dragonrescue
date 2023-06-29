using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "IC", Namespace = "")]
[Serializable]
public class ItemDataCategory
{
	[XmlElement(ElementName = "cid")]
	public int CategoryId;

	[XmlElement(ElementName = "cn")]
	public string CategoryName;

	[XmlElement(ElementName = "i", IsNullable = true)]
	public string IconName;
}
