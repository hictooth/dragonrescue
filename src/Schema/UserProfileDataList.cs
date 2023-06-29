using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "ArrayOfUserProfileDisplayData")]
[Serializable]
public class UserProfileDataList
{
	[XmlElement(ElementName = "UserProfileDisplayData")]
	public UserProfileData[] UserProfiles;
}
