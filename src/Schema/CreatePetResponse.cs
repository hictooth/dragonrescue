using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "CPR", Namespace = "")]
[Serializable]
public class CreatePetResponse
{
	[XmlElement(ElementName = "rpd")]
	public RaisedPetData RaisedPetData { get; set; }

	[XmlElement(ElementName = "cir")]
	public CommonInventoryResponse UserCommonInventoryResponse { get; set; }
}
