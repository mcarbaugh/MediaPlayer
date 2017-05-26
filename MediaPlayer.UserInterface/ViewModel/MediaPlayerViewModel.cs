using CSCore.SoundOut;
using MediaPlayer.BaseClasses;
using MediaPlayer.UserInterface.Model;
using MediaPlayer.UserInterface.ViewModel.Engines;
using MediaPlayer.UserInterface.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace MediaPlayer.UserInterface.ViewModel
{
    public class MediaPlayerViewModel : BaseViewModel
    {
        public event EventHandler CloseApplicationRequest;
        public event EventHandler MaximizeApplicationRequest;
        public event EventHandler MinimizeApplicationRequest;

        public MediaPlayerViewModel()
        {
            InitializeViewModel();
        }

        public List<SongModel> AllSongs
        {
            get
            {
                return _allSongs;
            }
            set
            {
                _allSongs = value;
                NotifyPropertyChanged("AllSongs");
            }
        }
        public SongModel SelectedSong
        {
            get
            {
                return _selectedSong;
            }
            set
            {
                _selectedSong = value;
                NotifyPropertyChanged("SelectedSong");
            }
        }
        public SongModel ActiveSong { get; set; }
        public MusicPlayerEngine MusicPlayer
        {
            get
            {
                return _musicPlayer;
            }
        }

        #region Commands
        public ICommand AddSongsByDirectoryCommand
        {
            get
            {
                if (_addSongsByDirectoryCommand == null)
                    _addSongsByDirectoryCommand = new Command(AddSongsByDirectory);

                return _addSongsByDirectoryCommand;
            }
        }

        public ICommand AddSongsByFileCommand
        {
            get
            {
                if (_addSongsByFileCommand == null)
                    _addSongsByFileCommand = new Command(AddSongByFile);

                return _addSongsByFileCommand;
            }
        }

        public ICommand RemoveSelectedSongfromLibraryCommand
        {
            get
            {
                if (_removeSelectedSongFromLibraryCommand == null)
                    _removeSelectedSongFromLibraryCommand = new Command(RemoveSelectedSongFromLibrary);

                return _removeSelectedSongFromLibraryCommand;
            }
        }

        public ICommand TogglePlayButtonCommand
        {
            get
            {
                if (_togglePlayButtonCommand == null)
                    _togglePlayButtonCommand = new Command(TogglePlayButton);

                return _togglePlayButtonCommand;
            }
        }

        public ICommand PlaySelectedSongCommand
        {
            get
            {
                if (_playSelectedSongCommand == null)
                    _playSelectedSongCommand = new Command(PlaySelectedSong);

                return _playSelectedSongCommand;
            }
        }

        public ICommand PlayPreviousSongCommand
        {
            get
            {
                if (_playPreviousSongCommand == null)
                    _playPreviousSongCommand = new Command(PlayPreviousSong);

                return _playPreviousSongCommand;
            }
        }

        public ICommand PlayNextSongCommand
        {
            get
            {
                if (_playNextSongCommand == null)
                    _playNextSongCommand = new Command(PlayNextSong);

                return _playNextSongCommand;
            }
        }

        public ICommand CloseApplicationCommand
        {
            get
            {
                if (_closeApplicationCommand == null)
                    _closeApplicationCommand = new Command(CloseApplication);

                return _closeApplicationCommand;
            }
        }

        public ICommand MaximizeApplicationCommand
        {
            get
            {
                if (_maximizeApplicationCommand == null)
                    _maximizeApplicationCommand = new Command(MaximizeApplication);

                return _maximizeApplicationCommand;
            }
        }

        public ICommand MinimizeApplicationCommand
        {
            get
            {
                if (_minimizeApplicationCommand == null)
                    _minimizeApplicationCommand = new Command(MinimizeApplication);

                return _minimizeApplicationCommand;
            }
        }
        #endregion

        #region Private Methods
        private void AddSongsByDirectory()
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                folderDialog.Description = "Add Folder to Library";
                folderDialog.ShowNewFolderButton = false;

                DialogResult result = folderDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string directory = folderDialog.SelectedPath;
                    string selectedSong = (SelectedSong == null) ? null : SelectedSong.FileLocation;
                    string activeSong = (ActiveSong == null) ? null : ActiveSong.FileLocation;

                    List<string> newSongs = SongUtility.GetMP3FilesByDirectory(directory);
                    UserPreferencesUtility.AddSongsToLibrary(newSongs);

                    List<string> songLibrary = UserPreferencesUtility.GetSongLibraryAsList();
                    AllSongs = new List<SongModel>(SongUtility.GetSongTagsByFiles(songLibrary));

                    if (selectedSong != null)
                        SelectedSong = AllSongs.FirstOrDefault(x => x.FileLocation == selectedSong);
                    if (activeSong != null)
                        ActiveSong = AllSongs.FirstOrDefault(x => x.FileLocation == activeSong);
                }
            }
            catch (MediaPlayerException ex)
            {
                MessageBox.Show(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void AddSongByFile()
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Add Files to Library";
                fileDialog.Multiselect = true;
                fileDialog.Filter = "|*.mp3";

                DialogResult result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string selectedSong = (SelectedSong == null) ? null : SelectedSong.FileLocation;
                    string activeSong = (ActiveSong == null) ? null : ActiveSong.FileLocation;
                    List<string> songLibrary = UserPreferencesUtility.GetSongLibraryAsList();
                    List<string> newSongs = fileDialog.FileNames.AsEnumerable().ToList();

                    foreach (string newSong in newSongs)
                    {
                        if (!songLibrary.Contains(newSong))
                            songLibrary.Add(newSong);
                    }

                    AllSongs = SongUtility.GetSongTagsByFiles(songLibrary);

                    if (selectedSong != null)
                        SelectedSong = AllSongs.FirstOrDefault(x => x.FileLocation == selectedSong);
                    if (activeSong != null)
                        ActiveSong = AllSongs.FirstOrDefault(x => x.FileLocation == activeSong);

                    UserPreferencesUtility.UpdateSongLibrary(songLibrary);
                }
            }
            catch (MediaPlayerException ex)
            {
                MessageBox.Show(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveSelectedSongFromLibrary()
        {
            try
            {
                string selectedSong = (SelectedSong == null) ? null : SelectedSong.FileLocation;
                string activeSong = (ActiveSong == null) ? null : ActiveSong.FileLocation;
                List<string> songLibrary = UserPreferencesUtility.GetSongLibraryAsList();
                MusicPlayerEngine musicPlayer = MusicPlayer;

                if (!string.IsNullOrEmpty(selectedSong))
                {
                    if (selectedSong != activeSong)
                    {
                        songLibrary.Remove(selectedSong);
                    }
                    else
                    {
                        musicPlayer.RemoveStopEvent();
                        musicPlayer.Stop();
                        songLibrary.Remove(selectedSong);
                    }
                }
                
                AllSongs = SongUtility.GetSongTagsByFiles(songLibrary);
                
                if (selectedSong != null)
                    SelectedSong = AllSongs.FirstOrDefault(x => x.FileLocation == selectedSong);
                if (activeSong != null)
                    ActiveSong = AllSongs.FirstOrDefault(x => x.FileLocation == activeSong);

                UserPreferencesUtility.UpdateSongLibrary(songLibrary);
            }
            catch (MediaPlayerException ex)
            {
                MessageBox.Show(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TogglePlayButton()
        {
            try
            {
                switch (MusicPlayer.PlaybackState)
                {
                    case PlaybackState.Playing:
                        MusicPlayer.Pause();
                        break;
                    case PlaybackState.Paused:
                        MusicPlayer.Resume();
                        break;
                    case PlaybackState.Stopped:
                        if (AllSongs.Count > 0)
                        {
                            if (SelectedSong == null)
                            {
                                SelectedSong = AllSongs[0];
                            }

                            MusicPlayer.OpenSong(SelectedSong.FileLocation);
                            MusicPlayer.Play();

                            int index = AllSongs.IndexOf(SelectedSong);
                            ActiveSong = AllSongs[index];
                        }
                        break;
                }
            }
            catch (MediaPlayerException ex)
            {
                MessageBox.Show(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PlaySelectedSong()
        {
            try
            {
                if (SelectedSong != null)
                {
                    SongModel song = SelectedSong;

                    if (File.Exists(song.FileLocation))
                    {
                        if (song != ActiveSong)
                        {
                            MusicPlayer.RemoveStopEvent();
                            MusicPlayer.OpenSong(song.FileLocation);
                            MusicPlayer.Play();
                            ActiveSong = AllSongs[AllSongs.IndexOf(song)];
                        }
                        else if (MusicPlayer.PlaybackState == CSCore.SoundOut.PlaybackState.Paused)
                        {
                            MusicPlayer.Play();
                        }
                    }
                }
            }
            catch (MediaPlayerException ex)
            {
                System.Windows.MessageBox.Show(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void PlayPreviousSong()
        {
            try
            {
                int index = 0;
                if (ActiveSong != null)
                    index = AllSongs.IndexOf(ActiveSong);

                index--;

                if (AllSongs != null)
                {
                    if (index < 0)
                    {
                        MusicPlayer.RemoveStopEvent();
                        MusicPlayer.Stop();
                        ActiveSong = null;
                    }
                    else if (index <= AllSongs.Count - 1)
                    {
                        MusicPlayer.RemoveStopEvent();
                        MusicPlayer.Stop();
                        MusicPlayer.OpenSong(AllSongs[index].FileLocation);
                        MusicPlayer.Play();
                        ActiveSong = AllSongs[index];
                    }
                }
            }
            catch (MediaPlayerException ex)
            {
                System.Windows.MessageBox.Show(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void PlayNextSong()
        {
            try
            {
                MusicPlayer.Stop();
            }
            catch (MediaPlayerException ex)
            {
                MessageBox.Show(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseApplication()
        {
            if(CloseApplicationRequest != null)
                CloseApplicationRequest(this, EventArgs.Empty);
        }

        private void MaximizeApplication()
        {
            if (MaximizeApplicationRequest != null)
                MaximizeApplicationRequest(this, EventArgs.Empty);
        }

        private void MinimizeApplication()
        {
            if (MinimizeApplicationRequest != null)
                MinimizeApplicationRequest(this, EventArgs.Empty);
        }

        private void InitializeViewModel()
        {
            ActiveSong = null;
            SelectedSong = null;
            _musicPlayer = new MusicPlayerEngine();
            _musicPlayer.PlaybackStopped += OnPlaybackStopped;

            try
            {
                List<string> songLibrary = UserPreferencesUtility.GetSongLibraryAsList();
                AllSongs = SongUtility.GetSongTagsByFiles(songLibrary);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _musicPlayer.Dispose();
                _musicPlayer = null;
            }
        }

        private List<SongModel> _allSongs;
        private SongModel _selectedSong;
        private MusicPlayerEngine _musicPlayer;
        private ICommand _addSongsByDirectoryCommand;
        private ICommand _addSongsByFileCommand;
        private ICommand _removeSelectedSongFromLibraryCommand;
        private ICommand _togglePlayButtonCommand;
        private ICommand _playSelectedSongCommand;
        private ICommand _playPreviousSongCommand;
        private ICommand _playNextSongCommand;
        private ICommand _closeApplicationCommand;
        private ICommand _maximizeApplicationCommand;
        private ICommand _minimizeApplicationCommand;

        private void OnPlaybackStopped(object sender, CSCore.SoundOut.PlaybackStoppedEventArgs e)
        {
            if (MusicPlayer != null)
            {
                if (AllSongs.Count > 0)
                {
                    if (ActiveSong != null)
                    {
                        int index = AllSongs.IndexOf(ActiveSong);

                        if (index == AllSongs.Count - 1)
                        {
                            SelectedSong = null;
                            ActiveSong = null;
                        }
                        else
                        {
                            ActiveSong = AllSongs[index + 1];
                            MusicPlayer.OpenSong(ActiveSong.FileLocation);
                            MusicPlayer.Play();
                        }
                    }
                }
            }
        }
    }
}
