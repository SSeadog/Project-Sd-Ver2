using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMonsterPanerlUI : MonoBehaviour
{
    // Todo
    // 소환 가능한 몬스터들 목록 가져와서 버튼들 만들어내기 -> 데이터를 어떻게 관리하지.. -> 일단 패스
    // 버튼에 맞는 키(1,2,3) 누르면 동작하는 애니메이션 넣기

    FriendlyMonsterItem[] _items;

    PlayerStat _playerStat;

    void Start()
    {
        _items = GetComponentsInChildren<FriendlyMonsterItem>();
    }

    void Update()
    {
        // 여기도 수정 필요
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
