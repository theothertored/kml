using kml2kustomclip.Exceptions;
using kml2kustomclip.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kml2kustomclip
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(Console.In);

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
            Console.Write(serialized);

            var result = new StringBuilder()
                .AppendLine("##KUSTOMCLIP##")
                .AppendLine(serialized)
                .AppendLine("##KUSTOMCLIP##")
                .ToString();

            System.Windows.Forms.Clipboard.SetText(result);

            return 0;
        }
    }
}
