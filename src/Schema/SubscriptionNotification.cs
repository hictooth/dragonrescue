using System.Xml.Serialization;

namespace dragonrescue.Schema;

public class SubscriptionNotification
{
	[XmlElement(ElementName = "Type")]
	public SubscriptionNotificationType Type;
}
