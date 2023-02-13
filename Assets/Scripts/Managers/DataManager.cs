using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    public Dictionary<string, Define.SettingInfo> settingInfo;

    public void Init()
    {
        settingInfo = Util.LoadJsonDict<Define.SettingInfo>("GameSettings/Stages/Setting_1");
    }

    public void Clear()
    {

    }
}
