using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;

    [SerializeField] float _backOffset = 0.6f;
    [SerializeField] float _heightOffset = 1.3f;
    [SerializeField] float _rightOffset = 0.4f;

    float _sensitivity = 30f;

    public void Init()
    {
        _target = Managers.Game.Player.transform;
    }

    void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 rot = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * Time.deltaTime * _sensitivity;

        rot = transform.eulerAngles + rot;
        if (rot.x < 0)
            rot.x += 360f;

        if (rot.x < 280f && rot.x > 190f)
            rot.x = 280f;

        if (rot.x > 80f && rot.x < 170f)
            rot.x = 80f;

        transform.eulerAngles = rot;

        Quaternion euler = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        transform.position = _target.position + (euler * Vector3.back * _backOffset) + (Vector3.up * _heightOffset) + (_target.transform.right * _rightOffset);
    }
}
