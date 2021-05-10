using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour{
    Collisions coll;
    Rigidbody2D rigid;
    PlayerAnimation anim;
    //GrabItem grabItem;
    [Header("Player Stat")]
    // 플레이어 능력치
    public float speed = 10;
    public float jumpForce = 7;
    public float dashSpeed = 20;
    public float wallSlideSpeed = 2;
    public float wallJumpForce = 10;
    public float wallClimbSpeed = 7;
    public float playerStamina = 100;
    //public float coyoteTime = 0.15f;
    [Header("Player State")]
    // 플레이어 상태 변경을 위한 bool
    public bool onGround;       // 지금 땅 위에 있는가
    public bool touchGround;
    public bool isGrabWall;     // 지금 벽을 잡고 있는가
    public bool isWallJump;     // 벽점프를 했는가
    public bool isWallSlide;    // coll.onWall || !coll.onGround 일때 트루
    public bool canMove = true; // 플레이어 통제권
    public bool dashing;        // 지금 대쉬하고 있는가
    public bool dashed;         // 대쉬한 직후
    public int direction = 1;

    void Start(){
        coll = GetComponent<Collisions>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimation>();
        //grabItem = GetComponent<GrabItem>();
    }

    void Update(){
        // Walk
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x,y);

        Walk(dir);
        anim.HorizontalMovement(x, y, rigid.velocity.y);

        isGrabWall = coll.onWall && Input.GetKey(KeyCode.LeftShift) && playerStamina > 0;
        
        if(coll.onGround && !touchGround){
            PlayerOnGround();
            touchGround = true;
        }
        if(!coll.onGround && touchGround){
            touchGround = false;
        }
        //else
            //coyoteTime -= Time.delatTime;
        //벽에서 미끄러지는건 방향키를 벽으로 해야 할 수 있다
        if(coll.onWall && !coll.onGround){
            if (x != 0 && !isGrabWall){
                isWallSlide = true;
                WallSlide();
            }
        }


        // 벽을 잡고 있다면 기어올라갈 수 있게, 스테미너도 줄어들게
        if(isGrabWall){
            rigid.velocity = new Vector2(rigid.velocity.x, y * wallClimbSpeed);
            playerStamina -= Time.deltaTime * 10f;
        }

        if(Input.GetButtonDown("Jump")){
            // 점프 애니메이션 넣는 부분

            // 그냥 땅 위에 있다면 일반 점프
            if(coll.onGround) // || coyoteTime > 0 추가
                Jump(Vector2.up, false);
            // 벽과 붙어있다면 벽점프로 이동
            if(coll.onWall && ! coll.onGround)
                WallJump();
        }

        // Dash
        if(Input.GetButtonDown("Fire1") && !dashed){
            // dashed는 땅에 닿아야 true가 된다. 대쉬 후 false
            if(xRaw != 0 || yRaw != 0){
                Dash(xRaw, yRaw);
            }  
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

        // 잡고 있지 않으면 미끄러지게 한다
        if(coll.onWall && !coll.onGround){
            if (x != 0 && !isGrabWall){
                isWallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround)
            isWallSlide = false;


        // todo : 아래 코드는 뭘까? 조이스틱용?
        if (coll.onWall && Input.GetButton("Fire3") && canMove){
            isGrabWall = true;
            isWallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove){
            isGrabWall = false;
            isWallSlide = false;
        }

        if(x > 0){
            direction = 1;
            anim.Flip(direction);
            //grabItem.grabDir(direction);
        }
        if(x < 0){
            direction = -1;
            anim.Flip(direction);
            //grabItem.grabDir(direction);
        }
            
    }

    // 땅 위에 착지하면 초기화  할 것들
    void PlayerOnGround(){
        dashed = false;
        dashing = false;
        playerStamina = 100;
        //coyoteTime = 0.15;
        direction = anim.spriteRenderer.flipX ? -1 : 1;
    }

    // 걷는 로직
    private void Walk(Vector2 dir){
        // 제어권이 없거나 벽타기중이라면 못움직이기ㅔ
        if(!canMove)
            return;
        if(isGrabWall)
            return;

        if(!isWallJump)
            rigid.velocity = new Vector2(dir.x * speed, rigid.velocity.y);
        else
            rigid.velocity = Vector2.Lerp(rigid.velocity, (new Vector2(dir.x * speed, rigid.velocity.y)), wallJumpForce * Time.deltaTime);
    }

    // 점프 로직
    private void Jump(Vector2 dir, bool isWall){
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        rigid.velocity += dir * jumpForce;
    }

    private void WallJump(){

        if ((direction == 1 && coll.onRightWall) || direction == -1 && !coll.onRightWall){
            direction *= -1;
            anim.Flip(direction);
        }
        
        StopCoroutine(CantMove(0));
        StartCoroutine(CantMove(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        isWallJump = true;
    }
    private void WallSlide(){
        if(coll.wallSide != direction)
            anim.Flip(direction * -1);

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
        rigid.velocity += direction.normalized * dashSpeed;

        StartCoroutine(AfterDash());
    }
    IEnumerator AfterDash(){
        FindObjectOfType<GhostTrailEffect>().ShowGhost();
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
        //Debug.Log("땅 대쉬");
        yield return new WaitForSeconds(.15f);
        if(coll.onGround)
            dashed = false;
    }

    // 통제권 제외 후 time시간 후에 돌려줌
    public IEnumerator CantMove(float time){
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
