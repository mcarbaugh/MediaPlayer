using System;

namespace MediaPlayer.BaseClasses
{
    [Serializable]
    public class MediaPlayerException : Exception
    {
        public MediaPlayerException()
        {
            // Add implementation.
        }
        public MediaPlayerException(string message)
        {
            ErrorMessage = message;
        }
        public MediaPlayerException(string message, Exception inner)
        {
            // Add implementation.
        }

        public string ErrorMessage { get; set; }
    }
}
