using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class BuffPanelController : MonoBehaviour
{

    Text buffName;
    Text buffDescription;
    public GameObject buffPanel;
    public StageChoice stageChoice;
    
    StatBuff buff;

    void Start()
    {
        buffName = GameObject.Find("Canvas").transform.Find("BuffPanel/BuffName").gameObject.GetComponent<Text>();
        buffDescription = GameObject.Find("Canvas").transform.Find("BuffPanel/BuffDescription").gameObject.GetComponent<Text>();
    }

    void OnEnable()
    {
        if (buffName != null && buffDescription != null)
        {
            int i = Random.Range(1, 11); // json 파일 버프 수가 늘어나면 바꾸기
            buff = JsonDB.GetBuff($"buff{i}");
            buffName.text = buff.name;
            buffDescription.text = buff.description;
        }
    }

    public void OnClickBuffButton()
    {
        GameState.Instance.player.AddBuff(buff);
        stageChoice.MoveToNextStage();
    }
}
