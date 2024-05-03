using System.Collections.ObjectModel;
using System.Timers;

namespace AppCam
{
    public partial class MainPage : ContentPage
    {
        private System.Timers.Timer _timer;
        private int _counter;
        private ObservableCollection<ImageSource> _snapshots;

        public MainPage()
        {
            InitializeComponent();
            _snapshots = new ObservableCollection<ImageSource>();
            _timer = new System.Timers.Timer(1000); // 5 photos per second
            _timer.Elapsed += TakeSnapShot;
            SnapshotsView.ItemsSource = _snapshots; // Bind the collection to the CollectionView
        }

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            _counter = 0;
            _timer.Start();
        }

        private void TakeSnapShot(object sender, ElapsedEventArgs e)
        {
            if (_counter >= 10) // 10 seconds * 5 photos per second
            {
                _timer.Stop();
                return;
            }

            var snapshot = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _snapshots.Add(snapshot);
            });
            _counter++;
        }
    }
}
