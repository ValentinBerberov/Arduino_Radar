using System.IO.Ports;
using System.Threading.Tasks;

namespace ArduinoInterfacing;

public partial class Form1 : Form
{
    public SerialPort sp = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
    public Point[] points = new Point[161];
    public readonly int X_offset = 600;
    public readonly int Y_offset = 600;
    public int angle = 0;
    public int distance = 0;
    public int x_pos = 0;
    public int y_pos = 0;

    SerialDataReceivedEventHandler SP_DataHandler;

    public Form1()
    {
        for (int i = 0; i<161; ++i)
        {
            points[i].X = 0;
            points[i].Y = 0;
        }

        SP_DataHandler = new SerialDataReceivedEventHandler(sp_DataReceived);

        sp.DataReceived += SP_DataHandler;

        sp.Open();

        InitializeComponent();

        this.Paint += MyForm_Paint;
    }

    private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            string sp_output = sp.ReadLine();
            // Console.WriteLine(sp_output);

            _ = Task.Run(() => ProcessData(sp_output));

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private async Task ProcessData(string input)
    {
        int[] array_output = input.Trim().Split(" ").Select(int.Parse).ToArray();

        angle = array_output[0];
        distance = array_output[1];

        x_pos = X_offset + 10 + (int)Math.Floor(Math.Cos(Math.PI * angle / 180) * distance) * 5;
        y_pos = Y_offset - 10 - (int)Math.Floor(Math.Sin(Math.PI * angle / 180) * distance) * 5;

        points[angle].X = x_pos;
        points[angle].Y = y_pos;

        this.Refresh();
    }

    private void MyForm_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        Brush brush = Brushes.Black;
        Brush sensorBrush = Brushes.Red;

        g.FillRectangle(sensorBrush, X_offset, Y_offset, 10, 10);

        foreach (Point p in points)
        {
            // if(p.X!=0 && p.Y!=0)
            g.FillRectangle(brush, p.X, p.Y, 5, 5);
        }

    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        this.Paint -= MyForm_Paint;

        sp.DataReceived -= SP_DataHandler;

        base.OnFormClosing(e);
    }

}
