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
    public Text[] reward = new Text[3];
    string full_name;

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
        Debug.Log(reward_kind + ", " + reward_modify);
        
        if(reward_modify <= 5) full_name += "망가진";
        else if(reward_modify <= 30) full_name += "약한";
        else if(reward_modify <= 70) full_name += "평범한";
        else if(reward_modify <= 95) full_name += "튼튼한";
        else full_name += "놀라운";
        
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

    void UpdateRewardName(int index, string name){

    }
}
