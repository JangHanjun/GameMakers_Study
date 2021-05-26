using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFsm : MonoBehaviour{

    public enum NpcState { IDLE, TRACK, WARN }
    public NpcState curState;
    private Transform player;
    Vector3 moveToward;
    Vector3 playerOffset;
    float detectDistance = 16f;
    private PlayerMovement playerDash;
    private NPCTalk npcTalk;
    WaitForSeconds delay = new WaitForSeconds(0.1f);
    int randDir;
    
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerDash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        npcTalk = GetComponent<NPCTalk>();
        curState = NpcState.IDLE;
        StartCoroutine(IdleMove());
    }

    // TODO : Update 함수에서 플레이어 이동 함수 구현

    IEnumerator IdleMove(){
        Debug.Log("Idle");
        yield return delay;
        if(playerDash.dashed){
            Debug.Log("대쉬함");
            playerOffset = player.position - this.transform.position;
            if(playerOffset.sqrMagnitude < detectDistance){
                // 플레이어 감지
                Debug.Log("플레이어 위치 : " + player.position);
                npcTalk.Talk();
                StartCoroutine(DetectPlayer(player.position));
            } 
            else
                StartCoroutine(IdleMove());
        } 
        else
            StartCoroutine(IdleMove());
    }

    IEnumerator DetectPlayer(Vector3 playerPos){
        moveToward.x = playerPos.x;
        Debug.Log("이곳으로 이동 : " + );
        yield return new WaitForSeconds(3f);
        StartCoroutine(IdleMove());
    }

    IEnumerator Warning(){
        Debug.Log("Warn");
        yield return delay;
    }
}
