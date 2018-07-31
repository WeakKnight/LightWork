using GameProtocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TestClient : MonoBehaviour
{
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private Byte[] buffer;

    // Use this for initialization
    void Start()
    {
        buffer = new Byte[1024];
        ConnectToTcpServer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessage();
        }
    }

    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }

        catch (Exception e)
        {
            Debug.Log("On Client Connect Exception" + e);
        }
    }

    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 35353);
            socketConnection.Connect(ip);

            while (socketConnection.Connected)
            {
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        Byte[] incomingData = new Byte[length];
                        Array.Copy(buffer, 0, incomingData, 0, length);
                        string serverMessage = Encoding.ASCII.GetString(incomingData);
                        Debug.Log("server message received as" + serverMessage);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket Exception:" + socketException);
        }
    }

    private void SendMessage()
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                //string clientMessage = "This is a message from one of your clients.";
                LoginProtocol loginProtocol = new LoginProtocol() {userId = "hello", userPassword = "world" };
                Byte[] serializedData = ProtocolHelper.ConvertProtocolToBytes(loginProtocol);
                byte[] clientMessageAsByteArray = GameProtocol.Encoder.Encode(serializedData);
                //byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Client sent his message - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception:" + socketException);
        }
    }
}
