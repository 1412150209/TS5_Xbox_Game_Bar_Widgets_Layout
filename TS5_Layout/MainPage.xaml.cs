using System.Net;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.Foundation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WidgetSampleCS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            ApplicationView.PreferredLaunchViewSize = new Size(500, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(300, 300));
            this.InitializeComponent();
            _ = ip.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                ip.Text = SocketClient.GetConfig("ip");
            });
            _ = port.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                port.Text = SocketClient.GetConfig("port");
            });
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (SocketClient.Instance == null)
            {
                throw new Exception("Did not open the Xbox Game Bar Widget.");
            }
            bool flag = false;
            if (ip.Text != "")
            {
                if (IPAddress.TryParse(ip.Text, out IPAddress ipAddr))
                {
                    SocketClient.SetConfig("ip", ipAddr.ToString());
                    flag = true;
                }
                else
                {
                    ContentDialog ErrorIP = new ContentDialog()
                    {
                        Title = "IP set incorrectly.",
                        Content = "Check ip address and try again.(ipv4)",
                        CloseButtonText = "Ok"
                    };
                    _ = ErrorIP.ShowAsync();
                }
            }
            if (port.Text != "")
            {
                if (int.TryParse(port.Text, out int portA))
                {
                    SocketClient.SetConfig("port", portA.ToString());
                    flag = true;
                }
                else
                {
                    ContentDialog ErrorPort = new ContentDialog()
                    {
                        Title = "Port set incorrectly.",
                        Content = "Check port and try again.(int)",
                        CloseButtonText = "Ok"
                    };
                    _ = ErrorPort.ShowAsync();
                }
            }
            if (flag)
            {
                SocketClient.Instance.ReConnect(null, null);
            }
        }
    }
}
