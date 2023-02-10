using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // �������� ������
    //// ������ �������� ��ȣ

    // �ΰ��� ������
    //// ���� �������� ��ȣ

    //// �÷��̾� Ÿ��
    //// �� Ÿ��

    //// �÷��̾� ���͵�
    //// �� ���͵�

    //// �÷��̾�

    //// �÷��� �ð�

    //// ��ȯ�� �Ʊ� ���� ��
    //// ��ȯ�� �� ���� ��
    //// óġ�� �Ʊ� ���� ��
    //// óġ�� �� ���� ��

    public int stageNum;

    public GameObject playerTower;
    public GameObject enemyTower;

    // ���� ������ id�� �ο��ؼ� Dict�� ���� �׳� List�� ���� �����
    public List<GameObject> playerMonsters;
    public List<GameObject> enemyMonsters;

    public GameObject player;

    public float playTime;

    public int spawnedPlayerMonsterCount;
    public int spawnedEnemyMonsterCount;
    public int killedPlayerMonsterCount;
    public int killedEnemyMonsterCount;

    public void Init()
    {
        playerMonsters= new List<GameObject>();
        enemyMonsters= new List<GameObject>();
    }

    public GameObject Spawn(Define.ObjectType objectType, string path, Transform parent = null)
    {
        GameObject original = Resources.Load<GameObject>(path);
        GameObject instance = GameObject.Instantiate(original, parent);

        switch (objectType)
        {
            case Define.ObjectType.PlayerMeleeMonster:
                Managers.Game.playerMonsters.Add(instance);
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
            case Define.ObjectType.PlayerMeleeMonster:
                Managers.Game.playerMonsters.Remove(gameObject);
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

        playerTower = null;
        enemyTower = null;

        playerMonsters.Clear();
        enemyMonsters.Clear();

        player = null;

        playTime = 0f;

        spawnedPlayerMonsterCount = 0;
        spawnedEnemyMonsterCount = 0;
        killedPlayerMonsterCount = 0;
        killedEnemyMonsterCount = 0;
    }
}
