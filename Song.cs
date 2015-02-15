using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongNameCleaner
{
    class Song
    {
        public string artist { get; set; }
        public string title { get; set; }
        public string remixer { get; set; }
        public string absolutePathOld { get; set; }
        public string absolutePathNew { get; set; }
    }
}
