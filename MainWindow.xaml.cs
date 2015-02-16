using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows;
using System.Collections;
using Ookii.Dialogs.Wpf;

namespace SongNameCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArrayList songList;

        public MainWindow()
        {
            InitializeComponent();
            //string a = "Jos", b = null;
            //TestLabel.Content = a + b;
            //string selectedFolder = @"D:\house";
            //selectedFolderLBL.Content = selectedFolder;
            //songList = Extractor.GetSongObjects(GetFileNames(selectedFolder));
        }

        private void browseBTN_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            string selectedFolder = "";

            if (dialog.ShowDialog() == true)
            {
                selectedFolder = dialog.SelectedPath;
            }
            selectedFolderLBL.Content = selectedFolder;

            if (selectedFolder != String.Empty)
            {
                try
                {
                    songList = Extractor.GetSongObjects(GetFileNames(selectedFolder));
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to find or access this folder. It might not exist or you might not have sufficient permissions to access it.\n\rNo Files where selected. Please try again.", "Folder inaccessible", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FilenamesRewriteBTN_Click(object sender, RoutedEventArgs e)
        {
            fileNamesResultLBL.Content = "Working...";
            try
            {
                int result = Modifier.Rename(songList);
                if (result == 0)
                    fileNamesResultLBL.Content = "Completed.";
                else if (result == 1)
                    fileNamesResultLBL.Content = "Completed with errors. " + result + " file was not renamed.";
                else
                    fileNamesResultLBL.Content = "Completed with errors. " + result + " files where not renamed.";
            }
            catch (NullReferenceException ex)
            {
                fileNamesResultLBL.Content = "No files where selected or processed.";
                Console.Error.Write(ex.StackTrace);
            }

        }

        private void Id3RewriteBTN_Click(object sender, RoutedEventArgs e)
        {
            fileNamesResultLBL.Content = "Working...";
            try
            {
                int result = Modifier.WriteNewId3Tags(songList);
                if (result == 0)
                    id3ResultLBL.Content = "Completed.";
                else if (result == 1)
                    id3ResultLBL.Content = "Completed with errors. The ID3-tags of " + result + " file could not be rewritten.";
                else
                    id3ResultLBL.Content = "Completed with errors. The ID3-tags of " + result + " files could not be rewritten.";
            }
            catch (NullReferenceException ex)
            {
                fileNamesResultLBL.Content = "No files where selected or processed.";
                Console.Error.Write(ex.StackTrace);
            }
        }

        private string[] GetFileNames(string selectedFolder)
        {
            //string[] absolutePathsSongs = Directory.GetFiles(selectedFolder, "*.*", SearchOption.AllDirectories);
            string[] absolutePathsSongs = null;
            absolutePathsSongs = Directory.GetFiles(selectedFolder, "*.*");
            return absolutePathsSongs;
        }

    }
}
