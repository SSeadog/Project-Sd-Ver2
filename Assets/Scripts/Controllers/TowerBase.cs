using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    // 스탯 -> Stat스크립트 따로 분리
    // 스폰 루트
    // waypoints
    float _moveDeg = 180f;
    float _degGap = 89f;
    List<Transform> _lstWaypoint = new List<Transform>();

    void Start()
    {
        SetWaypoints();

        //foreach (Transform t in lstWaypoint)
        //{
        //    Debug.Log(t.name);
        //}

        float tempDeg = _moveDeg;
        Debug.DrawRay(Vector3.up * 10f, new Vector3(Mathf.Sin(_moveDeg * Mathf.Deg2Rad), 0f, Mathf.Cos(_moveDeg * Mathf.Deg2Rad)) * 10f, Color.red, 10f);
        tempDeg = _moveDeg + _degGap;
        Debug.DrawRay(Vector3.up * 10f, new Vector3(Mathf.Sin(tempDeg * Mathf.Deg2Rad), 0f, Mathf.Cos(tempDeg * Mathf.Deg2Rad)) * 10f, Color.red, 10f);
        tempDeg = _moveDeg - _degGap;
        Debug.DrawRay(Vector3.up * 10f, new Vector3(Mathf.Sin(tempDeg * Mathf.Deg2Rad), 0f, Mathf.Cos(tempDeg * Mathf.Deg2Rad)) * 10f, Color.red, 10f);

        StartCoroutine(CoWaypointsCheck());
    }

    void SetWaypoints()
    {
        // 1. waypoints에서 가장 가까운 점 찾기 -> 시작점에서 가장 가까운 점 찾기로 수정해야할 거 같은데
        // 2. 1번의 점에서 가장 가까운 점 찾기
        // 반복

        Transform[] waypoints = transform.Find("waypoints").GetComponentsInChildren<Transform>();

        Queue<Transform> queue = new Queue<Transform>();
        Transform startPoint = transform;
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

                if (basePoint.name == "waypoint (3)")
                {
                    int a = 0;
                }

                float dist = Vector3.Distance(basePoint.position, waypoints[i].position);
                if (dist < minDist)
                {
                    // 최단 거리여도 각도를 이용해 방향을 기준으로 한번 더 체크
                    Vector3 moveVec = (waypoints[i].position - basePoint.position).normalized;
                    float curMoveDeg = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
                    if (curMoveDeg < 0f)
                        curMoveDeg += 360f;

                    //float tempDeg = curMoveDeg;
                    //Debug.DrawRay(basePoint.position + Vector3.up * 10f, new Vector3(Mathf.Sin(curMoveDeg * Mathf.Deg2Rad), 0f, Mathf.Cos(curMoveDeg * Mathf.Deg2Rad)) * 10f, Color.red, 10f);
                    //tempDeg = curMoveDeg + _degGap;
                    //Debug.DrawRay(basePoint.position + Vector3.up * 10f, new Vector3(Mathf.Sin(tempDeg * Mathf.Deg2Rad), 0f, Mathf.Cos(tempDeg * Mathf.Deg2Rad)) * 10f, Color.red, 10f);
                    //tempDeg = curMoveDeg - _degGap;
                    //Debug.DrawRay(basePoint.position + Vector3.up * 10f, new Vector3(Mathf.Sin(tempDeg * Mathf.Deg2Rad), 0f, Mathf.Cos(tempDeg * Mathf.Deg2Rad)) * 10f, Color.red, 10f);

                    if ((curMoveDeg > _moveDeg - _degGap) && (curMoveDeg < _moveDeg + _degGap))
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

        //for (int i = 0; i < waypoints.Length; i++)
        //{
        //    Transform basePoint = waypoints[i];
        //    float minDist = 99999f;
        //    Transform minDistPoint = null;
        //    for (int j = 0; j < waypoints.Length; j++)
        //    {
        //        if (waypoints[j].name == "waypoints")
        //            continue;

        //        if (i == j)
        //            continue;

        //        float dist = Vector3.Distance(basePoint.position, waypoints[j].position);
        //        if (dist < minDist)
        //        {
        //            // 이때 방향을 각도로 한번 더 체크
        //            Vector3 moveVec = (waypoints[j].position - basePoint.position).normalized;
        //            float curMoveDeg = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
        //            Debug.Log($"curMoveDeg: {curMoveDeg}");
        //            if (curMoveDeg > moveDeg - 50f && curMoveDeg < moveDeg + 50f)
        //            {
        //                minDist = dist;
        //                minDistPoint = waypoints[j];
        //            }
        //        }
        //    }

        //    if (minDistPoint != null)
        //        lstWaypoint.Add(minDistPoint);
        //}
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
