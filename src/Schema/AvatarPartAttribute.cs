using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "Attribute", Namespace = "")]
[Serializable]
public class AvatarPartAttribute
{
	[XmlElement(ElementName = "K")]
	public string Key;

	[XmlElement(ElementName = "V")]
	public string Value;
}
