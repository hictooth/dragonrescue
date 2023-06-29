using System.Xml.Serialization;

namespace dragonrescue.Schema;

public enum RaisedPetSetResult
{
	[XmlEnum("1")]
	Success = 1,
	
	[XmlEnum("2")]
	Failure,
	
	[XmlEnum("3")]
	Invalid,
	
	[XmlEnum("4")]
	InvalidPetName
}
