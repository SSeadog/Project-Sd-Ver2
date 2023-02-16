using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMonsterArcher : EnemyMonster
{
    public GameObject arrow;
    public Transform arrowPosition;

    protected override void AttckTarget()
    {
        // Animation Event로 실행하여 따로 동작 필요x
    }

    public void FireArrow()
    {
        if (_attackTarget== null)
            return;

        GameObject instanceArrow = Instantiate(arrow, arrowPosition.position, transform.rotation * Quaternion.Euler(90f, 0, 0));
        instanceArrow.GetComponent<WeaponStat>().Init(_stat);

        Rigidbody arrowRigid = instanceArrow.GetComponent<Rigidbody>();

        arrowRigid.AddForce((_attackTarget.transform.position - transform.position).normalized * 2000f);
    }

    public void EndFireArrow()
    {
        PlayAnim(Anims.Idle);
    }
}
