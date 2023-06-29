using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "ImageDataDecal", Namespace = "")]
[Serializable]
public class ImageDataDecal
{
	[XmlElement(ElementName = "Name")]
	public string Name;

	[XmlElement(ElementName = "Position")]
	public ImageDataDecalPosition Position;

	[XmlElement(ElementName = "Width")]
	public int Width;

	[XmlElement(ElementName = "Height")]
	public int Height;
}
