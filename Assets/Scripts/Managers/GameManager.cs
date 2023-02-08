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
