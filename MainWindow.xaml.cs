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
using System.Collections;
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
            ArrayList ar = StaticNameCleaner.GetSongObjects(oldSongs);
            SongToLabels((Song)ar[0]);
        }

        private void SongToLabels(Song song)
        {
            artistLBL.Content = song.Artist;
            titleLBL.Content = song.Title;
            remixerLBL.Content = song.Remixer;
            outputTB.Text = song.AbsolutePathNew;
        }
    }
}
