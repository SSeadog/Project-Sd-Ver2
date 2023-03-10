using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // Todo
    // Spawn()에서 path로 original 못 찾으면 예외처리? 해주기
    // 인스턴스할 게임오브젝트를 로드해뒀는지 확인 후 안했을 때만 로드하도록, 이미 로드했었다면 로드해둔 거 이용하도록 개선 필요

    // 게임스테이지와 그 외 스테이지 구분 필요. 게임스테이지에서만 동작해야하는 부분들이 있음(playTime기록 등)
    // -1 = 게임 외 스테이지. 1~n 스테이지 번호로 두며 게임 스테이지로 취급
    public int stageNum;

    public UIController uIController;

    public GameObject friendlyTower;
    public GameObject enemyTower;

    public List<Define.spawnItem> spawnInfo;

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
        Application.targetFrameRate = 60;
        playTime = 0f;

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

    public void UpdatePlayTime(float time)
    {
        playTime += time;
    }

    public void UpdatePlayerRp(float time)
    {
        if (player == null)
            return;

        PlayerStat playerStat = player.GetComponent<PlayerStat>();
        if (playerStat.ResourcePoint + time * 10f < playerStat.MaxResourcePoint)
        {
            player.GetComponent<PlayerStat>().ResourcePoint += time * 2.5f;
        }
        else
        {
            player.GetComponent<PlayerStat>().ResourcePoint = playerStat.MaxResourcePoint;
        }
    }


    public GameObject Spawn(Define.ObjectType objectType, string path, Transform parent = null)
    {
        GameObject original = Resources.Load<GameObject>(path);
        GameObject instance = GameObject.Instantiate(original, parent);

        switch (objectType)
        {
            case Define.ObjectType.FriendlyMeleeMonster:
            case Define.ObjectType.FriendlyRangedMonster:
            case Define.ObjectType.FriendlyPowerMonster:
                instance.transform.position = friendlyTower.GetComponent<TowerBase>().GetSpawnRoot().position;
                instance.GetComponent<MonsterStat>().Init(objectType, Managers.Data.monsterStats[objectType.ToString()]);
                Managers.Game.friendlyMonsters.Add(instance);
                spawnedFriendlyMonsterCount++;
                break;
            case Define.ObjectType.EnemyMeleeMonster:
            case Define.ObjectType.EnemyRangedMonster:
            case Define.ObjectType.EnemyPowerMonster:
                instance.transform.position = enemyTower.GetComponent<TowerBase>().GetSpawnRoot().position;
                instance.GetComponent<MonsterStat>().Init(objectType, Managers.Data.monsterStats[objectType.ToString()]);
                Managers.Game.enemyMonsters.Add(instance);
                spawnedEnemyMonsterCount++;
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

    public void GameLose()
    {
        Time.timeScale = 0f;
        SetActiveCursor(true);
        uIController._gameEndingUI.ShowLoseUI();
    }

    public void GameWin()
    {
        Time.timeScale = 0f;
        SetActiveCursor(true);
        uIController._gameEndingUI.ShowWinUI();
    }

    public void Clear()
    {
        stageNum = -1;

        uIController = null;

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

        spawnInfo = null;
    }
}
