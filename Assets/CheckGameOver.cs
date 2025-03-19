using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckGameOver : MonoBehaviour
{
    public TextMeshProUGUI gameOver;
    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Coffin" || collision.gameObject.tag == "JointPoint" || collision.gameObject.tag == "Player")&& !GameManager.instance.isGameClear) return;
        GameManager.instance.isGameOver = true;
        Debug.Log("부딪힘! Info : "+collision.gameObject.name+", tag : "+collision.gameObject.tag);
        Instantiate(gameOver, FindAnyObjectByType<Canvas>().transform);
    }
}
