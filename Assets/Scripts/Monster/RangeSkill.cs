using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 범위 스킬 
public class RangeSkill : MonoBehaviour
{
    // 이넘 단타 , 연타
    private enum SkillKind
    {

        SkillKind_Once,
        SkillKind_NotOnce
    }

    private void OnTriggerStay(Collider other)
    {
     // 플레이어가 맞았으면 플레이어의 체력을 감소 시킴    
     //other.GetComponent<LivingEntity>().OnDamage();
     // 스위치문 단타 or 연타 (몇초 단위로 대미지를 줄지)

     


    }
}
