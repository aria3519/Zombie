using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillKind
{

    Once,
    NotOnce
}
// ���� ��ų 
public class RangeSkill : MonoBehaviour
{
    // �̳� ��Ÿ , ��Ÿ

    private SkillKind type;
    [SerializeField] public float damage = 20f; // ���ݷ�

    private void OnTriggerStay(Collider other)
    {
        // �÷��̾ �¾����� �÷��̾��� ü���� ���� ��Ŵ    
        //other.GetComponent<LivingEntity>().OnDamage();
        // ����ġ�� ��Ÿ or ��Ÿ (���� ������ ������� ����)

        //gameObject.SetActive(true)
       // Debug.Log("RangeAttack");
        LivingEntity attackTarget = other.GetComponent<LivingEntity>();
        Vector3 hitPoint = attackTarget.transform.position;
        Vector3 hitNormal = (transform.position - hitPoint).normalized; // ���Ϳ� �÷��̾� ��ġ�� ������ ���� ���� -> ���Ͱ� �÷��̾� ���� ����
        attackTarget.OnDamage(damage, hitPoint, hitNormal);
       /* GameObject me = transform.gameObject;
        me.SetActive(false);
*/
        /*  switch (type)
          {
              case SkillKind.Once: // ����� �ѹ� �� 
                  // ����� �ִ� �ڵ� 
                  LivingEntity attackTarget = other.GetComponent<LivingEntity>();
                  Vector3 hitPoint = attackTarget.transform.position;
                  Vector3 hitNormal = (transform.position - hitPoint).normalized; // ���Ϳ� �÷��̾� ��ġ�� ������ ���� ���� -> ���Ͱ� �÷��̾� ���� ����
                  attackTarget.OnDamage(damage, hitPoint, hitNormal);
                  break;
              case SkillKind.NotOnce: // ����� ������ �� 
                  break;

          }*/
        // ����� �ִ� �ڵ� 

        //



    }

    private void OnDisable()
    {
        GameObject parent = transform.parent.gameObject;
        parent.SetActive(false);
    }

    

}
