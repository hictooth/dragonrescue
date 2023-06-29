using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "ItemStateCriteriaConsumable", Namespace = "")]
[Serializable]
public class ItemStateCriteriaConsumable : ItemStateCriteria
{
	[XmlElement(ElementName = "ItemID")]
	public int ItemID;

	[XmlElement(ElementName = "ConsumeUses")]
	public bool ConsumeUses;

	[XmlElement(ElementName = "Amount")]
	public int Amount;
}
