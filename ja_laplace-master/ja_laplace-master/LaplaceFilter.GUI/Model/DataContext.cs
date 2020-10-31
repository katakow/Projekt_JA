using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace LaplaceFilter.Model
{

    class DataContext : INotifyPropertyChanged
    {
        public DataContext()
        {
            CSharpImplementationMode = true;
        }

        private string _elapsedTime;
        private string _inputFilePath;
        private BitmapImage _imageSource;
        private Bitmap _image;
        private Bitmap _filteredImage;
        private double _progressBar;


        public double ProgressBar
        {
            get { return _progressBar; }
            set
            {
                _progressBar = value;
                OnPropertyChanged(nameof(ProgressBar));
            }
        }
        public bool SaveEnabled
        {
            get => _filteredImage != null;
        }
        public bool FilterEnabled
        {
            get => _image != null;
        }
        public bool AssemblerImplementationMode { get; set; }
        public bool CSharpImplementationMode { get; set; }

        public string InputFilePath
        {
            get => _inputFilePath; set
            {
                _inputFilePath = value;
                OnPropertyChanged(nameof(InputFilePath));
            }
        }
        public string ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }
        public Bitmap Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(FilterEnabled));
            }
        }
        public Bitmap FilteredImage
        {
            get => _filteredImage;
            set
            {
                _filteredImage = value;
                OnPropertyChanged(nameof(FilteredImage));
                OnPropertyChanged(nameof(SaveEnabled));
            }
        }
        public BitmapImage ImageSource
        {
            get => _imageSource; set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
