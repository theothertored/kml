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
            kclip.ClipModules = KModule.SubmodulesNodeToKModuleList(submodulesNode);

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
