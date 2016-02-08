using System.Xml.Serialization;

namespace YouTrackImporter.Data.Models
{
    public class ReportItemError
    {
        [XmlAttribute("fieldName")]
        public string FieldName { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
        [XmlText]
        public string Message { get; set; }
    }
}
