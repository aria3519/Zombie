using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : BossBase
{


    // Start is called before the first frame update


    protected override void Stay()
    {
       
        if (Time.time >= lastStayTime + StayTime )
        {
            Debug.Log("Stay");
            // 상태가 변경 되기 위한 조건 
            // 라스트 타입 가지고 있어야함 
            if (hasTarget)
            {
                PlayerPoint = PlayerEntity.transform.position; // 플레이어 위치 
                var dist = Vector3.Distance(PlayerPoint, transform.position);
                if (FineRange >= dist) // 지정된 범위 내에 플레이어만 공격 하게 
                {
                    if (Time.time >= RangelastAttackTime + RangeAttackTime && AttackRange <= dist) // 범위 공격이 기본
                    {
                        bossStates = BossStates.RangeAttack;
                    }
                    else if(AttackRange >= dist && Time.time >= AttacklastAttackTime+ AttackTime)
                    {
                        bossStates = BossStates.Attack;
                    }

                }
               
            }
            lastStayTime = Time.time;
        }

    }
   


    /*private void OnDrawGizmosSelected()
    {
        // 범위 그려주는 코드 
        Gizmos.DrawWireSphere(transform.position, FineRange);
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }*/




}
