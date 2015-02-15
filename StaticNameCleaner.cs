using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections;

namespace SongNameCleaner
{
    public class StaticNameCleaner
    {

        public static ArrayList GetSongObjects(string[] oldSongs)
        {
            ArrayList songObjects = new ArrayList();

            for (int i = 0; i < oldSongs.Length; i++)
            {
                string oldSong = oldSongs[i];
                Song songObj = new Song();
                songObj.AbsolutePathOld = oldSong;

                // Will break up the filename to the variables path, oldSong and extension.
                // Example for oldSong variable:
                // C:\music\Odesza - Bloom (Lane 8 Remix).mp3   =>  oldSong = "Odesza - Bloom (Lane 8 Remix)";
                oldSong = DecomposeFileName(oldSong, songObj);

                // Removes leading characters and replaces underscores with spaces.
                oldSong = InitialCleanUp(oldSong);

                // Determines the index of the first hypen.
                // Extracts the artist from oldSong and places the titlecase string in the artist variable.
                int firstHypen = GetArtist(oldSong, songObj);

                // Extracts the title by extracting the text from the hypen (not included) to the first opening paranthese (not included) if a paranthese exists.
                // The first letter of the title is set to uppercase.
                int firstParanthese = GetTitle(firstHypen, ref oldSong, songObj);

                // Checks the firstParanthese variable first. If it has a usable value, the text from that value to the index of the closing paranthese, will be extracted to the remixer variable.
                GetRemixer(firstParanthese, oldSong, songObj);

                ComposeNewFileName(songObj);

                // Write artist, title, remixer, old absolute path and new absolute path to a song object and add to arraylist.
                songObjects.Add(songObj);
            }
            return songObjects;
        }

        private static string DecomposeFileName(string oldSong, Song songObj)
        {
            songObj.Path = Path.GetDirectoryName(oldSong);
            songObj.Extension = Path.GetExtension(oldSong);
            return Path.GetFileNameWithoutExtension(oldSong);
        }

        private static string InitialCleanUp(string oldSong)
        {
            oldSong = oldSong.Replace("_", " ");
            oldSong.Trim();
            while (Char.IsDigit(oldSong, 0) || oldSong.Substring(0, 1).Equals("-") || oldSong.Substring(0, 1).Equals(".") || oldSong.Substring(0, 1).Equals("_"))
            {
                oldSong = oldSong.Substring(1);
            }
            return oldSong;
        }

        private static int GetArtist(string oldSong, Song songObj)
        {
            int firstHypen = oldSong.IndexOf("-");
            songObj.Artist = oldSong.Substring(0, firstHypen).Trim();
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;
            songObj.Artist = ti.ToTitleCase(songObj.Artist);
            return firstHypen;
        }

        private static int GetTitle(int firstHypen, ref string oldSong, Song songObj)
        {
            oldSong = oldSong.Substring(firstHypen + 1).Trim();
            string firstLetter = oldSong.Substring(0, 1).ToUpper();
            oldSong = oldSong.Remove(0, 1).Insert(0, firstLetter);
            int firstParanthese = GetFirstParantheseIndex(oldSong);
            if (firstParanthese == -1)
            {
                songObj.Remixer = "";
                songObj.Title = oldSong;
            }
            else
            {
                songObj.Title = oldSong.Substring(0, firstParanthese).Trim();
            }
            return firstParanthese;
        }

        private static void GetRemixer(int firstParanthese, string oldSong, Song songObj)
        {
            if (firstParanthese != -1)
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
                    remixer = remixer.Remove(temp.IndexOf(" remix", " remix".Length));
                }
                else if (temp.Contains(" mix"))
                {
                    remixer = remixer.Remove(temp.IndexOf(" mix", " mix".Length));
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