using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Mirror;
using Steamworks;
using TMPro;
using Unity.Networking.Transport.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {
    private List<GameObject> spawnablePrefabs;
    public GameObject timerText;
    private GameObject Coffin = null;
    private GameObject JointPoint;

    private Transform CoffinBase;
    private Transform CoffinLid;
    private Rigidbody baseRb;
    private Rigidbody lidRb;
    private Coroutine nowCoroutine;
    private bool isCoroutine = false;
    private bool gameStarted = false;

    private void Start() {
        spawnablePrefabs =NetworkManager.singleton.spawnPrefabs;
        Coffin = SpawnPrefab("Coffin_Black", new Vector3(0, 1.8f, 0));
        JointPoint = SpawnPrefab("JointPoint", new Vector3(0,0,0));
        CoffinBase = Coffin.transform.Find("Base");
        CoffinLid = Coffin.transform.Find("Lid");
        
        baseRb = CoffinBase.GetComponent<Rigidbody>();
        lidRb = CoffinLid.GetComponent<Rigidbody>();
        baseRb.isKinematic = true;
        lidRb.isKinematic = true;

        togglePanel(false);

        
    }

    [Server]
    private bool CheckAllReady() {
        bool allReady = false;
        foreach (NetworkConnectionToClient player in NetworkServer.connections.Values) {
            // player.identity가 null일 수 있기 때문에 null 체크 추
            if (player.identity == null) continue; // identity가 없으면 해당 플레이어를 건너뛰고 계속 진행
            allReady = true;
            PlayerControl playerControl = player.identity.gameObject.GetComponent<PlayerControl>();
                
            if (playerControl != null && playerControl.GetReady() == false) {
                allReady = false;
            }
        }
        return allReady;
    }

    [Server]
    private void StartCountDown(float countTime) {
        if (CheckAllReady()) {// 모두 준비했고
            if (isCoroutine) { //코루틴 실행 중 아닐때
                isCoroutine = true;
                togglePanel(true);
                nowCoroutine = StartCoroutine(CountDownCoroutine(countTime));
            }
        } else {
            if (isCoroutine) {
                isCoroutine = false;
                togglePanel(false);
                StopCoroutine(nowCoroutine);
            }
        }
    }

    [Server]
    private IEnumerator CountDownCoroutine(float countTIme) {
        float currentTime = countTIme;
        while(currentTime > 0f) {
            UpdateText(currentTime);
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
        }
        GameStart();
    }

    [ClientRpc]
    private void GameStart() {
        Debug.Log("Start!!");
        baseRb.isKinematic = false;
        lidRb.isKinematic = false;
        Coffin.GetComponent<ConfigurableJoint>().connectedBody = JointPoint.GetComponent<Rigidbody>();
    }

    [ClientRpc]
    private void togglePanel(bool value) {
        timerText.SetActive(value);
    }

    [ClientRpc]
    private void UpdateText(float currentTime) {
        timerText.GetComponent<TextMeshProUGUI>().text = currentTime.ToString("0");
    }

    public GameObject SpawnPrefab(string objectName, Vector3 spawnLocation) {
        GameObject selectedObject = null;

        foreach (GameObject obj in spawnablePrefabs) {
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

    private void Update() {
        if (!gameStarted && isServer) {
            StartCountDown(3f);
        } 
    }
}
