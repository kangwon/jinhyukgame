using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatPanelUIController : MonoBehaviour
{
    Text ATK, DEF, SPD, HP;
    Slider HP_slider;
    Text buffName;
    Text buffSummary;

    Player player = GameState.Instance.player;

    void Start()
    {
        ATK = GameObject.Find("ATK text").GetComponent<Text>();
        DEF = GameObject.Find("DEF text").GetComponent<Text>();
        SPD = GameObject.Find("SPD text").GetComponent<Text>();
        HP = GameObject.Find("HP Txt").GetComponent<Text>();
        HP_slider = GameObject.Find("HP_Slider").GetComponent<Slider>();
        
        buffName = GameObject.Find("BF name").GetComponent<Text>();
        buffSummary = GameObject.Find("Canvas").transform.Find("PlayerStatPanel/BF popup/BF description").gameObject.GetComponent<Text>();
    }

    void Update()
    {
        UpdateStat();
    }

    void UpdateStat()
    {
        ATK.text = player.GetStat().attack.ToString();
        DEF.text = player.GetStat().defense.ToString();
        SPD.text = player.GetStat().speed.ToString();

        string hp_now = player.hp.ToString(), hp_full; //임시로 설정
        hp_full = player.GetStat().maxHp.ToString();
        HP.text = "HP  " + hp_now + " / " + hp_full;

        HP_slider.value = Convert.ToInt32(hp_now);

        buffName.text = player.GetBuff().name;
        buffSummary.text = player.GetBuff().description;
    }
}