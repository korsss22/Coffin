using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPosition : MonoBehaviour
{
    public GameObject[] playerList;
    // Start is called before the first frame update
    void Start()
    {
 
    }

    //플레이어가 입장할때마다 플레이어 리스트를 업데이트 할 메소드 추가 필요


    public Vector3 getAverage()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
        int listLength = playerList.Length;
        Vector3 avVector = new Vector3(0, 0, 0);

        for (int i = 0; i < listLength; i++)
        {
            avVector += playerList[i].transform.position;
        }

        if (listLength == 0)
        {
            return new Vector3(0f, 0f, 0f);
        }

        return avVector / listLength;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = getAverage() + new Vector3(0f, 1.8f, 0f);

    }
}
