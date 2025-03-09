using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;
using System.Drawing;
using System.Runtime.Remoting.Messaging;

public class SteamLobby : MonoBehaviour
{
    public GameObject ButtonPanel = null;
    public GameObject ConnectPanel = null;
    public GameObject CancelButton = null;
    public GameObject Message = null;
    public TMP_InputField LobbyIdInputField;
    public TMP_InputField PasswordInputField;
    public CSteamID currentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private NetworkManager networkManager;
    private string LobbyID;
    private string LobbyPassword;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private void Start() {
        networkManager = GetComponent<NetworkManager>();

        if (!SteamManager.Initialized) return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested  = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    // 로비가 생성되었을 때 호출되는 콜백
    private void OnLobbyCreated(LobbyCreated_t callback) {
        if (callback.m_eResult != EResult.k_EResultOK) {
            ButtonPanel.SetActive(true);
            return;
        }

        networkManager.StartHost();
        currentLobbyID = new CSteamID(callback.m_ulSteamIDLobby);

<<<<<<< Updated upstream
        SteamMatchmaking.SetLobbyData(currentLobbyID, "Captain", SteamUser.GetSteamID().ToString());
=======
        SteamMatchmaking.SetLobbyData(currentLobbyID, HostAddressKey, SteamUser.GetSteamID().ToString());
>>>>>>> Stashed changes
        SteamMatchmaking.SetLobbyData(currentLobbyID, "Password", "1234");      
    }

    // 로비에 성공적으로 입장한 후 호출되는 콜백
    private void OnLobbyEntered(LobbyEnter_t callback) {
        if (NetworkServer.active) return;

        currentLobbyID = new CSteamID(callback.m_ulSteamIDLobby);

<<<<<<< Updated upstream
    // 네트워크 주소를 설정
    networkManager.networkAddress = currentLobbyID.ToString();
    
    // 클라이언트 시작
    networkManager.StartClient();

    hostButton.SetActive(false);
}


    // 호스트 로비 생성
    public void HostLobby() {
        hostButton.SetActive(false);
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    // 클라이언트 버튼 클릭 시 연결 대기 패널 활성화
    public void ClickClient() {
        hostButton.SetActive(false);
        ClientButton.SetActive(false);
        ConnectPanel.SetActive(true);
    }

    // 연결 시작 (로비 ID 및 비밀번호 확인 후)
    public void StartConnect() {
        LobbyID = LobbyIdInputField.text; // 입력된 로비 ID 저장
        LobbyPassword = PasswordInputField.text; // 입력된 비밀번호 저장

        if (string.IsNullOrEmpty(LobbyID) || string.IsNullOrEmpty(LobbyPassword)) {
            Debug.LogError("로비 ID와 비밀번호를 모두 입력해주세요.");
=======
        // 만약 hostAddress가 null이거나 비어 있다면 오류 메시지를 출력하고 종료합니다.
        if (string.IsNullOrEmpty(currentLobbyID.ToString())) {
            Debug.LogError("호스트 주소를 가져오지 못했습니다.");
>>>>>>> Stashed changes
            return;
        }

        Debug.Log(currentLobbyID);

        // 네트워크 주소를 설정
        networkManager.networkAddress = SteamMatchmaking.GetLobbyData(currentLobbyID, HostAddressKey);
        
        // 클라이언트 시작
        networkManager.StartClient();
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback) {
        SteamMatchmaking.JoinLobby(currentLobbyID);
    }

    public void ClickHostButton() {
        ButtonPanel.SetActive(false);

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    public void ClickClientButton() {
        ButtonPanel.SetActive(false);
        ConnectPanel.SetActive(true);
    }

    public void ClickConnectButton() {
        LobbyID = LobbyIdInputField.text;
        LobbyPassword = PasswordInputField.text;
        
        if (LobbyPassword == "1234") {
            Debug.Log("Password correct!! Trying to Join Lobby....");
            SteamMatchmaking.JoinLobby(new CSteamID(ulong.Parse(LobbyID)));
            Message.SetActive(true);
        }
    }
    public void ClickCancelButton() {
        ButtonPanel.SetActive(true);
        ConnectPanel.SetActive(false);
    }
}
