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
using Mirror;

public class UIManager : NetworkBehaviour
{
    public static UIManager instance {get; private set;}
    private TextMeshProUGUI id;
    private TextMeshProUGUI pw;
    private TextMeshProUGUI ready;
    public GameObject infoPanel; 
    public GameObject lobbyData;
    private bool panelState = false;
    private Canvas canvas;
    public LobbyDataSO dataSO;

    private void ToggleInfo() {
        if (Input.GetKeyDown(KeyCode.U) && isServer) {
            panelState = !panelState;
            lobbyData.SetActive(panelState);
        }
    }

    private void PrintPlayerReady() {
        ready.text = GameManager.instance.readyPlayer +" / "+ NetworkServer.connections.Count;
    }

    public void UpdateTimer(GameObject timer, int currentTime,bool value) {
        timer.SetActive(value);
        timer.GetComponent<TextMeshProUGUI>().text = currentTime.ToString("0");
    }

    private void GetComponents() {
        id = infoPanel.transform.Find("IDValue").GetComponent<TextMeshProUGUI>();
        pw = infoPanel.transform.Find("PWValue").GetComponent<TextMeshProUGUI>();
        ready = infoPanel.transform.Find("Ready").GetComponent<TextMeshProUGUI>();
    }

    public void DisplayLobbyData() {
        if (isServer && (dataSO != null)) {
            id.text = dataSO.LobbyID;
            pw.text = dataSO.password;
        }
    }

    public void SetTimer() {
        GetCanvas();
        InstantiateAsChild(GameManager.instance.timer, canvas.transform);
    }

    public Canvas GetCanvas() {
        canvas = FindObjectOfType<Canvas>();
        return canvas;
    }

    public GameObject InstantiateAsChild(GameObject prefab, Transform parents) {
        Debug.Log("UI 생성 완료료");
        return Instantiate(prefab, parents);
    }

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        } 
            instance = this;
            DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        GetComponents();
    }

    private void Update() {
        ToggleInfo();
        PrintPlayerReady();
        Debug.Log("isServer" + isServer);
        Debug.Log("panel State : "+panelState);
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