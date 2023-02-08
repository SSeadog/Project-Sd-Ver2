using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // 스테이지 데이터
    //// 선택한 스테이지 번호

    // 인게임 데이터
    //// 현재 스테이지 번호

    //// 플레이어 타워
    //// 적 타워

    //// 플레이어 몬스터들
    //// 적 몬스터들

    //// 플레이어

    //// 플레이 시간

    //// 소환된 아군 몬스터 수
    //// 소환된 적 몬스터 수
    //// 처치된 아군 몬스터 수
    //// 처치된 적 몬스터 수

    public int stageNum;

    public GameObject playerTower;
    public GameObject enemyTower;

    // 몬스터 관리는 id를 부여해서 Dict로 할지 그냥 List로 할지 고민중
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
