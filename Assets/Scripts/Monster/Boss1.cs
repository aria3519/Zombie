using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : BossBase
{
    private float RangelastAttackTime; // 마지막으로 범위 공격 한 시간 
    private float AttacklastAttackTime; // 마지막으로 근접공격 한 시간 
    private float RangeAttackTime = 3f; // 3초마다 범위 공격 간격 
    private float AttackTime = 5f; // 5초마다 근접 공격 간격 
   
    [SerializeField] public GameObject BossSkill1; // 보스 스킬 1
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
        // 상태가 변경 되기 위한 조건 
        // 라스트 타입 가지고 있어야함 
        if(hasTarget)
        {
            PlayerPoint = PlayerEntity.transform.position; // 플레이어 위치 
            if (FineRange >= Vector3.Distance(PlayerPoint, transform.position)) // 지정된 범위 내에 플레이어만 공격 하게 
            {
                if (Time.time >= RangelastAttackTime + RangeAttackTime) // 범위 공격이 기본
                {
                    bossStates = BossStates.RangeAttack;
                }
                else if(Time.time >= AttacklastAttackTime + AttackTime && AttackRange >= Vector3.Distance(PlayerPoint, transform.position)) // 근접 하면 근접 공격 
                {
                    bossStates = BossStates.Attack;
                }
            }
        }

    }

   /* private void OnDrawGizmosSelected()
    {
        // 범위 그려주는 코드 
        Gizmos.DrawWireSphere(transform.position, FineRange);
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }*/




}
