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
                var rewardEquipment = MonsterCard.rewardEquipments[i];
                rewardButton.transform.GetChild(0).GetComponent<Text>().text = rewardEquipment.name;
                rewardButton.onClick.RemoveAllListeners();
                rewardButton.onClick.AddListener(() => OnClickRewardButton(rewardEquipment));
            }
        }
    }

    void OnClickRewardButton(Equipment reward)
    {
        Player player = GameState.Instance.player;
        if (reward.type == "weapon")
        {
            player.SetEquipment(reward);
            player.money += rewardGetCoin;
            this.gameObject.SetActive(false);
            if (player.GetWeaponList().Count > 10)
                weaponChangePanel.SetActive(true);
        }
        else
        {
            equipmentChanger.DisplayPanel(reward, (e) => 
            {
                player.SetEquipment(e);
                player.money += rewardGetCoin;
                this.gameObject.SetActive(false);
            });
        }
    }
}
