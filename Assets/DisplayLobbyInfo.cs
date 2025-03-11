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


/* 패널을 직접 만드는 방식식
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
    public GameObject infoPanel; 
    private GameObject genObject;
    private bool panelState;
    public LobbyDataSO dataSO;
    private Canvas canvas;

    private void ToggleInfo() {
        if (Input.GetKeyDown(KeyCode.U)) {
            panelState = !panelState;
            infoPanel.SetActive(panelState);
        }
    }

    private void GeneratePanel() {
        genObject = Instantiate(infoPanel);
        genObject.transform.SetParent(canvas.transform, false);
    }

    private void SetLobbyValue() {
        genObject.transform.Find("IDValue").gameObject.GetComponent<TextMeshProUGUI>().text = dataSO.LobbyID;
        genObject.transform.Find("PWValue").gameObject.GetComponent<TextMeshProUGUI>().text = dataSO.password;
    }

    private void Start() {
        if (dataSO == null) return;
        canvas = FindObjectOfType<Canvas>();
        GeneratePanel();
        SetLobbyValue();
    }

    private void Update() {
        ToggleInfo();
    }
}

*/