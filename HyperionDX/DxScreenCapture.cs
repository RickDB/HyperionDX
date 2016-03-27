using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Rectangle = SharpDX.Rectangle;

namespace HyperionDX
{
  public class DxScreenCapture
  {
    // # of graphics card adapter
    int numAdapter = 0;

    // # of output device (i.e. monitor)
    int numOutput = 0;

    // Create DXGI Factory1
    private Factory1 _factory;
    private Adapter _adapter;

    // Get DXGI.Output
    private Output _output;
    private Output1 _output1;
    private OutputDuplication _duplicatedOutput;
    private Texture2DDescription _textureDesc;
    private Texture2D _screenTexture;

    // Create device from Adapter
    private Device.Device _device;

    public void Init()
    {
      _factory = new Factory1();
      _adapter = _factory.GetAdapter1(numAdapter);
      _device = new Device.Device(_adapter);

      _output = _adapter.GetOutput(numOutput);
      _output1 = _output.QueryInterface<Output1>();
      _duplicatedOutput = _output1.DuplicateOutput(_device);

    }
    public Byte[] CaptureScreen()
    {
      Byte[] imageBytes = new byte[100];

      try
      {
        // Width/Height of desktop to capture
        int width = ((Rectangle)_output.Description.DesktopBounds).Width;
        int height = ((Rectangle)_output.Description.DesktopBounds).Height;

        // Create Staging texture CPU-accessible
        var textureDesc = new Texture2DDescription
        {
          CpuAccessFlags = CpuAccessFlags.Read,
          BindFlags = BindFlags.None,
          Format = Format.B8G8R8A8_UNorm,
          Width = width,
          Height = height,
          OptionFlags = ResourceOptionFlags.None,
          MipLevels = 1,
          ArraySize = 1,
          SampleDescription = { Count = 1, Quality = 0 },
          Usage = ResourceUsage.Staging
        };
        var screenTexture = new Texture2D(_device, textureDesc);

        bool captureDone = false;
        for (int i = 0; !captureDone; i++)
        {
          try
          {
            SharpDX.DXGI.Resource screenResource;
            OutputDuplicateFrameInformation duplicateFrameInformation;

            // Try to get duplicated frame within given time
            _duplicatedOutput.AcquireNextFrame(10000, out duplicateFrameInformation, out screenResource);

            if (i > 0)
            {
              // copy resource into memory that can be accessed by the CPU
              using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
                _device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);

              // Get the desktop capture texture
              var mapSource = _device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);

              // Create Drawing.Bitmap
              var bitmap = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb);
              var boundsRect = new System.Drawing.Rectangle(0, 0, width, height);

              // Copy pixels from screen capture Texture to GDI bitmap
              var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
              var sourcePtr = mapSource.DataPointer;
              var destPtr = mapDest.Scan0;
              for (int y = 0; y < height; y++)
              {
                // Copy a single line 
                Utilities.CopyMemory(destPtr, sourcePtr, width * 4);

                // Advance pointers
                sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                destPtr = IntPtr.Add(destPtr, mapDest.Stride);
              }

              // Release source and dest locks
              bitmap.UnlockBits(mapDest);
              _device.ImmediateContext.UnmapSubresource(screenTexture, 0);

              // Re-create image in different format
              //System.Drawing.RectangleF cloneRect = new System.Drawing.RectangleF(0, 0, bitmap.Width, bitmap.Height);
              //Bitmap cloneBitmap = bitmap.Clone(cloneRect, PixelFormat.Format32bppArgb);

              Bitmap bitmapResized = new Bitmap(bitmap, new Size(64, 64));
              //bitmapResized.Save("10.bmp");

              MemoryStream ms = new System.IO.MemoryStream();
              bitmapResized.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
              imageBytes = ms.ToArray();

              // Capture done
              captureDone = true;
            }

            screenResource.Dispose();
            _duplicatedOutput.ReleaseFrame();

          }
          catch (SharpDXException e)
          {
            StreamWriter sw = new StreamWriter("debug.log", true);
            sw.WriteLine(e.Message);
            sw.Close();

            if (e.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
            {
              //throw e;
            }
          }
        }
      }
      catch (Exception ex)
      {
        StreamWriter sw = new StreamWriter("debug.log", true);
        sw.WriteLine(ex.Message);
        sw.Close();
      }
      return imageBytes;
    }
  }
}