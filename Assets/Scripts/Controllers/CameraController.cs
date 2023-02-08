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

    float _sensitivity = 0.1f;
    float _moveSpeed = 20f;
    float minRotX = -80f;
    float maxRotX = 80f;

    Vector3 _initMousePos;

    void Start()
    {
        _initMousePos = Input.mousePosition;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 rot = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * _sensitivity;

        if (rot.x < minRotX)
            rot.x = minRotX;
        if (rot.x > maxRotX)
            rot.x = maxRotX;

        transform.eulerAngles += rot;

        Quaternion euler = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        transform.position = _target.position + (euler * Vector3.back * _backOffset) + (Vector3.up * _heightOffset) + (_target.transform.right * _rightOffset);
    }
}
