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
    private GameObject jointPoint;
    private Coroutine nowCoroutine;
    public bool isGameOver = false;
    public bool isGameClear = false;
    private void OnReadyPlayerUpdated(int oldValue, int newValue) {
        Debug.Log("readyPlayer의 값이 "+newValue+"가 되었어요!! *^^*");
    }

    [Server]
    public void GameStart() {
        if (!isServer) {Debug.Log("isnotServer");return;}

        if (readyPlayer == NetworkServer.connections.Count) {
            Debug.Log("allReady!!!!");
            if (nowCoroutine == null) {
                nowCoroutine = StartCoroutine(CountDown(3));
                Debug.Log("started Coroutine!!!!");
            }
        } else {
            if (nowCoroutine != null) {
                StopCoroutine(nowCoroutine);
                nowCoroutine = null;
                Debug.Log("StopCoroutine");
                // UIManager.instance.UpdateTimer(timer, 0,false);
            }
        }
    }

    [Server]
    private IEnumerator CountDown(int countTime) {
        currentTime = countTime;

        while (currentTime > 0) {
            // UIManager.instance.UpdateTimer(timer,currentTime,true);
            yield return new WaitForSeconds(1);
            currentTime -= 1;
        }

        StartObject();
    }

    private void StartObject() {
    coffin = GameObject.FindGameObjectWithTag("Coffin");

    coffin_Base = coffin.transform.Find("Base");
    coffin_Lid = coffin.transform.Find("Lid");

    Rigidbody baseRB = coffin_Base.gameObject.GetComponent<Rigidbody>();
    Rigidbody lidRB = coffin_Lid.gameObject.GetComponent<Rigidbody>();

    baseRB.isKinematic = false;
    lidRB.isKinematic = false;

    jointPoint = GameObject.FindGameObjectWithTag("JointPoint");

    ConfigurableJoint baseCJ = coffin_Base.gameObject.AddComponent<ConfigurableJoint>();
    baseCJ.connectedBody = jointPoint.GetComponent<Rigidbody>();
    baseCJ.breakForce = 5000f;

    baseCJ.xMotion = ConfigurableJointMotion.Limited;
    baseCJ.yMotion = ConfigurableJointMotion.Limited;
    baseCJ.zMotion = ConfigurableJointMotion.Limited;

    baseCJ.angularXMotion = ConfigurableJointMotion.Locked;
    baseCJ.angularYMotion = ConfigurableJointMotion.Locked;
    baseCJ.angularZMotion = ConfigurableJointMotion.Locked;

    JointDrive strongDrive = new JointDrive();
    strongDrive.positionSpring = 1f;  
    strongDrive.positionDamper = 500f;
    strongDrive.maximumForce = float.PositiveInfinity;

    baseCJ.xDrive = strongDrive;
    baseCJ.yDrive = strongDrive;
    baseCJ.zDrive = strongDrive;

    baseCJ.rotationDriveMode = RotationDriveMode.Slerp;
    JointDrive rotationDrive = new JointDrive();
    rotationDrive.positionSpring = 1000000f;
    rotationDrive.positionDamper = 500f;
    rotationDrive.maximumForce = float.PositiveInfinity;

    baseCJ.slerpDrive = rotationDrive;

    
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

    void Start() {
        spawnPrefabs = MyNetworkManager.instance.spawnPrefabs;
    }

    void Update()
    {

    }

}
