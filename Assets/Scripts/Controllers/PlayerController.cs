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
        Run
    }
    Anims _curAnim;

    bool isMoving = false;

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

        Vector3 moveVec = new Vector3(x, 0, y);

        transform.Translate(moveVec * Time.deltaTime * _moveSpeed);

        if (moveVec == Vector3.zero)
            isMoving = false;
        else
            isMoving = true;
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
        if (isMoving)
        {
            if (_curAnim.ToString() == "Run")
                return;

            _anim.CrossFade("Run", 0.2f);
            _curAnim = Anims.Run;
        }
        else
        {
            if (_curAnim.ToString() == "Idle")
                return;

            _anim.CrossFade("Idle", 0.2f);
            _curAnim = Anims.Idle;
        }
    }
}
