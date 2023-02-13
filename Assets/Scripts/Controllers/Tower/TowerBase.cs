using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] protected Define.ObjectType _type;
    // 스탯 -> Stat스크립트 따로 분리
    TowerStat _stat;
    // 스폰 루트
    Transform _spawnRoot;
    // waypoints
    Transform _oppsiteTowerTransform;
    float _moveDeg = 0f;
    float _degGap = 80f;
    
    List<Transform> _lstWaypoint = new List<Transform>();
    public List<Transform> LstWayPoint { get { return _lstWaypoint; } }

    public virtual void Init()
    {
        _spawnRoot = transform.Find("spawnRoot");
    }

    void Start()
    {
        Init();

        if (_type == Define.ObjectType.FriendlyTower)
        {
            _oppsiteTowerTransform = Managers.Game.enemyTower.transform;
            _moveDeg = 180f;
        }
        else if (_type == Define.ObjectType.EnemyTower)
        {
            _oppsiteTowerTransform = Managers.Game.friendlyTower.transform;
            _moveDeg = 0f;
        }

        SetWaypoints();

        StartCoroutine(CoWaypointsCheck());
    }

    // waypoints를 위치에 맞게 적절한 순서로 정리하는 함수(내용에 맞게 이름 변경 필요)
    void SetWaypoints()
    {
        // 1. 반대편 타워에서 가장 가까운 점을 제일 먼저 둠
        // 2. 해당 점을 시작으로 가장 가까운 다음 점들을 찾아나감
        // 2-1. 거리가 가장 짧으면서 이동방향에 맞는지를 체크. 이동방향 같은 경우 각도를 통해 판단
        // * 적절하게 waypoint를 배치하지 않으면 모든 waypoint를 이용하지 않을 수도 있음

        Transform[] waypoints = transform.Find("waypoints").GetComponentsInChildren<Transform>();

        Queue<Transform> queue = new Queue<Transform>();
        Transform startPoint = _oppsiteTowerTransform;
        {
            float minDist = 99999f;
            Transform minDistPoint = null;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i].name == "waypoints")
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
                if (waypoints[i].name == "waypoints")
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

    void Update()
    {
        
    }

    public Transform GetSpawnRoot()
    {
        return _spawnRoot;
    }
}
