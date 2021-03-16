using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public float maxSpeed;
    public float jumpPower;
    int jumpCount = 0;
    public int maxJumpCount = 2;
    // 방향을 가져오기 위한 변수
    Vector3 dirVec;
    // 레이로 스캔하는 물체 받아오는 변수
    GameObject scanObject;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update(){
        // 버튼에서 손을 떼면 움직임 멈춤
        if(Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);

        // Flip
        if (Input.GetButton("Horizontal"))
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
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

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
                Debug.Log("땅");
                jumpCount = 0;
            }
        }

        // 조사를 하기 위한 Ray
        // 씬 상에서 빨간색 선을 쏴서 레이에 닿으면 상호작용 가능하게 개조하면 됨
        Debug.DrawRay(rigid.position, dirVec * 1.5f, new Color(1, 0, 0));
        RaycastHit2D rayHit2 = Physics2D.Raycast(rigid.position, dirVec, 1.5f, LayerMask.GetMask("Object"));

        if(rayHit2.collider != null){
            scanObject = rayHit2.collider.gameObject;
        }
        else{
            scanObject = null;
        }
        
    }
}
