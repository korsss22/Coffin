using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckEndPoint : MonoBehaviour
{
    public TextMeshProUGUI gameOver;
    void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.isGameOver) return;
        GameManager.instance.isGameClear = true;
        Instantiate(gameOver, FindAnyObjectByType<Canvas>().transform);
    }
}
