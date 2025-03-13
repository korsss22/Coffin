using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    public static MyNetworkManager instance {get; private set;}

    private GameObject startPoint;
    private GameObject endPoint;

    void Awake()
    {
        base.Awake();
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        } 
            instance = this;
            DontDestroyOnLoad(gameObject);
    }

    public override void OnServerChangeScene(string newSceneName) //씬 바꿀때 콜
    {
        base.OnServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName) //씬 로드가 완전 끝났을때
    {
        base.OnServerSceneChanged(sceneName);

        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        endPoint = GameObject.FindGameObjectWithTag("EndPoint");

        Vector3 coffinVec = startPoint.transform.position + new Vector3(0, 2.1f, 0);
        GameObject Coffin = GameManager.instance.SpawnNetworkObject("Coffin_Black", coffinVec, startPoint.transform.rotation);
        Coffin.transform.Find("Base").gameObject.GetComponent<Rigidbody>().isKinematic = true;
        GameManager.instance.SpawnNetworkObject("JointPoint", startPoint.transform.position, startPoint.transform.rotation);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn) // 플레이어가 추가될때 콜백
    {
        GameObject player = Instantiate(playerPrefab, startPoint.transform.position, startPoint.transform.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
        Debug.Log(player.name+"가 참여함! 위치 : "+startPoint.transform.position);
    }
}
