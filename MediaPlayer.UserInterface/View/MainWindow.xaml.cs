using MediaPlayer.UserInterface.ViewModel;
using System;
using System.Windows;

namespace MediaPlayer.UserInterface.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MediaPlayerViewModel vm = new MediaPlayerViewModel();
            vm.CloseApplicationRequest += new EventHandler(OnCloseApplicationRequest);
            vm.MaximizeApplicationRequest += new EventHandler(OnMaximizeApplicationRequest);
            vm.MinimizeApplicationRequest += new EventHandler(OnMinimizeApplicationRequest);
            DataContext = vm;
            SongList.Focus();
        }

        private void OnCloseApplicationRequest(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnMaximizeApplicationRequest(object sender, EventArgs e)
        {
            if (ThisWindow.WindowState == WindowState.Normal)
                ThisWindow.WindowState = WindowState.Maximized;
            else
                ThisWindow.WindowState = WindowState.Normal;
        }

        private void OnMinimizeApplicationRequest(object sender, EventArgs e)
        {
            ThisWindow.WindowState = WindowState.Minimized;
        }

        private void SongList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MediaPlayerViewModel vm = (this.DataContext as MediaPlayerViewModel);
            vm.PlaySelectedSongCommand.Execute(null);
        }

        private void ThisWindow_Closed(object sender, EventArgs e)
        {
            MediaPlayerViewModel vm = (this.DataContext as MediaPlayerViewModel);
            vm.Dispose();
        }
    }
}
