﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace YouTrackImporter
{
    class Program
    {
        static Dictionary<ArgumentEnum, string> args;

        enum ArgumentEnum { Transform, Import, Stylesheet, InputUri, ResultFile, Project, YouTrackUri }

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
                Task task = RunImportIssues(Program.args[ArgumentEnum.InputUri], Program.args[ArgumentEnum.YouTrackUri], Program.args[ArgumentEnum.Project]);

                task.Wait();
            }
        }

        static async Task<HttpResponseMessage> RunImportIssues(string inputUri, string baseUri, string project)
        {
            string requestUri = string.Format("rest/import/{0}/issues", project);
            HttpResponseMessage requestResponse = null;

            string content;

            // read content of the file to import
            using (StreamReader reader = new StreamReader(inputUri, Encoding.UTF8))
            {
                content = reader.ReadToEnd();
            }

            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = new CookieContainer() })
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    // set base uri and request headers
                    client.BaseAddress = new Uri(baseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    // login
                    FormUrlEncodedContent loginContent = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("login", "ChristerH"),
                        new KeyValuePair<string, string>("password", "olle123")});
                    HttpResponseMessage loginResponse = await client.PostAsync("/rest/user/login", loginContent);

                    // if login fails then exit
                    if (!loginResponse.IsSuccessStatusCode)
                    {
                        Console.Write("YouTrack server returned an error: {0:d} {1}\r\n", loginResponse.StatusCode, loginResponse.ReasonPhrase);
                        return loginResponse;
                    }

                    // set authentication cookies
                    foreach (string cookieHeader in loginResponse.Headers.Where(h => h.Key == "Set-Cookie").SelectMany(c => c.Value))
                    {
                        handler.CookieContainer.SetCookies(new Uri(baseUri), cookieHeader);
                    }

                    // put the import
                    requestResponse = await client.PutAsync(requestUri, new StringContent(content, Encoding.UTF8, "application/xml"));
                }
            }

            if (requestResponse.IsSuccessStatusCode)
                Console.Write("'{0}' was imported successfully\r\n\r\n", Path.GetFileName(inputUri));
            else
                Console.Write("YouTrack server returned an error: {0:d} {1}\r\n", requestResponse.StatusCode, requestResponse.ReasonPhrase);

            return requestResponse;
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
