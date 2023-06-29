using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "NameValidationResponse", Namespace = "")]
public class NameValidationResponse
{
	[XmlElement(ElementName = "ErrorMessage")]
	public string ErrorMessage;

	[XmlElement(ElementName = "Category")]
	public NameValidationResult Result;
}
