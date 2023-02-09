using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterSworder : EnemyMonster
{
    //[SerializeField]
    //private GameObject effect;

    //[SerializeField]
    //AudioSource audioSource;

    protected override void AttckTarget()
    {
        anim.SetTrigger("IsAttack");
        //audioSource.PlayOneShot(audioSource.clip);
    }

    public void OnAttackEvent()
    {
        if (attackTarget == null)
            return;

        attackTarget.GetComponent<Stat>().OnAttacked(_stat);
    }
}
