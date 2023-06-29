using System.Xml.Serialization;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "Answers", IsNullable = true, Namespace = "")]
[Serializable]
public class ProfileUserAnswer
{
	[XmlElement(ElementName = "AID")]
	public int AnswerID;

	[XmlElement(ElementName = "QID")]
	public int QuestionID;
}
