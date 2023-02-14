using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;

    Animator _anim;

    enum Anims
    {
        Idle,
        WalkForward,
        WalkBack,
        WalkLeft,
        WalkRight,
        Attack
    }
    Anims _curAnim;

    bool _isAttack = false;

    Vector3 _moveVec = Vector3.zero;

    GameObject _arrowOriginal;

    GameObject _arrowPosition;

    void Start()
    {
        _stat = GetComponent<PlayerStat>();
        _anim = GetComponent<Animator>();

        Managers.Game.SetActiveCursor(false);

        _arrowOriginal = Resources.Load<GameObject>("Prefabs/Weapons/PlayerArrow");
        _arrowPosition = transform.Find("ArrowPosition").gameObject;
    }

    void Update()
    {
        Move();
        Rotate();
        Attack();
        Spawn();
        Anim();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        _moveVec = new Vector3(x, 0, y);

        transform.Translate(_moveVec * Time.deltaTime * _stat.Speed);
    }

    void Rotate()
    {
        Vector3 cameraRot = Camera.main.transform.eulerAngles;
        cameraRot.x = 0;
        cameraRot.z = 0;
        transform.eulerAngles = cameraRot;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !_isAttack)
        {
            _isAttack = true;
            _anim.CrossFade("Attack", 0.2f);
            _curAnim = Anims.Attack;
        }
    }
    
    // Spawn()이 플레이어 컨트롤러 밑에 둘만 한가..? 따로 빼야하나?? 고민 필요
    void Spawn()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject instance = Managers.Game.Spawn(Define.ObjectType.FriendlyMeleeMonster, "Prefabs/Monsters/FriendlyMeleeMonster");
            instance.transform.position = Managers.Game.friendlyTower.GetComponent<FriendlyTowerController>().GetSpawnRoot().position;
        }
    }

    void Anim()
    {
        if (_isAttack)
            return;

        if (_moveVec.x > 0)
        {
            if (_curAnim.ToString() == "WalkRight")
                return;

            _anim.CrossFade("WalkRight", 0.1f);
            _curAnim = Anims.WalkRight;
        }
        else if (_moveVec.x < 0)
        {
            if (_curAnim.ToString() == "WalkLeft")
                return;

            _anim.CrossFade("WalkLeft", 0.1f);
            _curAnim = Anims.WalkLeft;
        }
        else if ( _moveVec.z > 0)
        {
            if (_curAnim.ToString() == "WalkForward")
                return;

            _anim.CrossFade("WalkForward", 0.1f);
            _curAnim = Anims.WalkForward;
        }
        else if (_moveVec.z < 0)
        {
            if (_curAnim.ToString() == "WalkBack")
                return;

            _anim.CrossFade("WalkBack", 0.1f);
            _curAnim = Anims.WalkBack;
        }
        else
        {
            if (_curAnim.ToString() == "Idle")
                return;

            _anim.CrossFade("Idle", 0.1f);
            _curAnim = Anims.Idle;
        }
    }

    public void FireArrow()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            Vector3 moveVec = (raycastHit.point - _arrowPosition.transform.position).normalized;
            GameObject instantArrow = Instantiate(_arrowOriginal, _arrowPosition.transform.position, Quaternion.LookRotation(moveVec));
            Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
            arrowRigid.AddForce(moveVec * 2000f);
        }
    }

    public void EndFireArrow()
    {
        _isAttack = false;
    }
}
