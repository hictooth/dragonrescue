using System.Xml.Serialization;

namespace dragonrescue.Schema;

public enum ApiTokenStatus {
    [XmlEnum("1")]
    TokenValid = 1,
    [XmlEnum("3")]
    TokenNotFound = 3
}
