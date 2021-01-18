using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random=System.Random;

public class RewardPanelController : MonoBehaviour
{    
    public readonly Random Random;
    public StageChoice stageChoice;
    
    GameObject weaponChangePanel;
    EquipmentChangePanelController equipmentChanger;

    Button[] rewardButtons = new Button[3];
    Text coinReward;

    List<List<double>> rankPercentage = new List<List<double>>
    {
        new List<double> {1,    0,    0,    0,    0},
        new List<double> {0.75, 0.25, 0,    0,    0},
        new List<double> {0.58, 0.4,  0.02, 0,    0},
        new List<double> {0.35, 0.5,  0.15, 0,    0},
        new List<double> {0.15, 0.43, 0.4,  0.02, 0},
        new List<double> {0.1,  0.3,  0.54, 0.05, 0.01},
        new List<double> {0.03, 0.25, 0.6,  0.1,  0.02},
        new List<double> {0.01, 0.2,  0.64, 0.12, 0.03},
        new List<double> {0.01, 0.15, 0.64, 0.15, 0.05}
    };
    int[] coinMin = new int[12] {0, 15, 25, 35, 50, 75, 75, 90,  100, 100, 120, 120};
    int[] coinMax = new int[12] {0, 25, 35, 45, 60, 90, 90, 100, 110, 110, 130, 150};
    List<double> prefixPercentage = new List<double>() { 0.05, 0.25, 0.40, 0.25, 0.05 };
    
    public RewardPanelController()
    {
        this.Random = new Random();
    }

    public void OnClickRewardAbandon()
    {
        stageChoice.MoveToNextStage();
    }

    Equipment GetRewardEquipment(int worldNum)
    {
        int rewardPrefixIndex = CustomRandom<int>.WeightedChoice
        (
            new List<int> { 0, 1, 2, 3, 4 }, 
            prefixPercentage,
            this.Random
        );
        int rewardRankIndex = CustomRandom<int>.WeightedChoice
        (
            new List<int> { 0, 1, 2, 3, 4 }, 
            rankPercentage[worldNum - 1],
            this.Random
        );
        var rewardTypeRand = CustomRandom<int>.WeightedChoice
        (
            new List<int> {0, 1},
            new List<double> {0.7, 0.3},
            this.Random
        );
        if (rewardTypeRand == 0)
        {
            var weaponType = (int)CustomRandom<WeaponType>.Choice
            (
                Enum.GetValues(typeof(WeaponType)).Cast<WeaponType>().ToList(), 
                this.Random
            );
            string weaponId = $"weapon_{weaponType}{rewardRankIndex}{rewardPrefixIndex}";
            return JsonDB.GetWeapon(weaponId);
        }
        else 
        {
            var idBase = CustomRandom<string>.Choice
            (
                JsonDB.GetEquipmentIdBases(),
                this.Random
            );
            string equipId = $"{idBase}_{rewardRankIndex}{rewardPrefixIndex}";
            return JsonDB.GetEquipment(equipId);
        }
    }

    int GetRewardCoin(int worldNum)
    {
        return this.Random.Next(coinMin[worldNum], coinMax[worldNum]);
    }

    void Start()
    {
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
            int rewardCoin = GetRewardCoin(worldNum);
            coinReward.text = $"+ {rewardCoin}";

            foreach(var rewardButton in rewardButtons)
            {
                var rewardEquipment = GetRewardEquipment(worldNum);
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
        this.gameObject.SetActive(false);
        stageChoice.MoveToNextStage();
    }
}