using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesUI : MonoBehaviour
{
    GameObject _gridPanel;
    GameObject _stageIconOrigianl;

    // 강의에서 UI요소는 Init을 가지고 Init은 public으로 뒀지만 Start에서 실행하도록 하고
    // Text, 이미지 등 설정해야하는 요소는 SetText(), SetImage() 등으로 함수로 만들어두고 Instantiate할때 설정하도록 했는데 왜 그렇게 했을까
    // Init()에서 받아서 만들 수는 없었나? 했는데 Init에 두면 상속이 어려움. 매개변수 받는 모양마다 미리 다 부모에서 정의를 해둬야하는데 그게 쉽지 않을 거 같네

    public void Init()
    {
        _gridPanel = transform.Find("Panel/GridPanel").gameObject;
        _stageIconOrigianl = Resources.Load<GameObject>("Prefabs/UI/SubItem/StageIcon");
    }

    void Start()
    {
        Init();

        for (int i = 1; i < 11; i++)
        {
            GameObject instance = Instantiate(_stageIconOrigianl, _gridPanel.transform);
            instance.GetComponent<StageIcon>().SetStageNumText(i);
        }
    }

}
