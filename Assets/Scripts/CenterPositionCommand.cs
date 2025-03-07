using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPositionCommand : MonoBehaviour
{

    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject gameObject3;
    public GameObject gameObject4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setPos();
    }


    void setPos()
    {
        Vector3 v = new Vector3((gameObject1.transform.position.x + gameObject2.transform.position.x + gameObject3.transform.position.x + gameObject4.transform.position.x)/4,
            (gameObject1.transform.position.y + gameObject2.transform.position.y + gameObject3.transform.position.y + gameObject4.transform.position.y)/4,
            (gameObject1.transform.position.z + gameObject2.transform.position.z + gameObject3.transform.position.z + gameObject4.transform.position.z)/4);
        this.transform.position = v;
    }
}
