using System.Collections.Generic;
using System.Xml.Serialization;

namespace YouTrackImporter.Data.Models
{
    public class ReportItem
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("imported")]
        public bool Imported { get; set; }
        [XmlElement("error")]
        public List<ReportItemError> Errors { get; set; }
    }
}
