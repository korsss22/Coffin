using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPosition : MonoBehaviour
{
    public GameObject[] playerList;

    private Vector3 GetAvgVec() {
        playerList = GameObject.FindGameObjectsWithTag("Player");
        int listLength = playerList.Length;
        Vector3 avgVec = new Vector3(0,0,0);
        for (int i = 0; i < listLength; i++) {
             avgVec += playerList[i].transform.position;
        }
        return avgVec/listLength;
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        gameObject.transform.position = GetAvgVec() + new Vector3(0, 1.8f, 0);
    }
}
