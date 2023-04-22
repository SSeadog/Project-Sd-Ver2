using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    Dictionary<string, Define.SettingInfo> settingInfo;
    Dictionary<string, Define.MonsterStat> monsterStats;

    public Dictionary<string, Define.SettingInfo> SettingInfo { get { return settingInfo; } }
    public Dictionary<string, Define.MonsterStat> MonsterStats { get { return monsterStats; } }

    public void Init()
    {
        settingInfo = Util.LoadJsonDict<Define.SettingInfo>("Data/Stages/Setting_1");
        monsterStats = Util.LoadJsonDict<Define.MonsterStat>("Data/MonsterStats");
    }

    public void Clear()
    {

    }
}
