using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "URR", Namespace = "")]
[Serializable]
public class UserRoomResponse {
    [XmlElement(ElementName = "ur")]
    public List<UserRoom> UserRoomList;
}
