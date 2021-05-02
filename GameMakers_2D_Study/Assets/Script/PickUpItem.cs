using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour{
    public GameObject slotItme;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            InventoryUI inven = other.GetComponent<InventoryUI>();
            for(int i = 0; i < inven.slots.Count; i++){
                if(inven.slots[i].isEmpty){
                    Instantiate(slotItme, inven.slots[i].slotObject.transform, false);
                    inven.slots[i].isEmpty = false;
                    Destroy(this.gameObject);
                    break;
                }
            }
        }
    }
}
