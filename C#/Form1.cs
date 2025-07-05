using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Threading.Tasks;

namespace ArduinoInterfacing;

public partial class Form1 : Form
{
    public SerialPort sp = new();
    public Rectangle[] rects = new Rectangle[321];
    public readonly int X_offset = 600;
    public readonly int Y_offset = 600;
    public float angle = 0;
    public int distance = 0;
    public int x_pos = 0;
    public int y_pos = 0;
    public static volatile int refresh = 0;


    Bitmap bmp;
    Graphics graphics;
    GraphicsPath path;
    StreamReader streamReader;
    SerialDataReceivedEventHandler SP_DataHandler;
    CancellationTokenSource portSearchTokenSource = new();
    Task portSearchTask;

    public Form1()
    {

        InitializeComponent();

        CancellationToken portSearchCancellationToken = portSearchTokenSource.Token;

        portSearchTask = Task.Run(() => SearchForPort("COM4", portSearchCancellationToken));

        for (int i = 0; i < rects.Count(); ++i)
        {
            rects[i].X = 0;
            rects[i].Y = 0;
            rects[i].Height = 0;
            rects[i].Width = 0;
        }


        this.Paint += MyForm_Paint;
    }

    private async void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            string sp_output = await streamReader.ReadLineAsync();
            // Console.WriteLine(sp_output);

            _ = Task.Run(() => ProcessData(sp_output));

        }
        catch { }

    }

    private async Task ProcessData(string input)
    {
        float[] array_output = input.Trim().Split(" ").Select(float.Parse).ToArray();

        angle = array_output[0];
        distance = (int)array_output[1];

        x_pos = X_offset + (int)Math.Floor(Math.Cos(Math.PI * angle / 180) * distance) * 5;
        y_pos = Y_offset - (int)Math.Floor(Math.Sin(Math.PI * angle / 180) * distance) * 5;

        int idx = (int)Math.Round(angle * 2);

        rects[idx].X = x_pos;
        rects[idx].Y = y_pos;
        rects[idx].Height = 5;
        rects[idx].Width = 5;

        // Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

        ++refresh;

        if (refresh >= 5)
        {
            refresh = 0;

            // this.Refresh();

            bmp = new Bitmap(pictureBox.Width, pictureBox.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            graphics = Graphics.FromImage(bmp);
            path = new System.Drawing.Drawing2D.GraphicsPath();

            path.AddRectangles(rects);
            graphics.FillPath(Brushes.Black, path);
            graphics.FillRectangle(Brushes.Red, X_offset, Y_offset, 10, 10);

            Refresh();
        }

    }

    private void MyForm_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        Brush brush = Brushes.Black;
        Brush sensorBrush = Brushes.Red;

        // g.FillRectangles(brush, rects);
        
        pictureBox.Image = bmp;
        // pictureBox.BringToFront();

        g.FillRectangle(sensorBrush, X_offset, Y_offset, 10, 10);
    }

    private async Task SearchForPort(string serialPort, CancellationToken cancellationToken)
    {
        while (true)
        {
            try
            {
                if (!(sp?.IsOpen ?? false))
                {
                    sp.PortName = serialPort;
                    sp.BaudRate = 9600;
                    sp.Parity = Parity.None;
                    sp.DataBits = 8;
                    sp.StopBits = StopBits.One;

                    SP_DataHandler = new SerialDataReceivedEventHandler(sp_DataReceived);

                    sp.DataReceived += SP_DataHandler;

                    sp.Open();

                    streamReader = new(sp.BaseStream);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await Task.Delay(2000);
        }
        

    }

    protected async override void OnFormClosed(FormClosedEventArgs e)
    {
        this.Paint -= MyForm_Paint;

        sp.DataReceived -= SP_DataHandler;

        portSearchTokenSource.Cancel();

        await Task.WhenAll(portSearchTask);

        base.OnFormClosed(e);
    }

}
