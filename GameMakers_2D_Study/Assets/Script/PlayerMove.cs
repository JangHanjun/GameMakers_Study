using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour{
    Rigidbody2D rigid;

    public float maxSpeed;
    public float jumpPower;
    public int jumpCount = 0;
    public int maxJumpCount = 2;
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update(){
        // 버튼에서 손을 떼면 움직임 멈춤
        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
        }

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
        
    }
}
