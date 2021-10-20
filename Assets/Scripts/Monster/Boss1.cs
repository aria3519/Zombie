using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : BossBase
{
    private float RangelastAttackTime; // ���������� ���� ���� �� �ð� 
    private float AttacklastAttackTime; // ���������� �������� �� �ð� 
    private float RangeAttackTime = 3f; // 3�ʸ��� ���� ���� ���� 
    private float AttackTime = 5f; // 5�ʸ��� ���� ���� ���� 
   
    [SerializeField] public GameObject BossSkill1; // ���� ��ų 1
    private List<GameObject> listSkill1 = new List<GameObject>();
    Vector3 PlayerPoint;
    // Start is called before the first frame update
    void Start()
    {
        bossStates = BossStates.Stay;
       
    }
    public override void Attack()
    {
        Debug.Log("Attack");
        bossStates = BossStates.Stay;
        AttacklastAttackTime = Time.time;
    }
    public override void RangeAttack()
    {
        Debug.Log("RangeAttack");

        if (0 < listSkill1.Count)
        {
            listSkill1[0].transform.position = PlayerPoint;
            listSkill1[0].SetActive(true);
        }
        else
        {
            var skill = Instantiate(BossSkill1);
            listSkill1.Add(skill);
            skill.transform.position = PlayerPoint;
            skill.SetActive(true);
        }

        bossStates = BossStates.Stay;
        RangelastAttackTime = Time.time;
    }

    public override void SpecialAttack()
    {
        Debug.Log("SpecialAttack");
       
    }

    public override void Stay()
    {
       /* Debug.Log("Stay");*/
        // ���°� ���� �Ǳ� ���� ���� 
        // ��Ʈ Ÿ�� ������ �־���� 
        if(hasTarget)
        {
            PlayerPoint = PlayerEntity.transform.position; // �÷��̾� ��ġ 
            if (FineRange >= Vector3.Distance(PlayerPoint, transform.position)) // ������ ���� ���� �÷��̾ ���� �ϰ� 
            {
                if (Time.time >= RangelastAttackTime + RangeAttackTime) // ���� ������ �⺻
                {
                    bossStates = BossStates.RangeAttack;
                }
                else if(Time.time >= AttacklastAttackTime + AttackTime && AttackRange >= Vector3.Distance(PlayerPoint, transform.position)) // ���� �ϸ� ���� ���� 
                {
                    bossStates = BossStates.Attack;
                }
            }
        }

    }

   /* private void OnDrawGizmosSelected()
    {
        // ���� �׷��ִ� �ڵ� 
        Gizmos.DrawWireSphere(transform.position, FineRange);
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }*/




}
