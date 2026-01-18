using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    
    [SerializeField] TMP_InputField ip, port, nickname;
    

    public void Quit()
    {
        Application.Quit();
    }

    public void EnterLobby()
    {
        int p = 0;
        
        if ( (!(ip.text == "" || nickname.text == "" || port.text == "")) && Int32.TryParse(port.text, out p))
        {
            Client.ip = ip.text;
            Client.port = p;
            Client.username = nickname.text; 
        }
        
        SceneManager.LoadScene(1);
    }
}
