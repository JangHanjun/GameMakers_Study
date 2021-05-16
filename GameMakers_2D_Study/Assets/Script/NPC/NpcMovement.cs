using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour{
    public string[] sentences;
    public Transform chatTransform;
    public GameObject chatBox;

    public void Talk(){
        GameObject go = Instantiate(chatBox);
        go.GetComponent<ChatSystem>().Ondialogue(sentences, chatTransform);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Talk();
        }
    }
}
