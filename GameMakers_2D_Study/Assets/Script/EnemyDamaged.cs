using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    int maxHp;
    int curHp;

    private void Start() {
        maxHp = 2;
        curHp = maxHp;
    }

    public void Damaged(){
        if(curHp < 1){
            Debug.Log("이걸 죽네");
        }
        Debug.Log("특징: 아프다");
        curHp--;
    }
}
