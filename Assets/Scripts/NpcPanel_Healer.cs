using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcPanel_Healer : MonoBehaviour
{
    Player player = GameState.Instance.player;
    public void onClickHealButton() {
        int healAmount = player.GetStat().maxHp - player.hp;

        if(healAmount >= 1) {
            player.Heal(player.GetStat().maxHp);
            //TODO : healAmount로 재화 채감
            Debug.Log("힐됨!");
        } else {
            Debug.Log("이미 풀피!");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
