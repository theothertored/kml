using kml2kclip.Exceptions;
using kml2kclip.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kclip
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            var xmlDoc = new XmlDocument();
            string outFilename = null;

            try
            {
                if (args.Length == 0)
                {
                    xmlDoc.Load(Console.In);
                }
                else
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == "-in")
                        {
                            if (i == args.Length - 1)
                            {
                                Console.Write("Provide a filename for the -in argument.");
                                return 1;
                            }

                            xmlDoc.Load(args[i + 1]);
                            i++; // skip the next argument
                        }
                        else if (args[i] == "-out")
                        {
                            if (i == args.Length - 1)
                            {
                                Console.Write("Provide a filename for the -out argument.");
                                return 1;
                            }

                            outFilename = args[i + 1];
                            i++; // skip the next argument
                        }
                        else
                        {
                            Console.Write("Invalid argument detected: " + args[i]);
                            return 1;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not load XML file:");
                Console.WriteLine(ex.Message);
                return 1;
            }

            var presetNode = xmlDoc.SelectSingleNode("preset");
            var submodulesNode = presetNode.SelectSingleNode("submodules");

            var kclip = new KClip();

            try
            {
                kclip.ClipModules = KModule.SubmodulesNodeToKModuleList(submodulesNode);
            }
            catch (KmlParseException ex)
            {
                Console.WriteLine("Error when parsing KML:");
                Console.WriteLine(ex.Message);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception:");
                Console.WriteLine(ex.ToString());
                return 1;
            }

            var serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            var serialized = JsonConvert.SerializeObject(kclip, serializerSettings);

            var result = new StringBuilder()
                .AppendLine("##KUSTOMCLIP##")
                .AppendLine(serialized)
                .AppendLine("##KUSTOMCLIP##")
                .ToString();

            if (outFilename != null)
            {
                File.WriteAllText(outFilename, result);
            }
            else
            {
                Console.Write(result);
            }

            return 0;
        }
    }
}
