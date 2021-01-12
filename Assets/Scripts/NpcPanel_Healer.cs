using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcPanel_Healer : MonoBehaviour
{
    [HideInInspector]
    public Text healerText;

    public void onClickHealButton() 
    {
        Player player = GameState.Instance.player;
        int healAmount = player.GetStat().maxHp - player.hp;

        if(healAmount >= 1) {
            player.Heal(player.GetStat().maxHp);
            healerText.text = $"{healAmount}만큼 치료되었습니다. 나가세요.";
            //TODO : 재화 차감
        } else {
            healerText.text = "이미 최대 체력입니다.";
        }
        
    }

    void Awake() 
    {
        healerText = gameObject.transform.Find("HealerText").GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        healerText.text = "안녕하세요. 힐러입니다.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
