using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LaplaceFilter.Algorithm;
using LaplaceFilter.Model;

namespace LaplaceFilter
{

    public partial class MainWindow : Window
    {
        private Action<float> dispatcher;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataContext();

            //sprawdzanie ile watkow ma procesor
            watki.Text = Environment.ProcessorCount.ToString();

            dispatcher = (newValue) => Dispatcher.InvokeAsync(() =>
            {
                (this.DataContext as DataContext).ProgressBar = newValue;
            });
        }


        //wczytywanie obrazu 
        private void Input_Path_Find(object sender, RoutedEventArgs e)
        {

            var openFile = new OpenFileDialog
            {
                Filter = "1.JPeg Image|*.jpg|2.Bitmap Image|*.bmp",
                Title = "Please select an image file."
            };
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                DataContext is DataContext context)
            {
                context.InputFilePath = openFile.FileName;
                var newBitmap = new Bitmap(openFile.FileName);
                int minSize = new[] { newBitmap.Width, newBitmap.Height }.Min();
                if (minSize >= 50)
                {
                    context.Image = newBitmap;
                    PrintImageOnGUI(context.Image);
                }
                else
                {
                    context.Image = null;
                    System.Windows.Forms.MessageBox.Show("Picture is too small. Min height/weight is 50x50.");
                }
            }
        }

        // filtrowania obrazu 
        private async void Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            Find_Btn.IsEnabled = false;

            var context = (DataContext as DataContext);
            context.FilteredImage = null;
            var image = context.Image;
            context.Image = null;
            Stopwatch stopwatch = new Stopwatch();

            context.ElapsedTime = "Trwa filtrowanie...";
            context.ProgressBar = 0;
            ProgressBar.Visibility = Visibility.Visible;
            stopwatch.Start();
            if (context.CSharpImplementationMode)
            {
                context.FilteredImage = await RunCSharpLaplaceFilter(image);
            }
            else if (context.AssemblerImplementationMode)
            {
                // TODO: Run assembly implementation
                context.FilteredImage = await RunCSharpLaplaceFilter(image);
            }

            stopwatch.Stop();
            ProgressBar.Visibility = Visibility.Hidden;
            context.ElapsedTime = stopwatch.ElapsedMilliseconds.ToString();
            context.Image = image;
            PrintImageOnGUI(context.FilteredImage);
            Find_Btn.IsEnabled = true;

        }

        //zapisywanie obrazu po filtracji
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "1.JPeg Image|*.jpg|2.Bitmap Image|*.bmp",
                Title = "Save an Image File"
            };
            var result = saveFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var context = (DataContext as DataContext);
                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                        context.FilteredImage.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                        break;
                    case 2:
                        context.FilteredImage.Save(saveFileDialog.FileName, ImageFormat.Bmp);
                        break;
                    default:
                        break;
                }

            }

        }

        //maski Laplace'a
        private int[] _laplaceMask1 = new int[] {
                        0, -1, 0,
                        -1, 4, -1,
                        0, -1, 0 };

        private int[] _laplaceMask2 = new int[] {
                        -1, -1, -1,
                        -1,  8, -1,
                        -1, -1, -1 };

        private int[] _laplaceMask3 = new int[] {
                        1, -2, 1,
                        -2,  4, -2,
                        1, -2, 1 };

        //fitracja Laplace'a dla c#
        private async Task<Bitmap> RunCSharpLaplaceFilter(Bitmap image)
        {
            return await Task.Run(() =>
            {
                var laplaceFilter = new LaplaceFilterCSharp(_laplaceMask2, image, dispatcher);
                laplaceFilter.ApplyUnsafe();

                return laplaceFilter.FilteredImage;
            });
        }

        //wyśwetlanie obrazu po filtracji na ekranie
        private void PrintImageOnGUI(Bitmap image)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmapimage.Freeze();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    (DataContext as DataContext).ImageSource = bitmapimage;
                }));
            }

        }

    }
}
