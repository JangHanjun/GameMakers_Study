using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCam : MonoBehaviour
{
    public GameObject vituralCam;
    public GameObject Map;
    bool isPlayerExist;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            isPlayerExist = true;
            vituralCam.SetActive(true);
            Map.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            isPlayerExist = false;
            vituralCam.SetActive(false);
            StartCoroutine(SetTF());
        }
    }

    IEnumerator SetTF(){
        yield return new WaitForSeconds(1f);
        if(!isPlayerExist){
            Map.SetActive(false);
        }
    }
}