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

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        NetworkServer.AddPlayerForConnection(conn, player);

        Debug.Log(player.name + "가 로비에 참가함! 위치 : "+player.transform.position);
    }

    public override void OnServerSceneChanged(string newSceneName)
    {
        base.OnServerSceneChanged(newSceneName);

        startPositionObject = GameObject.Find(START_POSITION);
        endPositionObject = GameObject.Find(END_POSITION);
        if (startPositionObject == null) {
            Debug.Log("startPositionObject is null");
            return; 
        }
        spawnPosition = startPositionObject.transform.position;
        Debug.Log("StartPosition : "+spawnPosition);
    }
}
