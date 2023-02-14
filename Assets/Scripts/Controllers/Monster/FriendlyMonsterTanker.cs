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
    [SerializeField]
    private GameObject hitVfx;

    protected override void AttckTarget()
    {

    }

    void KnockBack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPosition.position, splashRange);
        Vector3 hitVfxPosition = new Vector3(attackPosition.position.x, transform.position.y, attackPosition.position.z);
        Instantiate(hitVfx, hitVfxPosition, Quaternion.identity);

        foreach (Collider collider in colliders)
        {
            Rigidbody rbody = collider.GetComponent<Rigidbody>();
            if (rbody != null)
            {
                if (rbody.tag == "PlayerMonster" || rbody.tag == "Player")
                    continue;

                rbody.AddExplosionForce(1000f, attackPosition.position, 7f);
                StartCoroutine(SetVelocityZero(rbody));

                rbody.GetComponent<Stat>().GetAttacked(_stat);
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
