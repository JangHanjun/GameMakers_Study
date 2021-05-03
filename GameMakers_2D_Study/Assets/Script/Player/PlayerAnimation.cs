using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour{
    Animator anim;
    PlayerMovement playerMovement;
    Collisions coll;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collisions>();
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    void Update(){
        anim.SetBool("onGround", coll.onGround);
        anim.SetBool("onWall", coll.onWall);
        anim.SetBool("onRightWall", coll.onRightWall);
        anim.SetBool("onLeftWall", coll.onLeftWall);
        anim.SetBool("Dashing", playerMovement.dashing);
        anim.SetBool("grabWall", playerMovement.isGrabWall);
        anim.SetBool("canMove", playerMovement.canMove);
    }

    public void HorizontalMovement(float x,float y, float yVel){
        anim.SetFloat("HAxis", x);
        anim.SetFloat("VAxis", y);
        anim.SetFloat("VVelocity", yVel);
    }
    public void Flip(int dir){
        if(playerMovement.isGrabWall || playerMovement.isWallSlide){
            if(dir == 1 && !spriteRenderer.flipX)
                return;
            if(dir == -1 && spriteRenderer.flipX)
                return;
        }

        bool state = (dir == 1) ? true : false;
        spriteRenderer.flipX = state;
    }
}
