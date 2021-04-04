using UnityEngine;
using Cinemachine;
 
public class FollowPlayer : MonoBehaviour{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    private CinemachineVirtualCamera vcam;
    void Start(){
        vcam = GetComponent<CinemachineVirtualCamera>();
        tPlayer = null;
    }
    // 밑 업데이트가 아닌 start에서 한번만 실행해도 되는게 아닐까?
    void Update(){
        if (tPlayer == null){
            tPlayer = GameObject.FindWithTag("Player");
            if (tPlayer != null){
                tFollowTarget = tPlayer.transform;
                vcam.Follow = tFollowTarget;
            }
        }
    }
}