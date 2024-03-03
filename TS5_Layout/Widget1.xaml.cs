using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WidgetSampleCS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Widget1 : Page
    {
        public static Widget1 Instance { get; private set; }
        private readonly SocketClient socketClient = null;
        public Widget1()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                return;
            }
            this.InitializeComponent();
            socketClient = new SocketClient("127.0.0.1", 5899);
        }

        public void Connected(object sender, EventArgs e)
        {
            _ = disconnected.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                disconnected.Opacity = 0;
            });
        }

        public void Disconnected(object sender, EventArgs e)
        {
            _ = disconnected.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                disconnected.Opacity = 1;
            });
        }

        public void Saying()
        {
            _ = avatar.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                avatar.Opacity = 1;
            });
        }

        public void NoSaying()
        {
            _ = avatar.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                avatar.Opacity = 0.2;
            });
        }

        public void Muted()
        {
            _ = muted.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                muted.Opacity = 1;
            });
        }

        public void NoMuted()
        {
            _ = muted.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                muted.Opacity = 0;
            });
        }
    }
}
