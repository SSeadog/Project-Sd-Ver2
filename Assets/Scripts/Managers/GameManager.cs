using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // Todo
    // Spawn()에서 path로 original 못 찾으면 예외처리? 해주기
    // 인스턴스할 게임오브젝트를 로드해뒀는지 확인 후 안했을 때만 로드하도록, 이미 로드했었다면 로드해둔 거 이용하도록 개선 필요

    // 게임스테이지와 그 외 스테이지 구분 필요. 게임스테이지에서만 동작해야하는 부분들이 있음(playTime기록 등)
    public int stageNum;

    public GameObject friendlyTower;
    public GameObject enemyTower;

    // 몬스터 관리는 id를 부여해서 Dict로 할지 그냥 List로 할지 고민중
    public List<GameObject> friendlyMonsters;
    public List<GameObject> enemyMonsters;

    public GameObject player;

    public float playTime;

    public int spawnedFriendlyMonsterCount;
    public int spawnedEnemyMonsterCount;
    public int killedFriendlyMonsterCount;
    public int killedEnemyMonsterCount;

    public void Init()
    {
        playTime = 0f;

        friendlyMonsters = new List<GameObject>();
        enemyMonsters= new List<GameObject>();
    }

    public void UpdatePlayTime(float time)
    {
        playTime += time;
    }

    public GameObject Spawn(Define.ObjectType objectType, string path, Transform parent = null)
    {
        GameObject original = Resources.Load<GameObject>(path);
        GameObject instance = GameObject.Instantiate(original, parent);

        switch (objectType)
        {
            case Define.ObjectType.FriendlyMeleeMonster:
                Managers.Game.friendlyMonsters.Add(instance);
                break;
            case Define.ObjectType.EnemyMeleeMonster:
                Managers.Game.enemyMonsters.Add(instance);
                break;
        }

        return instance;
    }

    public void Despawn(Define.ObjectType objectType, GameObject gameObject)
    {
        switch (objectType)
        {
            case Define.ObjectType.FriendlyMeleeMonster:
                Managers.Game.friendlyMonsters.Remove(gameObject);
                break;
            case Define.ObjectType.EnemyMeleeMonster:
                Managers.Game.enemyMonsters.Remove(gameObject);
                break;
        }

        Object.Destroy(gameObject, 1f);
    }

    public void Clear()
    {
        stageNum = -1;

        friendlyTower = null;
        enemyTower = null;

        friendlyMonsters.Clear();
        enemyMonsters.Clear();

        player = null;

        playTime = 0f;

        spawnedFriendlyMonsterCount = 0;
        spawnedEnemyMonsterCount = 0;
        killedFriendlyMonsterCount = 0;
        killedEnemyMonsterCount = 0;
    }
}
