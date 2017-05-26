using MediaPlayer.BaseClasses;
using MediaPlayer.DataAccess.Contracts;
using MediaPlayer.DataContracts;
using System.Collections.Generic;

namespace MediaPlayer.BusinessLogic.Tests.DataAccessMocks
{
    public class SongLibraryDataAccessMock : BaseDataAccessMock, ISongLibraryDataAccess
    {
        public List<SongContract> GetSongTagsByLocation(List<string> files)
        {
            List<SongContract> songs = new List<SongContract>();
            songs.Add(new SongContract()
            {
                Title = "Sparks",
                Album = "Parachutes",
                Artist = "ColdPlay",
                TrackNumber = 4,
                FileLocation = @"testDirectory\MusicLibrary\Sparks",
                Minutes = 3,
                Seconds = 47,
                Year = 2000,
                Format = AudioFileFormat.mp3
            });

            songs.Add(new SongContract()
            {
                Title = "Yellow",
                Album = "Parachutes",
                Artist = "ColdPlay",
                TrackNumber = 5,
                FileLocation = @"testDirectory\MusicLibrary\Yellow",
                Minutes = 4,
                Seconds = 29,
                Year = 2000,
                Format = AudioFileFormat.mp3
            });

            return songs;
        }

        public List<string> GetMP3FilesByDirectory(string directory)
        {
            return new List<string>();
        }
    }
}
