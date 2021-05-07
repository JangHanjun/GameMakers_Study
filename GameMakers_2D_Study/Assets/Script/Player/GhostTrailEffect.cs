using UnityEngine;
using DG.Tweening;

public class GhostTrailEffect : MonoBehaviour{
    PlayerMovement playerMovement;
    SpriteRenderer sprite;
    PlayerAnimation anim;
    public Transform ghostsParent;
    public Color trailColor;
    public Color fadeColor;
    public float trailInterval;
    public float fadeTime;
    void Start(){
        sprite = GetComponent<SpriteRenderer>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        anim = FindObjectOfType<PlayerAnimation>();
    }

    public void ShowGhost() {
        Sequence s = DOTween.Sequence();


        /*  DOTween
            Append : 애미메이션 바로 실행
            AppendCallback : 기다린 후 콜백호출
            AppendInterval : 시간 기다림
        */
        for (int i = 0; i < ghostsParent.childCount; i++){
            Transform currentGhost = ghostsParent.GetChild(i);
            s.AppendCallback(()=> currentGhost.position = playerMovement.transform.position);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = anim.spriteRenderer.flipX);
            s.AppendCallback(()=>currentGhost.GetComponent<SpriteRenderer>().sprite = anim.spriteRenderer.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(trailInterval);
        }
    }
    // 효과 페이드 아웃
    public void FadeSprite(Transform current){
        current.GetComponent<SpriteRenderer>().material.DOKill();
        current.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

    
}
