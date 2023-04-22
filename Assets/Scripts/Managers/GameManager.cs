using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // Todo
    // Spawn()���� path�� original �� ã���� ����ó��? ���ֱ�
    // �ν��Ͻ��� ���ӿ�����Ʈ�� �ε��ص״��� Ȯ�� �� ������ ���� �ε��ϵ���, �̹� �ε��߾��ٸ� �ε��ص� �� �̿��ϵ��� ���� �ʿ�

    // ���ӽ��������� �� �� �������� ���� �ʿ�. ���ӽ������������� �����ؾ��ϴ� �κе��� ����(playTime��� ��)
    // -1 = ���� �� ��������. 1~n �������� ��ȣ�� �θ� ���� ���������� ���
    int stageNum;

    UIController uIController;
    GameObject player;
    GameObject friendlyTower;
    GameObject enemyTower;
    List<Define.spawnItem> spawnInfo;
    List<GameObject> friendlyMonsters;
    List<GameObject> enemyMonsters;

    public int StageNum { get { return stageNum; } set { stageNum = value; } }

    public UIController UIController { get { return uIController; } set { uIController = value; } }
    public GameObject Player { get { return player; } set { player = value; } }
    public GameObject FriendlyTower { get { return friendlyTower; } set { friendlyTower = value; } }
    public GameObject EnemyTower { get { return enemyTower; } set { enemyTower = value; } }
    public List<Define.spawnItem> SpawnInfo { get { return spawnInfo; } set { spawnInfo = value; } }
    public List<GameObject> FriendlyMonsters { get { return friendlyMonsters; } }
    public List<GameObject> EnemyMonsters { get { return enemyMonsters; } }

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
                instance.GetComponent<MonsterStat>().Init(objectType, Managers.Data.MonsterStats[objectType.ToString()]);
                Managers.Game.friendlyMonsters.Add(instance);
                spawnedFriendlyMonsterCount++;
                break;
            case Define.ObjectType.EnemyMeleeMonster:
            case Define.ObjectType.EnemyRangedMonster:
            case Define.ObjectType.EnemyPowerMonster:
                instance.transform.position = enemyTower.GetComponent<TowerBase>().GetSpawnRoot().position;
                instance.GetComponent<MonsterStat>().Init(objectType, Managers.Data.MonsterStats[objectType.ToString()]);
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
        // ���͵� ���� ����
        for (int i = 0; i < friendlyMonsters.Count; i++)
            GameObject.Destroy(friendlyMonsters[i].gameObject);
        for (int i = 0; i < enemyMonsters.Count; i++)
            GameObject.Destroy(enemyMonsters[i].gameObject);

        friendlyMonsters.Clear();
        enemyMonsters.Clear();

        // ���� ���� �ʱ�ȭ
        ReSetSpawnInfo();

        // �ð� �ʱ�ȭ
        playTime = 0f;

        // �÷��̾� ��ġ �ʱ�ȭ -> Scene��ũ��Ʈ�� ó��
        // �÷��̾� ü�� �ʱ�ȭ -> Scene��ũ��Ʈ�� ó��
        // Ÿ���� ü�� �ʱ�ȭ -> Scene��ũ��Ʈ�� ó��
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
