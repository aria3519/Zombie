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
    public LivingEntity PlayerEntity; // �÷��̾� ��ġ 
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
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true 
            if (PlayerEntity != null && !PlayerEntity.dead)//&& FineRange >= targetEntity.transform.position
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }
}
