using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // Todo
    // Spawn()���� path�� original �� ã���� ����ó��? ���ֱ�
    // �ν��Ͻ��� ���ӿ�����Ʈ�� �ε��ص״��� Ȯ�� �� ������ ���� �ε��ϵ���, �̹� �ε��߾��ٸ� �ε��ص� �� �̿��ϵ��� ���� �ʿ�

    // ���ӽ��������� �� �� �������� ���� �ʿ�. ���ӽ������������� �����ؾ��ϴ� �κе��� ����(playTime��� ��)
    public int stageNum;

    public GameObject friendlyTower;
    public GameObject enemyTower;

    public List<Define.spawnItem> spawnInfo;

    // ���� ������ id�� �ο��ؼ� Dict�� ���� �׳� List�� ���� �����
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
