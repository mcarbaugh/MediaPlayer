using MediaPlayer.DataContracts;
using System;
using System.Collections.Generic;

namespace MediaPlayer.DataAccess.Contracts
{
    public interface ISongLibraryDataAccess : IDisposable
    {
        List<SongContract> GetSongTagsByLocation(List<string> files);

        List<string> GetMP3FilesByDirectory(string directory);
    }
}
