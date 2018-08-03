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

        messageText.text = "";
        idInput.interactable = false;
        passwordInput.interactable = false;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
