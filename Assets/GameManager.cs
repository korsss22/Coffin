using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance {get; private set;}

    [SyncVar(hook = nameof(OnReadyPlayerUpdated))]
    public int readyPlayer = 0;
    public GameObject timer;
    [Range(1, 10)]
    public int countTime;
    [SyncVar]
    public int currentTime;
    private List<GameObject> spawnPrefabs;
    public GameObject coffin;
    public Transform coffin_Base;
    public Transform coffin_Lid;
    public GameObject jointPoint;
    private Coroutine nowCoroutine;
    private void OnReadyPlayerUpdated(int oldValue, int newValue) {
        Debug.Log("readyPlayer의 값이 "+newValue+"가 되었어요!! *^^*");
    }

<<<<<<< Updated upstream
    public bool started = false;

    private void Awake() {
    // 이미 인스턴스가 존재하면, 새로 만들지 않고 기존 인스턴스를 사용
        if (instance != null && instance != this) {
            Destroy(gameObject); // 다른 GameManager가 이미 존재하는 경우 현재 객체를 파괴
=======
    
    public void GameStart() {
        if (!isServer) {Debug.Log("isnotServer");return;}

        if (readyPlayer == NetworkServer.connections.Count) {
            Debug.Log("allReady!!!!");
            if (nowCoroutine == null) {
                nowCoroutine = StartCoroutine(CountDown(3));
                Debug.Log("started Coroutine!!!!");
            }
>>>>>>> Stashed changes
        } else {
            if (nowCoroutine != null) {
                StopCoroutine(nowCoroutine);
            }
        }
    }

    private IEnumerator CountDown(int countTime) {
        currentTime = countTime;

        while (currentTime > 0) {
            yield return new WaitForSeconds(1);
            currentTime -= 1;
        }

        StartObject();
    }

    private void StartObject() {
        coffin = GameObject.FindGameObjectWithTag("Coffin");

        coffin_Base = coffin.transform.Find("Base");
        coffin_Lid = coffin.transform.Find("Lid");

        Rigidbody baseRB = coffin.transform.Find("Base").gameObject.GetComponent<Rigidbody>();
        Rigidbody lidRB = coffin.transform.Find("Lid").gameObject.GetComponent<Rigidbody>();

        baseRB.isKinematic = false;
        lidRB.isKinematic = false;

        // Joint 연결 설정
        ConfigurableJoint coffinJoint = coffin_Base.gameObject.GetComponent<ConfigurableJoint>();
        if (coffinJoint != null)
        {
            coffinJoint.connectedBody = jointPoint.GetComponent<Rigidbody>();
            Debug.Log("Joint가 연결되었습니다.");
        }
        else
        {
            Debug.LogError("ConfigurableJoint 컴포넌트를 찾을 수 없습니다.");
        }
    }

    public GameObject SpawnNetworkObject(string objectName, Vector3 location, Quaternion rotation) {
        GameObject selectedObject = null;
        foreach (GameObject obj in spawnPrefabs) {
            if (obj.name == objectName) {
                selectedObject = obj;
                break;
            }
        }
        if (selectedObject == null) {
            Debug.LogError("입력하신 오브젝트를 찾을수가 없어요 ㅠㅠ NetworkManager의 rigistered spawnable prefabs 목록을 확인하거나 입력하신 문자열을 확인해주세요..");
            return selectedObject;
        }
        GameObject instantiatedObject = Instantiate(selectedObject, location, rotation);
        NetworkServer.Spawn(instantiatedObject);

        return instantiatedObject;
    }
    
    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        } 
            instance = this;
            DontDestroyOnLoad(gameObject);
    }

<<<<<<< Updated upstream
    private void LateUpdate() {
        if (CheckAllPlayerReady && !started)
        {
            GameStart();
            started = false;
        }
=======
    void Start() {
        spawnPrefabs = MyNetworkManager.instance.spawnPrefabs;
>>>>>>> Stashed changes
    }

    void Update()
    {

    }

}
