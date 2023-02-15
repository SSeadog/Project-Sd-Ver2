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
    private float splashRange = 10f;

    protected override void AttckTarget()
    {
        // Animation Event로 실행하여 따로 동작 필요x
    }

    void KnockBack()
    {
        if (_curAnim == Anims.Dead)
            return;

        Collider[] colliders = Physics.OverlapSphere(attackPosition.position, splashRange);
        Vector3 hitVfxPosition = new Vector3(attackPosition.position.x, transform.position.y, attackPosition.position.z);

        foreach (Collider collider in colliders)
        {
            Rigidbody rbody = collider.GetComponent<Rigidbody>();
            if (rbody != null)
            {
                if (CheckTeamTagname(rbody.tag) == false)
                    continue;

                rbody.AddExplosionForce(1000f, attackPosition.position, 7f);
                StartCoroutine(SetVelocityZero(rbody));

                rbody.GetComponent<Stat>().GetAttacked(_stat);
            }
        }

        PlayAnim(Anims.Idle);
    }

    IEnumerator SetVelocityZero(Rigidbody rbody)
    {
        yield return new WaitForSeconds(0.3f);
        if (rbody)
            rbody.velocity = Vector3.zero;
    }
}
