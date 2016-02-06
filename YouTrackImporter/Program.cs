using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace YouTrackImporter
{
    class Program
    {
        static Dictionary<ArgumentEnum, string> args;

        enum ArgumentEnum { Transform, Import, Stylesheet, InputUri, ResultFile }

        /// <summary>
        /// Executes Xslt transformation to YouTrack Xml or import issues to YouTrack
        /// </summary>
        /// <param name="args"></param>
        /// <example>Transform: YouTrackImporter.exe -Transform -StyleSheet [Xslt stylesheet path} -InputUri [input file path] -ResultFile [result file path]</example>

        static void Main(string[] args)
        {
            Program.args = ParseAguments(args);

            if (Program.args.Keys.Any(k => k == ArgumentEnum.Transform))
            {
                Transform(Program.args[ArgumentEnum.Stylesheet], Program.args[ArgumentEnum.InputUri], Program.args[ArgumentEnum.ResultFile]);
 
            }
            if (Program.args.Keys.Any(k => k == ArgumentEnum.Import))
            {
                ImportIssues(Program.args[ArgumentEnum.InputUri]);
            }
                
        }

        static void ImportIssues(string inputUri)
        {
            Console.Write("'{0}' was imported successfully\r\n\r\n", Path.GetFileName(inputUri));
        }

        static void Transform(string stylesheetUri, string inputUri, string resultFile)
        {
            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(stylesheetUri);

            xsl.Transform(inputUri, resultFile);

            Console.Write("'{0}' was transformed to '{1}' successfully.\r\n\r\n", Path.GetFileName(inputUri), resultFile);
        }

        static Dictionary<ArgumentEnum, string> ParseAguments(string[] args)
        {
            var result = from s in args
                         where Regex.IsMatch(s, "^[/-]")
                         let pos = Array.IndexOf(args, s)
                         select pos + 1 < args.Length && !Regex.IsMatch(args[pos + 1], "^[/-]") ? new { Key = s.ToLower(), Value = args[pos + 1] } : new { Key = s.ToLower(), Value = "" };

            return result.ToDictionary(o => (ArgumentEnum)Enum.Parse(typeof(ArgumentEnum), o.Key.Substring(1, o.Key.Length - 1), true), o => o.Value);
        }
    }
}
