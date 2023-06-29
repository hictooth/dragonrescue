using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "RPAT", Namespace = "")]
[Serializable]
public class RaisedPetAttribute
{
	[XmlElement(ElementName = "k")]
	public string Key;

	[XmlElement(ElementName = "v")]
	public string Value;

	[XmlElement(ElementName = "dt")]
	public DataType Type;
}
