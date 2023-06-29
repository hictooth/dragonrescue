using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "ISM", Namespace = "", IsNullable = false)]
[Serializable]
public class ItemStatsMap
{
	[XmlElement(ElementName = "IID", IsNullable = false)]
	public int ItemID { get; set; }

	[XmlElement(ElementName = "IT", IsNullable = false)]
	public ItemTier ItemTier { get; set; }

	[XmlElement(ElementName = "ISS", IsNullable = false)]
	public ItemStat[] ItemStats { get; set; }
}
