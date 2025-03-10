using UnityEngine;
using Mirror;
public class PlayerControl : NetworkBehaviour
{
    public GameObject playerCamera;

    [Range(0.0f, 10.0f)]
    public float speed;

    [Range(0.0f, 10.0f)]
    public float mouseSensitivity;

    [Range(0.0f, 10f)]
    public float scaleRatio =0.0f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Animator anim;

    [SyncVar]
    private bool isReady = false;

    // 카메라 회전 함수
    void RotateCamera() {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        Vector3 minScale = new Vector3(1.0f, 1.0f, 1.0f);  // 최소 크기 (0.1f로 설정)
        Vector3 maxScale = new Vector3(8.0f, 8.0f, 8.0f);  // 최대 크기 (2.0f로 설정)
        Vector3 addScaleRatio = new (scaleRatio, scaleRatio, scaleRatio);

        if (scrollInput != 0) {
            if (scrollInput > 0f) { //휠 위로로
                playerCamera.transform.localScale -= addScaleRatio;
            } else {
                playerCamera.transform.localScale += addScaleRatio;
            }
            playerCamera.transform.localScale = Vector3.Max(playerCamera.transform.localScale, minScale);
            playerCamera.transform.localScale = Vector3.Min(playerCamera.transform.localScale, maxScale);
        }

        

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        xRotation -= mouseY;
        yRotation += mouseX;
        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    // 캐릭터 이동 함수
    void MoveCharacter() {
        if (Input.anyKey) {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) {
                moveDirection += playerCamera.transform.forward;
                anim.SetBool("Walk", true);
            }
            if (Input.GetKey(KeyCode.A)) {
                moveDirection += -playerCamera.transform.right;
                anim.SetBool("Walk", true);
            }
            if (Input.GetKey(KeyCode.S)) {
                moveDirection += -playerCamera.transform.forward;
                anim.SetBool("Walk", true);
            }
            if (Input.GetKey(KeyCode.D)) {
                moveDirection += playerCamera.transform.right;
                anim.SetBool("Walk", true);
            }
            if (moveDirection != Vector3.zero) {
                moveDirection.y = 0;
                moveDirection.Normalize(); //정규화 (벡터의 크기를 1로로)

                Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f); //부드럽게 회전전

                transform.position += Time.deltaTime * speed * moveDirection;   //벡터를 가장 나중에, 작은 값을 먼저 계산하는게 이득득
            }
        } else {
            anim.SetBool("Walk", false);
        }
    }

    private void Ready() {
        if (Input.GetKey(KeyCode.Q)) CmdSetReady(true);
        else CmdSetReady(false);
    }

    [Command]
    private void CmdSetReady(bool value) {
        isReady = value;
    }

    public bool GetReady() {
        return isReady;
    }

    public override void OnStartLocalPlayer()
    {
        if (playerCamera) playerCamera.SetActive(true);
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
        gameObject.TryGetComponent(out Animator animator);
        anim = animator;
        if (isLocalPlayer) playerCamera.SetActive(true);
       // StartCoroutine(DecreaseSpeed(0.5f));
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!isLocalPlayer) return;

        MoveCharacter(); // 캐릭터 이동
        RotateCamera(); // 카메라 회전
        Ready();
    }
}
