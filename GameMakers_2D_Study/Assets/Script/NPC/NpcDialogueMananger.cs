using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogueMananger : MonoBehaviour{
    public static NpcDialogueMananger chatInstance;
    [SerializeField]
    GameObject quadPrefab;
    Queue<ChatSystem> chatPool = new Queue<ChatSystem>();

    private void Awake() {
        chatInstance = this;
        InitChatPool(3);
    }

    private void InitChatPool(int count){
        for(int i = 0; i < count; i++){
            chatPool.Enqueue(CreateQuad());
        }
    }

    private ChatSystem CreateQuad(){
        var obj = Instantiate(quadPrefab).GetComponent<ChatSystem>();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);

        return obj;
    }

    public static ChatSystem NpcTalk(GameObject npc){
        var quad = chatInstance.chatPool.Dequeue();
        quad.gameObject.SetActive(true);
        quad.transform.SetParent(npc.transform);
        return quad;
    }

    public static void NpcTalkEnd(ChatSystem quad){
        quad.gameObject.SetActive(false);
        quad.transform.SetParent(chatInstance.transform);
        chatInstance.chatPool.Enqueue(quad);
    }
}
