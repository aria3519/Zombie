using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ��ų 
public class RangeSkill : MonoBehaviour
{
    // �̳� ��Ÿ , ��Ÿ
    private enum SkillKind
    {

        SkillKind_Once,
        SkillKind_NotOnce
    }

    private void OnTriggerStay(Collider other)
    {
     // �÷��̾ �¾����� �÷��̾��� ü���� ���� ��Ŵ    
     //other.GetComponent<LivingEntity>().OnDamage();
     // ����ġ�� ��Ÿ or ��Ÿ (���� ������ ������� ����)

     


    }
}
