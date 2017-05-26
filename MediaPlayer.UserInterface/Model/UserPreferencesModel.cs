using System.Collections.Generic;

namespace MediaPlayer.UserInterface.Model
{
    public class UserPreferencesModel : BaseModel
    {
        public string DefaultMediaLocation { get; set; }
        public List<string> SongLocations { get; set; }
        public bool OpenOnStartup { get; set; }
        public bool CrossfadeBetweenSongs { get; set; }
        public bool AutosizeColumns { get; set; }
    }
}
