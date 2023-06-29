using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "AT", Namespace = "")]
[Serializable]
public class ItemAttribute
{
	[XmlElement(ElementName = "k")]
	public string Key;

	[XmlElement(ElementName = "v")]
	public string Value;
}
