using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class BuffPanelController : MonoBehaviour
{
    GameObject buffPanel;
    Text buffName;
    Text buffDescription;
    StageChoice stageChoice;
    
    public BuffCard BuffCard;

    void Start()
    {
        buffPanel = GameObject.Find("Canvas").transform.Find("BuffPanel").gameObject;
        buffName = GameObject.Find("Canvas").transform.Find("BuffPanel/BuffName").gameObject.GetComponent<Text>();
        buffDescription = GameObject.Find("Canvas").transform.Find("BuffPanel/BuffDescription").gameObject.GetComponent<Text>();
        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
    }

    void OnEnable()
    {
        if (BuffCard != null)
        {
            buffName.text = BuffCard.Buff.name;
            buffDescription.text = BuffCard.Buff.description;
        }
    }

    public void OnClickBuffButton()
    {
        GameState.Instance.player.AddBuff(BuffCard.Buff);
        stageChoice.MoveToNextStage();
    }
}
