using CSCore;
using CSCore.Codecs;
using CSCore.SoundOut;
using MediaPlayer.BaseClasses;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace MediaPlayer.UserInterface.ViewModel.Engines
{
    public class MusicPlayerEngine : BaseEngine, IDisposable
    {
        public MusicPlayerEngine()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                _soundOut = new WasapiOut();
            else
                _soundOut = new DirectSoundOut();
        }

        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;

        public void RemoveStopEvent()
        {
            if (PlaybackStopped != null && _soundOut != null)
            {
                _soundOut.Stopped -= PlaybackStopped;
            }
        }

        public void OpenSong(string file)
        {
            try
            {
                Stop();

                _waveSource = CodecFactory.Instance.GetCodec(file);

                if (WasapiOut.IsSupportedOnCurrentPlatform)
                    _soundOut = new WasapiOut();
                else
                    _soundOut = new DirectSoundOut();

                _soundOut.Initialize(_waveSource);
                if (PlaybackStopped != null)
                    _soundOut.Stopped += PlaybackStopped;
            }
            catch (Exception)
            {
                throw new MediaPlayerException("Could not open audio file.");
            }
        }

        public void Play()
        {
            if (_soundOut != null)
            {
                _soundOut.Play();
                NotifyPropertyChanged("PlaybackState");
            }
        }

        public void Resume()
        {
            if (_soundOut != null)
            {
                _soundOut.Resume();
                NotifyPropertyChanged("PlaybackState");
            }
        }

        public void Pause()
        {
            if (_soundOut != null)
            {
                _soundOut.Pause();
                NotifyPropertyChanged("PlaybackState");
            }
        }

        public void Stop()
        {
            if (_soundOut != null)
            {
                _soundOut.Stop();
                CleanupPlayback();
                NotifyPropertyChanged("PlaybackState");
            }
        }

        public PlaybackState PlaybackState
        {
            get
            {
                if (_soundOut == null)
                    return PlaybackState.Stopped;
                else
                    return _soundOut.PlaybackState;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                CleanupPlayback();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        private void CleanupPlayback()
        {
            if (_soundOut != null)
            {
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }
        }

        private bool disposed = false;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private ISoundOut _soundOut;
        private IWaveSource _waveSource;
    }
}
