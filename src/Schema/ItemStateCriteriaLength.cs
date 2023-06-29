using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "ItemStateCriteriaLength", Namespace = "")]
[Serializable]
public class ItemStateCriteriaLength : ItemStateCriteria
{
	[XmlElement(ElementName = "Period")]
	public int Period;
}
