using MediaPlayer.BaseClasses;
using MediaPlayer.DataContracts;
using MediaPlayer.UserInterface.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MediaPlayer.UserInterface.ViewModel.Utilities
{
    public static class SongUtility
    {
        public static List<SongModel> GetSongTagsByFiles(List<string> files)
        {
            try
            {
                List<SongModel> songs = new List<SongModel>();

                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        songs.Add(GetSongModelFromFile(file));
                    }
                }

                return new List<SongModel>(songs
                    .OrderBy(x => x.Artist)
                    .ThenBy(x => x.Album)
                    .ThenBy(x => x.TrackNumber));
            }
            catch (Exception ex)
            {
                throw new MediaPlayerException("Failed to retrieve information for a song in your library.", ex);
            }
        }

        public static List<string> GetMP3FilesByDirectory(string directory)
        {
            try
            {
                MediaPlayerAssert.FolderExists(directory);

                List<string> mp3Files = new List<string>();
                EnumerateFiles(directory, mp3Files);

                return mp3Files;
            }
            catch (Exception ex)
            {
                throw new MediaPlayerException(ex.Message, ex);
            }
        }

        private static void EnumerateFiles(string directory, List<string> mp3Files)
        {
            foreach (string subDirectory in Directory.GetDirectories(directory).AsEnumerable().ToList())
            {
                try
                {
                    EnumerateFiles(subDirectory, mp3Files);
                }
                catch
                {
                    // do nothing
                }
            }

            mp3Files.AddRange(Directory.EnumerateFiles(directory, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => s.EndsWith(".mp3")).ToList());
        }

        private static SongModel GetSongModelFromFile(string file)
        {
            TagLib.File song = TagLib.File.Create(file);

            SongModel songModel = new SongModel();
            songModel.Title = String.IsNullOrWhiteSpace(song.Tag.Title) ? String.Empty : song.Tag.Title;
            songModel.Album = String.IsNullOrWhiteSpace(song.Tag.Album) ? String.Empty : song.Tag.Album;
            songModel.TrackNumber = song.Tag.Track;
            songModel.Year = song.Tag.Year;
            songModel.Genre = song.Tag.FirstGenre;
            songModel.Minutes = song.Properties.Duration.Minutes;
            songModel.Seconds = song.Properties.Duration.Seconds;
            songModel.Format = AudioFileFormat.mp3;
            songModel.FileLocation = file;

            if (song.Tag.AlbumArtists.Length > 0)
            {
                songModel.Artist = song.Tag.AlbumArtists[0];
            }
            else if (song.Tag.Artists.Length > 0)
            {
                songModel.Artist = song.Tag.Artists[0];
            }
            else
            {
                songModel.Artist = "unknown";
            }

            return songModel;
        }
    }
}