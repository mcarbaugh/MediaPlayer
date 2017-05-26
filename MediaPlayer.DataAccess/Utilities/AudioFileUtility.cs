using MediaPlayer.BaseClasses;
using MediaPlayer.DataContracts;
using System;

namespace MediaPlayer.DataAccess.Utilities
{
    public static class AudioFileUtility
    {
        public static SongContract ConvertMp3ToSongContract(string file)
        {
            TagLib.File song = TagLib.File.Create(file);

            SongContract songContract = new SongContract();
            songContract.Title = String.IsNullOrWhiteSpace(song.Tag.Title) ? String.Empty : song.Tag.Title;
            songContract.Album = String.IsNullOrWhiteSpace(song.Tag.Album) ? String.Empty : song.Tag.Album;
            songContract.TrackNumber = song.Tag.Track;
            songContract.Year = song.Tag.Year;
            songContract.Genre = song.Tag.FirstGenre;
            songContract.Minutes = song.Properties.Duration.Minutes;
            songContract.Seconds = song.Properties.Duration.Seconds;
            songContract.Format = AudioFileFormat.mp3;
            songContract.FileLocation = file;

            if (song.Tag.AlbumArtists.Length > 0)
            {
                songContract.Artist = song.Tag.AlbumArtists[0];
            }
            else if (song.Tag.Artists.Length > 0)
            {
                songContract.Artist = song.Tag.Artists[0];
            }
            else
            {
                songContract.Artist = "unknown";
            }

            return songContract;
        }
    }
}
