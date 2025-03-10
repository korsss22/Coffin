using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Mirror;
using TMPro;
using Unity.Networking.Transport.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : NetworkBehaviour {
    private List<GameObject> spawnablePrefabs = NetworkManager.singleton.spawnPrefabs;
    public GameObject timerText;
    private GameObject Coffin = null;
    public GameObject JointPoint;

    Transform CoffinBase;
    Transform CoffinLid;
    private bool allReady;
    private bool gameStarted = false;

    private void Start() {
        Coffin = SpawnPrefab("Coffin_Black", new Vector3(0, 1.8f, 0));

        CoffinBase = Coffin.transform.Find("Base");
        CoffinLid = Coffin.transform.Find("Lid");
        
        CoffinBase.GetComponent<Rigidbody>().isKinematic = true;
        CoffinLid.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CheckReadyState() {
        if (!NetworkServer.active) return;

        allReady = true;

        foreach (var conn in NetworkServer.connections.Values) {
            if (conn.identity == null) continue;
            if (!conn.identity.gameObject.GetComponent<PlayerControl>().GetReady()) {
                allReady = false;
                break;
            }
        }
        StartCoroutine(StartCountDownTimer(3f));
    }

    private IEnumerator StartCountDownTimer(float countTime) {
        float currentTime = countTime;
        while (currentTime > 0f) {
            if (!allReady) {
                timerText.SetActive(false);
                yield break;
            }
            timerText.SetActive(true);
            timerText.GetComponent<TextMeshProUGUI>().text = currentTime.ToString("F0");
            currentTime -= Time.deltaTime;
            yield return null;
        }
        timerText.SetActive(false);
        StartGame();
    } 

    private void StartGame() {
        Debug.Log("StartGame");
        gameStarted = true;
        CoffinBase.GetComponent<Rigidbody>().isKinematic = false;
        CoffinLid.GetComponent<Rigidbody>().isKinematic = false;
        CoffinBase.GetComponent<ConfigurableJoint>().connectedBody = JointPoint.GetComponent<Rigidbody>();
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
        if (!gameStarted) CheckReadyState();
    }
}
