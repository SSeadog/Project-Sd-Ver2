using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    Collider _collider;
    Rigidbody _r;

    public float attackPower = 0f;

    private float aliveTime = 5f;
    private float curAliveTime = 0f;

    private Rigidbody rigid;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = true;
        rigid = GetComponent<Rigidbody>();
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

        Vector3 v = rigid.velocity;
        //rigid.velocity -= new Vector3(v.x * 0.001f, 0, v.z * 0.001f);
        //rigid.velocity += Vector3.down * 0.98f;
    }

    void SetRotation()
    {
        Vector3 v = rigid.velocity;

        float combinedVelocity = Mathf.Sqrt(v.x * v.x + v.z * v.z);
        float fallAngle = (float)(-1 * Mathf.Atan2(v.y, combinedVelocity) * 180 / Math.PI);

        transform.eulerAngles = new Vector3(fallAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name}");
        Destroy(gameObject);
    }
}
