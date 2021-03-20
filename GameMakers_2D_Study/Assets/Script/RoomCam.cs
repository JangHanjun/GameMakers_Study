using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCam : MonoBehaviour
{
    public GameObject vituralCam;
    public GameObject Map;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !other.isTrigger){
            vituralCam.SetActive(true);
            Map.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player") && !other.isTrigger){
            StartCoroutine(SetTF());
        }
    }

    IEnumerator SetTF(){
        yield return new WaitForSeconds(1f);
        vituralCam.SetActive(false);
        Map.SetActive(false);
    }
}