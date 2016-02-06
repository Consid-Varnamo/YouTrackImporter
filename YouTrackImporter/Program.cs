using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace YouTrackImporter
{
    class Program
    {
        static Dictionary<string, string> args;

        static void Main(string[] args)
        {
            Program.args = ParseAguments(args);
            Transform(@"d:\users\christer\documents\visual studio 2015\Projects\YouTrackImporter\YouTrackImporter\Data\Transform\backlog-transform.xslt",
                @"d:\users\christer\documents\visual studio 2015\Projects\YouTrackImporter\YouTrackImporter\Data\backlog.xml",
                @"d:\users\christer\documents\visual studio 2015\Projects\YouTrackImporter\YouTrackImporter\Data\backlog-tranformed.xml");
        }

        private static void Transform(string stylesheetUri, string inputUri, string resultFile)
        {
            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(stylesheetUri);

            xsl.Transform(inputUri, resultFile);
        }

        static Dictionary<string, string> ParseAguments(string[] args)
        {
            var result = from s in args
                         where Regex.IsMatch(s, "^[/-]")
                         let pos = Array.IndexOf(args, s)
                         select pos + 1 < args.Length && !Regex.IsMatch(args[pos + 1], "^[/-]") ? new { Key = s, Value = args[pos + 1] } : new { Key = s, Value = "" };

            return result.ToDictionary(o => o.Key, o => o.Value);
        }
    }
}
