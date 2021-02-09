using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanelController : MonoBehaviour
{
    public MonsterCard monsterCard;
    public BossCard bossCard;
    
    StageChoice stageChoice;
    GameObject weaponChangePanel;
    GameObject artifactChangePanel;
    EquipmentChangePanelController equipmentChanger;
    
    Text coinReward;
    Button AbandonButton;
    Button[] rewardButtons = new Button[3];

    int rewardGetCoin;
    
    public int rewardCount; // 보상 횟수(재화 제외, 아이템만) default = 1 

    void Start()
    {
        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
        weaponChangePanel = GameObject.Find("Canvas").transform.Find("WeaponChangePanel").gameObject;
        artifactChangePanel = GameObject.Find("Canvas").transform.Find("ArtifactChangePanel").gameObject;
        equipmentChanger = GameObject.Find("Canvas").transform.Find("EquipmentChangePanel").gameObject.GetComponent<EquipmentChangePanelController>();
        coinReward = GameObject.Find("RewardPanel/gold_reward").GetComponent<Text>();
        AbandonButton = GameObject.Find("RewardPanel/AbandonButton").GetComponent<Button>();
        AbandonButton.onClick.AddListener(OnClickRewardAbandon);

        for(int i = 0; i < 3; i++)
            rewardButtons[i] = GameObject.Find($"RewardPanel").transform.Find($"Reward{i+1}").gameObject.GetComponent<Button>();

        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Player player = GameState.Instance.player;
        if (monsterCard != null)
        {
            rewardGetCoin = (int)(monsterCard.rewardCoin * (1 + GameState.Instance.player.GetStat().rewardCoinPer));
            
            if(player.GetBuff().continueBattleCoin != 0 && player.CheckConsecutiveLocation(Location.Monster))
            {
                rewardGetCoin += player.GetBuff().continueBattleCoin;
            }
            
            coinReward.text = $"+ {rewardGetCoin}";

            for(int i = 0; i < 3; i++)
            {
                var rewardButton = rewardButtons[i];
                if (monsterCard.monster.isBoss)
                {
                    var reward = bossCard.rewards[i];
                    rewardButton.transform.GetChild(0).GetComponent<Text>().text = reward.title;
                    rewardButton.onClick.RemoveAllListeners();
                    rewardButton.onClick.AddListener(() => OnClickRewardButton(reward));
                }
                else
                {
                    var reward = monsterCard.rewards[i];
                    rewardButton.transform.GetChild(0).GetComponent<Text>().text = reward.title;
                    rewardButton.onClick.RemoveAllListeners();
                    rewardButton.onClick.AddListener(() => OnClickRewardButton(reward));
                }
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
                if (player.GetWeaponList().Count > 10)
                    weaponChangePanel.SetActive(true);
                AfterChooseReward();
                break;
            case MonsterRewardType.Equipment:
                equipmentChanger.DisplayPanel(reward.equipment, (e) => 
                {
                    player.SetEquipment(e);
                    AfterChooseReward();
                });
                break;
            case MonsterRewardType.Heal:
                player.Heal((int)(player.GetStat().maxHp * reward.healPercent));
                AfterChooseReward();
                break;
        }
    }

    void OnClickRewardButton(BossReward reward)
    {
        Player player = GameState.Instance.player;
        player.SetEquipment(reward.artifact);
        if (player.GetArtifacts().Count > 3)
            artifactChangePanel.SetActive(true);
        AfterChooseReward();
    }

    void OnClickRewardAbandon()
    {
        AfterChooseReward();
    }

    void AfterChooseReward()
    {
        Player player = GameState.Instance.player;
        player.money += rewardGetCoin;
        this.gameObject.SetActive(false);
    }
}
