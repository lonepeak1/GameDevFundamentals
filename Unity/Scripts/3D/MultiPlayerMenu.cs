using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiPlayerMenu : MonoBehaviour
{
    public string PlayerName = "";
    public TMPro.TMP_InputField nameField;
    public TMPro.TMP_Text ipField;
    public TMPro.TMP_Text portField;
    public TMPro.TMP_Text connectedIPDisplayText;

    public GameObject NameEntryUI;
    public GameObject StartUI;
    public GameObject StopUI;

    NetworkManager netWorkManager;
    Unity.Netcode.Transports.UNET.UNetTransport transport;
    // Start is called before the first frame update
    void Start()
    {
        netWorkManager = GameObject.FindObjectOfType<NetworkManager>();
        netWorkManager.OnClientConnectedCallback += NetWorkManager_OnClientConnectedCallback;
        netWorkManager.OnClientDisconnectCallback += NetWorkManager_OnClientDisconnectCallback;
        transport = GameObject.FindObjectOfType<Unity.Netcode.Transports.UNET.UNetTransport>();
        connectedIPDisplayText.text = "";
        if(connectedIPDisplayText!=null)
            connectedIPDisplayText.gameObject.active = false;
        NameEntryUI.active = true;
        StartUI.active = false;
        StopUI.active = false;
    }

    private void NetWorkManager_OnClientDisconnectCallback(ulong obj)
    {
        UpdateConnectedClientCount();
    }

    private void NetWorkManager_OnClientConnectedCallback(ulong obj)
    {
        UpdateConnectedClientCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartHost()
    {
        getSettings();
        netWorkManager.StartHost();
        
        StartUI.active = false;
        StopUI.active = true;
    }

    public void StartClient()
    {
        getSettings();
        netWorkManager.StartClient();
        StartUI.active = false;
        StopUI.active = true;
    }

    void getSettings()
    {
        string connectAddress = ipField.text.Trim((char)8203) == string.Empty ? "127.0.0.1" : ipField.text.Trim((char)8203);
        transport.ConnectAddress = connectAddress;
        transport.ConnectPort = portField.text.Trim((char)8203)== string.Empty ? 7777 : int.Parse(portField.text.Trim((char)8203));
        UpdateConnectedClientCount();
    }

    public void UpdateConnectedClientCount()
    {
        if (NameEntryUI != null)
        {
            connectedIPDisplayText.gameObject.active = true;
            connectedIPDisplayText.text = transport.ConnectAddress + ":" + transport.ConnectPort.ToString();
            if (netWorkManager.IsHost)
            {
                connectedIPDisplayText.text += "\n" + netWorkManager.ConnectedClientsList.Count + " Connections";
            }
        }
    }

    public void stop()
    {
        netWorkManager.Shutdown();
        StartUI.active = true;
        StopUI.active = false;
    }

    public void SaveName()
    {
        PlayerName = nameField.text;
        NameEntryUI.active = false;
        StartUI.active = true;
        StopUI.active = false;
    }
}
