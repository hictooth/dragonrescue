using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "ReplenishableRate", Namespace = "")]
[Serializable]
public class ReplenishableRate
{
	[XmlElement(ElementName = "Uses")]
	public int Uses;

	[XmlElement(ElementName = "Rate")]
	public double Rate;

	[XmlElement(ElementName = "MaxUses")]
	public int MaxUses;

	[XmlElement(ElementName = "Rank", IsNullable = true)]
	public int? Rank;
}
