using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FriendlyMonsterTanker : FriendlyMonster
{
    [SerializeField]
    private Transform attackPosition;
    [SerializeField]
    private float splashRange = 5f;

    protected override void AttckTarget()
    {
        // Animation Event로 실행하여 따로 동작 필요x
    }

    void KnockBack()
    {
        if (_curAnim == Anims.Dead)
            return;

        Collider[] colliders = Physics.OverlapSphere(attackPosition.position, splashRange);

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
