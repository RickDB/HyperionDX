using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using SharpDX;
using proto;

namespace HyperionDX
{
  public partial class Form1 : Form
  {
    private static TcpClient Socket = new TcpClient();
    private readonly Stream Stream;
    private int _captureWidth = 64;
    private int _captureHeight = 64;

    private readonly DxScreenCapture _dxScreen;
    public Form1()
    {
      InitializeComponent();
      _dxScreen = new DxScreenCapture();
      _dxScreen.Init();


      // Proto
      Socket = new TcpClient();
      Socket.SendTimeout = 5000;
      Socket.ReceiveTimeout = 5000;
      Socket.Connect("10.1.2.83", 19445);
      Stream = Socket.GetStream();

      // JSON
      ConnectToServer("10.1.2.83", 19444);
    }

    private void btnTakeImage_Click(object sender, EventArgs e)
    {
      byte[] pixeldata = _dxScreen.CaptureScreen();

      // Hyperion expects the bytestring to be the size of 3*width*height.
      // So 3 bytes per pixel, as in RGB.
      // Given pixeldata however is 4 bytes per pixel, as in RGBA.
      // So we need to remove the last byte per pixel.
      byte[] newpixeldata = new byte[_captureHeight * _captureWidth * 3];
      int x = 0;
      int i = 0;
      while (i <= (newpixeldata.GetLength(0) - 2))
      {
        newpixeldata[i] = pixeldata[i + x + 2];
        newpixeldata[i + 1] = pixeldata[i + x + 1];
        newpixeldata[i + 2] = pixeldata[i + x];
        i += 3;
        x++;
      }

      // JSON
      var y = Convert.ToBase64String(newpixeldata);
      setImage(y, 1, 5000);

      // PROTO
      //ChangeImage(pixelData, null);
    }

    static void setImage(string base64Image, int priority, int duration)
    {
      try
      {
        Hashtable command = new Hashtable();

        command["command"] = "image";
        command["priority"] = priority;
        command["imagewidth"] = 64;
        command["imageheight"] = 64;
        command["imagedata"] = base64Image;
        if (duration > 0)
        {
          command["duration"] = duration;
        }

        // send command message
        SendMessage(command);
        command = null;
      }
      catch (Exception ex)
      {
        MessageBox.Show("Failed to create JSON Message. " + ex.Message);
      }
    }
    static byte[] RemoveAlpha(DataStream ia)
    {
      List<byte> newImage = new List<byte>();
      while (ia.Position < ia.Length)
      {
        var a = new byte[4];
        ia.Read(a, 0, 4);
        newImage.Add(a[2]);
        newImage.Add(a[1]);
        newImage.Add(a[0]);
      }

      return newImage.ToArray();
    }

    public void ChangeImage(byte[] pixeldata, byte[] bmiInfoHeader)
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

        SendRequest(request);
      }
      catch (Exception e)
      {
        MessageBox.Show("ChangeImage: " + e.Message);
      }
    }
    private void SendRequest(HyperionRequest request)
    {
      try
      {
        if (Socket.Connected)
        {
          int size = request.SerializedSize;

          Byte[] header = new byte[4];
          header[0] = (byte)((size >> 24) & 0xFF);
          header[1] = (byte)((size >> 16) & 0xFF);
          header[2] = (byte)((size >> 8) & 0xFF);
          header[3] = (byte)((size) & 0xFF);

          int headerSize = header.Count();
          Stream.Write(header, 0, headerSize);
          request.WriteTo(Stream);
          Stream.Flush();

          // Enable reply message if needed (debugging only)
          HyperionReply reply = ReceiveReply();
          MessageBox.Show(reply.ToString());
        }
      }
      catch (Exception e)
      {
        MessageBox.Show("SendRequest: " + e.Message);
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
        MessageBox.Show("receiveReply: " + e.Message);
        return null;
      }
    }


    static TcpClient hyperionServer;
    static NetworkStream serverStream;
    static StreamWriter sendToServer;
    static StreamReader readFromServer;

    static void ConnectToServer(string ip, int port)
    {
      try
      {
        hyperionServer = new TcpClient(ip, port);
        hyperionServer.NoDelay = true;
        serverStream = hyperionServer.GetStream();
        sendToServer = new StreamWriter(serverStream);
        sendToServer.AutoFlush = true;

        readFromServer = new StreamReader(serverStream);


      }
      catch (Exception ex)
      {
      }
    }

    static void SendMessage(Hashtable command)
    {
      try
      {
        var message = Serialize(command);
        sendToServer.WriteLine(message);
        message = null;
        string response = readFromServer.ReadLine();

        if (response != "{\"success\":true}")
        {
          MessageBox.Show("Hyperion error. " + response);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Failed to send message to Hyperion Server. " + ex.Message);
      }

    }

    static string Serialize(Hashtable n)
    {
      return Newtonsoft.Json.JsonConvert.SerializeObject(n);
    }

  }
}
