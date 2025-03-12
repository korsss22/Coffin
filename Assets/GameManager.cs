using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance {get; private set;} 

    public GameObject timerText; // UI 텍스트 (로컬에서만 사용)

    [Range(1f, 10f)]
    public int countTime = 3;
    private Coroutine countdownCoroutine;

    [SyncVar(hook = nameof(OnTimeUpdated))]
    public int timer = 0; // 서버에서 동기화되는 타이머 값

    [SyncVar(hook = nameof(OnReadyUpdated))]
    public int readyPlayers = 0; // Q 버튼을 누르고 있는 플레이어 수

    private Coroutine nowCoroutine;
    public bool isCountDown = false;

    private void Awake() {
    // 이미 인스턴스가 존재하면, 새로 만들지 않고 기존 인스턴스를 사용
        if (instance != null && instance != this) {
            Destroy(gameObject); // 다른 GameManager가 이미 존재하는 경우 현재 객체를 파괴
        } else {
            instance = this; // 현재 인스턴스를 설정
            DontDestroyOnLoad(gameObject); // 씬이 변경되더라도 객체가 파괴되지 않도록 설정
        }
    }

    public void CreateObjectOnCanvas(GameObject obj) {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) {
            Debug.LogError("canvas is null!");
            return;
        }
        if (obj == null) {
            Debug.LogError("Object is null!!");
            return;
        }
        Instantiate(obj, canvas.transform);
    }

    private void OnReadyUpdated(int oldCount, int newCount) {
        readyPlayers = newCount;
        Debug.Log("ReadyPlayerCount : "+readyPlayers);
    }

    private void OnTimeUpdated(int oldTime, int newTime) {
        timer = newTime;
    }

    private bool CheckAllPlayerReady() {
        int maxPlayer = NetworkServer.connections.Count;
        if (readyPlayers == maxPlayer && maxPlayer != 0) {
            return true; 
        }
        return false;
    }

    private void CountDown() {
    if (isCountDown) return; // 이미 실행 중이면 중복 실행 방지
    Debug.Log("CountDown 실행");
    isCountDown = true;
    timerText.SetActive(true);
    nowCoroutine = StartCoroutine(CountDownCoroutine(countTime));
}


    private IEnumerator CountDownCoroutine(int countTime) {
    timer = countTime;
    Debug.Log("timerText 보임");
    
    while (timer > 0) {
        yield return new WaitForSeconds(1);
        timer--;
    }
    
    GameStart();
    isCountDown = false; // 카운트다운 종료 후 상태 초기화
}
    [Command]
    public void TurnIsKinematic(bool value) {
        MyNetworkManager.instance.Base.GetComponent<Rigidbody>().isKinematic = value;
        MyNetworkManager.instance.Lid.GetComponent<Rigidbody>().isKinematic = value;
    }

    private void CheckCountDown() {
    if (CheckAllPlayerReady()) {
        Debug.Log("현재 true");
        CountDown(); // 이미 실행 중이면 실행되지 않음
    } else {
        Debug.Log("현재 false");
        if (isCountDown) {
            isCountDown = false;
            // timerText.SetActive(false);
            if (nowCoroutine != null) {
                StopCoroutine(nowCoroutine);
                nowCoroutine = null;
            }
        }
    }
}

    private void UpdateTimer() {
        if (isCountDown) {
            timerText.GetComponent<TextMeshProUGUI>().text = timer.ToString("0");
        }
    }

    [Server]
    private void GameStart()
    {
        Debug.Log("Game Start!!!");
        timerText.SetActive(false);

        if (MyNetworkManager.instance == null) {
            Debug.LogError("instance is null!");
        }
        MyNetworkManager.instance.Base.GetComponent<Rigidbody>().isKinematic = false;
        MyNetworkManager.instance.Lid.GetComponent<Rigidbody>().isKinematic = false;
        MyNetworkManager.instance.Base.GetComponent<ConfigurableJoint>().connectedBody =
        MyNetworkManager.instance.jointPoint.GetComponent<Rigidbody>();
        // 게임 시작 로직 추가
    }

    private void Update() {
        CheckCountDown();
        UpdateTimer();
    }
}
