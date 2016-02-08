using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace YouTrackImporter.Data.Models
{
    [XmlRoot("importReport")]
    public class Report
    {
        [XmlElement("item")]
        public List<ReportItem> Items { get; set; }

        public static Report Serialize(Stream ResponseContent)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Report));

            return (Report)serializer.Deserialize(ResponseContent);
        }
    }
}
