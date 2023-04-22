using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    protected Define.ObjectType _type;
    protected TowerStat _stat;
    protected GameObject _oppositeTower;
    protected float _moveDeg = 0f;
    
    Transform _spawnRoot; // 스폰 루트
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

    // 좌표로 데이터화해서 로드하는 방식으로 변경하는 게 좋을 듯
    void LoadWayPoints()
    {
        string path = "Prefabs/WayPoints/GameScene" + Managers.Game.StageNum + "/" + _type.ToString() + "WayPoints";
        GameObject waypoints = Resources.Load<GameObject>(path);
        GameObject instance = Instantiate(waypoints, transform);
        instance.name = "Waypoints";
    }

    // waypoints를 위치에 맞게 적절한 순서로 정리하는 함수(내용에 맞게 이름 변경 필요)
    void SetWaypoints()
    {
        // 1. 반대편 타워에서 가장 가까운 점을 제일 먼저 둠
        // 2. 해당 점을 시작으로 가장 가까운 다음 점들을 찾아나감
        // 2-1. 거리가 가장 짧으면서 이동방향에 맞는지를 체크. 이동방향 같은 경우 각도를 통해 판단
        // * 적절하게 waypoint를 배치하지 않으면 모든 waypoint를 이용하지 않을 수도 있음

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
                    // 최단 거리여도 각도를 이용해 방향을 기준으로 한번 더 체크
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
