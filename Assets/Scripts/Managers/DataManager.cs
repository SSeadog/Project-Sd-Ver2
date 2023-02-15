using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    public Dictionary<string, Define.SettingInfo> settingInfo;
    public Dictionary<string, Define.MonsterStat> monsterStats;

    public void Init()
    {
        settingInfo = Util.LoadJsonDict<Define.SettingInfo>("Data/Stages/Setting_1");
        monsterStats = Util.LoadJsonDict<Define.MonsterStat>("Data/MonsterStats");
    }

    public void Clear()
    {

    }
}
