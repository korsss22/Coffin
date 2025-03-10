using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks;
using Mirror.BouncyCastle.Bcpg;
using Unity.VisualScripting;
using System.Security.Cryptography;
using UnityEngine.PlayerLoop;
using System.Runtime.Remoting;

public class DisplayLobbyInfo : MonoBehaviour
{
    public TextMeshProUGUI id;
    public TextMeshProUGUI pw;
    public GameObject infoPanel; 
    private bool panelState;
    public LobbyDataSO dataSO;

    private void ToggleInfo() {
        if (Input.GetKeyDown(KeyCode.U)) {
            panelState = !panelState;
            infoPanel.SetActive(panelState);
        }
    }

    private void Start() {
        if (dataSO == null) return;
        id.text = dataSO.LobbyID;
        pw.text = dataSO.password;
    }

    private void Update() {
        ToggleInfo();
    }
}
