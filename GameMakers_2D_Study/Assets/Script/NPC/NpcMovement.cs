using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour{
    public string[] sentences;
    public Transform chatTransform;
    public GameObject chatBox;
    Vector3 dir;
    RaycastHit2D rayHit;
    public bool talking;
    public int moveRand;
    Rigidbody2D rigid;
    SpriteRenderer sr;
    

    private void Start() {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(walk());
    }

    private void FixedUpdate() {
        rigid.velocity = new Vector2(moveRand, rigid.velocity.y);

        if(moveRand > 0){
            sr.flipX = true;
            dir = Vector3.right;
        }
        else{
            sr.flipX = false;
            dir = Vector3.left;
        }

        rayHit = Physics2D.Raycast(rigid.position, dir, 1.5f, LayerMask.GetMask("Ground"));   
        if(rayHit){
            moveRand *= -1;
            Debug.Log("방향전환");
        }
        
    }

    IEnumerator walk(){
        moveRand = Random.Range(-1, 2);
        
        yield return new WaitForSeconds(2f);
        StartCoroutine(walk());
    }
    public void Talk(){
        talking = true;
        ChatSystem go = NpcDialogueMananger.NpcTalk(gameObject);
        go.GetComponent<ChatSystem>().Ondialogue(sentences, chatTransform);
        Invoke("Init", 8f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && talking == false){
            Talk();
        }
    }

    void Init(){
        talking = false;
    }
}
