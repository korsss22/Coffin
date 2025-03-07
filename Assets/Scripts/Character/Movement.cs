using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Mirror;

public class Movement : NetworkBehaviour
{
    public GameObject mainCamera;

    [Range(0.0f, 10.0f)]
    public float speed;

    [Range(0.0f, 10.0f)]
    public float mouseSensitivity;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Animator anim;

    // 카메라 회전 함수
    void RotateCamera() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        xRotation -= mouseY;
        yRotation += mouseX;
        mainCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    // 캐릭터 이동 함수
    void MoveCharacter() {
        if (Input.anyKey) {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) {
                moveDirection += mainCamera.transform.forward;
                anim.SetBool("Walk", true);
            }
            if (Input.GetKey(KeyCode.A)) {
                moveDirection += -mainCamera.transform.right;
                anim.SetBool("Walk", true);
            }
            if (Input.GetKey(KeyCode.S)) {
                moveDirection += -mainCamera.transform.forward;
                anim.SetBool("Walk", true);
            }
            if (Input.GetKey(KeyCode.D)) {
                moveDirection += mainCamera.transform.right;
                anim.SetBool("Walk", true);
            }
            // if (Input.GetKeyDown(KeyCode.Space)) {
            //     finalSpeed += speed;
            // }
            if (moveDirection != Vector3.zero) {
                moveDirection.y = 0;
                moveDirection.Normalize(); //정규화 (벡터의 크기를 1로로)

                Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);

                transform.position += moveDirection * speed * Time.deltaTime;   

                CmdMove(transform.position);
            }
        } else {
            anim.SetBool("Walk", false);
        }
    }
    /*
    private IEnumerator DecreaseSpeed(float interval) {
        while (true) {
            finalSpeed *= 0.5f;
            yield return new WaitForSeconds(interval);
        }
    }
    */

    [Command]
    private void CmdMove(Vector3 newPosition) {
        transform.position = newPosition;
    }


    public override void OnStartLocalPlayer() // LocalPlayer인 경우에 실행
    {
        if (mainCamera) mainCamera.SetActive(true);
    }

    public override void OnStartClient()
        {
            name = $"Player[{netId}|{(isLocalPlayer ? "local" : "remote")}]";
        }

    public override void OnStartServer()
        {
            name = $"Player[{netId}|server]";
        }

    // Start is called before the first frame update
    void Start() {
        gameObject.TryGetComponent<Animator>(out Animator animator);
        anim = animator;
       // StartCoroutine(DecreaseSpeed(0.5f));
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!isLocalPlayer) return;

        MoveCharacter(); // 캐릭터 이동
        RotateCamera(); // 카메라 회전
    }
}
