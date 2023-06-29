using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "ARM", IsNullable = true, Namespace = "")]
[Serializable]
public class RewardMultiplier
{
	[XmlElement(ElementName = "PT")]
	public int PointTypeID;

	[XmlElement(ElementName = "MF")]
	public int MultiplierFactor;

	[XmlElement(ElementName = "MET")]
	public DateTime MultiplierEffectTime;
}
