using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerMovement playerState;
    Vector2 respawnPosition;
    Rigidbody2D rigid;
    void Start(){
        playerState = GetComponent<PlayerMovement>();
        rigid = GetComponent<Rigidbody2D>();
        respawnPosition = this.transform.position;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            PlayerRespawn();
            playerState.StartCoroutine(playerState.CantMove(1f));
        }
            
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            if(playerState.dashing){
                other.GetComponent<EnemyDamaged>().Damaged();
            }
        }

        // 리스폰 포인트 업데이트
        if(other.CompareTag("RespawnPoint")){
            Debug.Log("리스폰 위치 변경");
            respawnPosition = other.transform.position;
        }
    }

    // 플레이어 리스폰 함수
    void PlayerRespawn(){
        rigid.velocity = new Vector2(0, 0);
        this.transform.position = respawnPosition;
    }
    
}
