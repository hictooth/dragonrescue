using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "RPST", Namespace = "")]
[Serializable]
public class RaisedPetState
{
	[XmlElement(ElementName = "k")]
	public string Key;

	[XmlElement(ElementName = "v")]
	public float Value;
}
