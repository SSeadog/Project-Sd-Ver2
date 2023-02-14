using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMonsterSworder : FriendlyMonster
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
        if (_attackTarget == null)
            return;

        _attackTarget.GetComponent<Stat>().GetAttacked(_stat);
    }
}
