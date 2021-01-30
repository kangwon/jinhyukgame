using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcHealerController : MonoBehaviour
{
    Text healerText;

    Player player;
    int healAmount;
    int price { get => healAmount * GameState.Instance.World.Number; }

    public void onClickHealButton() 
    {
        if(healAmount >= 1) 
        {
            if (player.Pay(price))
            {
                player.Heal(healAmount);
                healerText.text = $"{healAmount}만큼 치료되었습니다. 나가세요.";
            }
            else
            {
                healerText.text = "돈이 부족합니다.";
            }
        } 
        else 
        {
            healerText.text = "이미 최대 체력입니다.";
        }
        
    }

    void Awake() 
    {
        healerText = gameObject.transform.Find("HealerText").GetComponent<Text>();
    }

    void OnEnable()
    {
        player = GameState.Instance.player;
        if (player != null)
        {
            healAmount = player.GetStat().maxHp - player.hp;
            healerText.text = $"안녕하세요. 힐러입니다.\n체력 회복: {price} G";
        }
    }
}
