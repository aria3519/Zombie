using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : BossBase
{

    
    // Start is called before the first frame update
   
    public override void Stay()
    {

        if (Time.time >= lastStayTime + StayTime)
        {
            Debug.Log("Stay");
            // 상태가 변경 되기 위한 조건 
            // 라스트 타입 가지고 있어야함 
            if (hasTarget)
            {
                PlayerPoint = PlayerEntity.transform.position; // 플레이어 위치 
                // 보스 체력을 체크 해서 스페셜 공격을 할지 말지 결정 
                if(0 == startingHealth % 3000 )
                {
                    bossStates = BossStates.SpecialAttack;
                }
                else if (FineRange >= Vector3.Distance(PlayerPoint, transform.position)) // 지정된 범위 내에 플레이어만 공격 하게 
                {
                    // 플레이어가 findRange 안 AttackRange 밖에 있는 경우 공격 3초뒤 또 공격 
                    if (Time.time >= RangelastAttackTime + RangeAttackTime && AttackRange <= Vector3.Distance(PlayerPoint, transform.position)) 
                    {
                        bossStates = BossStates.RangeAttack;
                    } 
                    // 플레이어가 AttackRange 안에 들어온 경우 공격 5초 뒤 또 공격 
                    else if (AttackRange >= Vector3.Distance(PlayerPoint, transform.position) && Time.time >= AttacklastAttackTime + AttackTime)
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