using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class RewardPanelController : MonoBehaviour
{    
    public StageChoice stageChoice;

    Text gold_reward;
    Text[] reward = new Text[3];

    string full_name;
    string[] rank_name = new string [5] {"커먼","언커먼","레어","유니크","레전더리"};
    List<int[]> rank_percentage = new List<int[]>();
    
    public void OnClickRewardAbandon()
    {
        stageChoice.MoveToNextStage();
    }

    public string RewardMaking(int index)
    {
        System.Random r = new System.Random(index);
        // Random r = new Random(unchecked((int)DateTime.Now.Ticks) + index);
        int reward_kind = r.Next(100);
        int reward_modify = r.Next(100);
        int reward_rank = r.Next(100);
        int world_num = Convert.ToInt32(GameState.Instance.World.Number);
        
        if(reward_modify <= 5) full_name += "망가진";
        else if(reward_modify <= 30) full_name += "약한";
        else if(reward_modify <= 70) full_name += "평범한";
        else if(reward_modify <= 95) full_name += "튼튼한";
        else full_name += "놀라운";
        
        Debug.Log("월드 " + GameState.Instance.World.Number);

        for(int i=0;i<5;i++){
            if(rank_percentage[world_num - 1][i] != 0){
                if(reward_rank <= rank_percentage[world_num - 1][i]) 
                {
                    full_name += rank_name[i];
                    break;
                }
            }
        }

        if(reward_kind <= 70) reward_weapon();
        else reward_armor();

        return full_name;
    }

    //퇴근 후 집에가서 더 할예정
    public void reward_weapon(){
        full_name += "무기";
    }
    public void reward_armor(){
        full_name += "방어구";
    }

    // Start is called before the first frame update
    void Start()
    {
        rank_percentage.Add(new int[] {100,0,0,0,0});
        rank_percentage.Add(new int[] {75,100,0,0,0});
        rank_percentage.Add(new int[] {58,98,100,0,0});
        rank_percentage.Add(new int[] {35,85,100,0,0});
        rank_percentage.Add(new int[] {15,58,98,100,0});
        rank_percentage.Add(new int[] {10,40,94,99,100});
        rank_percentage.Add(new int[] {3,28,88,98,100});
        rank_percentage.Add(new int[] {1,21,85,97,100});
        rank_percentage.Add(new int[] {1,16,80,95,100});



        gold_reward = GameObject.Find("gold_reward").GetComponent<Text>();

        for(int i = 0; i < 3; i++)
        {
            reward[i] = GameObject.Find($"reward_name{i+1}").GetComponent<Text>();
            reward[i].text = RewardMaking(i);
            full_name = "";
        }
    }

    // Update is called once per frame
    void Update()
    {   
        
    }
}
