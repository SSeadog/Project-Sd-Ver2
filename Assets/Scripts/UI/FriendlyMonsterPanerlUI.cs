using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMonsterPanerlUI : MonoBehaviour
{
    // Todo
    // ��ȯ ������ ���͵� ��� �����ͼ� ��ư�� ������ -> �����͸� ��� ��������.. -> �ϴ� �н�
    // ��ư�� �´� Ű(1,2,3) ������ �����ϴ� �ִϸ��̼� �ֱ�

    FriendlyMonsterItem[] _items;

    PlayerStat _playerStat;

    void Start()
    {
        _items = GetComponentsInChildren<FriendlyMonsterItem>();
    }

    void Update()
    {
        // ���⵵ ���� �ʿ�
        if (_playerStat == null)
        {
            _playerStat = Managers.Game.player.GetComponent<PlayerStat>();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_playerStat.ResourcePoint < 10f)
                return;

            StartCoroutine(_items[0].CoSelected());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_playerStat.ResourcePoint < 15f)
                return;

            StartCoroutine(_items[1].CoSelected());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_playerStat.ResourcePoint < 25f)
                return;

            StartCoroutine(_items[2].CoSelected());
        }
    }
}
