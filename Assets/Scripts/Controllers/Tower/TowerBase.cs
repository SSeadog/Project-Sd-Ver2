using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    protected Define.ObjectType _type;
    protected TowerStat _stat;
    protected GameObject _oppositeTower;
    protected float _moveDeg = 0f;
    
    Transform _spawnRoot; // ���� ��Ʈ
    float _degGap = 80f;
    
    public List<Transform> LstWayPoint { get { return _lstWaypoint; } }
    List<Transform> _lstWaypoint = new List<Transform>();

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
            _moveDeg = 180f;
        }
        else if (_type == Define.ObjectType.EnemyTower)
        {
            _oppositeTower = Managers.Game.FriendlyTower;
            _moveDeg = 0f;
        }

        LoadWayPoints();
        SetWaypoints();

        StartCoroutine(CoWaypointsCheck());
    }

    // ��ǥ�� ������ȭ�ؼ� �ε��ϴ� ������� �����ϴ� �� ���� ��
    void LoadWayPoints()
    {
        string path = "Prefabs/WayPoints/GameScene" + Managers.Game.StageNum + "/" + _type.ToString() + "WayPoints";
        GameObject waypoints = Resources.Load<GameObject>(path);
        GameObject instance = Instantiate(waypoints, transform);
        instance.name = "Waypoints";
    }

    // waypoints�� ��ġ�� �°� ������ ������ �����ϴ� �Լ�(���뿡 �°� �̸� ���� �ʿ�)
    void SetWaypoints()
    {
        // 1. �ݴ��� Ÿ������ ���� ����� ���� ���� ���� ��
        // 2. �ش� ���� �������� ���� ����� ���� ������ ã�Ƴ���
        // 2-1. �Ÿ��� ���� ª���鼭 �̵����⿡ �´����� üũ. �̵����� ���� ��� ������ ���� �Ǵ�
        // * �����ϰ� waypoint�� ��ġ���� ������ ��� waypoint�� �̿����� ���� ���� ����

        Transform[] waypoints = transform.Find("Waypoints").GetComponentsInChildren<Transform>();

        Queue<Transform> queue = new Queue<Transform>();
        Transform startPoint = _oppositeTower.transform;
        {
            float minDist = 99999f;
            Transform minDistPoint = null;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i].name == "Waypoints")
                    continue;

                float dist = Vector3.Distance(startPoint.position, waypoints[i].position);
                if (dist < minDist)
                {
                    minDist = dist;
                    minDistPoint = waypoints[i];
                }
            }

            if (minDistPoint != null)
            {
                queue.Enqueue(minDistPoint);
                _lstWaypoint.Add(minDistPoint);
            }
        }

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
                if (dist < minDist)
                {
                    // �ִ� �Ÿ����� ������ �̿��� ������ �������� �ѹ� �� üũ
                    Vector3 moveVec = (waypoints[i].position - basePoint.position).normalized;
                    float curMoveDeg = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
                    if (curMoveDeg < 0f)
                        curMoveDeg += 360f;

                    float minDeg = (_moveDeg - _degGap < 0f) ? _moveDeg - _degGap + 360f : _moveDeg - _degGap;
                    float maxDeg = _moveDeg + _degGap;

                    if (minDeg > maxDeg)
                    {
                        if ((curMoveDeg > minDeg && curMoveDeg < 360f) || (curMoveDeg > 0f && curMoveDeg < maxDeg))
                        {
                            minDist = dist;
                            minDistPoint = waypoints[i];
                        }
                    }
                    else
                    {
                        if ((curMoveDeg > minDeg) && (curMoveDeg < maxDeg))
                        {
                            minDist = dist;
                            minDistPoint = waypoints[i];
                        }
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
