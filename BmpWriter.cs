using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using proto;
using System.Linq;
using System.Windows;

namespace ScreenShot
{

  /// <summary>
  /// Uses default .net GIF encoding and adds animation headers.
  /// </summary>
  public class BmpWriter : IDisposable
  {
    #region Header Constants

    const byte FileTrailer = 0x3b,
      ApplicationBlockSize = 0x0b,
      GraphicControlExtensionBlockSize = 0x04;

    const int ApplicationExtensionBlockIdentifier = 0xff21,
      GraphicControlExtensionBlockIdentifier = 0xf921;

    const long SourceGlobalColorInfoPosition = 10,
      SourceGraphicControlExtensionPosition = 781,
      SourceGraphicControlExtensionLength = 8,
      SourceImageBlockPosition = 789,
      SourceImageBlockHeaderLength = 11,
      SourceColorBlockPosition = 13,
      SourceColorBlockLength = 768;

    const string ApplicationIdentification = "NETSCAPE2.0",
      FileType = "GIF",
      FileVersion = "89a";

    #endregion

    readonly object SyncLock = new object();

    BinaryWriter Writer;
    bool FirstFrame = true;

    private static TcpClient Socket = new TcpClient();
    private static Stream Stream;

    private bool hyperionErrorOccured;

    public BmpWriter(Stream OutStream, int DefaultFrameDelay = 500, int Repeat = -1)
    {
      Writer = new BinaryWriter(OutStream);
      this.DefaultFrameDelay = DefaultFrameDelay;
      this.Repeat = Repeat;
    }

    public BmpWriter(string FileName, int DefaultFrameDelay = 500, int Repeat = -1, string hyperionIP = "10.1.2.83", int hyperionProtoPort = 19445)
      : this(
        new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read), DefaultFrameDelay, Repeat)
    {
      if (Socket.Connected == false)
      {
        Logger("Connecting to Hyperion...");

        Socket = new TcpClient();
        Socket.SendTimeout = 5000;
        Socket.ReceiveTimeout = 5000;
        Socket.Connect(hyperionIP, hyperionProtoPort);
        Stream = Socket.GetStream();

        Logger("Connected to Hyperion!");
      }
    }

    #region Properties

    public int DefaultWidth { get; set; }

    public int DefaultHeight { get; set; }

    /// <summary>
    /// Default Delay in Milliseconds
    /// </summary>
    public int DefaultFrameDelay { get; set; }

    /// <summary>
    /// The Number of Times the Animation must repeat.
    /// -1 indicates no repeat. 0 indicates repeat indefinitely
    /// </summary>
    public int Repeat { get; private set; }

    #endregion

    /// <summary>
    /// Adds a frame to this animation.
    /// </summary>
    /// <param name="Image">The image to add</param>
    /// <param name="XOffset">The positioning x offset this image should be displayed at.</param>
    /// <param name="YOffset">The positioning y offset this image should be displayed at.</param>
    public void WriteFrame(Image Image, int Delay = 0)
    {
      lock (SyncLock)
        using (Image)
        using (var stream = new MemoryStream())
        {
          try
          {
            if (Image != null)
            {
              Image.Save(stream, ImageFormat.Bmp);
            }
            else
            {
              Logger("Write frame (image save): got empty Image");
            }
            //Image.Save(gifStream, ImageFormat.Gif);
          }
          catch (Exception e)
          {
            Logger("Write frame (image save): " + e.Message);
            return;
          }

          try
          {
            byte[] pixeldata = stream.ToArray();
            WriteImageToHyperion(pixeldata);

            /*
              // Steal the global color table info
              if (FirstFrame) InitHeader(gifStream, Writer, Image.Width, Image.Height);

              WriteGraphicControlBlock(gifStream, Writer, Delay == 0 ? DefaultFrameDelay : Delay);
              WriteImageBlock(gifStream, Writer, !FirstFrame, 0, 0, Image.Width, Image.Height);*/
          }
          catch (Exception e)
          {
            Logger("Write frame (to hyperion): " + e.Message);
            return;
          }
        }

      if (FirstFrame) FirstFrame = false;
    }

    private void WriteImageToHyperion(byte[] pixeldataRaw)
    {
      MemoryStream stream = new MemoryStream(pixeldataRaw);
      BinaryReader reader = new BinaryReader(stream);

      stream.Position = 0; // ensure that what start at the beginning of the stream. 
      reader.ReadBytes(14); // skip bitmap file info header
      byte[] bmiInfoHeader = reader.ReadBytes(4 + 4 + 4 + 2 + 2 + 4 + 4 + 4 + 4 + 4 + 4);

      int rgbL = (int) (stream.Length - stream.Position);
      int rgb = (int) (rgbL/(64*64));

      byte[] pixelData = reader.ReadBytes((int) (stream.Length - stream.Position));

      byte[] h1pixelData = new byte[64*rgb];
      byte[] h2pixelData = new byte[64*rgb];

      // We need to flip the image horizontally.
      // Because after reading the bytes into the bytearray with BinaryReader the image is upside down (bmp characteristic).
      int i;
      for (i = 0; i < ((64/2) - 1); i++)
      {
        Array.Copy(pixelData, i*64*rgb, h1pixelData, 0, 64*rgb);
        Array.Copy(pixelData, (64 - i - 1)*64*rgb, h2pixelData, 0, 64*rgb);
        Array.Copy(h1pixelData, 0, pixelData, (64 - i - 1)*64*rgb, 64*rgb);
        Array.Copy(h2pixelData, 0, pixelData, i*64*rgb, 64*rgb);
      }

      try
      {
        // Hyperion expects the bytestring to be the size of 3*width*height.
        // So 3 bytes per pixel, as in RGB.
        // Given pixeldata however is 4 bytes per pixel, as in RGBA.
        // So we need to remove the last byte per pixel.
        byte[] newpixeldata = new byte[64*64*3];
        int x = 0;
        int i2 = 0;
        while (i2 <= (newpixeldata.GetLength(0) - 2))
        {
          newpixeldata[i2] = pixelData[i2 + x + 2];
          newpixeldata[i2 + 1] = pixelData[i2 + x + 1];
          newpixeldata[i2 + 2] = pixelData[i2 + x];
          i2 += 3;
          x++;
        }

        // PROTO
        Logger("Newpixeldata length: " + newpixeldata.Length);

        SendImageToHyperion(newpixeldata, null);
      }
      catch (Exception e)
      {
        Logger("Write image to hyperion: " + e.Message);
        hyperionErrorOccured = true;
      }
    }

    public void SendImageToHyperion(byte[] pixeldata, byte[] bmiInfoHeader)
    {
      try
      {
        ImageRequest imageRequest = ImageRequest.CreateBuilder()
          .SetImagedata(Google.ProtocolBuffers.ByteString.CopyFrom(pixeldata))
          .SetImageheight(64)
          .SetImagewidth(64)
          .SetPriority(10)
          .SetDuration(-1)
          .Build();

        HyperionRequest request = HyperionRequest.CreateBuilder()
          .SetCommand(HyperionRequest.Types.Command.IMAGE)
          .SetExtension(ImageRequest.ImageRequest_, imageRequest)
          .Build();

        SendRequestToHyperion(request);
      }
      catch (Exception e)
      {
        Logger("Send image to hyperion: " + e.Message);
        hyperionErrorOccured = true;
      }
    }


    public void SendColorToHyperion(int red, int green, int blue)
    {
      if (!Socket.Connected)
      {
        return;
      }

      ColorRequest colorRequest = ColorRequest.CreateBuilder()
        .SetRgbColor((red*256*256) + (green*256) + blue)
        .SetPriority(10)
        .SetDuration(-1)
        .Build();

      HyperionRequest request = HyperionRequest.CreateBuilder()
        .SetCommand(HyperionRequest.Types.Command.COLOR)
        .SetExtension(ColorRequest.ColorRequest_, colorRequest)
        .Build();

      SendRequestToHyperion(request);
    }

    private void SendRequestToHyperion(HyperionRequest request)
    {
      try
      {

        if (Socket.Connected)
        {
          int size = request.SerializedSize;
          Byte[] header = new byte[4];
          header[0] = (byte) ((size >> 24) & 0xFF);
          header[1] = (byte) ((size >> 16) & 0xFF);
          header[2] = (byte) ((size >> 8) & 0xFF);
          header[3] = (byte) ((size) & 0xFF);

          int headerSize = header.Count();
          Stream.Write(header, 0, headerSize);
          request.WriteTo(Stream);
          Stream.Flush();

          // Enable reply message if needed (debugging only)
          //HyperionReply reply = ReceiveReply();
        }
      }
      catch (Exception e)
      {
        Logger("Send request to hyperion: " + e.Message);
        hyperionErrorOccured = true;
      }
    }

    private HyperionReply ReceiveReply()
    {
      try
      {
        Stream input = Socket.GetStream();
        byte[] header = new byte[4];
        input.Read(header, 0, 4);
        int size = (header[0] << 24) | (header[1] << 16) | (header[2] << 8) | (header[3]);
        byte[] data = new byte[size];
        input.Read(data, 0, size);
        HyperionReply reply = HyperionReply.ParseFrom(data);

        return reply;
      }
      catch (Exception e)
      {
        Logger("Received hyperion reply error: " + e.Message);
        hyperionErrorOccured = true;
        return null;
      }
    }

    private void Logger(string message)
    {
      StreamWriter sw = new StreamWriter("debug.log", true);
      sw.WriteLine(message);
      sw.Close();
    }

    #region Write

    void InitHeader(Stream sourceGif, BinaryWriter Writer, int w, int h)
    {
      // File Header
      Writer.Write(FileType.ToCharArray());
      Writer.Write(FileVersion.ToCharArray());

      Writer.Write((short) (DefaultWidth == 0 ? w : DefaultWidth)); // Initial Logical Width
      Writer.Write((short) (DefaultHeight == 0 ? h : DefaultHeight)); // Initial Logical Height

      sourceGif.Position = SourceGlobalColorInfoPosition;
      Writer.Write((byte) sourceGif.ReadByte()); // Global Color Table Info
      Writer.Write((byte) 0); // Background Color Index
      Writer.Write((byte) 0); // Pixel aspect ratio
      WriteColorTable(sourceGif, Writer);

      // App Extension Header for Repeating
      if (Repeat != -1)
      {
        unchecked
        {
          Writer.Write((short) ApplicationExtensionBlockIdentifier);
        }
        ;
        Writer.Write((byte) ApplicationBlockSize);
        Writer.Write(ApplicationIdentification.ToCharArray());
        Writer.Write((byte) 3); // Application block length
        Writer.Write((byte) 1);
        Writer.Write((short) Repeat); // Repeat count for images.
        Writer.Write((byte) 0); // terminator
      }
    }

    void WriteColorTable(Stream sourceGif, BinaryWriter Writer)
    {
      sourceGif.Position = SourceColorBlockPosition; // Locating the image color table
      var colorTable = new byte[SourceColorBlockLength];
      sourceGif.Read(colorTable, 0, colorTable.Length);
      Writer.Write(colorTable, 0, colorTable.Length);
    }

    void WriteGraphicControlBlock(Stream sourceGif, BinaryWriter Writer, int frameDelay)
    {
      sourceGif.Position = SourceGraphicControlExtensionPosition; // Locating the source GCE
      var blockhead = new byte[SourceGraphicControlExtensionLength];
      sourceGif.Read(blockhead, 0, blockhead.Length); // Reading source GCE

      unchecked
      {
        Writer.Write((short) GraphicControlExtensionBlockIdentifier);
      }
      ; // Identifier
      Writer.Write((byte) GraphicControlExtensionBlockSize); // Block Size
      Writer.Write((byte) (blockhead[3] & 0xf7 | 0x08)); // Setting disposal flag
      Writer.Write((short) (frameDelay/10)); // Setting frame delay
      Writer.Write((byte) blockhead[6]); // Transparent color index
      Writer.Write((byte) 0); // Terminator
    }

    void WriteImageBlock(Stream sourceGif, BinaryWriter Writer, bool includeColorTable, int x, int y, int w, int h)
    {
      sourceGif.Position = SourceImageBlockPosition; // Locating the image block
      var header = new byte[SourceImageBlockHeaderLength];
      sourceGif.Read(header, 0, header.Length);
      Writer.Write((byte) header[0]); // Separator
      Writer.Write((short) x); // Position X
      Writer.Write((short) y); // Position Y
      Writer.Write((short) w); // Width
      Writer.Write((short) h); // Height

      if (includeColorTable) // If first frame, use global color table - else use local
      {
        sourceGif.Position = SourceGlobalColorInfoPosition;
        Writer.Write((byte) (sourceGif.ReadByte() & 0x3f | 0x80)); // Enabling local color table
        WriteColorTable(sourceGif, Writer);
      }
      else Writer.Write((byte) (header[9] & 0x07 | 0x07)); // Disabling local color table

      Writer.Write((byte) header[10]); // LZW Min Code Size

      // Read/Write image data
      sourceGif.Position = SourceImageBlockPosition + SourceImageBlockHeaderLength;

      var dataLength = sourceGif.ReadByte();
      while (dataLength > 0)
      {
        var imgData = new byte[dataLength];
        sourceGif.Read(imgData, 0, dataLength);

        Writer.Write((byte) dataLength);
        Writer.Write(imgData, 0, dataLength);
        dataLength = sourceGif.ReadByte();
      }

      Writer.Write((byte) 0); // Terminator
    }

    #endregion

    public void Dispose()
    {
      try
      {
        // Complete File
        Writer.Write(FileTrailer);

        Writer.BaseStream.Dispose();
        Writer.Dispose();
      }
      catch
      {
      }
    }
  }
}