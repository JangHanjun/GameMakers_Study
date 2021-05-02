using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour{
    public GameObject ItemPanel;
    bool isPanelActive;
    public List<SlotData> slots = new List<SlotData>();
    private int maxItem = 5;
    public GameObject slotPrefab;
    GameObject slot;

    private void Start() {
        GameObject slotPanel = GameObject.Find("Panel");

        for(int i = 0; i < maxItem; i++){
            slot = Instantiate(slotPrefab, slotPanel.transform, false);
            slot.name = "slot_" + i;
            SlotData SD = new SlotData();
            SD.isEmpty = true;
            SD.slotObject = slot;
            slots.Add(SD);
        }
    }
    
    private void Update() {
        if(Input.GetKeyDown(KeyCode.I)){
            if(isPanelActive){
                ItemPanel.SetActive(false);
                isPanelActive = false;
            } else {
                ItemPanel.SetActive(true);
                isPanelActive = true;
            }
        }
    }
}
