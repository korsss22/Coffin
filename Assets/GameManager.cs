using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance {get; private set;} 

    public Text timerText; // UI 텍스트 (로컬에서만 사용)

    [Range(1f, 10f)]
    public int countTime = 3;
    private Coroutine countdownCoroutine;

    [SyncVar]
    public float timer = 0f; // 서버에서 동기화되는 타이머 값

    [SyncVar(hook = nameof(OnReadyUpdated))]
    public int readyPlayers = 0; // Q 버튼을 누르고 있는 플레이어 수

    private void Awake() {
    // 이미 인스턴스가 존재하면, 새로 만들지 않고 기존 인스턴스를 사용
        if (instance != null && instance != this) {
            Destroy(gameObject); // 다른 GameManager가 이미 존재하는 경우 현재 객체를 파괴
        } else {
            instance = this; // 현재 인스턴스를 설정
            DontDestroyOnLoad(gameObject); // 씬이 변경되더라도 객체가 파괴되지 않도록 설정
        }
    }


    private void OnReadyUpdated(int oldCount, int newCount) {
        readyPlayers = newCount;
        Debug.Log("ReadyPlayerCount : "+readyPlayers);
    }

    private void CheckAllPlayerReady() {
        if (readyPlayers == NetworkServer.connections.Count) {
            //StartCoroutine(countdownCoroutine(countTime));
        }
    }

    [Server]
    private void GameStart()
    {
        Debug.Log("Game Start!!!");
        // 게임 시작 로직 추가
    }
}
