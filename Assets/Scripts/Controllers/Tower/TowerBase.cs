using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    protected Define.ObjectType _type;
    protected TowerStat _stat;
    protected GameObject _oppositeTower;
    protected float _baseDir = 0f;
    
    Transform _spawnRoot; // 스폰 루트
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

    // 좌표로 데이터화해서 로드하는 방식으로 변경하는 게 좋을 듯
    void LoadWayPoints()
    {
        string path = "Prefabs/WayPoints/GameScene" + Managers.Game.StageNum + "/" + _type.ToString() + "WayPoints";
        GameObject waypoints = Resources.Load<GameObject>(path);
        GameObject instance = Instantiate(waypoints, transform);
        instance.name = "Waypoints";
    }

    // * 적절하게 waypoint를 배치하지 않으면 모든 waypoint를 이용하지 않을 수도 있음
    // * 특정 waypoint에서 적절한 다음 waypoint로 설정되려면 _baseDir - _degGap ~ _baseDir + _degGap사이에 있어야함
    void SetWaypoints()
    {
        Transform[] waypoints = transform.Find("Waypoints").GetComponentsInChildren<Transform>();
        Queue<Transform> queue = new Queue<Transform>();

        // 반대편 타워에서 가장 가까운 점을 시작점으로 잡음
        Transform startPoint = _oppositeTower.transform;
        queue.Enqueue(startPoint);

        // 해당 점을 시작으로 적절한 이동 방향의 가장 가까운 다음 점들을 찾아나감
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
                // 최단 거리인지
                if (dist < minDist)
                {
                    // 해당점이 적절한 이동방향인지 확인
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

    // 각도를 이용해 적절한 이동 방향인지 확인하는 함수
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

    // SetWaypoints()함수 디버깅하는 함수
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
