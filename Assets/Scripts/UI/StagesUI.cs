using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesUI : MonoBehaviour
{
    GameObject _gridPanel;
    GameObject _stageIconOrigianl;

    // ���ǿ��� UI��Ҵ� Init�� ������ Init�� public���� ������ Start���� �����ϵ��� �ϰ�
    // Text, �̹��� �� �����ؾ��ϴ� ��Ҵ� SetText(), SetImage() ������ �Լ��� �����ΰ� Instantiate�Ҷ� �����ϵ��� �ߴµ� �� �׷��� ������
    // Init()���� �޾Ƽ� ���� ���� ������? �ߴµ� Init�� �θ� ����� �����. �Ű����� �޴� ��縶�� �̸� �� �θ𿡼� ���Ǹ� �ص־��ϴµ� �װ� ���� ���� �� ����

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
