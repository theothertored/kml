using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kml2kclip.Models
{
    public class KClip
    {
        public int ClipVersion { get; set; }
        public bool ClipCut => false;
        public List<KModule> ClipModules { get; set; }
    }
}
