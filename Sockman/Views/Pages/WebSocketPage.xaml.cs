using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ComponentModel;
using System.Net.WebSockets;
using System.Threading;

namespace Sockman.Views.Pages
{
    public class WebSocketPageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string url;
        private bool isOpened;

        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Url"));
            }
        }

        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OpenButtonVisibility"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CloseButtonVisibility"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SendButtonVisibility"));
            }
        }

        public Visibility OpenButtonVisibility
        {
            get { return isOpened ? Visibility.Collapsed : Visibility.Visible; ; }
        }

        public Visibility CloseButtonVisibility
        {
            get { return isOpened ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility SendButtonVisibility
        {
            get { return isOpened ? Visibility.Visible : Visibility.Collapsed; }
        }
    }

    /// <summary>
    /// WebSocketPage.xaml 的交互逻辑
    /// </summary>
    public partial class WebSocketPage : Page
    {
        public WebSocketPageModel Model { get; private set; }
        public ClientWebSocket Client { get; private set; }

        public WebSocketPage()
        {
            Model = new WebSocketPageModel
            {
                Url = "ws://127.0.0.1",
                IsOpened = false,
            };
            Client = new ClientWebSocket();
            InitializeComponent();
            DataContext = this;
        }

        private async void onClickConnectButton(object sender, RoutedEventArgs e)
        {
            if (Client.State != WebSocketState.Open)
            {
                TextBlock tb = new TextBlock();
                try
                {
                    Uri uri = new Uri(Model.Url);
                    await Client.ConnectAsync(uri, new CancellationToken());
                    tb.Text = string.Format("{0} 连接成功：{1}", DateTime.Now, Model.Url);
                    Model.IsOpened = true;
                }
                catch (Exception ex)
                {
                    Model.IsOpened = false;
                    Client = new ClientWebSocket();
                    tb.Text = ex.Message;
                }
                InfoPanel.Children.Add(tb);
            }
        }

        private async void onClickSendButton(object sender, RoutedEventArgs e)
        {
            if (Client.State == WebSocketState.Open)
            {
                await Client.SendAsync(Encoding.UTF8.GetBytes(MessageTextarea.Text), WebSocketMessageType.Text, true, new CancellationToken());
                TextBlock stb = new TextBlock();
                stb.Text = string.Format("{0} 发送：{1}", DateTime.Now, MessageTextarea.Text);
                InfoPanel.Children.Add(stb);
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                await Client.ReceiveAsync(buffer, new CancellationToken());
                TextBlock rtb = new TextBlock();
                rtb.Text = string.Format("{0} 接收：{1}", DateTime.Now, Encoding.UTF8.GetString(buffer.Array));
                InfoPanel.Children.Add(rtb);
                
            }
        }

        private async void onClickDisconnectButton(object sender, RoutedEventArgs args)
        {
            if (Client.State == WebSocketState.Open)
            {
                try
                {
                    await Client.CloseAsync(WebSocketCloseStatus.NormalClosure, null, new CancellationToken());
                }
                catch (WebSocketException e)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = string.Format("{0} 异常：{1}", DateTime.Now, e.Message);
                    InfoPanel.Children.Add(tb);
                }
                finally
                {
                    Client = new ClientWebSocket();
                    Model.IsOpened = false;
                }
            }
        }
    }
}
