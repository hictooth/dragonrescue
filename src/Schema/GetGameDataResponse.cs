using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using dragonrescue.Schema;

namespace dragonrescue.Schema;

[XmlRoot(ElementName = "GetGameDataResponse", Namespace = "")]
[Serializable]
public class GetGameDataResponse {
    [XmlElement(ElementName = "GameDataSummaryList")]
    public List<GameDataSummary> GameDataSummaryList { get; set; }
}
