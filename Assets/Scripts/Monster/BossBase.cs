using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BossStates
{

    Stay,
    Attack,
    RangeAttack,
    SpecialAttack,
    Die
}


abstract public class BossBase : LivingEntity
{
    protected BossStates bossStates;
    public LivingEntity PlayerEntity; // 플레이어 위치 
    public float FineRange = 1;
    public float AttackRange = 1;
   

    // Start is called before the first frame update
    public abstract void Stay();
    public abstract void Attack();
    public abstract void RangeAttack();
    public abstract void SpecialAttack();
    

    private void Update()
    {
      switch(bossStates)
        {
            case BossStates.Stay:
                Stay();
                break;
            case BossStates.Attack:
                Attack();
                break;
            case BossStates.RangeAttack:
                RangeAttack();
                break;
            case BossStates.SpecialAttack:
                SpecialAttack();
                break;
            case BossStates.Die:
                Die();
                break;
        }
    }

    public bool hasTarget
    {

        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true 
            if (PlayerEntity != null && !PlayerEntity.dead)//&& FineRange >= targetEntity.transform.position
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }
}
