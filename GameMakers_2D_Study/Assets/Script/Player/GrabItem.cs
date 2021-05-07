using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    bool isGrab;
    RaycastHit2D item;
    public float distance = 0.5f;
    public Transform holdPoint;
    public float throwPower;
    public int XDir;
    Vector3 dirVec;
    void Update(){
        if (Input.GetMouseButtonDown(1)){
            if(!isGrab){
                // 물건 잡기
                Physics2D.queriesStartInColliders = false;
                Debug.DrawRay(transform.position, dirVec, Color.red);
                item = Physics2D.Raycast(transform.position, dirVec, distance);
                
                if(item.collider != null && item.collider.CompareTag("Grabable")){
                    isGrab = true;
                }

            }
            else{
                // 던지기
                isGrab = false;

                if(item.collider.gameObject.GetComponent<Rigidbody2D>() != null){
                    // todo : 좌우로 던지는 힘이 약해 개선할 필요 있음
                    item.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XDir, 1) * throwPower;
                }
            }
        }

        if(isGrab)
            item.collider.gameObject.transform.position = holdPoint.position;
    }

    public void grabDir(int dir){
        if(dir == 1){
            dirVec = Vector3.right;
            XDir = 2;
        }
        else{
            dirVec = Vector3.left;
            XDir = -2;
        }
            
    }
}
