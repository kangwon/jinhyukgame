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
        int worldNum = GameState.Instance.World?.Number ?? -1;
        if (worldNum > -1)
        {
            coinReward.text = $"+ {MonsterCard.rewardCoin}";

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
            if (player.GetWeaponList().Count > 10)
                weaponChangePanel.SetActive(true);
        }
        else
        {
            equipmentChanger.DisplayPanel(reward);
        }


        if (player.GetStat().maxHp < player.hp)
            GameState.Instance.player.hp = player.GetStat().maxHp;
    }
}
