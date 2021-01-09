using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestPanelController : MonoBehaviour
{
    Text chestType;
    Text chestDescription;
    StageChoice stageChoice;
    GameObject weaponChangePanel;

    Player player = GameState.Instance.player;

    public ChestCard ChestCard;

    void Start()
    {
        chestType = GameObject.Find("ChestPanel/ChestType").GetComponent<Text>();
        chestDescription = GameObject.Find("ChestPanel/ChestDescription").GetComponent<Text>();
        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
        weaponChangePanel = GameObject.Find("Canvas").transform.Find("WeaponChangePanel").gameObject;
    }

    void OnEnable()
    {
        if (ChestCard != null)
        {
            chestType.text = $"<{ChestCard.ChestType.ToString()}>";
            chestDescription.text = ChestCard.ToString();
        }
    }
    
    public void OnClickTreasureButton()
    {
        switch (ChestCard.ChestType)
        {
            case ChestType.Equipment:
                player.SetEquipment(JsonDB.GetWeapon("weapon_000"));
                if (player.GetWeaponList().Count > 10)
                {
                    weaponChangePanel.SetActive(true);
                }
                break;
            case ChestType.Heal:
                player.Heal((int)(ChestCard.HealPercent * player.GetStat().maxHp));
                break;
            case ChestType.Dispel:
                break;
            case ChestType.Damage:
                player.Damage((int)(ChestCard.DamagePercent * player.GetStat().maxHp));
                break;
            case ChestType.Debuff:
                break;
        }
       stageChoice.MoveToNextStage();
    }
}
