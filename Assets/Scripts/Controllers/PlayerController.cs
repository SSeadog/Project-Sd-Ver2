using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // OnDead�� �Լ� �־������
        // ���â ����?
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

        _moveVec = new Vector3(x, 0, y).normalized;

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

            Invoke("EndFireArrow", 0.5f); // Animation Event�ε� �����ϰ� �ִϸ��̼��� ����Ǿ� ������ ���� ���� �͵� ����Ͽ� Invoke�� �־���
        }
    }
    
    // Spawn()�� �÷��̾� ��Ʈ�ѷ� �ؿ� �Ѹ� �Ѱ�..? ���� �����ϳ�?? ��� �ʿ�
    // Spawn ��� �����ʹ� ��� ��������..?
    void Spawn()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_stat.ResourcePoint < 10f)
                return;

            _stat.ResourcePoint -= 10f;
            Managers.Game.Spawn(Define.ObjectType.FriendlyMeleeMonster, "Prefabs/Monsters/FriendlyMeleeMonster");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_stat.ResourcePoint < 15f)
                return;

            _stat.ResourcePoint -= 15f;
            Managers.Game.Spawn(Define.ObjectType.FriendlyRangedMonster, "Prefabs/Monsters/FriendlyRangedMonster");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_stat.ResourcePoint < 25f)
                return;

            _stat.ResourcePoint -= 25f;
            Managers.Game.Spawn(Define.ObjectType.FriendlyPowerMonster, "Prefabs/Monsters/FriendlyPowerMonster");
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

    private bool CheckAttackCollisionTagname(string collder_tag)
    {
        if (collder_tag == Define.TagName.FriendlyProjectile.ToString())
            return false;

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckAttackCollisionTagname(other.tag))
            return;

        Stat attackerStat = other.GetComponent<Stat>();
        _stat.GetAttacked(attackerStat);
    }
}
