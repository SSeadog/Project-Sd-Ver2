using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMonsterItem : MonoBehaviour
{
    public IEnumerator CoSelected()
    {
        RectTransform rt = GetComponent<RectTransform>();
        while (rt.localScale.x < 1.15f)
        {
            rt.localScale = new Vector3(rt.localScale.x * 1.02f, rt.localScale.y * 1.02f, rt.localScale.z * 1.02f);
            yield return new WaitForSeconds(0.01f);
        }

        while (rt.localScale.x > 1f)
        {
            rt.localScale = new Vector3(rt.localScale.x * 0.98f, rt.localScale.y * 0.98f, rt.localScale.z * 0.98f);
            yield return new WaitForSeconds(0.01f);
        }

        rt.localScale = new Vector3(1f, 1f, 1f);
    }
}
