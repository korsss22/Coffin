using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;


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

    private void OnLobbyCreated(LobbyCreated_t callback) {
        if (callback.m_eResult != EResult.k_EResultOK) {
            ButtonPanel.SetActive(true);
            return;
        }
        networkManager.StartHost();
        currentLobbyID = new CSteamID(callback.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(currentLobbyID, HostAddressKey, SteamUser.GetSteamID().ToString());

        SteamMatchmaking.SetLobbyData(currentLobbyID, "Password", "1234");      
    }

    private void OnLobbyEntered(LobbyEnter_t callback) {
        if (NetworkServer.active) return;

        currentLobbyID = new CSteamID(callback.m_ulSteamIDLobby);
        string HostAddress = SteamMatchmaking.GetLobbyData(currentLobbyID, HostAddressKey);
        
        if (HostAddress == null) {
            Debug.Log("Couldn't bring HostAddressKey.");    
            return;
        }

        networkManager.networkAddress = SteamMatchmaking.GetLobbyData(currentLobbyID, HostAddressKey);
        
        networkManager.StartClient();

        ButtonPanel.SetActive(false);
    }
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback) {
        SteamMatchmaking.JoinLobby(currentLobbyID);
    }

    public void ClickClient() {
        ButtonPanel.SetActive(false);
        ConnectPanel.SetActive(true);
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