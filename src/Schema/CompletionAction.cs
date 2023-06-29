using System.Xml.Serialization;

namespace dragonrescue.Schema;

[Serializable]
public class CompletionAction
{
	[XmlElement(ElementName = "Transition")]
	public StateTransition Transition;
}
