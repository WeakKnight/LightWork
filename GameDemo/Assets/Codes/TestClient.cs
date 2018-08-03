using GameProtocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TestClient : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoginProtocol loginProtocol = new LoginProtocol() { userId = "hello", userPassword = "world" };
            ClientManager.instance.SendMessage(loginProtocol);
        }
    }
}
