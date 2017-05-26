using MediaPlayer.BaseClasses;

namespace MediaPlayer.DataContracts
{
    public class SongContract
    {
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public uint TrackNumber { get; set; }
        public uint Year { get; set; }
        public string Genre { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public AudioFileFormat Format { get; set; }
        public string FileLocation { get; set; }
    }
}
