using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongNameCleaner
{
    class Song
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Remixer { get; set; }
        public string Path { get; set; }
        public string AbsolutePathOld { get; set; }
        public string AbsolutePathNew { get; set; }
        public string Extension { get; set; }
        public bool HasRemixer { get; set; }
    }
}
