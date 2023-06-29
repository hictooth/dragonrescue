using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "IT", Namespace = "")]
[Serializable]
public class ItemDataTexture
{
	[XmlElement(ElementName = "n")]
	public string TextureName;

	[XmlElement(ElementName = "t")]
	public string TextureTypeName;

	[XmlElement(ElementName = "x", IsNullable = true)]
	public float? OffsetX;

	[XmlElement(ElementName = "y", IsNullable = true)]
	public float? OffsetY;
}
