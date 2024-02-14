using System;
using Newtonsoft.Json.Linq;
using WidgetSampleCS;
using Windows.Storage;
using WebSocket4Net;
using SuperSocket.ClientEngine;

public class SocketClient
{
    public static SocketClient Instance { get; private set; }
    public static WebSocket clientWebSocket;
    public static string serverUri;
    public static string apikey;
    private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;


    public SocketClient(string ip, int port)
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }
        if (GetConfig("ip") != "" && GetConfig("port") != "")
        {
            ip = GetConfig("ip");
            port = int.Parse(GetConfig("port"));
        }
        SetConfig("ip", ip);
        SetConfig("port", port.ToString());
        ReConnect(null, null);
    }

    public void ReConnect(object sender, ErrorEventArgs e)
    {
        Release();
        serverUri = $@"ws://{GetConfig("ip")}:{GetConfig("port")}";
        clientWebSocket = new WebSocket(serverUri);
        clientWebSocket.Opened += Initialize;
        clientWebSocket.Opened += Widget1.Instance.Connected;
        clientWebSocket.Closed += Widget1.Instance.Disconnected;
        clientWebSocket.Error += ReConnect;
        clientWebSocket.Open();
    }

    ~SocketClient()
    {
        Release();
    }

    private void Release()
    {
        if (clientWebSocket != null)
        {
            if (clientWebSocket.State == WebSocketState.Open)
            {
                clientWebSocket.Close();
            }
            clientWebSocket.Dispose();
        }
    }

    public static string GetConfig(string element)
    {
        return localSettings.Values[element] as string ?? "";
    }

    public static void SetConfig(string element, string value)
    {
        localSettings.Values[element] = value;
    }

    public void Initialize(object sender, EventArgs e)
    {
        if (clientWebSocket.State == WebSocketState.Open)
        {
            if (GetConfig("apikey") != "")
            {
                apikey = GetConfig("apikey");
            }
            else
            {
                apikey = "";
            }
            clientWebSocket.MessageReceived += Authentication;
            SendMessage($@"{{
                ""type"": ""auth"",
                ""payload"": {{
                    ""identifier"": ""fun.lers.app"",
                    ""version"": ""0.1.0"",
                    ""name"": ""Xbox Game Bar Layer"",
                    ""description"": ""An application to show whether you are taking or not."",
                    ""content"": {{
                        ""apiKey"": ""{apikey}""
                    }}
                }}
            }}");
        }
    }

    private void SendMessage(string msg)
    {
        clientWebSocket.Send(msg);
    }


    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        ReadMessage(JObject.Parse(e.Message));
    }

    public void ReadMessage(JObject message)
    {
        if (message != null && message["type"].ToString() == "clientSelfPropertyUpdated" && message["payload"]["flag"].ToString() == "flagTalking")
        {
            if (bool.Parse(message["payload"]["newValue"].ToString()) == true)
            {
                Widget1.Instance.Saying();
            }
            else
            {
                Widget1.Instance.Muting();
            }
        }
        // 解决内存越用越多的问题
        GC.Collect();
    }

    public void Authentication(object sender, MessageReceivedEventArgs e)
    {
        var message = JObject.Parse(e.Message);
        if (message != null && message["type"].ToString() == "auth" && message["status"]["message"].ToString() == "ok")
        {
            apikey = message["payload"]["apiKey"].ToString();
            SetConfig("apikey", apikey);
            clientWebSocket.MessageReceived -= Authentication;
            clientWebSocket.MessageReceived += OnMessageReceived;
        }
        else
        {
            throw new Exception("Failed to connect TS5");
        }
    }
}