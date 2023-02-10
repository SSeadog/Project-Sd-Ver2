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
