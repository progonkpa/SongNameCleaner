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
using System.Windows.Shapes;
using System.Windows;
using Ookii.Dialogs;

namespace SongNameCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] oldSongs = { inputTB.Text };
            NameCleaner nc = new NameCleaner(oldSongs);
            string[] newSongs = nc.GetNewFileNames();
            outputTB.Text = newSongs[0];
            SongToLabels(nc.GetSong());
        }

        private void SongToLabels(Song song)
        {
            artistLBL.Content = song.artist;
            titleLBL.Content = song.title;
            remixerLBL.Content = song.remixer;
        }
    }
}
