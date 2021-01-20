using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatPanelUIController : MonoBehaviour
{
    Text CoinText;
    Text ATK, DEF, SPD, HP;
    Slider HpSlider;
    Text buffName;
    Text buffSummary;

    void Start()
    {
        CoinText = GameObject.Find("CoinText").GetComponent<Text>();

        ATK = GameObject.Find("ATK text").GetComponent<Text>();
        DEF = GameObject.Find("DEF text").GetComponent<Text>();
        SPD = GameObject.Find("SPD text").GetComponent<Text>();
        HP = GameObject.Find("HP Txt").GetComponent<Text>();
        HpSlider = GameObject.Find("HpSlider").GetComponent<Slider>();
        
        buffName = GameObject.Find("BF name").GetComponent<Text>();
        buffSummary = GameObject.Find("Canvas").transform.Find("BuffandStatPanel/BF popup/BF description").gameObject.GetComponent<Text>();
    }

    void Update()
    {
        Player player = GameState.Instance.player;
        if (player != null)
            UpdateStat(player);
    }

    void UpdateStat(Player player)
    {       
        CoinText.text = $"{player.money} G";

        ATK.text = player.GetStat().attack.ToString();
        DEF.text = player.GetStat().defense.ToString();
        SPD.text = player.GetStat().speed.ToString();

        string hp_now = player.hp.ToString(), hp_full; //임시로 설정
        hp_full = player.GetStat().maxHp.ToString();
        HP.text = "HP  " + hp_now + " / " + hp_full;

        HpSlider.value = Convert.ToInt32(hp_now);
        HpSlider.maxValue = Convert.ToInt32(hp_full);
        buffName.text = player.GetBuff().name;
        buffSummary.text = player.GetBuff().description;
    }
}