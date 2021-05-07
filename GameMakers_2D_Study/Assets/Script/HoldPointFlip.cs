using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPointFlip : MonoBehaviour{

    public void nFlip(){
        transform.position = new Vector3(-0.87f, 0.22f, 0);
    }

    public void yFlip(){
        transform.position = new Vector3(0.87f, 0.22f, 0);
    }
    
}
