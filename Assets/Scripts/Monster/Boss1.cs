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
            // ���°� ���� �Ǳ� ���� ���� 
            // ��Ʈ Ÿ�� ������ �־���� 
            if (hasTarget)
            {
                PlayerPoint = PlayerEntity.transform.position; // �÷��̾� ��ġ 
                var dist = Vector3.Distance(PlayerPoint, transform.position);
                if (FineRange >= dist) // ������ ���� ���� �÷��̾ ���� �ϰ� 
                {
                    if (Time.time >= RangelastAttackTime + RangeAttackTime && AttackRange <= dist) // ���� ������ �⺻
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
        // ���� �׷��ִ� �ڵ� 
        Gizmos.DrawWireSphere(transform.position, FineRange);
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }*/




}
