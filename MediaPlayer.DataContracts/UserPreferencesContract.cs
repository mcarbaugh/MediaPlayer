using System.Collections.Generic;

namespace MediaPlayer.DataContracts
{
    public class UserPreferencesContract
    {
        public string DefaultMediaDirectory { get; set; }
        public List<string> MediaDirectories { get; set; }
    }
}
