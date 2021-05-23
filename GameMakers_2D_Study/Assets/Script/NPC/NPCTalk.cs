using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour{
    
    public string[] sentences;
    public Transform chatTransform;
    public GameObject chatBox;
    RaycastHit2D rayHit;
    public bool talking;
    Rigidbody2D rigid;
    SpriteRenderer sr;

    private void Start() {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Talk(){
        talking = true;
        ChatSystem go = NpcDialogueMananger.NpcTalk(gameObject);
        go.GetComponent<ChatSystem>().Ondialogue(sentences, chatTransform);
        Invoke("Init", 8f);
    }

    void Init(){
        talking = false;
    }
    
}
