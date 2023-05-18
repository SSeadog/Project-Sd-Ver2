using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody rigid;
    private float aliveTime = 5f;
    private float curAliveTime = 0f;


    void Start()
    {
        _collider = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
        _collider.enabled = true;
    }

    void Update()
    {
        curAliveTime += Time.deltaTime;
        if (curAliveTime > aliveTime)
            Destroy(gameObject, 0f);
    }

    void FixedUpdate()
    {
        if (rigid.velocity == Vector3.zero)
            return;

        SetRotation();
    }

    void SetRotation()
    {
        Vector3 v = rigid.velocity;

        float combinedVelocity = Mathf.Sqrt(v.x * v.x + v.z * v.z);
        float fallAngle = (float)(-1 * Mathf.Atan2(v.y, combinedVelocity) * Mathf.Rad2Deg);

        transform.eulerAngles = new Vector3(fallAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
