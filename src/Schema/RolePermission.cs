using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "RP", IsNullable = true)]
[Serializable]
public class RolePermission {
    [XmlElement(ElementName = "G")]
    public GroupType GroupType;

    [XmlElement(ElementName = "R")]
    public UserRole Role;

    [XmlElement(ElementName = "P")]
    public List<string> Permissions;
}
