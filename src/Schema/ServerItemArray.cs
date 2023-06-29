using System.Xml.Serialization;

namespace dragonrescue.Schema {

    // NOTE: This schema is NOT used by the client
    // This is a schema specific to the sodoff server
    [XmlRoot(ElementName = "Items", Namespace = "")]
    public class ServerItemArray {
        [XmlElement(ElementName = "I", IsNullable = true)]
        public ItemData[] ItemDataArray;
    }
}
