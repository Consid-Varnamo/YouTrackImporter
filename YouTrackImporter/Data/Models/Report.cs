using System.Collections.Generic;
using System.IO;
using log4net;
using System.Xml.Serialization;

namespace YouTrackImporter.Data.Models
{
    [XmlRoot("importReport")]
    public class Report
    {
        private static ILog logger = LogManager.GetLogger(typeof(Report));

        [XmlElement("item")]
        public List<ReportItem> Items { get; set; }

        public static Report Deserialize(Stream responseContentStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Report));

            return (Report)serializer.Deserialize(responseContentStream);
        }

        public void WriteToLog()
        {
            foreach(ReportItem item in Items)
            {
                if (item.Imported)
                {
                    logger.InfoFormat("Issue {0} {1} was imported successfully", item.Id, item.Message);
                }
                else
                {
                    logger.WarnFormat("Could not import issue {0} {1}", item.Id, item.Message);
                    foreach(ReportItemError error in item.Errors)
                    {
                        logger.WarnFormat("\t- field '{0}' with value '{1}' caused the error '{2}'", error.FieldName, error.Value, error.Message);
                    }
                }
            }
        }
    }
}
