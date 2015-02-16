using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using Ookii.Dialogs.Wpf;
using System.IO;
using System.Windows.Forms;

namespace SongNameCleaner
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private ArrayList songList;

        public TestWindow()
        {
            InitializeComponent();
        }

        private void browseBTN_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            string selectedFolder = "";

            if (Directory.Exists(@"D:\house"))
            {
                dialog.SelectedPath = @"D:\house";
            }

            if (dialog.ShowDialog() == true)
            {
                selectedFolder = dialog.SelectedPath;
            }
            selectedFolderLBL.Content = selectedFolder;

            songList = Extractor.GetSongObjects(GetFileNames(selectedFolder));
        }

        private string[] GetFileNames(string selectedFolder)
        {
            //string[] absolutePathsSongs = Directory.GetFiles(selectedFolder, "*.*", SearchOption.AllDirectories);
            string[] absolutePathsSongs = Directory.GetFiles(selectedFolder, "*.*");
            return absolutePathsSongs;
        }

        private void Id3RewriteBTN_Click(object sender, RoutedEventArgs e)
        {
            int result = Modifier.WriteNewId3Tags(songList);
            if (result == 0)
                id3ResultLBL.Content = "Completed";
            else if (result == 1)
                id3ResultLBL.Content = "Completed with errors. The ID3-tags of " + result + " file could not be rewritten";
            else
                id3ResultLBL.Content = "Completed with errors. The ID3-tags of " + result + " files could not be rewritten";
        }

        private void cleanBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FilenamesRewriteBTN_Click(object sender, RoutedEventArgs e)
        {
            int result = Modifier.Rename(songList);
            if (result == 0)
                fileNamesResultLBL.Content = "Completed";
            else if (result == 1)
                fileNamesResultLBL.Content = "Completed with errors. " + result + " file was not renamed";
            else
                fileNamesResultLBL.Content = "Completed with errors. " + result + " files where not renamed";
        }
    }
}
