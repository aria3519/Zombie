using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ���� �ڵ�
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
    [SerializeField] protected LivingEntity PlayerEntity; // �÷��̾� ��ġ 
    [SerializeField] protected float FineRange = 1;
    [SerializeField] protected float AttackRange = 1;

    [SerializeField] protected ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    [SerializeField] protected AudioClip deathSound; // ����� ����� �Ҹ�
    [SerializeField] protected AudioClip hitSound; // �ǰݽ� ����� �Ҹ�

    [SerializeField] protected Animator BossAnimator; // �ִϸ����� ������Ʈ
    [SerializeField] protected AudioSource BossAudioPlayer; // ����� �ҽ� ������Ʈ
    [SerializeField] protected Renderer BossRenderer; // ������ ������Ʈ

    protected float lastStayTime; // ���������� ����ð� 
    protected float StayTime = 3f; // stay 3�� ���� 
    protected float RangelastAttackTime; // ���������� ���� ���� �� �ð� 
    protected float AttacklastAttackTime; // ���������� �������� �� �ð� 
    protected float RangeAttackTime = 3f; // 3�ʸ��� ���� ���� ���� 
    protected float AttackTime = 5f; // 5�ʸ��� ���� ���� ���� 

    [SerializeField] protected GameObject BossSkill1; // ���� ��ų 1
    [SerializeField] protected GameObject BossSkill2; // ���� ��ų 2
    protected List<GameObject> listSkill1 = new List<GameObject>();
    protected List<GameObject> listSkill2 = new List<GameObject>();
    protected Vector3 PlayerPoint;

    [SerializeField] protected Slider Bosshealth; // ü���� ǥ���� UI �����̴�


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
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true 
            if (PlayerEntity != null && !PlayerEntity.dead)//&& FineRange >= targetEntity.transform.position
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }

    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        //��� �ִϸ��̼� ���
        BossAnimator.SetTrigger("Die");
        // ��� ȿ����
        BossAudioPlayer.PlayOneShot(deathSound);

    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            // ���ݹ��� ������ �������� ��ƼŬ ȿ�� ��� 
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }
        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
        BossHPbar();

    }

    protected void BossHPbar()
    {
        // ü�� �����̴��� ���� ���� ü�°����� ����
        Bosshealth.value = health;
    }

}
