using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapIconUI : MonoBehaviour
{
    public Transform _parent;
    public Image _icon;

    void Start()
    {
        _parent = transform.parent;

        _icon = GetComponentInChildren<Image>();

        switch(_parent.GetComponent<Stat>()._type)
        {
            case Define.ObjectType.Player:
            case Define.ObjectType.FriendlyMeleeMonster:
            case Define.ObjectType.FriendlyRangedMonster:
            case Define.ObjectType.FriendlyPowerMonster:
                _icon.color = new Color(0f, 169f / 255f, 1f);
                break;
            case Define.ObjectType.EnemyMeleeMonster:
            case Define.ObjectType.EnemyRangedMonster:
            case Define.ObjectType.EnemyPowerMonster:
                _icon.color = new Color(1f, 0f, 0f);
                break;
        }
    }

    void Update()
    {
        
    }
}
