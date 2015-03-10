using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Threading.Tasks;

namespace SongNameCleaner
{
    public class Modifier
    {
        public static int Rename(ArrayList songObjects)
        {
            int unprocessedFileCount = 0;
            foreach (Song song in songObjects)
            {
                try
                {
                    File.Move(song.AbsolutePathOld, song.AbsolutePathNew);
                }
                catch (Exception e)
                {
                    ++unprocessedFileCount;
                }
            }
            return unprocessedFileCount;
        }

        public static int WriteNewId3Tags(ArrayList songObjects)
        {
            int unprocessedFileCount = 0;
            foreach (Song song in songObjects)
            {
                try
                {
                    TagLib.File track = TagLib.File.Create(song.AbsolutePathNew);
                    track.Tag.Title = song.Title;
                    track.Tag.Performers = new string[] { song.Artist };
                    track.Tag.Remixer = new string[] { song.Remixer };
                    track.Save();
                }
                catch (Exception)
                {
                    unprocessedFileCount++;
                }
            }
            return unprocessedFileCount;
        }

        private static string FormTitleWithRemixer(Song song)
        {
            return song.Title + " (" + song.Remixer + " remix)";
        }
    }
}
