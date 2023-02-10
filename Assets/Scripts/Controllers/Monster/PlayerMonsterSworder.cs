using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonsterSworder : PlayerMonster
{
    //[SerializeField]
    //private GameObject effect;

    //[SerializeField]
    //AudioSource audioSource;

    protected override void AttckTarget()
    {
        //audioSource.PlayOneShot(audioSource.clip);
    }

    public void OnAttackEvent()
    {
        if (attackTarget == null)
            return;

        attackTarget.GetComponent<Stat>().OnAttacked(_stat);
    }
}
