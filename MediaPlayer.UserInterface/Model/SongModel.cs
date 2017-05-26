using MediaPlayer.BaseClasses;

namespace MediaPlayer.UserInterface.Model
{
    public class SongModel : BaseModel
    {
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public uint Year { get; set; }
        public string Length
        {
            get
            {
                return string.Format("{0}:{1:D2}", Minutes, Seconds);
            }
        }
        public string Genre { get; set; }
        public AudioFileFormat Format { get; set; }
        public string FileLocation { get; set; }
        public uint TrackNumber { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}
