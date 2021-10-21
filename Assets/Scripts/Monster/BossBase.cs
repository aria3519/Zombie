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

    [SerializeField] public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    [SerializeField] public AudioClip deathSound; // 사망시 재생할 소리
    [SerializeField] public AudioClip hitSound; // 피격시 재생할 소리

    public Animator BossAnimator; // 애니메이터 컴포넌트
    public AudioSource BossAudioPlayer; // 오디오 소스 컴포넌트
    public Renderer BossRenderer; // 렌더러 컴포넌트

    public float lastStayTime; // 마지막으로 멈춘시간 
    public float StayTime = 3f; // stay 3초 마다 
    public float RangelastAttackTime; // 마지막으로 범위 공격 한 시간 
    public float AttacklastAttackTime; // 마지막으로 근접공격 한 시간 
    public float RangeAttackTime = 3f; // 3초마다 범위 공격 간격 
    public float AttackTime = 5f; // 5초마다 근접 공격 간격 

    [SerializeField] public GameObject BossSkill1; // 보스 스킬 1
    [SerializeField] public GameObject BossSkill2; // 보스 스킬 2
    public List<GameObject> listSkill1 = new List<GameObject>();
    public List<GameObject> listSkill2 = new List<GameObject>();
    public Vector3 PlayerPoint;
    // Start is called before the first frame update
    public abstract void Stay();
    public virtual void Attack()
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
    public virtual void RangeAttack()
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
    public virtual void SpecialAttack()
    {
        Debug.Log("SpecialAttack");
        bossStates = BossStates.Stay;
    }


    void Start()
    {
        BossAnimator = GetComponent<Animator>();
        BossAudioPlayer = GetComponent<AudioSource>();
        BossRenderer = GetComponentInChildren<Renderer>();

        bossStates = BossStates.Stay;

    }

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
    }

}
