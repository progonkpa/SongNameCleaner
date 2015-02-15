using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace SongNameCleaner
{
    public class Extractor
    {

        public static ArrayList GetSongObjects(string[] oldSongs)
        {
            ArrayList songObjects = new ArrayList();

            for (int i = 0; i < oldSongs.Length; i++)
            {
                string oldSong = oldSongs[i];
                Song songObj = new Song();
                songObj.AbsolutePathOld = oldSong;

                // Will break up the filename and return the songname as a string. Meanwhile, this method gives value to the public properties Path and Extension of songObj.
                // Example returned string:
                // input    =   "C:\music\Odesza - Bloom (Lane 8 Remix).mp3"
                // output   =   "Odesza - Bloom (Lane 8 Remix)";
                oldSong = DecomposeFileName(oldSong, songObj);

                // Removes leading characters and replaces underscores with spaces. Returns a cleaned up song name string.
                oldSong = InitialCleanUp(oldSong);

                // Determines the index of the first hypen and returns that value.
                // Extracts the artist and places the titlecase string in songObj.Artist property.
                int firstHypen = GetArtist(oldSong, songObj);

                // Extracts the title by extracting the text from the hypen (not included) to the first opening paranthese (not included) if a paranthese exists.
                // The first letter of the title is set to uppercase.
                // The index of the first paranthese is returned.
                int firstParanthese = GetTitle(firstHypen, ref oldSong, songObj);

                // Checks firstParanthese first. If it has a usable value (not -1), the text from that value to the index of the closing paranthese, will be extracted to the songObj.Remixer property.
                GetRemixer(firstParanthese, oldSong, songObj);

                // All previously gathered information in songObj, is used to create a new clean absolute path and written to songObj.AbsolutePathNew property.
                ComposeNewFileName(songObj);

                songObjects.Add(songObj);
            }
            return songObjects;
        }

        private static string DecomposeFileName(string oldSong, Song songObj)
        {
            songObj.Path = Path.GetDirectoryName(oldSong) + @"\";
            songObj.Extension = Path.GetExtension(oldSong);
            return Path.GetFileNameWithoutExtension(oldSong);
        }

        private static string InitialCleanUp(string oldSong)
        {
            oldSong = oldSong.Replace("_", " ");
            oldSong = oldSong.Replace("—", "-");
            oldSong = oldSong.Replace("--", "-");
            oldSong = oldSong.Replace("&quot;", "&");
            oldSong = oldSong.Replace("&amp;", "&");
            oldSong = Regex.Replace(oldSong, @"\s+", " ");
            oldSong.Trim();
            while (Char.IsDigit(oldSong, 0) || oldSong.Substring(0, 1).Equals("-") || oldSong.Substring(0, 1).Equals("."))
            {
                oldSong = oldSong.Substring(1);
            }
            return oldSong;
        }

        private static int GetArtist(string oldSong, Song songObj)
        {
            int firstHypen = oldSong.IndexOf("-");
            if (firstHypen == -1)
            {
                songObj.Artist = oldSong.Substring(0);
            }
            else
            {
                songObj.Artist = oldSong.Substring(0, firstHypen).Trim();
                TextInfo ti = new CultureInfo("en-US", false).TextInfo;
                songObj.Artist = ti.ToTitleCase(songObj.Artist);
            }
            return firstHypen;
        }

        private static int GetTitle(int firstHypen, ref string oldSong, Song songObj)
        {
            if (firstHypen != -1)
            {
                oldSong = oldSong.Substring(firstHypen + 1).Trim();
                string firstLetter = oldSong.Substring(0, 1).ToUpper();
                oldSong = oldSong.Remove(0, 1).Insert(0, firstLetter);
            }
            int firstParanthese = GetFirstParantheseIndex(oldSong);
            return firstParanthese;
        }

        private static void GetRemixer(int firstParanthese, string oldSong, Song songObj)
        {
            if (firstParanthese == -1)
            {
                songObj.Remixer = "";
                if (!songObj.Artist.Equals(oldSong))
                    songObj.Title = oldSong;
            }
            else
            {
                // Get the text between the parantheses
                string remixer = oldSong.Substring(firstParanthese + 1);
                int length = remixer.Length;
                if (remixer.Contains(")"))
                {
                    length = remixer.IndexOf(")", StringComparison.Ordinal);
                }
                else if (remixer.Contains("]"))
                {
                    length = remixer.IndexOf("]", StringComparison.Ordinal);
                }
                else if (remixer.Contains("}"))
                {
                    length = remixer.IndexOf("}", StringComparison.Ordinal);
                }
                remixer = remixer.Substring(0, length);

                // Remove the mix or remix-portion
                string temp = remixer.ToLower();
                if (temp.Contains(" remix"))
                {
                    remixer = remixer.Remove(temp.IndexOf(" remix"), " remix".Length);
                }
                else if (temp.Contains(" mix"))
                {
                    remixer = remixer.Remove(temp.IndexOf(" mix"), " mix".Length);
                }
                songObj.Remixer = remixer;
            }
        }
        private static int GetFirstParantheseIndex(string toExamine)
        {
            int index = -1;
            if (toExamine.Contains("("))
            {
                index = toExamine.IndexOf("(", StringComparison.Ordinal);
            }
            else if (toExamine.Contains("["))
            {
                index = toExamine.IndexOf("[", StringComparison.Ordinal);
            }
            else if (toExamine.Contains("{"))
            {
                index = toExamine.IndexOf("{", StringComparison.Ordinal);
            }
            return index;
        }

        private static void ComposeNewFileName(Song songObj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(songObj.Path);
            sb.Append(songObj.Artist);
            sb.Append(" - ");
            sb.Append(songObj.Title);
            sb.Append(" (");
            sb.Append(songObj.Remixer);
            sb.Append(" remix)");
            sb.Append(songObj.Extension);
            songObj.AbsolutePathNew = sb.ToString();
        }

    }
}