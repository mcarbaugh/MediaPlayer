using MediaPlayer.BaseClasses;
using MediaPlayer.BusinessLogic.Tests.DataAccessMocks;
using MediaPlayer.DataAccess;
using MediaPlayer.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MediaPlayer.BusinessLogic.Tests
{
    [TestClass]
    public class SongLibraryManagerTests
    {
        [TestMethod]
        public void SongLibraryDataAccess_IsNotNull_WhenInstantiated()
        {
            SongLibraryManager songLibraryManager = new SongLibraryManager(new SongLibraryDataAccess());
            Assert.IsNotNull(songLibraryManager.SongLibraryDataAccess);
        }

        [TestMethod]
        public void GetSongTagsByLocation_ReturnsTags_WhenInputIsValid()
        {
            SongLibraryManager songLibraryManager = new SongLibraryManager(new SongLibraryDataAccessMock());
            List<string> files = new List<string>();
            files.Add(@"testDirectory\MusicLibrary\Sparks");
            files.Add(@"testDirectory\MusicLibrary\Yellow");
            List<SongContract> songTags = songLibraryManager.GetSongTagsByFile(files);
            Assert.IsNotNull(songTags);

            SongContract songTag1 = new SongContract()
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
            };

            SongContract songTag2 = new SongContract()
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
            };

            Assert.IsNotNull(songTags[0]);
            Assert.AreEqual(songTag1.Title, songTags[0].Title);
            Assert.AreEqual(songTag1.Album, songTags[0].Album);
            Assert.AreEqual(songTag1.Artist, songTags[0].Artist);
            Assert.AreEqual(songTag1.TrackNumber, songTags[0].TrackNumber);
            Assert.AreEqual(songTag1.FileLocation, songTags[0].FileLocation);
            Assert.AreEqual(songTag1.Minutes, songTags[0].Minutes);
            Assert.AreEqual(songTag1.Seconds, songTags[0].Seconds);
            Assert.AreEqual(songTag1.Year, songTags[0].Year);
            Assert.AreEqual(songTag1.Format, songTags[0].Format);

            Assert.IsNotNull(songTags[1]);
            Assert.AreEqual(songTag2.Title, songTags[1].Title);
            Assert.AreEqual(songTag2.Album, songTags[1].Album);
            Assert.AreEqual(songTag2.Artist, songTags[1].Artist);
            Assert.AreEqual(songTag2.TrackNumber, songTags[1].TrackNumber);
            Assert.AreEqual(songTag2.FileLocation, songTags[1].FileLocation);
            Assert.AreEqual(songTag2.Minutes, songTags[1].Minutes);
            Assert.AreEqual(songTag2.Seconds, songTags[1].Seconds);
            Assert.AreEqual(songTag2.Year, songTags[1].Year);
            Assert.AreEqual(songTag2.Format, songTags[1].Format);
        }

        [TestMethod]
        [ExpectedException(typeof(MediaPlayerException))]
        public void GetSongTagsByLocation_ThrowsMediaPlayerException_WhenInputIsNull()
        {
            SongLibraryManager songLibraryManager = new SongLibraryManager(new SongLibraryDataAccessMock());
            List<SongContract> songContracts = songLibraryManager.GetSongTagsByFile(null);
        }
    }
}