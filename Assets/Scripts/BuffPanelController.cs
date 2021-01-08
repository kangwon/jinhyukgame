using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class BuffPanelController : MonoBehaviour
{

    GameObject buffName;
    GameObject buffDescription;
    public GameObject buffView;
    public GameObject buffPanel;
    public StageChoice stageChoice;
    public GameObject buffSummary;

    public bool buffUpdate = false;

    public string str = "버프 없음";
    public string str2 = "버프 없음";

    void Start()
    {
        buffSummary.SetActive(true);
        this.buffSummary = GameObject.Find("BF summary");
        buffSummary.SetActive(false);
        this.buffView = GameObject.Find("BF view");
        this.buffName = GameObject.Find("BuffName");
        this.buffDescription = GameObject.Find("BuffDescription");
    }

    public void OnClickBuffButton()
    {
        buffUpdate = false;
        buffView.GetComponent<Text>().text = str;
        buffSummary.GetComponent<Text>().text = str2;
        stageChoice.MoveToNextStage();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (buffUpdate == false)
        {
            int i = Random.Range(1, 11); // json 파일 버프 수가 늘어나면 바꾸기
            StatBuff buff = JsonDB.GetBuff($"buff{i}");
            Debug.Log(buff.speedPercent);
            GameState.Instance.player.AddBuff(buff);
            str = buff.name;
            str2 = buff.description;
            buffName.GetComponent<Text>().text = buff.name;
            buffDescription.GetComponent<Text>().text = buff.description;
            buffUpdate = true;
        }
    }
}
