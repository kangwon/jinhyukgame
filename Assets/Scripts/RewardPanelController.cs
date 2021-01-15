using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class RewardPanelController : MonoBehaviour
{    
    
    public StageChoice stageChoice;

    GameObject[] reward = new GameObject[3];
    GameObject weaponChangePanel;
    Text coinReward;

    string full_name;
    int rankNameIndex, rewardPrefixIndex, coinRand;
    int[] coinMin = new int[12] {0, 15, 25, 35, 50, 75, 75, 90,  100, 100, 120, 120};
    int[] coinMax = new int[12] {0, 25, 35, 45, 60, 90, 90, 100, 110, 110, 130, 150};
    List<int[]> rankPercentage = new List<int[]>();
    
    public void OnClickRewardAbandon()
    {
        stageChoice.MoveToNextStage();
    }

    public string rewardMaking(int index)
    {
        System.Random r = new System.Random(index);
        int rewardType = r.Next(100);
        int rewardPrefixRand = r.Next(100);
        int rewardRank = r.Next(100);
        int world_num = Convert.ToInt32(GameState.Instance.World.Number);
        
        coinRand = r.Next(coinMin[world_num], coinMax[world_num]);

        if(rewardPrefixRand <= 5) rewardPrefixIndex = 0;
        else if(rewardPrefixRand <= 30) rewardPrefixIndex = 1;
        else if(rewardPrefixRand <= 70) rewardPrefixIndex = 2;
        else if(rewardPrefixRand <= 95) rewardPrefixIndex = 3;
        else rewardPrefixIndex = 4;

        for(int i=0;i<5;i++){
            if(rankPercentage[world_num - 1][i] != 0){
                if(rewardRank <= rankPercentage[world_num - 1][i]) 
                {
                    rankNameIndex = i;
                    break;
                }
            }
        }

        if(rewardType <= 70) rewardWeapon(index);
        else rewardArmor(index);

        return full_name;
    }

    public void rewardWeapon(int index){
        System.Random r = new System.Random();
        int randWeapon = r.Next(5);
        full_name = JsonDB.GetWeapon($"weapon_{randWeapon}{rankNameIndex}{rewardPrefixIndex}").name;
    }
    public void rewardArmor(int index){
        System.Random r = new System.Random();
        int randArmor = r.Next(5);
        full_name = JsonDB.GetEquipment($"helmet0_00").name;
    }

    void Start()
    {
        weaponChangePanel = GameObject.Find("Canvas").transform.Find("WeaponChangePanel").gameObject;

        rankPercentage.Add(new int[] {100,0,0,0,0});
        rankPercentage.Add(new int[] {75,100,0,0,0});
        rankPercentage.Add(new int[] {58,98,100,0,0});
        rankPercentage.Add(new int[] {35,85,100,0,0});
        rankPercentage.Add(new int[] {15,58,98,100,0});
        rankPercentage.Add(new int[] {10,40,94,99,100});
        rankPercentage.Add(new int[] {3,28,88,98,100});
        rankPercentage.Add(new int[] {1,21,85,97,100});
        rankPercentage.Add(new int[] {1,16,80,95,100});

        coinReward = GameObject.Find("gold_reward").GetComponent<Text>();

        for(int i = 0; i < 3; i++)
        {
            reward[i] = GameObject.Find($"RewardPanel").transform.Find($"Reward{i+1}").gameObject;
            reward[i].transform.GetChild(0).GetComponent<Text>().text = rewardMaking(i);
            full_name = "";
        }
    }

    void Update()
    {   
        coinReward.text = "+" + coinRand.ToString();
    }
}