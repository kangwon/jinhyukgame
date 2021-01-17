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
    GameObject rewardPanel;
    Text coinReward;

    string full_name;
    int rewardRankIndex, rewardPrefixIndex, rewardCoin;
    List<List<double>> rankPercentage = new List<List<double>>();
    double[,] percentage = new double[9,5] {
        {1,    0,    0,    0,    0},
        {0.75, 0.25, 0,    0,    0},
        {0.58, 0.4,  0.02, 0,    0},
        {0.35, 0.5,  0.15, 0,    0},
        {0.15, 0.43, 0.4,  0.02, 0},
        {0.1,  0.3,  0.54, 0.05, 0.01},
        {0.03, 0.25, 0.6,  0.1,  0.02},
        {0.01, 0.2,  0.64, 0.12, 0.03},
        {0.01, 0.15, 0.64, 0.15, 0.05}
    };
    int[] coinMin = new int[12] {0, 15, 25, 35, 50, 75, 75, 90,  100, 100, 120, 120};
    int[] coinMax = new int[12] {0, 25, 35, 45, 60, 90, 90, 100, 110, 110, 130, 150};
    
    public RewardPanelController(){
        this.Random = new Random();
    }

    public void OnClickRewardAbandon()
    {
        stageChoice.MoveToNextStage();
    }

    public string rewardMaking(int index)
    {
        int world_num = Convert.ToInt32(GameState.Instance.World.Number);
        List<int> coinRange = new List<int>();

        for(int i=coinMin[world_num];i<=coinMax[world_num];i++)
            coinRange.Add(i);
            
        var coinRand = CustomRandom<int>.Choice(coinRange, this.Random);
        rewardCoin = coinRand;

        var  rewardPrefixRand = CustomRandom<int>.WeightedChoice
        (
            new List<int> {0, 1, 2, 3, 4}, 
            new List<double> {0.05, 0.25, 0.4, 0.25, 0.05},
            this.Random
        );
        switch (rewardPrefixRand)
        {
            case 0: 
                rewardPrefixIndex = 0;
                break;
            case 1: 
                rewardPrefixIndex = 1;
                break;
            case 2: 
                rewardPrefixIndex = 2;
                break;
            case 3: 
                rewardPrefixIndex = 3;
                break;
            case 4: 
                rewardPrefixIndex = 4;
                break;
            default: 
                throw new NotImplementedException($"Invalid Random type ");
        }

        var rewardRankRand = CustomRandom<int>.WeightedChoice
        (
            new List<int> {0, 1, 2, 3, 4}, 
            rankPercentage[world_num - 1],
            this.Random
        );
        Debug.Log(rankPercentage[world_num - 1]);
        switch (rewardRankRand)
        {
            case 0: 
                rewardRankIndex = 0;
                break;
            case 1: 
                rewardRankIndex = 1;
                break;
            case 2: 
                rewardRankIndex = 2;
                break;
            case 3: 
                rewardRankIndex = 3;
                break;
            case 4: 
                rewardRankIndex = 4;
                break;
            default: 
                throw new NotImplementedException($"Invalid Random type ");
        }
        
        var rewardTypeRand = CustomRandom<int>.WeightedChoice
        (
            new List<int> {0, 1},
            new List<double> {0.7, 0.3},
            this.Random
        );
        if(rewardTypeRand == 0) rewardWeapon(index);
        else rewardEquipment(index);

        return full_name;
    }

    public void rewardWeapon(int index){
        var randWeapon = CustomRandom<int>.Choice(new List<int> {1, 2, 3, 4 ,5}, this.Random);
        // full_name = JsonDB.GetWeapon($"weapon_{randWeapon}{rewardRankIndex}{rewardPrefixIndex}").name;
        full_name = "weapon_" + randWeapon.ToString() + rewardRankIndex.ToString() + rewardPrefixIndex.ToString();
    }
    public void rewardEquipment(int index){
        int randEquipment = CustomRandom<int>.Choice(new List<int> {1, 2, 3, 4 ,5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}, this.Random);
        full_name = "Equipment_" + randEquipment.ToString() + rewardRankIndex.ToString() + rewardPrefixIndex.ToString();
    }

    void Start()
    {
        Button[] reward = new Button[3];
        weaponChangePanel = GameObject.Find("Canvas").transform.Find("WeaponChangePanel").gameObject;
        rewardPanel = GameObject.Find("RewardPanel");  
        coinReward = GameObject.Find("gold_reward").GetComponent<Text>();

        for(int i=0;i<9;i++){
            rankPercentage.Add(new List<double>());
            for(int j=0;j<5;j++){
                rankPercentage[i].Add(percentage[i,j]);
            }
        }
        
        for(int i = 0; i < 3; i++)
        {
            reward[i] = GameObject.Find($"RewardPanel").transform.Find($"Reward{i+1}").GetComponent<Button>();
            reward[i].transform.GetChild(0).GetComponent<Text>().text = rewardMaking(i);
            full_name = "";

            reward[i].onClick.AddListener(() => {
                rewardPanel.SetActive(false);
                weaponChangePanel.SetActive(true);
            });
        }
    }

    void Update()
    {   
        coinReward.text = "+" + rewardCoin.ToString();
    }
}