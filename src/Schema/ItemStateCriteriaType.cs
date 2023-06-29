using System.Xml.Serialization;

namespace dragonrescue.Schema;

[Serializable]
public enum ItemStateCriteriaType
{
	[XmlEnum("1")]
	Length = 1,
	
	[XmlEnum("2")]
	ConsumableItem,
	
	[XmlEnum("3")]
	ReplenishableItem,
	
	[XmlEnum("4")]
	SpeedUpItem,
	
	[XmlEnum("5")]
	StateExpiry
}
