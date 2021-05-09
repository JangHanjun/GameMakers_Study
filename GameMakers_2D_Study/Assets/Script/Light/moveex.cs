using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveex : MonoBehaviour{
    Rigidbody2D rigid;
    void Start(){
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(9.8f * 25f, 9.8f * 25f));
    }
}
