using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    private const string START_POSITION = "StartPoint";
    private const string END_POSITION = "EndPoint";
    private GameObject startPositionObject;
    private GameObject endPositionObject;
    private Vector3 spawnPosition;
    private GameObject coffin;
    private GameObject jointPoint;
    Transform coffinTrans;
    public GameObject Base;
    public GameObject Lid;


    public static MyNetworkManager instance {get; private set;} 

    private void FindStartPosition() {
        startPositionObject = GameObject.Find(START_POSITION);
        endPositionObject = GameObject.Find(END_POSITION);
        if (startPositionObject == null) {
            Debug.Log("startPositionObject is null");
            return; 
        }
        spawnPosition = startPositionObject.transform.position;
        Debug.Log("StartPosition : "+spawnPosition);
    }

    private void SetGame() {
        Vector3 coffinPos = spawnPosition + new Vector3(0, 1.8f, 0);

        coffin = SpawnPrefab("Coffin_Black", coffinPos);
        GameObject jointPoint = SpawnPrefab("JointPoint", spawnPosition);

        coffinTrans = coffin.transform;
        Base = coffinTrans.Find("Base").gameObject;
        Lid = coffinTrans.Find("Lid").gameObject;
        
        Base.GetComponent<Rigidbody>().isKinematic = true;
        Lid.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void SetUI() {
        GameManager.instance.CreateObjectOnCanvas(GameManager.instance.timerText);
        GameManager.instance.timerText.SetActive(false);
    }

    public GameObject SpawnPrefab(string objectName, Vector3 spawnLocation) {
        GameObject selectedObject = null;

        foreach (GameObject obj in spawnPrefabs) {
            if (obj.name == objectName) {
                selectedObject = obj;
                break;
            }
        }

        if (selectedObject == null) {
            Debug.LogError("prefab no found with name : "+objectName);
            return null;
        }

        GameObject instantiatedObject = Instantiate(selectedObject, spawnLocation, Quaternion.identity);

        NetworkServer.Spawn(instantiatedObject);

        return instantiatedObject;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn) //플레이어가 추가될때 실행되는 콜백.
    {
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        NetworkServer.AddPlayerForConnection(conn, player);

        Debug.Log(player.name + "가 로비에 참가함! 위치 : "+player.transform.position);
    }

    public override void OnServerSceneChanged(string newSceneName) //서버 씬이 Change 완료 했을때 실행되는 콜백
    {
        base.OnServerSceneChanged(newSceneName);

        FindStartPosition();
        SetGame();
        SetUI();
    }
}
