using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Capture;
using Capture.Interface;
using System.Windows.Input;

namespace ScreenShot
{
  public partial class MainWindow : Window
  {
    CaptureProcess _captureProcess;
    DateTime start;
    BmpWriter Writer;
    bool ContinueCapture;
    Task Work;

    public string hyperionIP = "10.1.2.83";
    public int hyperionProtoPort = 19445;
    public int bmpFrameDelay = 60;

    public static readonly DependencyProperty InjectedProperty =
      DependencyProperty.Register("Injected", typeof (bool), typeof (MainWindow), new UIPropertyMetadata(false));

    public bool Injected
    {
      get { return (bool) GetValue(InjectedProperty); }
      private set
      {
        SetValue(InjectedProperty, value);

        InjectButton.Content = value ? "Detach" : "Inject";

        ExeName.IsEnabled = !value;
      }
    }

    public static readonly DependencyProperty RecordingProperty =
      DependencyProperty.Register("Recording", typeof (bool), typeof (MainWindow), new UIPropertyMetadata(false));

    public bool Recording
    {
      get { return (bool) GetValue(RecordingProperty); }
      private set
      {
        SetValue(RecordingProperty, value);

        RecordButton.Content = (value ? "Stop" : "Start") + " Hyperion forwarding";
      }
    }

    public MainWindow()
    {
      InitializeComponent();

      CommandBindings.Add(new CommandBinding(ApplicationCommands.Open,
        (s, e) => Inject(),
        (s, e) => e.CanExecute = Process.GetProcessesByName(ExeName.Text).Length > 0));

      CommandBindings.Add(new CommandBinding(ApplicationCommands.New,
        (s, e) => Record(),
        (s, e) => e.CanExecute = Injected));

      Application.Current.DispatcherUnhandledException += (s, e) =>
      {
        MessageBox.Show(e.Exception.Message);
        if (e.Exception.InnerException != null) MessageBox.Show(e.Exception.InnerException.Message);
        e.Handled = true;
      };

      AppDomain.CurrentDomain.UnhandledException += (s, e) =>
      {
        var E = e.ExceptionObject as Exception;

        MessageBox.Show(E.Message);
        if (E.InnerException != null) MessageBox.Show(E.InnerException.Message);
        MessageBox.Show("App will terminate");
        Application.Current.Shutdown();
      };
    }

    void Inject()
    {
      try
      {
        if (_captureProcess == null) AttachProcess();
        else
        {
          _captureProcess.CaptureInterface.Disconnect();
          _captureProcess = null;
          Injected = false;
          Task.Factory.StartNew(() => Writer.SendColorToHyperion(0, 0, 0));
          Task.Factory.StartNew(() => Writer.SendColorToHyperion(0, 0, 0));
          Task.Factory.StartNew(() => Writer.SendColorToHyperion(0, 0, 0));
        }
      }
      catch (Exception E)
      {
        MessageBox.Show(E.Message);
        if (E.InnerException != null) MessageBox.Show(E.InnerException.Message);
      }
    }

    void AttachProcess()
    {
      string exeName = Path.GetFileNameWithoutExtension(ExeName.Text);

      Process[] processes = Process.GetProcessesByName(exeName);

      foreach (Process process in processes)
      {
        // Simply attach to the first one found.

        // If the process doesn't have a mainwindowhandle yet, skip it 
        // (we need to be able to get the hwnd to set foreground etc)
        if (process.MainWindowHandle == IntPtr.Zero)
          continue;

        // Skip if the process is already hooked (and we want to hook multiple applications)
        if (CaptureProcess.IsHooked(process.Id))
          continue;

        Direct3DVersion direct3DVersion = Direct3DVersion.Direct3D10;

        switch (DXVersion.SelectedIndex)
        {
          case 0:
            direct3DVersion = Direct3DVersion.AutoDetect;
            break;
          case 1:
            direct3DVersion = Direct3DVersion.Direct3D9;
            break;
          case 2:
            direct3DVersion = Direct3DVersion.Direct3D10;
            break;
          case 3:
            direct3DVersion = Direct3DVersion.Direct3D10_1;
            break;
          case 4:
            direct3DVersion = Direct3DVersion.Direct3D11;
            break;
        }

        var captureInterface = new CaptureInterface();
        captureInterface.RemoteMessage += (message) => Log.Dispatcher.Invoke(new Action(() =>
          Log.Text = string.Format("{0}\r\n{1}", message, Log.Text)));
        _captureProcess = new CaptureProcess(process, direct3DVersion, captureInterface);

        break;
      }

      Thread.Sleep(10);

      if (_captureProcess == null) MessageBox.Show("No DirectX executable found matching: '" + exeName + "'");
      else Injected = true;
    }

    void BeginWork()
    {
      Work = Task.Factory.StartNew(() =>
      {
        DateTime LastFrameWriteTime = DateTime.MinValue;
        Bitmap Frame = null;
        Task LastFrameWriteTask = null;

        try
        {
          while (ContinueCapture)
          {
            using (var screenshot = _captureProcess.CaptureInterface.GetScreenshot())
              if (screenshot != null && screenshot.Data != null)
                Frame = screenshot.ToBitmap();

            int Delay = LastFrameWriteTime == DateTime.MinValue
              ? 0
              : (int) (DateTime.Now - LastFrameWriteTime).TotalMilliseconds;

            LastFrameWriteTime = DateTime.Now;

            LastFrameWriteTask = Task.Factory.StartNew(() => Writer.WriteFrame(Frame, Delay));
          }

          if (LastFrameWriteTask != null) LastFrameWriteTask.Wait(500);
        }
        finally
        {
          if (Writer != null)
          {
            Writer.Dispose();
            Writer = null;
          }
        }
      });
    }

    void Record()
    {
      // Note: we bring the target application into the foreground because
      //       windowed Direct3D applications have a lower framerate 
      //       if not the currently focused window
      if (!Recording)
      {
        _captureProcess.BringProcessWindowToFront();

        start = DateTime.Now;

        ContinueCapture = true;

        Writer = new BmpWriter(
          Path.Combine(
            Environment.GetFolderPath(
              Environment.SpecialFolder.MyDocuments),
            start.ToString("yyyy-MM-dd-HH-mm-ss") + ".gif"), bmpFrameDelay, -1, hyperionIP, hyperionProtoPort);
        Recording = true;

        BeginWork();
      }
      else
      {
        ContinueCapture = false;

        Recording = false;

        if (Work != null) Work.Wait();

        Log.Text = string.Format("Debug: {0}\r\n{1}", "Total Time: " + (DateTime.Now - start).ToString(), Log.Text);
      }
    }

    private void HyperionIP_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        hyperionIP = HyperionIP.Text;
    }

    private void HyperionPort_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
      try
      {
          hyperionProtoPort = int.Parse(HyperionPort.Text);
      }
      catch (Exception)
      {
      }
    }

    private void FrameDelay_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
      try
      {
        bmpFrameDelay = int.Parse(FrameDelay.Text);
      }
      catch (Exception)
      {
      }
    }
  }
}
