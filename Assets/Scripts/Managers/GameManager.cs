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

    public List<Define.spawnItem> spawnInfo;

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

        spawnInfo = Util.LoadJsonList<List<Define.spawnItem>>("GameSettings/Stages/Spawn_1");

        friendlyMonsters = new List<GameObject>();
        enemyMonsters= new List<GameObject>();
    }

    public void SetActiveCursor(bool isActive)
    {
        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetDeActiveCursor()
    {
        
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

    public void RestartStage()
    {
        // 몬스터들 전부 삭제
        for (int i = 0; i < friendlyMonsters.Count; i++)
            GameObject.Destroy(friendlyMonsters[i].gameObject);
        for (int i = 0; i < enemyMonsters.Count; i++)
            GameObject.Destroy(enemyMonsters[i].gameObject);

        friendlyMonsters.Clear();
        enemyMonsters.Clear();

        // 스폰 정보 초기화
        ReSetSpawnInfo();

        // 시간 초기화
        playTime = 0f;

        // 플레이어 위치 초기화 -> Scene스크립트가 처리
        // 플레이어 체력 초기화 -> Scene스크립트가 처리
        // 타워들 체력 초기화 -> Scene스크립트가 처리
        Managers.Scene.Clear();
        Managers.Scene.CurrentScene.Init();
    }

    public void ReSetSpawnInfo()
    {
        spawnedEnemyMonsterCount = 0;

        for (int i = 0; i < Managers.Game.spawnInfo.Count; i++)
        {
            Managers.Game.spawnInfo[i].isSpawned = false;
        }
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
