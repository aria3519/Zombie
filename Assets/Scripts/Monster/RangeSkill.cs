using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillKind
{

    Once,
    NotOnce
}
// 범위 스킬 
public class RangeSkill : MonoBehaviour
{
    // 이넘 단타 , 연타

    private SkillKind type;
    [SerializeField] public float damage = 20f; // 공격력

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 맞았으면 플레이어의 체력을 감소 시킴    
        //other.GetComponent<LivingEntity>().OnDamage();
        // 스위치문 단타 or 연타 (몇초 단위로 대미지를 줄지)

        //gameObject.SetActive(true)
       // Debug.Log("RangeAttack");
        LivingEntity attackTarget = other.GetComponent<LivingEntity>();
        Vector3 hitPoint = attackTarget.transform.position;
        Vector3 hitNormal = (transform.position - hitPoint).normalized; // 몬스터와 플레이어 위치를 뺀값의 단위 백터 -> 몬스터가 플레이어 보는 방향
        attackTarget.OnDamage(damage, hitPoint, hitNormal);
       /* GameObject me = transform.gameObject;
        me.SetActive(false);*/

        /*  switch (type)
          {
              case SkillKind.Once: // 대미지 한번 줌 
                  // 대미지 주는 코드 
                  LivingEntity attackTarget = other.GetComponent<LivingEntity>();
                  Vector3 hitPoint = attackTarget.transform.position;
                  Vector3 hitNormal = (transform.position - hitPoint).normalized; // 몬스터와 플레이어 위치를 뺀값의 단위 백터 -> 몬스터가 플레이어 보는 방향
                  attackTarget.OnDamage(damage, hitPoint, hitNormal);
                  break;
              case SkillKind.NotOnce: // 대미지 여러번 줌 
                  break;

          }*/
       



    }

    private void OnDisable()
    {
        GameObject parent = transform.parent.gameObject;
        parent.SetActive(false);
    }



}
