using System.Xml.Serialization;

namespace dragonrescue.Schema;

public enum NameCategory
{
	[XmlEnum("1")]
	Avatar = 1,

	[XmlEnum("2")]
	Pet,

	[XmlEnum("3")]
	Group,

	[XmlEnum("4")]
	Default
}
