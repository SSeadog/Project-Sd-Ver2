using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMonsterTanker : EnemyMonster
{
    [SerializeField]
    Transform _attackPosition;
    [SerializeField]
    float _splashRange = 5f;

    [SerializeField]
    GameObject _explosionEffect;

    protected override void AttckTarget()
    {
        // Animation Event로 실행하여 따로 동작 필요x
    }

    void KnockBack()
    {
        if (_curAnim == Anims.Dead)
            return;

        Destroy(Instantiate(_explosionEffect, _attackPosition.position, Quaternion.identity), 2f);

        Collider[] colliders = Physics.OverlapSphere(_attackPosition.position, _splashRange);
        foreach (Collider collider in colliders)
        {
            Rigidbody rbody = collider.GetComponent<Rigidbody>();
            if (rbody != null)
            {
                if (CheckTeamTagname(rbody.tag) == false)
                    continue;

                rbody.GetComponent<Stat>().GetAttacked(_stat);
            }
        }

        PlayAnim(Anims.Idle);
    }
}
