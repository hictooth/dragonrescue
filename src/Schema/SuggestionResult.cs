using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "SuggestionResult", Namespace = "", IsNullable = false)]
[Serializable]
public class SuggestionResult
{
	[XmlElement(ElementName = "Suggestion", IsNullable = true)]
	public string[] Suggestion;
}
