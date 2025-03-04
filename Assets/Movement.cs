using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Movement : MonoBehaviour
{
    public Transform mainCamera;

    [Range(1.0f, 10.0f)]
    public float speed;

    [Range(1.0f, 10.0f)]
    public float mouseSensitivity;

    private float xRotation = 0f;
    private float yRotation = 0f;

    // 카메라 회전 함수
    void RotateCamera() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        yRotation += mouseX;
        mainCamera.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    // 캐릭터 이동 함수
    void MoveCharacter() {
        if (Input.anyKey) {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) {
                moveDirection += mainCamera.forward;
            }
            if (Input.GetKey(KeyCode.A)) {
                moveDirection += -mainCamera.right;
            }
            if (Input.GetKey(KeyCode.S)) {
                moveDirection += -mainCamera.forward;
            }
            if (Input.GetKey(KeyCode.D)) {
                moveDirection += mainCamera.right;
            }

            if (moveDirection != Vector3.zero) {
                moveDirection.y = 0;
                moveDirection.Normalize();

                Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f); // 부드럽게 회전

                transform.position += moveDirection * speed * Time.deltaTime;
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        MoveCharacter(); // 캐릭터 이동 및 회전
        RotateCamera(); // 카메라 회전
    }
}
