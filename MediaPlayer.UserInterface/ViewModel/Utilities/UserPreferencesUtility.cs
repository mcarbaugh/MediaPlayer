using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MediaPlayer.UserInterface.ViewModel.Utilities
{
    public static class UserPreferencesUtility
    {
        public static List<string> GetSongLibraryAsList()
        {
            StringCollection mySongs = Properties.Settings.Default.SongLocations;
            
            if(mySongs == null)
            {
                mySongs = new StringCollection();
            }

            return mySongs.Cast<string>().AsEnumerable().ToList();
        }

        public static void AddSongsToLibrary(List<string> songs)
        {
            try
            {
                StringCollection songLibrary = Properties.Settings.Default.SongLocations;

                if(songLibrary == null)
                {
                    songLibrary = new StringCollection();
                }

                List<string> newSongs = new List<string>();
                if (songs != null)
                {
                    foreach(string song in songs)
                    {
                        if(!songLibrary.Contains(song))
                        {
                            newSongs.Add(song);
                        }
                    }

                    songLibrary.AddRange(newSongs.ToArray());
                    Properties.Settings.Default.SongLocations = songLibrary;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateSongLibrary(List<string> songLibrary)
        {
            try
            {
                StringCollection mySongs = new StringCollection();

                if (songLibrary != null)
                {
                    mySongs.AddRange(songLibrary.ToArray());
                    Properties.Settings.Default.SongLocations = mySongs;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
