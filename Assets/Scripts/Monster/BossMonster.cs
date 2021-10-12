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
    //private NavMeshAgent pathFinder; // ��ΰ�� AI ������Ʈ

    [SerializeField] public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    [SerializeField] public AudioClip deathSound; // ����� ����� �Ҹ�
    [SerializeField] public AudioClip hitSound; // �ǰݽ� ����� �Ҹ�

    private Animator BossAnimator; // �ִϸ����� ������Ʈ
    private AudioSource BossAudioPlayer; // ����� �ҽ� ������Ʈ
    private Renderer BossRenderer; // ������ ������Ʈ

    [SerializeField] public float FineRange = 1;
    [SerializeField] public float damage = 20f; // ���ݷ�
    [SerializeField] public float timeBetAttack = 3f; // ���� ����
    private float lastAttackTime; // ������ ���� ����

    private void Awake()
    {
        //pathFinder = GetComponent<NavMeshAgent>();
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

    private void OnDrawGizmosSelected()
    {
        // ���� �׷��ִ� �ڵ� 
        Gizmos.DrawWireSphere(transform.position, FineRange);
    }

    private IEnumerator UpdatePath()
    {
        
        
        // ����ִ� ���� ���� ����
        while (!dead)
        {
            if (hasTarget)
            {
                //pathFinder.isStopped = false;
                // ����� ������ �ִ°�� Attack ���·� ���� 
                LivingEntity attackTarget
             = targetEntity.GetComponent<LivingEntity>();
                
                Vector3 hitPoint
                     = targetEntity.transform.position;
                Vector3 hitNormal
                    = (transform.position - targetEntity.transform.position).normalized;

                if (!dead && Time.time >= lastAttackTime + timeBetAttack)
                {
                    lastAttackTime = Time.time;
                    // ���� ����
                    Debug.Log("Attack");
                    attackTarget.OnDamage(damage, hitPoint, hitNormal);
                }

            }
            else
            {
                // ����� ������ ������ stay ���� 
                //pathFinder.isStopped = true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, FineRange, whatIsTarget);
                for (int i = 0; i < colliders.Length; i++)
                {
                    // �ݶ��̴��� ���� LivingEntity Component ��������
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    //LivingEntity Component�� �����ϸ�, �ش� LivingEntity �� ��� �ִٸ�
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // ����������� �ٸ� ����ִ� ����� ���� 
                        targetEntity = livingEntity;
                        break;
                    }
                }
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
