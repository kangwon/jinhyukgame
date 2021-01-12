using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class RewardPanelController : MonoBehaviour
{    
    
    public StageChoice stageChoice;

    Player[] reward_player = new Player[3];
    GameObject[] reward = new GameObject[3];
    GameObject weaponChangePanel;

    Text coinReward;
    string full_name;
    string[] rankName = new string [5] {"#000000","#088A08","#00BFFF","#BF00FF","#FFBF00"};
    int rankNameIndex;
    List<int[]> rankPercentage = new List<int[]>();
    
    public void OnClickRewardAbandon()
    {
        stageChoice.MoveToNextStage();
    }

    public string rewardMaking(int index)
    {
        System.Random r = new System.Random(index);
        int rewardKind = r.Next(100);
        int rewardModify = r.Next(100);
        int rewardRank = r.Next(100);
        int world_num = Convert.ToInt32(GameState.Instance.World.Number);
        
        if(rewardModify <= 5) full_name += "망가진 ";
        else if(rewardModify <= 30) full_name += "약한 ";
        else if(rewardModify <= 70) full_name += "평범한 ";
        else if(rewardModify <= 95) full_name += "튼튼한 ";
        else full_name += "놀라운 ";
        
        // Debug.Log("월드 " + GameState.Instance.World.Number);

        for(int i=0;i<5;i++){
            if(rankPercentage[world_num - 1][i] != 0){
                if(rewardRank <= rankPercentage[world_num - 1][i]) 
                {
                    // full_name += rankName[i];
                    rankNameIndex = i;
                    break;
                }
            }
        }

        if(rewardKind <= 70) rewardWeapon(index);
        else rewardArmor(index);

        return full_name;
    }

    public void rewardWeapon(int index){
        System.Random r = new System.Random();
        int randWeapon = r.Next(5);
        full_name += JsonDB.GetWeapon($"weapon_{randWeapon}00").name;
        // reward_player[index].SetEquipment(JsonDB.GetWeapon($"weapon_{randWeapon}00"));
    }
    public void rewardArmor(int index){
        System.Random r = new System.Random();
        int randArmor = r.Next(5);
        full_name += JsonDB.GetEquipment($"helmet0_00").name;
        // reward_player[index].SetEquipment(JsonDB.GetEquipment($"helmet0_00"));
    }

    // Start is called before the first frame update
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
            reward[i].transform.GetChild(0).GetComponent<Text>().text = "<color=" + rankName[rankNameIndex] + ">" + rewardMaking(i) + "</color>";
            full_name = "";
        }
    }

    // Update is called once per frame
    void Update()
    {   
        
    }
}
