using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] protected Define.ObjectType _type;
    // ���� -> Stat��ũ��Ʈ ���� �и�
    TowerStat _stat;
    // ���� ��Ʈ
    // waypoints
    Transform _oppsiteTowerTransform;
    float _moveDeg = 0f;
    float _degGap = 80f;
    List<Transform> _lstWaypoint = new List<Transform>();

    public virtual void Init()
    {
    }

    void Start()
    {
        if (_type == Define.ObjectType.PlayerTower)
        {
            _oppsiteTowerTransform = Managers.Game.enemyTower.transform;
            _moveDeg = 180f;
        }
        else if (_type == Define.ObjectType.EnemyTower)
        {
            _oppsiteTowerTransform = Managers.Game.playerTower.transform;
            _moveDeg = 0f;
        }

        SetWaypoints();

        StartCoroutine(CoWaypointsCheck());
    }

    void SetWaypoints()
    {
        // 1. waypoints���� ���� ����� �� ã�� -> ��� Ÿ������ ���� ����� �� ã��� �����ؾ��� �� ������
        // 2. 1���� ������ ���� ����� �� ã��
        // �ݺ�

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

                if (basePoint.name == "waypoint (10)")
                {
                    int a = 0;
                }

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

    void Update()
    {
        
    }
}
