using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanelController : MonoBehaviour
{
    public MonsterCard MonsterCard;

    StageChoice stageChoice;
    GameObject weaponChangePanel;
    EquipmentChangePanelController equipmentChanger;

    Button[] rewardButtons = new Button[3];
    Text coinReward;

    int rewardGetCoin;

    public void OnClickRewardAbandon()
    {
        stageChoice.MoveToNextStage();
    }

    void Start()
    {
        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
        weaponChangePanel = GameObject.Find("Canvas").transform.Find("WeaponChangePanel").gameObject;
        equipmentChanger = GameObject.Find("Canvas").transform.Find("EquipmentChangePanel").gameObject.GetComponent<EquipmentChangePanelController>();
        coinReward = GameObject.Find("gold_reward").GetComponent<Text>();

        for(int i = 0; i < 3; i++)
            rewardButtons[i] = GameObject.Find($"RewardPanel").transform.Find($"Reward{i+1}").gameObject.GetComponent<Button>();

        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (MonsterCard != null)
        {
            rewardGetCoin = (int)(MonsterCard.rewardCoin * (1 + GameState.Instance.player.GetStat().rewardCoinPer));
            coinReward.text = $"+ {rewardGetCoin}";

            for(int i = 0; i < 3; i++)
            {
                var rewardButton = rewardButtons[i];
                var reward = MonsterCard.rewards[i];
                rewardButton.transform.GetChild(0).GetComponent<Text>().text = reward.title;
                rewardButton.onClick.RemoveAllListeners();
                rewardButton.onClick.AddListener(() => OnClickRewardButton(reward));
            }
        }
    }

    void OnClickRewardButton(MonsterReward reward)
    {
        Player player = GameState.Instance.player;
        switch(reward.type)
        {
            case MonsterRewardType.Weapon:
                player.SetEquipment(reward.equipment);
                player.money += rewardGetCoin;
                this.gameObject.SetActive(false);
                if (player.GetWeaponList().Count > 10)
                    weaponChangePanel.SetActive(true);
                break;
            case MonsterRewardType.Equipment:
                equipmentChanger.DisplayPanel(reward.equipment, (e) => 
                {
                    player.SetEquipment(e);
                    player.money += rewardGetCoin;
                    this.gameObject.SetActive(false);
                });
                break;
            case MonsterRewardType.Heal:
                player.Heal((int)(player.GetStat().maxHp * reward.healPercent));
                this.gameObject.SetActive(false);
                break;
        }
    }
}
