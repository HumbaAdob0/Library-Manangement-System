using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LibraryManagementSystem.Helpers;
using OpenCvSharp;
using ZXing;
using ZXing.Common;

namespace LibraryManagementSystem.Views;

public partial class BarcodeScannerWindow : System.Windows.Window
{
    private readonly BarcodeReaderGeneric _barcodeReader;
    private VideoCapture? _capture;
    private DispatcherTimer? _timer;
    private bool _scanCompleted;

    public BarcodeScannerWindow()
    {
        InitializeComponent();

        _barcodeReader = new BarcodeReaderGeneric
        {
            AutoRotate = true,
            Options = new DecodingOptions
            {
                TryHarder = true,
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.EAN_13 }
            }
        };
    }

    public string? ScannedISBN { get; private set; }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        StartCamera();
    }

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        StopCamera();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void StartCamera()
    {
        try
        {
            _capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
            if (!_capture.IsOpened())
            {
                _capture.Release();
                _capture.Dispose();
                _capture = new VideoCapture(0);
            }

            if (!_capture.IsOpened())
            {
                StatusTextBlock.Text = "No camera was detected. Connect a camera and try again.";
                return;
            }

            _capture.Set(VideoCaptureProperties.FrameWidth, 1280);
            _capture.Set(VideoCaptureProperties.FrameHeight, 720);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(120)
            };
            _timer.Tick += ReadCameraFrame;
            _timer.Start();

            StatusTextBlock.Text = "Camera ready. Hold the ISBN barcode inside the frame.";
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = $"Camera could not start: {ex.Message}";
            StopCamera();
        }
    }

    private void StopCamera()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer.Tick -= ReadCameraFrame;
            _timer = null;
        }

        if (_capture != null)
        {
            _capture.Release();
            _capture.Dispose();
            _capture = null;
        }

        PreviewImage.Source = null;
    }

    private void ReadCameraFrame(object? sender, EventArgs e)
    {
        if (_scanCompleted || _capture == null || !_capture.IsOpened())
        {
            return;
        }

        try
        {
            using var frame = new Mat();
            if (!_capture.Read(frame) || frame.Empty())
            {
                return;
            }

            using var rgbFrame = new Mat();
            Cv2.CvtColor(frame, rgbFrame, ColorConversionCodes.BGR2RGB);

            var pixels = CopyPixels(rgbFrame);
            var stride = rgbFrame.Width * rgbFrame.Channels();

            var preview = BitmapSource.Create(
                rgbFrame.Width,
                rgbFrame.Height,
                96,
                96,
                PixelFormats.Rgb24,
                null,
                pixels,
                stride);
            preview.Freeze();
            PreviewImage.Source = preview;

            var source = new RGBLuminanceSource(
                pixels,
                rgbFrame.Width,
                rgbFrame.Height,
                RGBLuminanceSource.BitmapFormat.RGB24);
            var result = _barcodeReader.Decode(source);

            if (result != null)
            {
                AcceptBarcode(result.Text);
            }
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = $"Scanner error: {ex.Message}";
        }
    }

    private static byte[] CopyPixels(Mat mat)
    {
        var stride = mat.Width * mat.Channels();
        var length = stride * mat.Height;
        var pixels = new byte[length];
        Marshal.Copy(mat.Data, pixels, 0, length);
        return pixels;
    }

    private void AcceptBarcode(string rawValue)
    {
        var formattedISBN = ISBNHelper.FormatISBN13(rawValue);
        if (!ISBNHelper.IsValidISBN13(formattedISBN))
        {
            StatusTextBlock.Text = "Barcode detected, but it is not a valid ISBN-13.";
            return;
        }

        _scanCompleted = true;
        ScannedISBN = formattedISBN;
        StatusTextBlock.Text = $"Scanned {formattedISBN}";
        DialogResult = true;
        Close();
    }
}
