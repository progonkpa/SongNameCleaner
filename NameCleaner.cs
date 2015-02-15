using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace SongNameCleaner
{
    internal class NameCleaner
    {
        private string[] oldSongs;
        private string[] newSongs;
        private readonly TextInfo ti;
        private string artist;
        private string title;
        private string remixer;
        private string extension;
        private int firstHypen;
        private int firstParanthese;
        private string path;
        private string oldSong;


        public NameCleaner(string[] oldSongs)
        {
            this.oldSongs = oldSongs;
            newSongs = new string[oldSongs.Length];
            ti = new CultureInfo("en-US", false).TextInfo;
            firstParanthese = -1;
            GetNewFileNames();
        }

        public string[] GetNewFileNames()
        {
            for (int i = 0; i < oldSongs.Length; i++)
            {
                oldSong = oldSongs[i];

                // Will break up the filename to the variables path, oldSong and extension.
                // Example for oldSong variable:
                // C:\music\Odesza - Bloom (Lane 8 Remix).mp3   =>  oldSong = "Odesza - Bloom (Lane 8 Remix)";
                DecomposeFileName();

                // Removes leading characters and replaces underscores with spaces.
                InitialCleanUp();

                // Determines the index of the first hypen.
                // Extracts the artist from oldSong and places the titlecase string in the artist variable.
                GetArtist();

                // Extracts the title by extracting the text from the hypen (not included) to the first opening paranthese (not included) if a paranthese exists.
                // The first letter of the title is set to uppercase.
                GetTitle();

                // Checks the firstParanthese variable first. If it has a usable value, the text from that value to the index of the closing paranthese, will be extracted to the remixer variable.
                GetRemixer();

                string newSong = ComposeNewFileName();
                newSongs[i] = newSong;
            }
            return newSongs;
        }


        private void DecomposeFileName()
        {
            path = Path.GetDirectoryName(oldSong);
            extension = Path.GetExtension(oldSong);
            oldSong = Path.GetFileNameWithoutExtension(oldSong);
        }

        private void InitialCleanUp()
        {
            oldSong = oldSong.Replace("_", " ");
            oldSong.Trim();
            while (Char.IsDigit(oldSong, 0) || oldSong.Substring(0, 1).Equals("-") || oldSong.Substring(0, 1).Equals(".") || oldSong.Substring(0, 1).Equals("_"))
            {
                oldSong = oldSong.Substring(1);
            }
        }

        private void GetArtist()
        {
            firstHypen = oldSong.IndexOf("-");
            artist = oldSong.Substring(0, firstHypen).Trim();
            artist = ti.ToTitleCase(artist);
        }

        private void GetTitle()
        {
            oldSong = oldSong.Substring(firstHypen + 1).Trim();
            string firstLetter = oldSong.Substring(0, 1).ToUpper();
            title = oldSong.Remove(0, 1).Insert(0, firstLetter);
            firstParanthese = GetFirstParantheseIndex(title);
            if (firstParanthese == -1)
            {
                remixer = "";
            }
            else
            {
                title = title.Substring(0, firstParanthese).Trim();
            }
        }

        private void GetRemixer()
        {
            if (firstParanthese != -1)
            {
                // Get the text between the parantheses
                remixer = oldSong.Substring(firstParanthese + 1);
                firstParanthese = -1;
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
            }
        }
        private int GetFirstParantheseIndex(string toExamine)
        {
            int index = -1;
            if (toExamine.Contains("("))
            {
                index = toExamine.IndexOf("(", StringComparison.Ordinal);
            }
            else if (title.Contains("["))
            {
                index = toExamine.IndexOf("[", StringComparison.Ordinal);
            }
            else if (title.Contains("{"))
            {
                index = toExamine.IndexOf("{", StringComparison.Ordinal);
            }
            return index;
        }

        private string ComposeNewFileName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(path);
            sb.Append(artist);
            sb.Append(" - ");
            sb.Append(title);
            sb.Append(" (");
            sb.Append(remixer);
            sb.Append(" remix)");
            sb.Append(extension);
            return sb.ToString();
        }

        public Song GetSong()
        {
            Song songObj = new Song();
            songObj.artist = artist;
            songObj.title = title;
            songObj.remixer = remixer;
            return songObj;
        }
    }
}