using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    Collisions coll;
    Rigidbody2D rigid;
    // 플레이어 능력치
    public float speed = 10;
    public float jumpForce = 7;
    public float dashSpeed = 20;
    public float wallSlideSpeed = 2;
    public float wallJumpForce = 10;

    public bool isGrabWall;
    public bool isWallJump;
    public bool isWallSlide;
    public bool canMove = true;
    public bool dashing;
    public bool onGround;
    public bool dashed;
    void Start(){
        coll = GetComponent<Collisions>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update(){
        // Walk
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x,y);

        Walk(dir);

        if(coll.onWall && !coll.onGround){
            if (x != 0 && !isGrabWall){
                isWallSlide = true;
                WallSlide();
            }
        }

        isGrabWall = coll.onWall && Input.GetKey(KeyCode.LeftShift);
        if(isGrabWall)
            rigid.velocity = new Vector2(rigid.velocity.x, y * speed);

        if(Input.GetButtonDown("Jump")){
            if(coll.onGround)
                Jump(Vector2.up, false);
            if(coll.onWall && ! coll.onGround)
                WallJump();
        }

        // Dash
        if(Input.GetButtonDown("Fire1") && !dashed){
            if(xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }

        // 아래부터는 플레이어 위치에 따른 bool 변수 제어
        if (coll.onGround && !dashing){
            isWallJump = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if (isGrabWall && !dashing){
            rigid.gravityScale = 0;
            if(x > .2f || x < -.2f)
            rigid.velocity = new Vector2(rigid.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rigid.velocity = new Vector2(rigid.velocity.x, y * (speed * speedModifier));
        } else {
            rigid.gravityScale = 3;
        }

        if(coll.onWall && !coll.onGround){
            if (x != 0 && !isGrabWall){
                isWallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround)
            isWallSlide = false;

        if(coll.onGround)
            PlayerOnGround();

        if (coll.onWall && Input.GetButton("Fire3") && canMove){
            isGrabWall = true;
            isWallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove){
            isGrabWall = false;
            isWallSlide = false;
        }
            
    }

    // 땅 위에 착지하면 초기화  할 것들
    void PlayerOnGround(){
        dashed = false;
        dashing = false;
    }

    // fin
    private void Walk(Vector2 dir){
        if(!canMove)
            return;
        if(isGrabWall)
            return;

        if(!isWallJump)
            rigid.velocity = new Vector2(dir.x * speed, rigid.velocity.y);
        else
            rigid.velocity = Vector2.Lerp(rigid.velocity, (new Vector2(dir.x * speed, rigid.velocity.y)), wallJumpForce * Time.deltaTime);

    }

    // fin
    private void Jump(Vector2 dir, bool isWall){
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        rigid.velocity += dir * jumpForce;
    }

    private void WallJump(){

        StopCoroutine(CantMove(0));
        StartCoroutine(CantMove(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        isWallJump = true;
    }
    private void WallSlide(){
        if(!canMove)
            return;

        bool pushingWall = false;
        if((rigid.velocity.x > 0 && coll.onRightWall) || (rigid.velocity.x < 0 && coll.onLeftWall)){
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rigid.velocity.x;

        rigid.velocity = new Vector2(push, -wallSlideSpeed);
    }
    // Dash
    private void Dash(float x, float y){
        dashed = true;

        rigid.velocity = Vector2.zero;
        Vector2 direction = new Vector2(x,y);
        rigid.velocity = direction.normalized * dashSpeed;

        StartCoroutine(AfterDash());
    }
    IEnumerator AfterDash(){
        StartCoroutine(GroundDash());

        rigid.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        isWallJump = true;
        dashing = true;

        yield return new WaitForSeconds(.3f);

        rigid.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        isWallJump = false;
        dashing = false;
    }
    IEnumerator GroundDash(){
        yield return new WaitForSeconds(.15f);
        if(coll.onGround)
            dashed = false;
    }

    IEnumerator CantMove(float time){
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
