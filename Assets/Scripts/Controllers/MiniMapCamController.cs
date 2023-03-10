using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamController : MonoBehaviour
{
    void Update()
    {
        if (Managers.Game.player == null)
            return;

        Transform target = Managers.Game.player.transform;
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }
}
