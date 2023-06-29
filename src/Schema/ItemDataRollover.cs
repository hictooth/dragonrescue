using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "IRO", Namespace = "")]
[Serializable]
public class ItemDataRollover
{
	[XmlElement(ElementName = "d")]
	public string DialogName;

	[XmlElement(ElementName = "b")]
	public string Bundle;
}
