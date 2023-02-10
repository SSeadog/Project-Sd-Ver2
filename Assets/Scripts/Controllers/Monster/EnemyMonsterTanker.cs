using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMonsterTanker : EnemyMonster
{
    [SerializeField]
    private Transform attackPosition;
    [SerializeField]
    private float splashRange = 10f;

    protected override void AttckTarget()
    {

    }

    void KnockBack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPosition.position, splashRange);
        foreach (Collider collider in colliders)
        {
            Rigidbody rbody = collider.GetComponent<Rigidbody>();
            if (rbody != null)
            {
                if (rbody.tag == "EnemyMonster")
                    continue;

                rbody.AddExplosionForce(1000f, attackPosition.position, 7f);
                StartCoroutine(SetVelocityZero(rbody));

                rbody.GetComponent<Stat>().OnAttacked(_stat);
            }
        }
    }

    IEnumerator SetVelocityZero(Rigidbody rbody)
    {
        yield return new WaitForSeconds(0.3f);
        if (rbody)
            rbody.velocity = Vector3.zero;
    }
}
