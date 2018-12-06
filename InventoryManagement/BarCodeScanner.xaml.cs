using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;
using Edi.UWP.Helpers;

using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using Windows.ApplicationModel.Activation;

using InventoryManagement;
using System.IO;
using Windows.Storage.Pickers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using AssetObj;

namespace InventoryManagement
{

    public partial class BarCodeScanner : Page
    {
        // Provides functionality to capture the output from the camera
        public MediaCapture _mediaCapture;

        // This object allows us to manage whether the display goes to sleep 
        // or not while our app is active.
        private readonly DisplayRequest _displayRequest = new DisplayRequest();

        // Tells us if the camera is external or on board.
        //private bool _externalCamera = false;

        public BarCodeScanner()
        {
            InitializeComponent();

            // https://msdn.microsoft.com/en-gb/library/windows/apps/hh465088.aspx

        }
        /// <summary>
        /// Call Dipose() method if application suspends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            Dispose();
            deferral.Complete();
        }

     

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Dispose();
        }

      


        /// <summary>
        /// Take the raw capture from webcome and dipose of it
        /// </summary>

        private void Dispose()
        {
            if (_mediaCapture != null)
            {
                _mediaCapture.Dispose();
                _mediaCapture = null;
            }


        }
        /// <summary>
        /// Once the user takes a photo using the on screen button, store that image in my pictures for later use 
        /// and close the preview screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SoftwareButton(object sender, RoutedEventArgs e)
        {
            // This is where we want to save to.
            var storageFolder = KnownFolders.SavedPictures;

            // Create the file that we're going to save the photo to.
            var file = await storageFolder.CreateFileAsync("sample.jpg", CreationCollisionOption.ReplaceExisting);


            // Update the file with the contents of the photograph.
            await _mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);

            await _mediaCapture.StopPreviewAsync();

        }
        /// <summary>
        /// This method takes the photo and manipulates it to save it a raw and writeable bitmap to be read by
        /// the barcode library and "scan" the qr code
        /// the Camera capture raw jpeg is not ready to be written as a writeable bitmap until a filestream is opened and
        /// has the jpeg contents copied into the filestream which points to a writable bitmap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void btnscan_Click(object sender, RoutedEventArgs e)
        {

            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            var size = new Size(bounds.Width * scaleFactor, bounds.Height * scaleFactor);
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(size.Width, size.Height);
            StorageFile file = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file != null)
            {
                //QR code conversion from jepg and return string.
                WriteableBitmap writeableBitmap;
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);

                    writeableBitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                    writeableBitmap.SetSource(fileStream);
                    imgshow.Source = writeableBitmap;
                }
                // create a barcode reader instance
                BarcodeReader reader = new BarcodeReader();
                // detect and decode the barcode inside the  writeableBitmap
                var barcodeReader = new BarcodeReader
                {
                    AutoRotate = true,
                    Options = { TryHarder = true }
                };
                Result result = reader.Decode(writeableBitmap);
                // do something with the result
                if (result != null)
                {
                    txtDecoderType.Text = result.BarcodeFormat.ToString();

                    txtDecoderContent.Text = result.Text;
                }

            }

        }

        private void InitializeWebCam(object sender, RoutedEventArgs e)
        {
            Dispose();
            
        }
        /// <summary>
        /// This method takes in a GUID generated by an asset object and creates a bitmap.  Bitmap is stored on users computer
        /// where they can print and attach to phyiscal asset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //QR code conversion from jepg and return string.
            Asset A2 = new Asset("David's phone", "Bad phone (it's not an iphone)", 300, 12, "124", true);

            ZXing.IBarcodeWriter writer = new ZXing.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,//Mentioning type of bar code generation
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 300,
                    Width = 300,
                },
           
            };
            var result = writer.Write(A2.IDnumber.ToString());
            /// This cast as writeable bitmap was the only way to get the results from the barcode generator to be 
            /// stored as writeable bitmap
            var wb = result as Windows.UI.Xaml.Media.Imaging.WriteableBitmap;

            //Saving QRCode Image as png
            var localFolder = KnownFolders.SavedPictures;
            string filename = A2.IDnumber.ToString() + ".png";
            var file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            //this users a random access file stream to convert the raw writeable bitmap as a png.  
            using (var ras = await file.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                WriteableBitmap bitmap = wb;
                var stream = bitmap.PixelBuffer.AsStream();
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, ras);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 150, 150, buffer);
                await encoder.FlushAsync();
            }


        }
    }



}
