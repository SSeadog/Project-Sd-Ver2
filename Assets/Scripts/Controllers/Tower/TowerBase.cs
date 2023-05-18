using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    protected Define.ObjectType _type;
    protected TowerStat _stat;
    protected GameObject _oppositeTower;
    protected float _baseDir = 0f;
    
    Transform _spawnRoot; // ���� ��Ʈ
    float _degGap = 80f;
    List<Transform> _lstWaypoint = new List<Transform>();
    
    public List<Transform> LstWayPoint { get { return _lstWaypoint; } }

    public virtual void Init()
    {
        _spawnRoot = transform.Find("spawnRoot");
        _stat = GetComponent<TowerStat>();
        _stat.OnDeadAction += OnDead;
    }

    void Start()
    {
        if (_type == Define.ObjectType.FriendlyTower)
        {
            _oppositeTower = Managers.Game.EnemyTower;
            _baseDir = 180f;
        }
        else if (_type == Define.ObjectType.EnemyTower)
        {
            _oppositeTower = Managers.Game.FriendlyTower;
            _baseDir = 0f;
        }

        LoadWayPoints();
        SetWaypoints();
    }

    // ��ǥ�� ������ȭ�ؼ� �ε��ϴ� ������� �����ϴ� �� ���� ��
    void LoadWayPoints()
    {
        string path = "Prefabs/WayPoints/GameScene" + Managers.Game.StageNum + "/" + _type.ToString() + "WayPoints";
        GameObject waypoints = Resources.Load<GameObject>(path);
        GameObject instance = Instantiate(waypoints, transform);
        instance.name = "Waypoints";
    }

    // * �����ϰ� waypoint�� ��ġ���� ������ ��� waypoint�� �̿����� ���� ���� ����
    // * Ư�� waypoint���� ������ ���� waypoint�� �����Ƿ��� _baseDir - _degGap ~ _baseDir + _degGap���̿� �־����
    void SetWaypoints()
    {
        Transform[] waypoints = transform.Find("Waypoints").GetComponentsInChildren<Transform>();
        Queue<Transform> queue = new Queue<Transform>();

        // �ݴ��� Ÿ������ ���� ����� ���� ���������� ����
        Transform startPoint = _oppositeTower.transform;
        queue.Enqueue(startPoint);

        // �ش� ���� �������� ������ �̵� ������ ���� ����� ���� ������ ã�Ƴ���
        while (queue.Count > 0)
        {
            Transform basePoint = queue.Dequeue();

            float minDist = 99999f;
            Transform minDistPoint = null;

            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i].name == "Waypoints")
                    continue;

                if (waypoints[i].position == basePoint.position)
                    continue;

                float dist = Vector3.Distance(basePoint.position, waypoints[i].position);
                // �ִ� �Ÿ�����
                if (dist < minDist)
                {
                    // �ش����� ������ �̵��������� Ȯ��
                    if (IsRightDirection(basePoint, waypoints[i]) == true)
                    {
                        minDist = dist;
                        minDistPoint = waypoints[i];
                    }
                }
            }

            if (minDistPoint != null)
            {
                queue.Enqueue(minDistPoint);
                _lstWaypoint.Add(minDistPoint);
            }
        }
    }

    // ������ �̿��� ������ �̵� �������� Ȯ���ϴ� �Լ�
    bool IsRightDirection(Transform basePoint, Transform nextPoint)
    {
        Vector3 moveVec = (nextPoint.position - basePoint.position).normalized;
        float curMoveDeg = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
        if (curMoveDeg < 0f)
            curMoveDeg += 360f;

        float minDeg = (_baseDir - _degGap < 0f) ? _baseDir - _degGap + 360f : _baseDir - _degGap;
        float maxDeg = _baseDir + _degGap;

        if (minDeg > maxDeg)
        {
            if ((curMoveDeg > minDeg && curMoveDeg < 360f) || (curMoveDeg > 0f && curMoveDeg < maxDeg))
                return true;
        }
        else
        {
            if ((curMoveDeg > minDeg) && (curMoveDeg < maxDeg))
                return true;
        }

        return false;
    }

    // SetWaypoints()�Լ� ������ϴ� �Լ�
    IEnumerator CoWaypointsCheck()
    {
        foreach (Transform t in _lstWaypoint)
        {
            t.GetComponent<MeshRenderer>().material.color = Color.red;
            yield return new WaitForSeconds(1f);
            t.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Stat attackerStat = other.GetComponent<Stat>();
        _stat.GetAttacked(attackerStat);
    }

    public Transform GetSpawnRoot()
    {
        return _spawnRoot;
    }

    public virtual void OnDead()
    {
    }
}
