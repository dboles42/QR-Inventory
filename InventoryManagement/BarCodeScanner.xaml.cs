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
using DataAccessLibrary;

namespace InventoryManagement
{

    public partial class BarCodeScanner : Page
    {
        // Provides functionality to capture the output from the camera
        public MediaCapture _mediaCapture;

        // This object allows us to manage whether the display goes to sleep 
        // or not while our app is active.
        private readonly DisplayRequest _displayRequest = new DisplayRequest();

        Inventory i1 = new Inventory();
        DataAccess DataAccessKey = new DataAccess("Asset");
        Asset CheckInAsset = new Asset();

        public BarCodeScanner()
        {
            InitializeComponent();
            btnCheckIn.IsEnabled = false;
            btnCheckOut.IsEnabled = false;

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
            
                if (result != null)
                {
                    Asset ScannedAsset = i1.FindAsset(result.Text);
                    if (ScannedAsset != null)
                    {
                        Name.Text = ScannedAsset.Name.ToString();
                        Description.Text = ScannedAsset.Description.ToString();
                        Price.Text = ScannedAsset.Price.ToString();
                        Model.Text = ScannedAsset.ModelNumber.ToString();
                        Serial.Text = ScannedAsset.SerialNumber.ToString();
                        ScanResult.Text = "Scan Succcesful";
                        CheckInAsset = ScannedAsset;

                        if (ScannedAsset.CheckIn == true)
                        {
                            btnCheckOut.IsEnabled = true;
                        }
                        else
                            btnCheckIn.IsEnabled = true;
                    }
                    else 
                    ScanResult.Text = "Scan Unsucccesful. Try again";
                }
                else 
                    ScanResult.Text = "Scan Unsucccesful. Try again";
            }
        }



        private void InitializeWebCam(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Model_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Serial_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ScanResult_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(mainMenu));
            DataAccessKey.RemoveAllRows();
            DataAccessKey.InsertIntoTable(i1.listOfAssets);
            
        }

        private void btnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            i1.listOfAssets[i1.FindIndex(CheckInAsset)].CheckIn = true;
            if (CheckInAsset.CheckIn == true)
            {
                btnCheckIn.IsEnabled = false;
                btnCheckOut.IsEnabled = true;
            }
        }

        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            i1.listOfAssets[i1.FindIndex(CheckInAsset)].CheckIn = false;
            if (CheckInAsset.CheckIn == false)
            {
                btnCheckIn.IsEnabled = true;
                btnCheckOut.IsEnabled = false;
            }

        }

        /// <summary>
        /// This method takes in a GUID generated by an asset object and creates a bitmap.  Bitmap is stored on users computer
        /// where they can print and attach to phyiscal asset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

    }



}
