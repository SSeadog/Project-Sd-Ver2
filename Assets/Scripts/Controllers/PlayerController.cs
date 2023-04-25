using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;

    Vector3 _moveVec = Vector3.zero;

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
    Anims CurAnim
    {
        get { return _curAnim; }
        set
        {
            if (_curAnim != value)
            {
                _anim.CrossFade(value.ToString(), 0.1f);
                _curAnim = value;
            }
        }
    }
    Animator _anim;

    bool _isAttack = false;

    GameObject _arrowOriginal;
    GameObject _arrowPosition;

    [SerializeField] GameObject _handedArrow;

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
            //_anim.CrossFade("Attack", 0.1f);
            //CurAnim = Anims.Attack;

            StartCoroutine(EndFireArrow());
        }
    }
    
    // Spawn()이 플레이어 컨트롤러 밑에 둘만 한가..? 따로 빼야하나?? 고민 필요
    // Spawn 비용 데이터는 어떻게 관리하지..?
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
        //if (_isAttack)
        //    return;

        if (_isAttack)
            CurAnim = Anims.Attack;
        else if (_moveVec.x > 0)
            CurAnim = Anims.WalkRight;
        else if (_moveVec.x < 0)
            CurAnim = Anims.WalkLeft;
        else if (_moveVec.z > 0)
            CurAnim = Anims.WalkForward;
        else if (_moveVec.z < 0)
            CurAnim = Anims.WalkBack;
        else
            CurAnim = Anims.Idle;
    }

    public void FireArrow()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            Vector3 moveVec = (raycastHit.point - _arrowPosition.transform.position).normalized;
            GameObject instanceArrow = Instantiate(_arrowOriginal, _arrowPosition.transform.position, Quaternion.LookRotation(moveVec));
            instanceArrow.GetComponent<WeaponStat>().Init(_stat);
            Rigidbody arrowRigid = instanceArrow.GetComponent<Rigidbody>();
            arrowRigid.AddForce(moveVec * 2000f);
        }

        _handedArrow.SetActive(false);
    }

    IEnumerator EndFireArrow()
    {
        yield return new WaitForSeconds(0.5f);
        _isAttack = false;
        _handedArrow.SetActive(true);
        CurAnim = Anims.Idle; // CrossFade로 같은 애니메이션 실행 시 처음부터 재생되지 않아서 공격 종료 후 강제로 Idle을 실행해줌
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
