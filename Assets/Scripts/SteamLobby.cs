using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    public GameObject hostButton = null;
    public GameObject ClientButton = null;
    public GameObject ConnectPanel = null;
    public GameObject CancelButton = null;
    public GameObject Message = null;
    public TMP_InputField LobbyIdInputField;
    public TMP_InputField PasswordInputField;
    public CSteamID currentLobbyID;

    private NetworkManager networkManager;
    private string LobbyID;
    private string LobbyPassword;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private void Start() {
        networkManager = GetComponent<NetworkManager>();

        if (!SteamManager.Initialized) return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    // 로비가 생성되었을 때 호출되는 콜백
    private void OnLobbyCreated(LobbyCreated_t callback) {
        if (callback.m_eResult != EResult.k_EResultOK) {
            hostButton.SetActive(true);
            return;
        }

        networkManager.StartHost();
        currentLobbyID = new CSteamID(callback.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(currentLobbyID, "Captain", SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(currentLobbyID, "Password", "1234");      
    }

    // 로비에 성공적으로 입장한 후 호출되는 콜백
    private void OnLobbyEntered(LobbyEnter_t callback) {
    if (NetworkServer.active) return;
    Debug.Log("LobbyEntered");

    // 만약 hostAddress가 null이거나 비어 있다면 오류 메시지를 출력하고 종료합니다.
    if (string.IsNullOrEmpty(currentLobbyID.ToString())) {
        Debug.LogError("호스트 주소를 가져오지 못했습니다.");
        return;
    }

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
            return;
        }

        // 여기서 비밀번호를 검증하는 로직을 추가해야 한다
        // 예시: 서버에서 저장된 비밀번호와 일치하는지 확인하는 코드 추가
        if (ValidatePassword(LobbyPassword)) {
            Debug.Log("비밀번호 검증 성공, 로비에 참가합니다.");
            JoinLobby();
        } else {
            Debug.LogError("비밀번호가 올바르지 않습니다.");
        }
    }

    // 비밀번호 검증 함수 (여기서는 임시로 "1234"로 고정)
    private bool ValidatePassword(string password) {
        // 실제 비밀번호 확인 로직을 구현해야 함 (서버에 비밀번호 저장 후 확인)
        return password == "1234"; // 임시 비밀번호 검증
    }

    // 로비에 참가하는 함수
    private void JoinLobby() {
        // 로비에 참가
        SteamMatchmaking.JoinLobby(currentLobbyID);

        // 로비 참가 후 UI 처리
        hostButton.SetActive(false);
        ClientButton.SetActive(false);
        ConnectPanel.SetActive(false);
        Message.SetActive(true);
    }

    // 연결 취소 시 UI 상태 복원
    public void Cancel() {
        hostButton.SetActive(true);
        ClientButton.SetActive(true);
        ConnectPanel.SetActive(false);
        Message.SetActive(false);
    }
}
