using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
        UdpClient client = new UdpClient();

        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        private bool isListening = false;

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
            while (isListening)
            {
                var result = await client.ReceiveAsync();
                string message = Encoding.UTF8.GetString(result.Buffer);

                messages.Add(new MessageInfo(message));
            }
        }

        private void SendBtnClick(object sender, RoutedEventArgs e)
        {
            string text = msgTextBox.Text;
            SendMessage(text);
        }
        private void JoinBtnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("$<join>");
            if (!isListening)
            {
                isListening = true;
                Listen();
            }
        }

        private void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, serverEndPoint);
        }
    }
}
