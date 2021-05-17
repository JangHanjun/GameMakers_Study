using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour{
    public string[] sentences;
    public Transform chatTransform;
    public GameObject chatBox;
    public bool talking;

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
