using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class StagesUI : MonoBehaviour
{
    GameObject _gridPanel;
    GameObject _stageIconOrigianl;

    List<int> _stageNums = new List<int>();

    // 강의에서 UI요소는 Init을 가지고 Init은 public으로 뒀지만 Start에서 실행하도록 하고
    // Text, 이미지 등 설정해야하는 요소는 SetText(), SetImage() 등으로 함수로 만들어두고 Instantiate할때 설정하도록 했는데 왜 그렇게 했을까
    // Init()에서 받아서 만들 수는 없었나? 했는데 Init에 두면 상속이 어려움. 매개변수 받는 모양마다 미리 다 부모에서 정의를 해둬야하는데 그게 쉽지 않을 거 같네

    public void Init()
    {
        _gridPanel = transform.Find("Panel/GridPanel").gameObject;
        _stageIconOrigianl = Resources.Load<GameObject>("Prefabs/UI/SubItem/StageIcon");

        string path = Application.dataPath + "/Resources/Data/Stages";

        DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.dataPath, "Resources/Data/Stages"));
        foreach (FileInfo file in di.GetFiles())
        {
            if (!file.FullName.Contains("Spawn"))
                continue;

            string[] splitedName = file.FullName.Split(".");
            if (splitedName[splitedName.Length - 1] != "meta")
            {
                int num = int.Parse(Regex.Replace(GetRealFileName(file.FullName), @"\D", ""));
                _stageNums.Add(num);
            }
        }
    }

    string GetRealFileName(string name)
    {
        string[] splitedName = name.Split("\\");
        return splitedName[splitedName.Length - 1];
    }

    void Start()
    {
        Init();

        foreach (int num in _stageNums)
        {
            GameObject instance = Instantiate(_stageIconOrigianl, _gridPanel.transform);
            instance.GetComponent<StageIcon>().SetStageNum(num);
        }
    }

}
