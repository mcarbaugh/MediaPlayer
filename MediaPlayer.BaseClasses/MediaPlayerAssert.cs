using System.Collections.Generic;
using System.IO;

namespace MediaPlayer.BaseClasses
{
    public static class MediaPlayerAssert
    {
        public static void FileExists(string file)
        {
            if (!File.Exists(file))
            {
                throw new MediaPlayerException("The selected song could not be found.");
            }
        }

        public static void FolderExists(string file)
        {
            if (!Directory.Exists(file))
            {
                throw new MediaPlayerException("The selected folder could not be found.");
            }
        }

        public static void SongFilesIsNotNull(List<string> myStrings)
        {
            if (myStrings == null)
            {
                throw new MediaPlayerException("MediaPlayer could not find a song source.");
            }
        }
    }
}
