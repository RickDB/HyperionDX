using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading;
using EasyHook;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.Sockets;
using Capture.Interface;
using Capture.Hook;
using Capture;
using ImageFormat = Capture.Interface.ImageFormat;

namespace TestScreenshot
{
  public partial class Form1 : Form
  {
    private System.Windows.Forms.Timer hyperionTimer;
    private int hyperionInterval = 60;
    public Form1()
    {
      InitializeComponent();

      // JSON
      ConnectToServer("10.1.2.83", 19444);
      hyperionTimer = new System.Windows.Forms.Timer();
      hyperionTimer.Interval = hyperionInterval;
      hyperionTimer.Tick += HyperionInterval_Tick;
    }

    private void HyperionInterval_Tick(object sender, EventArgs e)
    {
      Size? resize = null;
      if (!String.IsNullOrEmpty(txtResizeHeight.Text) && !String.IsNullOrEmpty(txtResizeWidth.Text))
        resize = new System.Drawing.Size(int.Parse(txtResizeWidth.Text), int.Parse(txtResizeHeight.Text));
      _captureProcess.CaptureInterface.BeginGetScreenshot(
        new Rectangle(int.Parse(txtCaptureX.Text), int.Parse(txtCaptureY.Text), int.Parse(txtCaptureWidth.Text),
          int.Parse(txtCaptureHeight.Text)), new TimeSpan(0, 0, 2), Callback, resize,
        (ImageFormat)Enum.Parse(typeof(ImageFormat), cmbFormat.Text));
    }

    private void Form1_Load(object sender, EventArgs e)
    {
    }

    private void btnInject_Click(object sender, EventArgs e)
    {
      if (_captureProcess == null)
      {
        btnInject.Enabled = false;

        if (cbAutoGAC.Checked)
        {
          // NOTE: On some 64-bit setups this doesn't work so well.
          //       Sometimes if using a 32-bit target, it will not find the GAC assembly
          //       without a machine restart, so requires manual insertion into the GAC
          // Alternatively if the required assemblies are in the target applications
          // search path they will load correctly.

          // Must be running as Administrator to allow dynamic registration in GAC
          Config.Register("Capture",
            "Capture.dll");
        }

        AttachProcess();
      }
      else
      {
        HookManager.RemoveHookedProcess(_captureProcess.Process.Id);
        _captureProcess.CaptureInterface.Disconnect();
        _captureProcess = null;
      }

      if (_captureProcess != null)
      {
        btnInject.Text = "Detach";
        btnInject.Enabled = true;
      }
      else
      {
        btnInject.Text = "Inject";
        btnInject.Enabled = true;
      }
    }

    int processId = 0;
    Process _process;
    CaptureProcess _captureProcess;

    private void AttachProcess()
    {
      string exeName = Path.GetFileNameWithoutExtension(textBox1.Text);

      Process[] processes = Process.GetProcessesByName(exeName);
      foreach (Process process in processes)
      {
        // Simply attach to the first one found.

        // If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
        if (process.MainWindowHandle == IntPtr.Zero)
        {
          continue;
        }

        // Skip if the process is already hooked (and we want to hook multiple applications)
        if (HookManager.IsHooked(process.Id))
        {
          continue;
        }

        Direct3DVersion direct3DVersion = Direct3DVersion.Direct3D10;

        if (rbDirect3D11.Checked)
        {
          direct3DVersion = Direct3DVersion.Direct3D11;
        }
        else if (rbDirect3D10_1.Checked)
        {
          direct3DVersion = Direct3DVersion.Direct3D10_1;
        }
        else if (rbDirect3D10.Checked)
        {
          direct3DVersion = Direct3DVersion.Direct3D10;
        }
        else if (rbDirect3D9.Checked)
        {
          direct3DVersion = Direct3DVersion.Direct3D9;
        }
        else if (rbAutodetect.Checked)
        {
          direct3DVersion = Direct3DVersion.AutoDetect;
        }

        CaptureConfig cc = new CaptureConfig()
        {
          Direct3DVersion = direct3DVersion,
          ShowOverlay = cbDrawOverlay.Checked
        };

        processId = process.Id;
        _process = process;

        var captureInterface = new CaptureInterface();
        captureInterface.RemoteMessage += new MessageReceivedEvent(CaptureInterface_RemoteMessage);
        _captureProcess = new CaptureProcess(process, cc, captureInterface);

        break;
      }
      Thread.Sleep(10);

      if (_captureProcess == null)
      {
        MessageBox.Show("No executable found matching: '" + exeName + "'");
      }
      else
      {
        btnLoadTest.Enabled = true;
        btnCapture.Enabled = true;
      }
    }

    /// <summary>
    /// Display messages from the target process
    /// </summary>
    /// <param name="message"></param>
    void CaptureInterface_RemoteMessage(MessageReceivedEventArgs message)
    {
      txtDebugLog.Invoke(new MethodInvoker(delegate()
      {
        txtDebugLog.Text = String.Format("{0}\r\n{1}", message, txtDebugLog.Text);
      })
        );
    }

    /// <summary>
    /// Display debug messages from the target process
    /// </summary>
    /// <param name="clientPID"></param>
    /// <param name="message"></param>
    void ScreenshotManager_OnScreenshotDebugMessage(int clientPID, string message)
    {
      txtDebugLog.Invoke(new MethodInvoker(delegate()
      {
        txtDebugLog.Text = String.Format("{0}:{1}\r\n{2}", clientPID, message, txtDebugLog.Text);
      })
        );
    }

    DateTime start;
    DateTime end;

    private void btnCapture_Click(object sender, EventArgs e)
    {
      start = DateTime.Now;
      progressBar1.Maximum = 1;
      progressBar1.Step = 1;
      progressBar1.Value = 0;

      DoRequest();
    }

    private void btnLoadTest_Click(object sender, EventArgs e)
    {
      // Note: we bring the target application into the foreground because
      //       windowed Direct3D applications have a lower framerate 
      //       if not the currently focused window
      _captureProcess.BringProcessWindowToFront();
      start = DateTime.Now;
      progressBar1.Maximum = Convert.ToInt32(txtNumber.Text);
      progressBar1.Minimum = 0;
      progressBar1.Step = 1;
      progressBar1.Value = 0;
      DoRequest();
    }

    /// <summary>
    /// Create the screen shot request
    /// </summary>
    void DoRequest()
    {
      progressBar1.Invoke(new MethodInvoker(delegate()
      {
        if (progressBar1.Value < progressBar1.Maximum)
        {
          progressBar1.PerformStep();

          _captureProcess.BringProcessWindowToFront();
          // Initiate the screenshot of the CaptureInterface, the appropriate event handler within the target process will take care of the rest
          Size? resize = null;
          if (!String.IsNullOrEmpty(txtResizeHeight.Text) && !String.IsNullOrEmpty(txtResizeWidth.Text))
            resize = new System.Drawing.Size(int.Parse(txtResizeWidth.Text), int.Parse(txtResizeHeight.Text));
          _captureProcess.CaptureInterface.BeginGetScreenshot(
            new Rectangle(int.Parse(txtCaptureX.Text), int.Parse(txtCaptureY.Text), int.Parse(txtCaptureWidth.Text),
              int.Parse(txtCaptureHeight.Text)), new TimeSpan(0, 0, 2), Callback, resize,
            (ImageFormat) Enum.Parse(typeof (ImageFormat), cmbFormat.Text));
        }
        else
        {
          end = DateTime.Now;
          txtDebugLog.Text = String.Format("Debug: {0}\r\n{1}", "Total Time: " + (end - start).ToString(),
            txtDebugLog.Text);
        }
      })
        );
    }

    /// <summary>
    /// The callback for when the screenshot has been taken
    /// </summary>
    /// <param name="clientPID"></param>
    /// <param name="status"></param>
    /// <param name="screenshotResponse"></param>
    void Callback(IAsyncResult result)
    {
      using (Screenshot screenshot = _captureProcess.CaptureInterface.EndGetScreenshot(result))
        try
        {
          //_captureProcess.CaptureInterface.DisplayInGameText("Screenshot captured...");
          if (screenshot != null && screenshot.Data != null)
          {
            /*
            pictureBox1.Invoke(new MethodInvoker(delegate()
            {
              if (pictureBox1.Image != null)
              {
                pictureBox1.Image.Dispose();
              }
              pictureBox1.Image = screenshot.ToBitmap();

            })
              );*/

            Bitmap bitmapResized = new Bitmap(screenshot.ToBitmap(), new Size(64, 64));
            //bitmapResized.Save("10.bmp");

            MemoryStream ms = new System.IO.MemoryStream();
            System.Drawing.RectangleF cloneRect = new System.Drawing.RectangleF(0, 0, bitmapResized.Width, bitmapResized.Height);
            Bitmap cloneBitmap = bitmapResized.Clone(cloneRect, PixelFormat.Format32bppRgb);
            cloneBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            byte[] pixeldata = ms.ToArray();
            pushImage(pixeldata);
          }

          Thread t = new Thread(new ThreadStart(DoRequest));
          t.Start();
        }
        catch
        {
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

    private void pushImage(byte[] pixeldataRaw)
    {

      MemoryStream stream = new MemoryStream(pixeldataRaw);
      BinaryReader reader = new BinaryReader(stream);

      stream.Position = 0; // ensure that what start at the beginning of the stream. 
      reader.ReadBytes(14); // skip bitmap file info header
      byte[] bmiInfoHeader = reader.ReadBytes(4 + 4 + 4 + 2 + 2 + 4 + 4 + 4 + 4 + 4 + 4);

      int rgbL = (int)(stream.Length - stream.Position);
      int rgb = (int)(rgbL / (64 * 64));

      byte[] pixelData = reader.ReadBytes((int)(stream.Length - stream.Position));

      byte[] h1pixelData = new byte[64 * rgb];
      byte[] h2pixelData = new byte[64 * rgb];

      // We need to flip the image horizontally.
      // Because after reading the bytes into the bytearray with BinaryReader the image is upside down (bmp characteristic).
      int i;
      for (i = 0; i < ((64 / 2) - 1); i++)
      {
        Array.Copy(pixelData, i * 64 * rgb, h1pixelData, 0, 64 * rgb);
        Array.Copy(pixelData, (64 - i - 1) * 64 * rgb, h2pixelData, 0, 64 * rgb);
        Array.Copy(h1pixelData, 0, pixelData, (64 - i - 1) * 64 * rgb, 64 * rgb);
        Array.Copy(h2pixelData, 0, pixelData, i * 64 * rgb, 64 * rgb);
      }

      try
      {
        // Hyperion expects the bytestring to be the size of 3*width*height.
        // So 3 bytes per pixel, as in RGB.
        // Given pixeldata however is 4 bytes per pixel, as in RGBA.
        // So we need to remove the last byte per pixel.
        byte[] newpixeldata = new byte[64 * 64 * 3];
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

        // JSON
        var y = Convert.ToBase64String(newpixeldata);
        StreamWriter sw = new StreamWriter("output.log");
        sw.WriteLine(y);
        sw.Close();

        setImage(y, 1, 5000);
      }
      catch (Exception)
      {
      }
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

    private void textBox1_Validating(object sender, CancelEventArgs e)
    {
      Properties.Settings.Default.Save();
    }

    private void btnStartHyperionMonitor_Click(object sender, EventArgs e)
    {
      if (hyperionTimer.Enabled)
      {
        hyperionTimer.Stop();
        btnStartHyperionMonitor.Text = "Start Ambilight";
      }
      else
      {
        hyperionTimer.Start();
        btnStartHyperionMonitor.Text = "Stop Ambilight";
      }
    }
  }
}
