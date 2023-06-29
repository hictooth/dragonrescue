using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "RPSK", Namespace = "")]
[Serializable]
public class RaisedPetSkill
{
	[XmlElement(ElementName = "k")]
	public string Key;

	[XmlElement(ElementName = "v")]
	public float Value;

	[XmlElement(ElementName = "ud")]
	public DateTime UpdateDate;
}
