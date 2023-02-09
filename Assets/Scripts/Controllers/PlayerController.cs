using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _moveSpeed = 3f;

    Animator _anim;

    enum Anims
    {
        Idle,
        WalkForward,
        WalkBack,
        WalkLeft,
        WalkRight
    }
    Anims _curAnim;

    bool _isMoving = false;
    Vector3 _moveVec = Vector3.zero;


    void Start()
    {
        Managers mg = Managers.Instance;
        _anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Rotate();
        Anim();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _moveVec = new Vector3(x, 0, y);

        transform.Translate(_moveVec * Time.deltaTime * _moveSpeed);

        if (_moveVec == Vector3.zero)
            _isMoving = false;
        else
            _isMoving = true;
    }

    void Rotate()
    {
        Vector3 cameraRot = Camera.main.transform.eulerAngles;
        cameraRot.x = 0;
        cameraRot.z = 0;
        transform.eulerAngles = cameraRot;
    }

    void Anim()
    {
        if (_moveVec.x > 0)
        {
            if (_curAnim.ToString() == "WalkRight")
                return;

            _anim.CrossFade("WalkRight", 0.2f);
            _curAnim = Anims.WalkRight;
        }
        else if (_moveVec.x < 0)
        {
            if (_curAnim.ToString() == "WalkLeft")
                return;

            _anim.CrossFade("WalkLeft", 0.2f);
            _curAnim = Anims.WalkLeft;
        }
        else if ( _moveVec.z > 0)
        {
            if (_curAnim.ToString() == "WalkForward")
                return;

            _anim.CrossFade("WalkForward", 0.2f);
            _curAnim = Anims.WalkForward;
        }
        else if (_moveVec.z < 0)
        {
            if (_curAnim.ToString() == "WalkBack")
                return;

            _anim.CrossFade("WalkBack", 0.2f);
            _curAnim = Anims.WalkBack;
        }
        else
        {
            if (_curAnim.ToString() == "Idle")
                return;

            _anim.CrossFade("Idle", 0.2f);
            _curAnim = Anims.Idle;
        }

        //if (_isMoving)
        //{
        //    if (_curAnim.ToString() == "WalkForward")
        //        return;

        //    _anim.CrossFade("WalkForward", 0.2f);
        //    _curAnim = Anims.WalkForward;
        //}
        //else
        //{
        //    if (_curAnim.ToString() == "Idle")
        //        return;

        //    _anim.CrossFade("Idle", 0.2f);
        //    _curAnim = Anims.Idle;
        //}
    }
}
