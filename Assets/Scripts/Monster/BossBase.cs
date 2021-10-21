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

    [SerializeField] public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    [SerializeField] public AudioClip deathSound; // ����� ����� �Ҹ�
    [SerializeField] public AudioClip hitSound; // �ǰݽ� ����� �Ҹ�

    public Animator BossAnimator; // �ִϸ����� ������Ʈ
    public AudioSource BossAudioPlayer; // ����� �ҽ� ������Ʈ
    public Renderer BossRenderer; // ������ ������Ʈ

    public float lastStayTime; // ���������� ����ð� 
    public float StayTime = 3f; // stay 3�� ���� 
    public float RangelastAttackTime; // ���������� ���� ���� �� �ð� 
    public float AttacklastAttackTime; // ���������� �������� �� �ð� 
    public float RangeAttackTime = 3f; // 3�ʸ��� ���� ���� ���� 
    public float AttackTime = 5f; // 5�ʸ��� ���� ���� ���� 

    [SerializeField] public GameObject BossSkill1; // ���� ��ų 1
    [SerializeField] public GameObject BossSkill2; // ���� ��ų 2
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
    }

}
