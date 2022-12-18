using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectIct
{
    public partial class MainWindow : Window
    {
        public string ReceivedData;
        bool isConnected = false;
        String[] ports;
        SerialPort arduinoPort;
        uint highscore = 0;

        public MainWindow()
        {
            InitializeComponent();
            arduinoPortName.Items.Add("None");
            foreach (string s in SerialPort.GetPortNames())
            arduinoPortName.Items.Add(s);
            arduinoPort = new SerialPort();
            arduinoPort.BaudRate = 9600;
            arduinoPort.Parity = Parity.None;
            arduinoPort.StopBits = StopBits.One;
            arduinoPort.DataReceived += new SerialDataReceivedEventHandler(Receive);
        }

        private void arduinoPortName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(arduinoPort != null)
            {
                if(arduinoPort.IsOpen)
                   arduinoPort.Close();

                if (arduinoPortName.SelectedItem.ToString() != "None")
                {
                    arduinoPort.PortName = arduinoPortName.SelectedItem.ToString();
                    arduinoPort.Open();
                    buttonJump.IsEnabled = true;
                    buttonStart.IsEnabled = true;
                    buttonReset.IsEnabled = true;
                }
                else
                {   
                    buttonJump.IsEnabled = false;
                    buttonStart.IsEnabled = false;
                    buttonReset.IsEnabled = false;
                }
            }
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            arduinoPort.Write("S");
        }

        private void buttonJump_Click(object sender, RoutedEventArgs e)
        {
            arduinoPort.Write("J");
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            arduinoPort.Write("R");
        }

        private void Receive(object sender, SerialDataReceivedEventArgs e)
        {
            ReceivedData = arduinoPort.ReadExisting();
            Dispatcher.BeginInvoke(new ThreadStart(() => scoreText.Text = ReceivedData));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            arduinoPort.Write("R");
        }
    }
}