using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks;

public class DisplayLobbyInfo : MonoBehaviour
{
    public TextMeshProUGUI Id;
    public TextMeshProUGUI Pw;
    private SteamLobby steamLobby;

    private void Awake() {
        if (gameObject != null) {
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
