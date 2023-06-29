using System;
using System.Xml.Serialization;

namespace dragonrescue.Schema;

public class ArrayOfMessage {
    [XmlElement(ElementName = "Message")]
    public Message[] Messages;
}
