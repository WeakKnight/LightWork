using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameProtocol;
using System.Threading;


public class LoginManager : MonoBehaviour {

    public InputField idInput;
    public InputField passwordInput;
    public Text messageText;

    public void Login()
    {
        LoginProtocol loginProtocol = new LoginProtocol()
        {
            userId = idInput.text,
            userPassword = passwordInput.text
        };

        ClientManager.instance.SendMessage(loginProtocol);
        ClientManager.instance.receiveCallBack += LoginReceive;

        messageText.text = "";
        idInput.interactable = false;
        passwordInput.interactable = false;
    }

    private void LoginReceive(Protocol protocol)
    {
        if (protocol.GetType() == typeof(LoginFailedProtocol))
        {
            LoginFailedProtocol loginFailedProtocol = protocol as LoginFailedProtocol;
            ClientManager.instance.receiveCallBack -= LoginReceive;
            
            idInput.interactable = true;
            passwordInput.interactable = true;

            if (loginFailedProtocol.State == LoginFailedState.WrongID)
            {
                messageText.text = "登录失败,错误的ID";
            }
            else if (loginFailedProtocol.State == LoginFailedState.WrongPassword)
            {
                messageText.text = "登录失败，错误的密码";
            }
            else
            {
                messageText.text = "登录失败，我也不知道为啥";
            }
        }
        else if (protocol.GetType() == typeof(LoginSuccessProtocol))
        {
            ClientManager.instance.receiveCallBack -= LoginReceive;
            messageText.text = "登录成功";
        }
    }

    public void Register()
    {
        RegisterProtocol registerProtocol = new RegisterProtocol()
        {
            userId = idInput.text,
            userPassword = passwordInput.text
        };

        ClientManager.instance.SendMessage(registerProtocol);
        ClientManager.instance.receiveCallBack += RegisterReceive;

        messageText.text = "";
        idInput.interactable = false;
        passwordInput.interactable = false;
    }

    private void RegisterReceive(Protocol protocol)
    {
        if (protocol.GetType() == typeof(RegisterFailedProtocol))
        {
            RegisterFailedProtocol registerFailedProtocol = protocol as RegisterFailedProtocol;
            ClientManager.instance.receiveCallBack -= RegisterReceive;

            idInput.interactable = true;
            passwordInput.interactable = true;

            if (registerFailedProtocol.state == RegisterFailedState.IDAlreadyExist)
            {
                messageText.text = "注册失败,已经存在的ID";
            }
            else if (registerFailedProtocol.state == RegisterFailedState.Unknown)
            {
                messageText.text = "注册失败，我也不知道为啥";
            }
        }
        else if (protocol.GetType() == typeof(RegisterSuccessProtocol))
        {
            ClientManager.instance.receiveCallBack -= RegisterReceive;
            messageText.text = "注册成功";
        }
    }
}
