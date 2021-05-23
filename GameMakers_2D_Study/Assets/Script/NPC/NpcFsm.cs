using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFsm : MonoBehaviour{

    public enum NpcState { IDLE, TRACK, WARN }
    public NpcState curState;
    private Transform player;
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

    IEnumerator IdleMove(){
        yield return delay;
        if(playerDash.dashed){
            Debug.Log("대쉬함");
            StartCoroutine(DetectPlayer());
            npcTalk.Talk();
        } else
            StartCoroutine(IdleMove());
        // 근처에서 플레이어 감지를 하면
        // curState = NpcState.TRACK;
        // StartCoroutine(DetectPlayer());
    }

    IEnumerator DetectPlayer(){
        Debug.Log("Track");
        yield return delay;

        
    }

    IEnumerator Warning(){
        Debug.Log("Warn");
        yield return delay;
    }
}
