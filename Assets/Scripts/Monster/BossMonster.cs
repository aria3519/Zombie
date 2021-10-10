using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // AI, ������̼� �ý��� ���� �ڵ带 ��������

public class BossMonster : LivingEntity
{
   /* public enum States 
    { 
         
        Stay, 
        Attack, 
        Die
    }*/

   
    [SerializeField] public LayerMask whatIsTarget; // ���� ��� ���̾�

    private LivingEntity targetEntity; // ������ ���
    private NavMeshAgent pathFinder; // ��ΰ�� AI ������Ʈ

    [SerializeField] public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    [SerializeField] public AudioClip deathSound; // ����� ����� �Ҹ�
    [SerializeField] public AudioClip hitSound; // �ǰݽ� ����� �Ҹ�

    private Animator BossAnimator; // �ִϸ����� ������Ʈ
    private AudioSource BossAudioPlayer; // ����� �ҽ� ������Ʈ
    private Renderer BossRenderer; // ������ ������Ʈ

    [SerializeField] public float damage = 20f; // ���ݷ�
    [SerializeField] public float timeBetAttack = 0.5f; // ���� ����
    private float lastAttackTime; // ������ ���� ����

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        BossAnimator = GetComponent<Animator>();
        BossAudioPlayer = GetComponent<AudioSource>();

        BossRenderer = GetComponentInChildren<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        BossAnimator.SetBool("HasTarget", hasTarget);
    }

    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }

    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor)
    {
        // ü�� ����
        startingHealth = newHealth;
        health = newHealth;

        // ���ݷ� ����
        damage = newDamage;
       

    }


    private IEnumerator UpdatePath()
    {
        
        LivingEntity attackTarget
              = targetEntity.GetComponent<LivingEntity>();
        // ����ִ� ���� ���� ����
        while (!dead)
        {
            
           
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                // ����� ������ �ִ°�� Attack ���·� ���� 
                lastAttackTime = Time.time;
                Vector3 hitPoint
                     = targetEntity.transform.position;
                Vector3 hitNormal
                    = transform.position - targetEntity.transform.position;

                // ���� ����
                attackTarget.OnDamage(damage, hitPoint, hitNormal);


            }
            else
            {
                // ����� ������ ������ stay ���� 
                pathFinder.isStopped = true;
            }
            
            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
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

   /* private void OnTriggerStay(Collider other)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����   
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            // �������� ���� LivingEnitity  Ÿ���� �������� �õ�
            LivingEntity attackTarget
                = other.GetComponent<LivingEntity>();

            // ������ LivingEntity �� �ڽ��� ���� ����̶�� ���� ����
            if (attackTarget != null && attackTarget == targetEntity)
            {
                // �ֱ� ���� �ð��� ����
                lastAttackTime = Time.time;

                // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���
                Vector3 hitPoint
                    = other.ClosestPoint(transform.position);
                Vector3 hitNormal
                    = transform.position - other.transform.position;

                // ���� ����
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }*/

    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        //��� �ִϸ��̼� ���
        BossAnimator.SetTrigger("Die");
        // ��� ȿ����
        BossAudioPlayer.PlayOneShot(deathSound);

    }
}
