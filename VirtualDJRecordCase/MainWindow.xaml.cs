using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace VirtualDJRecordCase;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string _playlistDirectory = @"Z:\Users\Ward\Dropbox\VirtualDJ\History";
    private Dictionary<string, Song> songs = new();

    public MainWindow()
    {
        InitializeComponent();
        string xmlFilePath = @"Z:\Users\Ward\Dropbox\VirtualDJ\database.xml";

        XElement xElementDoc = new XElement("VirtualDJ_Database");
        try
        {
            xElementDoc = XElement.Load(xmlFilePath,LoadOptions.PreserveWhitespace);
            string[] playlists = Directory.GetFiles(_playlistDirectory, "*.m3u", SearchOption.AllDirectories);
            foreach (string playlist in playlists)
            {
                AddSongsFromPlaylist(playlist);
            }
        }
        catch (IOException e)
        {
            var error = e.Message + "\n" + e.StackTrace;
            MessageBox.Show(error);
            this.Close();
        }

        var songNodes = xElementDoc.Elements("Song");
        foreach (XElement songNode in songNodes)
        {
            var filePath = songNode?.Attribute("FilePath")?.Value;
            var artist = songNode?.Element("Tags")?.Attribute("Author")?.Value;
            var title = songNode?.Element("Tags")?.Attribute("Title")?.Value;
            var playCountString = songNode?.Element("Infos")?.Attribute("PlayCount")?.Value;
            if (playCountString == null) continue;
            var playCount = int.Parse(playCountString);
            try
            {
                var value = songs[filePath];
                songs[filePath].PlayCount += playCount;
                songNode?.Element("Infos")?.SetAttributeValue("PlayCount", songs[filePath].PlayCount.ToString());
                continue;
            }
            catch (KeyNotFoundException)
            { // not found
                songs.Add(filePath, new Song() { PlayCount = playCount, Url = filePath, artist = artist, title = title });
            }
        }
        SongList.DataContext = songs;
       // xElementDoc.Save(xmlFilePath, SaveOptions.DisableFormatting);
    }

    private void AddSongsFromPlaylist(string playlist)
    {
        string m3uString = File.ReadAllText(playlist);
        string[] lines = m3uString.Split('\n');
        var currentSong = new Song();

        foreach (string line in lines)
        {
            // Skip empty lines and comments (lines that start with "#")
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
            {
                if (line.StartsWith("#EXTVDJ:"))
                {
                    XmlDocument doc = new XmlDocument();
                    XmlDeclaration xmldecl;
                    string xmlString = (line.Replace(@"#EXTVDJ:", @"<Song>") + @"</Song>").Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;");
                    doc.Load(new StringReader(xmlString));
                    xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmldecl, root);
                    currentSong = GetSongFromString(doc);
                }
                continue;
            }
            var newLine = line.Replace(@"D:\", @"C:\D\").Replace(@"E:\", @"C:\D\").TrimEnd(Environment.NewLine.ToCharArray());
            if (songs.TryGetValue(newLine, out Song value))
            {
                value.Url = newLine;
                value.Artist = currentSong.artist;
                value.Title = currentSong.title;
                value.Time = currentSong.songlength;
                value.LastPlayTime = currentSong.lastplaytime;
                value.FileSize = currentSong.filesize;
                value.PlayCountFromLogs++;
            }
            else
            {
                // The song is not in the collection, so add it with a play count of 1
                Song song = new()
                {
                    Url = newLine.TrimEnd(Environment.NewLine.ToCharArray()),
                    PlayCountFromLogs = 1
                };

                // You can add other properties to the Song object, such as the title, artist, etc.
                // You can also use the URL to download the song's metadata (e.g. using the ID3 tags in the MP3 file)

                songs.Add(newLine.TrimEnd(Environment.NewLine.ToCharArray()), song);
            }
        }
    }

    private Song GetSongFromString(XmlDocument song)
    {
        Song resultSong = null;
        XmlSerializer serializer = new XmlSerializer(typeof(Song));
        using (XmlReader rdr = new XmlNodeReader(song))
        {
            resultSong = (Song)serializer.Deserialize(rdr);
        }
        return resultSong;
    }
}