using UnityEngine;
using DG.Tweening;

public class GhostTrailEffect : MonoBehaviour{
    SpriteRenderer sprite;
    public Transform ghostsParent;
    public Color trailColor;
    public Color fadeColor;
    public float trailInterval;
    public float fadeTime;
    void Start(){
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ShowGhost() {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < ghostsParent.childCount; i++){
            Transform currentGhost = ghostsParent.GetChild(i);
            //s.AppendCallback(()=> currentGhost.position = move.transform.position);
            //s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = anim.sr.flipX);
            //s.AppendCallback(()=>currentGhost.GetComponent<SpriteRenderer>().sprite = anim.sr.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(trailInterval);
        }
    }

    public void FadeSprite(Transform current){
        current.GetComponent<SpriteRenderer>().material.DOKill();
        current.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

    
}
