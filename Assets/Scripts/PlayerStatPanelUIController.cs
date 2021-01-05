using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatPanelUIController : MonoBehaviour
{
    public Text ATK, DEF, SPD, HP;
    public Slider HP_slider;
    Player player = GameState.Instance.player;
    // Start is called before the first frame update
    void Start()
    {
        ATK = GameObject.Find("ATK text").GetComponent<Text>();
        DEF = GameObject.Find("DEF text").GetComponent<Text>();
        SPD = GameObject.Find("SPD text").GetComponent<Text>();
        HP = GameObject.Find("HP Txt").GetComponent<Text>();
        HP_slider = GameObject.Find("HP_Slider").GetComponent<Slider>();
        UpdateStat();
    }

    // Update is called once per frame
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
    }

}