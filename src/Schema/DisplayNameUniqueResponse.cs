using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "DisplayNameUniqueResponse", Namespace = "")]
[Serializable]
public class DisplayNameUniqueResponse
{
	[XmlElement(ElementName = "suggestions", IsNullable = true)]
	public SuggestionResult Suggestions { get; set; }
}
