using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LobbyDataSO", menuName = "LobbyDataSO", order = 0)]
public class LobbyDataSO : ScriptableObject {
    public string LobbyID;
    public string password;

    public void SetLobbyData(string LobbyID, string password) {
        this.LobbyID = LobbyID;
        this.password = password;
    }
}
