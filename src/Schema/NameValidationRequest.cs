using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "NameValidationRequest", Namespace = "")]
public class NameValidationRequest
{
	[XmlElement(ElementName = "Name")]
	public string Name;

	[XmlElement(ElementName = "Category")]
	public NameCategory Category;
}
