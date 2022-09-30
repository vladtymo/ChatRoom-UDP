using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPEndPoint serverEndPoint;
        TcpClient client = new TcpClient();
        NetworkStream ns = null;
        StreamWriter sw = null;
        StreamReader sr = null;

        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = messages;

            string address = ConfigurationManager.AppSettings["ServerAddress"]!;
            short port = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);

            serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
        }

        private async void Listen()
        {
            while (true)
            {
                string? message = await sr.ReadLineAsync();
                messages.Add(new MessageInfo(message));
            }
        }

        private void DisconnectBtnClick(object sender, RoutedEventArgs e)
        {
            ns.Close();
            client.Close();
        }
        private void ConnectBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect(serverEndPoint);
                ns = client.GetStream();
                sw = new StreamWriter(ns);
                sr = new StreamReader(ns);
                Listen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendBtnClick(object sender, RoutedEventArgs e)
        {
            string message = msgTextBox.Text;

            sw.WriteLine(message);
            sw.Flush();
        }
    }
}
