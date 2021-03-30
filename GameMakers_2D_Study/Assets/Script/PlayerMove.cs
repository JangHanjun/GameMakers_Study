using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public float maxSpeed;
    // 점프 관련
    public float jumpPower;
    int jumpCount = 0;
    public int maxJumpCount = 1;
    // 방향을 가져오기 위한 변수
    Vector3 dirVec;
    // 레이로 스캔하는 물체 받아오는 변수
    GameObject scanObject;
    bool onWall;
    bool isWallJump;
    public float downSpeed;
    public float wallJumpPower;
    [SerializeField]
    LayerMask ableClimgLayer;
    // 스테미너 - 벽타기, 벽점프에 사용
    [SerializeField]
    private float playerStamina;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        // 스테미나 초기화
        RefreshStamina();
    }

    void Update(){
        // 버튼에서 손을 떼면 움직임 멈춤
        if(Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);

        // Flip
        if (Input.GetButton("Horizontal") && !isWallJump)
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        
        // 걷는 애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.2f || jumpCount > 0) //속도가 0이라면 = 단위벡터가 0
            anim.SetBool("isWalk", false);
        else
            anim.SetBool("isWalk", true);

        //바라보는 방향
        float h = Input.GetAxisRaw("Horizontal");

        if(h == -1)
            dirVec = Vector3.left;
        else if(h == 1)
            dirVec = Vector3.right;
        
        // 점프
        if(Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount){
            rigid.velocity = Vector2.up * jumpPower;
            jumpCount++;
        }
    }
    void FixedUpdate(){
        // 좌우 이동
        float h = Input.GetAxisRaw("Horizontal");
        if(!isWallJump){
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        }

        // 최대 스피드 maxSpeed 를 넘지 못하게 함
        if(rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < -maxSpeed)
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        // 플레이어 아래로 쏘는 ray, 내려올때만
        if(rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector3.down, new Color(1, 0, 0));
            RaycastHit2D downRay = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

            if(downRay.collider != null && downRay.distance < 0.5f){
                //Debug.Log("땅");
                jumpCount = 0;
                RefreshStamina();
            }
        }

        // 벽타기용 Ray
        Debug.DrawRay(rigid.position, dirVec * 0.4f, new Color(1, 0, 0));
        RaycastHit2D rayHit2 = Physics2D.Raycast(rigid.position, dirVec, 0.4f, ableClimgLayer);

        if(rayHit2.collider != null){
            onWall = true;

            if(Input.GetKey(KeyCode.W) && playerStamina > 0){
                //Debug.Log("climbing");
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y + downSpeed);
                playerStamina -= Time.deltaTime * 20;
            } else {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * downSpeed);
            }
            

            // wall jump
            if(Input.GetAxis("Jump") != 0 && playerStamina > 0) {
                isWallJump = true;
                playerStamina -= 20;
                Invoke("FreezX", 0.5f);
                rigid.velocity = new Vector2(-0.9f * wallJumpPower, 0.9f * wallJumpPower);
                if(spriteRenderer.flipX) {
                    spriteRenderer.flipX = false;
                } else if (!spriteRenderer.flipX) {
                    spriteRenderer.flipX = true;
                }
            }
        }
        else{
            onWall = false;
        }
        
    }

    void FreezX(){
        isWallJump = false;
        onWall = false;
    }

    // 스테미나 되돌리기
    void RefreshStamina(){
        playerStamina = 40;
    }
}
