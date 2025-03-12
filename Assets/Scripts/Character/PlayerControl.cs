using UnityEngine;
using Mirror;
public class PlayerControl : NetworkBehaviour
{
    public GameObject playerCamera;
    public Transform cameraTransform;
    public CharacterController characterController;
    public float yVelocity = 0;

    [Range(0.0f, 10.0f)]
    public float speed;
    [Range(0.0f, 10.0f)]
    public float maxSpeed;
    [Range(0.0f, 1000.0f)]
    public float jump;
    [Range(0.0f, 10.0f)]
    public float mouseSensitivity;
    [Range(0.0f, 10f)]
    public float scaleRatio =0.0f;
    public float gravity = -20f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private Animator anim;
    private bool isReady = false;


    //Rigidbody 컴포넌트를 담을 전역변수
    private Rigidbody rb;

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

    // 캐릭터 애니메이션 및 방향 전환 함수
    private void CharacterUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(h,0,v);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection *= speed;

        if(h != 0 || v != 0){
            anim.SetBool("Walk", true);
        }
        else{
            anim.SetBool("Walk", false);
        }
        
        if(characterController.isGrounded)
        {
            yVelocity = -1;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = jump;
            }
        }
        yVelocity +=(gravity*Time.deltaTime);
        moveDirection.y=yVelocity;
        characterController.Move(moveDirection*Time.deltaTime);
    }

    //캐릭터 움직임 구현 함수
    private void CharacterMove(){

        /*캐릭터의 이동 방식에는 3가지 방식이 있다.
        1. Transform.position
        2. RigidBody.MovePosition()
            충돌연산 실행
        3. RigidBody.AddForce()
            물리법칙 모두 계산
        4. RigidBody.velocity
            기존에 오브젝트가 받고있던 물리법칙 무시
        */

    

    }

    //게임 준비 키(Q)
    [Command]
    private void Ready() {
        if (GameManager.instance == null) {
            Debug.LogError("GameManager가 null입니다! GameManager가 씬에 배치되어 있는지 확인하세요.");
            return; // GameManager가 없으면 함수 종료
        }
        if (Input.GetKey(KeyCode.Q)) {
            if (!isReady) {
                GameManager.instance.readyPlayers++;
                isReady = true;
            }
        } else {
            if (isReady) {
                GameManager.instance.readyPlayers--;
                isReady = false;
            }
        }
        
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

        //Rigidbody 변수 설정
        //rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // void FixedUpdate() { //고정된 시간 간격마다 호출. 물리 엔진 업데이트와 동기화, 물리 계산을 처리하는데 사용. 물리 기반 움직임이나 충돌.
    //     if (!isLocalPlayer) return;

    //     CharacterUpdate();
    // }

    void Update(){ //매 프레임마다 한 번 호출. 사용자 입력 처리, 애니메이션 업데이트, 게임 로직 등을 처리할 때 사용
        if(!isLocalPlayer) return;

        CharacterUpdate();
        Ready();
    }

    void LateUpdate(){ // 모든 Update 메소드가 호출된 후 매 프레임마다 한 번씩 호출. 카메라를 위치시키는 카메라 추적의 경우 사용.
        if(!isLocalPlayer) return;

        RotateCamera(); //카메라 회전
        //이동 후 카메라 움직임이 매우매우 부드럽고 빨라짐. 굳
    }
}
