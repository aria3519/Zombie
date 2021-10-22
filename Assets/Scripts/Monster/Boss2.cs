using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : BossBase
{


    // Start is called before the first frame update

    protected override void Stay()
    {
        if (0 == startingHealth % 300)
        {
            bossStates = BossStates.SpecialAttack;
        }
        if (Time.time >= lastStayTime + StayTime)
        {
            Debug.Log("Stay");
            // ���°� ���� �Ǳ� ���� ���� 
            // ��Ʈ Ÿ�� ������ �־���� 
            
            if (hasTarget)
            {
                PlayerPoint = PlayerEntity.transform.position; // �÷��̾� ��ġ 
                // ���� ü���� üũ �ؼ� ����� ������ ���� ���� ���� 
                
                 if (FineRange >= Vector3.Distance(PlayerPoint, transform.position)) // ������ ���� ���� �÷��̾ ���� �ϰ� 
                {
                    // �÷��̾ findRange �� AttackRange �ۿ� �ִ� ��� ���� 3�ʵ� �� ���� 
                    if (Time.time >= RangelastAttackTime + RangeAttackTime && AttackRange <= Vector3.Distance(PlayerPoint, transform.position)) 
                    {
                        bossStates = BossStates.RangeAttack;
                    } 
                    // �÷��̾ AttackRange �ȿ� ���� ��� ���� 5�� �� �� ���� 
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
        // ���� �׷��ִ� �ڵ� 
        Gizmos.DrawWireSphere(transform.position, FineRange);
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }*/




}