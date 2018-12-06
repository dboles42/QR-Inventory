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
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BarcodeGenerator : Page
    {
        public Asset SelectedAsset = mainMenu.CurrentAsset;
        public BarcodeGenerator()
        {
            this.InitializeComponent();
        }

        private async void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //QR code conversion from jepg and return string.

            ZXing.IBarcodeWriter writer = new ZXing.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,//Mentioning type of bar code generation
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 300,
                    Width = 300,
                },

            };
            var result = writer.Write(SelectedAsset.IDnumber.ToString());
            /// This cast as writeable bitmap was the only way to get the results from the barcode generator to be 
            /// stored as writeable bitmap
            var wb = result as Windows.UI.Xaml.Media.Imaging.WriteableBitmap;

            //Saving QRCode Image as png
            var localFolder = KnownFolders.SavedPictures;
            string filename = SelectedAsset.IDnumber.ToString() + ".png";
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
            var success = await Windows.System.Launcher.LaunchFileAsync(file);
        }
    }
}
