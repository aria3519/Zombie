using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 코드
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
    [SerializeField] protected LivingEntity PlayerEntity; // 플레이어 위치 
    [SerializeField] protected float FineRange = 1;
    [SerializeField] protected float AttackRange = 1;

    [SerializeField] protected ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    [SerializeField] protected AudioClip deathSound; // 사망시 재생할 소리
    [SerializeField] protected AudioClip hitSound; // 피격시 재생할 소리

    [SerializeField] protected Animator BossAnimator; // 애니메이터 컴포넌트
    [SerializeField] protected AudioSource BossAudioPlayer; // 오디오 소스 컴포넌트
    [SerializeField] protected Renderer BossRenderer; // 렌더러 컴포넌트

    protected float lastStayTime; // 마지막으로 멈춘시간 
    protected float StayTime = 3f; // stay 3초 마다 
    protected float RangelastAttackTime; // 마지막으로 범위 공격 한 시간 
    protected float AttacklastAttackTime; // 마지막으로 근접공격 한 시간 
    protected float RangeAttackTime = 3f; // 3초마다 범위 공격 간격 
    protected float AttackTime = 5f; // 5초마다 근접 공격 간격 

    [SerializeField] protected GameObject BossSkill1; // 보스 스킬 1
    [SerializeField] protected GameObject BossSkill2; // 보스 스킬 2
    protected List<GameObject> listSkill1 = new List<GameObject>();
    protected List<GameObject> listSkill2 = new List<GameObject>();
    protected Vector3 PlayerPoint;

    [SerializeField] protected Slider Bosshealth; // 체력을 표시할 UI 슬라이더


    protected abstract void Stay();
    protected virtual void Attack()
    {
        Debug.Log("Attack");
        if (0 < listSkill2.Count)
        {
            listSkill2[0].transform.position = PlayerPoint;
            listSkill2[0].SetActive(true);
        }
        else
        {
            var skill = Instantiate(BossSkill2);
            listSkill2.Add(skill);
            skill.transform.position = PlayerPoint;
            skill.SetActive(true);
        }
        bossStates = BossStates.Stay;
        AttacklastAttackTime = Time.time;
    }
    protected virtual void RangeAttack()
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
    protected virtual void SpecialAttack()
    {
        Debug.Log("SpecialAttack");
        bossStates = BossStates.Stay;
    }


    protected void Start()
    {
        BossAnimator = GetComponent<Animator>();
        BossAudioPlayer = GetComponent<AudioSource>();
        BossRenderer = GetComponentInChildren<Renderer>();

        bossStates = BossStates.Stay;
    }

    protected void Update()
    {
      switch (bossStates)
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


    protected bool hasTarget
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

    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        //사망 애니메이션 재생
        BossAnimator.SetTrigger("Die");
        // 사망 효과음
        BossAudioPlayer.PlayOneShot(deathSound);

    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            // 공격받은 지점과 방향으로 파티클 효과 재생 
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }
        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
        BossHPbar();

    }

    protected void BossHPbar()
    {
        // 체력 슬라이더의 값을 현재 체력값으로 변경
        Bosshealth.value = health;
    }

}
