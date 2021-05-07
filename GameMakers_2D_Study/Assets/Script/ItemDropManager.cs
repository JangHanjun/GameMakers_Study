using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour{
    public static ItemDropManager Instance;
    [SerializeField]
    private GameObject itemPrefab;
    private Queue<PickUpItem> dropItemPool = new Queue<PickUpItem>();

    private void Awake(){
        Instance = this;
        Init(5);
    }

    private PickUpItem Create(){
        var obj = Instantiate(itemPrefab).GetComponent<PickUpItem>();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }

    private void Init(int count){
        for(int i = 0; i < count; i++){
            dropItemPool.Enqueue(Create());
        }
    }

    public static PickUpItem GetItem(){
        var obj = Instance.dropItemPool.Dequeue();
        obj.transform.SetParent(null);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public static void ReturnItem(PickUpItem item){
        item.gameObject.SetActive(false);
        item.transform.SetParent(Instance.transform);
        Instance.dropItemPool.Enqueue(item);
    }
}

