using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSystem : MonoBehaviour{

    public Queue<string> sentences;
    public TextMeshPro text;
    public GameObject quad;
    string s;
    float textW;
    public void Ondialogue(string[] lines, Transform pos){
        transform.position = pos.position;
        sentences = new Queue<string>();
        sentences.Clear();

        foreach(var line in lines){
            sentences.Enqueue(line);
        }
        // for(int i = 0; i < lines.Length; i++){
        //     sentences.Enqueue();
        // }
        StartCoroutine(Talk(pos));
    }

    IEnumerator Talk(Transform quadPos){
        yield return null;

        while(sentences.Count > 0) {
            text.text = sentences.Dequeue();

            // 말풍선(quad) 크기 지정
            textW = text.preferredWidth;
            textW = (textW > 3.5f) ? 3.5f : textW + 1;
            quad.transform.localScale = new Vector2(textW, text.preferredHeight + 0.3f);

            transform.position = new Vector2(quadPos.position.x, quadPos.position.y + text.preferredHeight/2);

            yield return new WaitForSeconds(3f);
        }

        NpcDialogueMananger.NpcTalkEnd(this);
    }
}
