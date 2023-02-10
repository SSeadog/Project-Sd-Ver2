using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonsterArcher : PlayerMonster
{
    public GameObject arrow;
    public Transform arrowPosition;

    protected override void AttckTarget()
    {

        // 활 쏘기
        GameObject instanceArrow = Instantiate(arrow, arrowPosition.position, transform.rotation * Quaternion.Euler(90f, 0, 0));
        Rigidbody arrowRigid = instanceArrow.GetComponent<Rigidbody>();

        arrowRigid.AddForce((attackTarget.transform.position - transform.position + Vector3.up * 0.5f).normalized * 2000f);
    }
}
