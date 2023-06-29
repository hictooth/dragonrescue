using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "GetGroupsResult", IsNullable = true)]
[Serializable]
public class GetGroupsResult {
    [XmlElement(ElementName = "Success")]
    public bool Success;

    [XmlElement(ElementName = "Groups")]
    public Group[]? Groups;

    [XmlElement(ElementName = "RolePermissions", IsNullable = true)]
    public List<RolePermission>? RolePermissions;
}
